using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Management.Network.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Repositories;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using pp3.dominio.DataTransferObjects;
using static pp3.dominio.Models.Globals;
using pp3.services.Handler;

namespace pp3.services.Services
{
    public class SeguridadService : ISeguridadRepository
    {
        private readonly Pp3roContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ServicesResult result = new ServicesResult();
        private readonly ILogger<SeguridadService> _logger;
        private readonly IMapper _mapper;
        




        public SeguridadService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, ILogger<SeguridadService> logger, IMapper mapper)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
            this._mapper = mapper;
        }
        public async Task<ServicesResult> InsertarLog(int tarea, string usuario, string anterior, string actual)
        {
            _logger.LogInformation($"Insertar Log ({tarea}, {usuario}, {anterior}, {actual})");

            try
            {
                var W_Tarea = new SqlParameter("@W_Tarea", tarea);
                var W_Usuario = new SqlParameter("@W_Usuario", Globals.user);   // qui hay que cambiar 'Globals.user'   por parametro que trae funcion  'usuario'
                var W_ValorAnterior = new SqlParameter("@W_ValorAnterior", anterior);
                var W_ValorActual = new SqlParameter("@W_ValorActual", actual);

                var execSP = await _context.Database.ExecuteSqlRawAsync("EXEC PR_INSERTLOG @W_Tarea, @W_Usuario, @W_ValorAnterior, @W_ValorActual", W_Tarea, W_Usuario, W_ValorAnterior, W_ValorActual);

                if (execSP > 0)
                {
                    result.Content = "True";
                    result.Message = HttpStatusCode.OK.ToString();
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                }
                else
                {
                    result.Content = "False";
                    result.Message = "No se pudo ejecutar el SP.";
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                }
            }
            catch (Exception ex)
            {
                result.Content = "False";
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error al insertar log. {ex.Message}";
                _logger.LogError($"Error en InsertarLog - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " /*+
                $"{ex.InnerException.ToString() ?? string.Empty}"*/);
            }

            return result;
        }
        public async Task<ServicesResult> UsuarioCliente(string USR_ID)
        {
            _logger.LogInformation($"Consultando Usuario por Cliente ({USR_ID})");

            try
            {
                List<USUARIOS> usuarios = await (from usuario in _context.USUARIOS
                                                 join perfil in _context.PERFILES
                                                 on usuario.PRF_ID equals perfil.PRF_ID
                                                 where usuario.USR_ID.ToUpper() == USR_ID.ToUpper()
                                                 && perfil.PRF_TIPOUSUARIO == "CLI"
                                                 select usuario)
                                .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(usuarios);
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                _logger.LogError($"Error en UsuarioCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }

            return result;
        }
        public async Task<ServicesResult> UsuariosPorCliente(int tipo, double numero)
        {
            _logger.LogInformation($"Consultando Usuario Por Cliente ({tipo}, {numero})");

            try
            {
                var usuariosPorCliente = await (from usuarios in _context.USUARIOS
                                                join perfiles in _context.PERFILES on usuarios.PRF_ID equals perfiles.PRF_ID
                                                join tipoDoc in _context.TIPODOC on usuarios.TDD_CLT_ID equals tipoDoc.TDD_ID
                                                where usuarios.TDD_CLT_ID == Convert.ToDecimal(tipo) && usuarios.CLT_NUMDOC == Convert.ToDecimal(numero)
                                                && perfiles.PRF_TIPOUSUARIO == "CLI"
                                                orderby usuarios.USR_ID
                                                select new
                                                {
                                                    usuarios.USR_ID,
                                                    usuarios.TDD_CLT_ID,
                                                    usuarios.CLT_NUMDOC,
                                                    usuarios.SUC_ID,
                                                    usuarios.USR_CLAVE,
                                                    usuarios.TDD_USR_ID,
                                                    usuarios.USR_NUMDOC,
                                                    usuarios.USR_NOMBRE,
                                                    usuarios.USR_APELLIDO,
                                                    usuarios.USR_MAR_ACTIVO,
                                                    usuarios.USR_INTENTOSFALLIDOS,
                                                    usuarios.USR_MAR_BAJA,
                                                    usuarios.USR_CARGO,
                                                    usuarios.USR_MAR_GENCLAVE,
                                                    usuarios.USR_MAR_CAMBIACLAVE,
                                                    usuarios.NROCOBIS,
                                                    perfiles.PRF_ID,
                                                    perfiles.PRF_DESCRIPCION,
                                                    perfiles.PRF_TIPOUSUARIO,
                                                    tipoDoc.TDD_ID,
                                                    tipoDoc.TDD_DESCRIPCION,
                                                    tipoDoc.TDD_DESCRIPCIONABREV,
                                                    tipoDoc.TDD_CODIGOSIAF,
                                                    tipoDoc.TDD_MASCARA,
                                                    tipoDoc.TDD_COBIS

                                                }).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(usuariosPorCliente);
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error al consultar usuarios por cliente. {ex.Message}";
                _logger.LogError($"Error en UsuariosPorCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }

            return result;
        }
        public async Task<ServicesResult> usuarios(string id)
        {
            _logger.LogInformation($"Consultando Usuarios ({id})");

            try
            {
                var consultaUsuarios = from usuarios in _context.USUARIOS
                                       join perfiles in _context.PERFILES on usuarios.PRF_ID equals perfiles.PRF_ID
                                       join tipoDoc in _context.TIPODOC on usuarios.TDD_USR_ID equals tipoDoc.TDD_ID
                                       where perfiles.PRF_TIPOUSUARIO != "INT"
                                       orderby usuarios.USR_ID
                                       select new
                                       {
                                           usuarios.USR_ID,
                                           usuarios.TDD_CLT_ID,
                                           usuarios.CLT_NUMDOC,
                                           usuarios.SUC_ID,
                                           usuarios.USR_CLAVE,
                                           usuarios.TDD_USR_ID,
                                           usuarios.USR_NUMDOC,
                                           usuarios.USR_NOMBRE,
                                           usuarios.USR_APELLIDO,
                                           usuarios.USR_MAR_ACTIVO,
                                           usuarios.USR_INTENTOSFALLIDOS,
                                           usuarios.USR_MAR_BAJA,
                                           usuarios.USR_CARGO,
                                           usuarios.USR_MAR_GENCLAVE,
                                           usuarios.USR_MAR_CAMBIACLAVE,
                                           usuarios.NROCOBIS,
                                           perfiles.PRF_DESCRIPCION,
                                           tipoDoc.TDD_DESCRIPCION
                                       };

                if (id != "null")
                {
                    consultaUsuarios = consultaUsuarios.Where(usuarios => usuarios.USR_ID.ToLower().Contains(id.ToLower()));
                    result.Content = JsonConvert.SerializeObject(await consultaUsuarios.ToListAsync());
                }
                else
                {
                    result.Content = JsonConvert.SerializeObject(await consultaUsuarios.Skip(0).Take(100).ToListAsync());
                }                

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error al consultar usuarios. {ex.Message}";
                _logger.LogError($"Error en usuarios - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");result.Content = ex.Message; 
            }

            return result;
        }
        public async Task<ServicesResult> UsuarIoD(string id)
        {
            _logger.LogInformation($"Consultando UsuarIoD ({id})");

            try
            {
                var usuario = await _context.USUARIOS
                    .Where(usuario => usuario.USR_ID == id.ToUpper())
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(usuario);
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error al consultar usuario por id. {ex.Message}";
                _logger.LogError($"Error en UsuarIoD - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }

            return result;
        }
        public async Task<ServicesResult> EliminarUsuario(string idUsuario)
        {
            _logger.LogInformation($"Eliminar por Usuario ({idUsuario})");

            // Iniciar la transacción
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var usuario = await _context.USUARIOS.FirstOrDefaultAsync(usuario => usuario.USR_ID.ToUpper() == idUsuario.ToUpper());

                if (usuario != null)
                {
                    usuario.USR_MAR_ACTIVO = 0;

                    await _context.SaveChangesAsync();

                    usuario.USR_MAR_BAJA = 1;

                    await _context.SaveChangesAsync();

                    _context.USUARIOS.RemoveRange(usuario);

                    await _context.SaveChangesAsync();

                    var ok = await InsertarLog((int)CodigosTareas.Usuarios_Eliminar, Globals.user, idUsuario, " ");
                    if (ok.Content == null)
                        throw new Exception("Error al insertar log.");

                    await _context.SaveChangesAsync();

                    // Confirmar la transacción
                    await transaction.CommitAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = "true";
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No se encontro usuario";
                    result.Content = "false";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en EliminarUsuario - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                // Revertir transacción
                await transaction.RollbackAsync();

                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error al eliminar usuario. {ex.Message}";
            }
            finally
            {
                // Liberar recursos del contexto y transacción
                await transaction.DisposeAsync();
                await _context.DisposeAsync();
            }

            return result;
        }
        public async Task<ServicesResult> Perfiles()
        {
            _logger.LogInformation($"Consultando Perfiles");

            try
            {
                var perfiles = await _context.PERFILES
                    .Where(perfil => perfil.PRF_TIPOUSUARIO == "CLI")
                    .OrderBy(perfil => perfil.PRF_DESCRIPCION)
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(perfiles);
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error al consultar perfiles. {ex.Message}";
                _logger.LogError($"Error en Perfiles - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }

            return result;
        }
        public async Task<ServicesResult> ModificarUsuario(UsuariosDto usuario)
        {
            _logger.LogInformation($"Modificar Usuario ({usuario})");

            // Iniciar la transacción
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var usuarioEncontrado = await _context.USUARIOS
                    .Where(u => u.USR_ID == usuario.USR_ID)
                    .FirstOrDefaultAsync();

                if (usuarioEncontrado != null)
                {
                    usuarioEncontrado.PRF_ID = usuario.PRF_ID;
                    usuarioEncontrado.USR_NOMBRE = usuario.USR_NOMBRE;
                    usuarioEncontrado.USR_APELLIDO = usuario.USR_APELLIDO;
                    usuarioEncontrado.TDD_USR_ID = usuario.TDD_USR_ID;
                    usuarioEncontrado.USR_NUMDOC = Convert.ToDecimal(usuario.USR_NUMDOC);
                    usuarioEncontrado.SUC_ID = usuario.SUC_ID;
                    usuarioEncontrado.TDD_CLT_ID = usuario.TDD_CLT_ID;
                    usuarioEncontrado.CLT_NUMDOC = Convert.ToDecimal(usuario.CLT_NUMDOC);
                    usuarioEncontrado.USR_CARGO = usuario.USR_CARGO;
                    usuarioEncontrado.NROCOBIS = Convert.ToDecimal(usuario.NROCOBIS);

                    _context.USUARIOS.Update(usuarioEncontrado);

                    await _context.SaveChangesAsync();

                    var ok = await InsertarLog((int)CodigosTareas.Usuarios_Modificar, Globals.user, usuario.USR_ID, usuario.USR_ID);
                    if (ok.Content == null)
                        throw new Exception("Error al insertar log.");


                    // Confirmar la transacción
                    await transaction.CommitAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = "true";
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No se encontro usuario";
                    result.Content = "false";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ModificarUsuario - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                // Revertir transacción
                await transaction.RollbackAsync();

                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error al modificar usuario. {ex.Message}";
            }
            finally
            {
                // Liberar recursos del contexto y transacción
                await transaction.DisposeAsync();
                await _context.DisposeAsync();
            }

            return result;
        }
        public async Task<ServicesResult> ActivarUsuario(string UserID)
        {
            _logger.LogInformation($"Activar Usuario ({UserID})");

            try
            {
                var usuarioEncontrado = await _context.USUARIOS
                            .Where(u => u.USR_ID.ToUpper() == UserID.ToUpper() && u.USR_MAR_BAJA == 0)
                            .FirstOrDefaultAsync();

                if (usuarioEncontrado != null)
                {
                    usuarioEncontrado.USR_MAR_ACTIVO = 1;
                    usuarioEncontrado.USR_INTENTOSFALLIDOS = 0;

                    await _context.SaveChangesAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = "true";
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No se encontro usuario";
                    result.Content = "false";
                }
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error al activar usuario. {ex.Message}";
                _logger.LogError($"Error en ActivarUsuario - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }

            return result;
        }
        public async Task<ServicesResult> DesactivarUsuario(string UserID)
        {
            _logger.LogInformation($"Desactivar Usuario ({UserID})");

            try
            {
                var usuarioEncontrado = await _context.USUARIOS.FirstOrDefaultAsync(u => u.USR_ID.ToUpper() == UserID.ToUpper());

                if (usuarioEncontrado != null)
                {
                    usuarioEncontrado.USR_MAR_ACTIVO = 0;

                    await _context.SaveChangesAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = "true";
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No se encontro usuario";
                    result.Content = "false";
                }
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error al desactivar usuario. {ex.Message}";
                _logger.LogError($"Error en DesactivarUsuario - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }

            return result;
        }
        public async Task<ServicesResult> NuevoUsuario(UsuariosDto usuario)
        {
            _logger.LogInformation($"Nuevo Usuario ({usuario})");

            // Iniciar la transacción
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                USUARIOS nuevoUsuario = new USUARIOS();
                int MarGenClave = 0;
                int MarCambClave = 0;

                if (usuario.PRF_ID == 1 || usuario.PRF_ID == 6)
                {
                    MarGenClave = 1;
                    MarCambClave = 1;
                }

                nuevoUsuario.USR_ID = usuario.USR_ID;
                nuevoUsuario.TDD_BNF_ID = null;
                nuevoUsuario.BNF_NUMDOC = null;
                nuevoUsuario.TDD_CLT_ID = usuario.TDD_CLT_ID;
                nuevoUsuario.CLT_NUMDOC = Convert.ToDecimal(usuario.CLT_NUMDOC);
                nuevoUsuario.SUC_ID = usuario.SUC_ID;
                nuevoUsuario.USR_CLAVE = null;
                nuevoUsuario.TDD_USR_ID = usuario.TDD_USR_ID;
                nuevoUsuario.USR_NUMDOC = Convert.ToDecimal(usuario.USR_NUMDOC);
                nuevoUsuario.USR_NOMBRE = usuario.USR_NOMBRE;
                nuevoUsuario.USR_APELLIDO = usuario.USR_APELLIDO;
                nuevoUsuario.PRF_ID = usuario.PRF_ID;
                nuevoUsuario.USR_MAR_ACTIVO = 1;
                nuevoUsuario.USR_INTENTOSFALLIDOS = 0;
                nuevoUsuario.USR_MAR_BAJA = 0;
                nuevoUsuario.USR_CARGO = usuario.USR_CARGO;
                nuevoUsuario.USR_MAR_GENCLAVE = MarGenClave;
                nuevoUsuario.USR_MAR_CAMBIACLAVE = MarCambClave;
                nuevoUsuario.NROCOBIS = Convert.ToDecimal(usuario.NROCOBIS);

                _context.USUARIOS.Add(nuevoUsuario);

                await _context.SaveChangesAsync();

                var ok = await InsertarLog((int)CodigosTareas.Usuarios_Agregar, Globals.user, " ", usuario.USR_ID);
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");


                // Confirmar la transacción
                await transaction.CommitAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = "true";
                result.Message = HttpStatusCode.OK.ToString();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en NuevoUsuario - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");result.Message = ex.Message; 
                // Revertir transacción
                await transaction.RollbackAsync();

                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error al agregar usuario. {ex.Message}";
            }
            finally
            {
                // Liberar recursos del contexto y transacción
                await transaction.DisposeAsync();
                await _context.DisposeAsync();
            }

            return result;
        }

    }
}
