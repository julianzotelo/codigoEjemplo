using System.Linq;
using System.Net;
using System.Runtime.Intrinsics.X86;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Management.Network.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using pp3.dominio.Context;
using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using pp3.services.Handler;
using pp3.services.Repositories;
using static pp3.dominio.Models.Globals;

namespace pp3.services.Services
{
    public class FirmasService : IFirmasRepository
    {
        private readonly Pp3roContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ServicesResult result = new ServicesResult();
        private readonly ILogger<FirmasService> _logger;
        private readonly IMapper _mapper;
        private readonly SeguridadService _seguridadService;
        

        public FirmasService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, SeguridadService seguridadService, IMapper mapper, ILogger<FirmasService> logger)
        {
            this._context = context;
            this._mapper = mapper;
            this._logger = logger;
            this._seguridadService = seguridadService;
            this._httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<ServicesResult> FirmasPorId(string USR_ID)
        {
            _logger.LogInformation($"Consultando Firmas por Id ({USR_ID})");

            try
            {
                List<FIRMAS_nueva> firmas = await (from firma in _context.FIRMAS
                                                   join estado in _context.ESTADOS on firma.FIR_ESTADO equals estado.EST_ID
                                                   where firma.USR_ID == USR_ID

                                    select new FIRMAS_nueva
                                    {
                                        USR_ID = firma.USR_ID,
                                        FIR_ESTADO = firma.FIR_ESTADO + " - " + estado.EST_DESCRIPCION,
                                        FIR_FIRMA = Convert.ToBase64String(firma.FIR_FIRMA) ,
                                        FIR_LEN_FIRMA = firma.FIR_LEN_FIRMA,
                                    }).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(setFirma(firmas));
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                _logger.LogError($"Error en FirmasPorId - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }

            return result;
        }
        public async Task<ServicesResult> FirmasPorIdDoc(string id, int tipo, decimal numero)
        {
            _logger.LogInformation($"Consultando Por Id Doc ({id}, {tipo}, {numero})");

            try
            {
                List<FIRMAS> firmas = await (from FIRMAS in _context.FIRMAS
                                             join USUARIOS in _context.USUARIOS
                                             on FIRMAS.USR_ID equals USUARIOS.USR_ID
                                             where USUARIOS.TDD_CLT_ID == tipo
                                             && USUARIOS.CLT_NUMDOC >= numero
                                             && FIRMAS.USR_ID.Contains(id.ToUpper())
                                             select FIRMAS).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(firmas);
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                _logger.LogError($"Error en FirmasPorIdDoc - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }

            return result;
        }
        public async Task<ServicesResult> FirmasPorDocumento(int tipo, decimal numero)
        {
            _logger.LogInformation($"Consultando Firmas Por Documento ({tipo}, {numero})");

            try
            {
                List<USUARIOS> usuarios = await _context.USUARIOS
                    .Where(f => f.TDD_CLT_ID == tipo && f.CLT_NUMDOC >= numero)
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(usuarios);
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                _logger.LogError($"Error en FirmasPorDocumento - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }

            return result;
        }
        public async Task<ServicesResult> FirmasPorCliente(int tipo, decimal numero)
        {
            _logger.LogInformation($"Consultando Firmas Por Cliente ({tipo}, {numero})");

            try
            {
                List<FIRMAS_nueva> firmas = await (from firma in _context.FIRMAS
                                                   join usuario in _context.USUARIOS on firma.USR_ID equals usuario.USR_ID
                                                   join estado in _context.ESTADOS on firma.FIR_ESTADO equals estado.EST_ID
                                                   where usuario.TDD_CLT_ID == tipo
                                                   && usuario.CLT_NUMDOC == numero
                                                   select new FIRMAS_nueva
                                                   {
                                                       USR_ID = firma.USR_ID,
                                                       FIR_ESTADO = firma.FIR_ESTADO + " - " + estado.EST_DESCRIPCION,
                                                       //FIR_FIRMA = Convert.ToBase64String(firma.FIR_FIRMA) ,      // NO SE SI SE USA, PARA GANAR TIEMPO POR AHORA DESHABILITO (ANDRES)
                                                       FIR_LEN_FIRMA = firma.FIR_LEN_FIRMA,
                                                   }).ToListAsync(); 

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(setFirma(firmas));
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                _logger.LogError($"Error en FirmasPorCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }

            return result;
        }
        public async Task<ServicesResult> FirmantesPorCliente(int tipo, decimal numero)
        {
            _logger.LogInformation($"Consultando Firmas Por Cliente ({tipo}, {numero})");

            try
            {
                List<FIRMAS_nueva> firmas = await (from firma in _context.FIRMAS
                                             join usuario in _context.USUARIOS on firma.USR_ID equals usuario.USR_ID
                                             join estado in _context.ESTADOS on firma.FIR_ESTADO equals estado.EST_ID
                                             where usuario.TDD_CLT_ID == tipo
                                             && usuario.CLT_NUMDOC == numero
                                             && usuario.PRF_ID == 1  //perfil - firmante
                                             select new FIRMAS_nueva
                                             {
                                                 USR_ID = firma.USR_ID,
                                                 FIR_ESTADO = firma.FIR_ESTADO + " - " + estado.EST_DESCRIPCION,
                                                 //FIR_FIRMA = Convert.ToBase64String(firma.FIR_FIRMA),         // NO SE SI SE USA, PARA GANAR TIEMPO POR AHORA DESHABILITO (ANDRES)
                                                 FIR_LEN_FIRMA = firma.FIR_LEN_FIRMA,
                                             }).ToListAsync();
                //select firma).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(firmas);
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                _logger.LogError($"Error en FirmantesPorCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }

            return result;
        }
        public async Task<ServicesResult> FirmasPorApellido(string apellido)
        {
            _logger.LogInformation($"Consultando Firmas Por Apellido ({apellido})");

            try
            {
                List<FIRMAS> firmas = await _context.FIRMAS
                    .Where(f => f.USR_ID.ToUpper().Contains(apellido.ToUpper()))
                    .OrderBy(f => f.USR_ID)
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(firmas);
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                _logger.LogError($"Error en FirmasPorApellido - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }

            return result;
        }
        public async Task<ServicesResult> Firma(string id)
        {
            _logger.LogInformation($"Consultando Firma ({id})");

            try
            {
                var firma = await _context.FIRMAS
                    .Where(f => f.USR_ID == id)
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(firma);
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                _logger.LogError($"Error en Firma - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }

            return result;
        }
        public async Task<ServicesResult> NuevaFirma(FIRMAS_nueva firmas)  
        {
            _logger.LogInformation($"Nueva Firma ({firmas})");

            const int chunkSize = 4096;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            try
            {
                string usr = firmas.USR_ID;
                string estado = firmas.FIR_ESTADO;
                string archivoCODIGO = firmas.FIR_FIRMA  ;

                byte[] firmaBytes =  Convert.FromBase64String(archivoCODIGO);

                CLAVEDIGITALIZACION? claveDigitalizacion = await _context.CLAVEDIGITALIZACION.FirstOrDefaultAsync();
                if (claveDigitalizacion == null)
                {
                    throw new Exception("Clave de digitalización no encontrada.");
                }

                string varKey = claveDigitalizacion.CDI_CLAVE;

                        FIRMAS? firma = await _context.FIRMAS
                            .FirstOrDefaultAsync(f => f.USR_ID == usr.ToUpper());

                        if (firma == null)
                        {
                            firma = new FIRMAS { USR_ID = usr.ToUpper() };
                            _context.FIRMAS.Add(firma);
                        }

                        FIRMASENCRIPTADAS? firmaEncriptada = await _context.FIRMASENCRIPTADAS
                            .FirstOrDefaultAsync(f => f.USR_ID == usr.ToUpper());

                        if (firmaEncriptada == null)
                        {
                            firmaEncriptada = new FIRMASENCRIPTADAS { USR_ID = usr.ToUpper() };
                            _context.FIRMASENCRIPTADAS.Add(firmaEncriptada);
                        }
                     
                        using (var memoryStream = new MemoryStream())
                        {
                            for (int i = 0; i < firmaBytes.Length; i += chunkSize)
                            {
                                int length = Math.Min(chunkSize, firmaBytes.Length - i);
                                byte[] chunk = new byte[length];
                                Array.Copy(firmaBytes, i, chunk, 0, length);

                                for (int j = 0; j < chunk.Length; j++)
                                {
                                    char keyChar = varKey[(j + 1) % varKey.Length];
                                    chunk[j] ^= (byte)keyChar;
                                }

                                await memoryStream.WriteAsync(chunk, 0, chunk.Length);
                            }

                            byte[] encryptedData = memoryStream.ToArray();
                            firmaEncriptada.FIR_FIRMA_ENCRIPTADA = encryptedData;
                            firma.FIR_FIRMA = encryptedData;
                            firma.FIR_LEN_FIRMA = encryptedData.Length;
                            firma.FIR_ESTADO = estado.ToUpper();
                        }

                        var ok = await _seguridadService.InsertarLog((int)CodigosTareas.Firmas_Agregar, Globals.user, " ", $"{usr.ToUpper()}");
                        if (ok.Content == null)
                            throw new Exception("Error al insertar log.");

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();                       
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = JsonConvert.SerializeObject(true);
                    result.Message = HttpStatusCode.OK.ToString();
                 
                }
            catch (Exception ex)
            {
                    _logger.LogError($"Error en NuevaFirma - Origen:  - " +
                    $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                    $"{ex.InnerException.ToString() ?? string.Empty}");
                    await transaction.RollbackAsync();
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error al ejecutar la consulta. {ex.Message}";
                result.Content = "False";
            }

            return result;
        }
        public async Task<ServicesResult> VerFirma(string firma)
        {
            _logger.LogInformation($"Consultando Ver Firma ({firma})");

            const int chunkSize = 4096;
            string archivo;
            byte[] rawData;
            byte[] rawDataDesincriptada;
            long offset = 0;

            try
            {

                CLAVEDIGITALIZACION? claveDigitalizacion = await _context.CLAVEDIGITALIZACION.FirstOrDefaultAsync();
                if (claveDigitalizacion == null)
                {
                    throw new Exception("Clave de digitalización no encontrada.");
                }

                string varKey = claveDigitalizacion.CDI_CLAVE;


                FIRMAS? firmaRecord = await _context.FIRMAS
                    .FirstOrDefaultAsync(f => f.USR_ID == firma);
                FIRMASENCRIPTADAS? firmaEncriptadaRecord = await _context.FIRMASENCRIPTADAS
                    .FirstOrDefaultAsync(fe => fe.USR_ID == firma);

                if (firmaRecord == null || firmaEncriptadaRecord == null)
                {
                    throw new Exception("Firma no encontrada.");
                }

                string pathDatos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Datos");
                if (!Directory.Exists(pathDatos))
                {
                    Directory.CreateDirectory(pathDatos);
                }

                archivo = Path.Combine(pathDatos, $"{firma.Trim()}.bmp");

                //using (FileStream fileStream = new FileStream(archivo, FileMode.Create, FileAccess.Write))        // CREACION ARCHIVOS FISICOS EN LA CARPETA "DATOS"
                //{
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        rawData = firmaEncriptadaRecord.FIR_FIRMA_ENCRIPTADA;
                        long rawLength = ((long)firmaRecord.FIR_LEN_FIRMA);

                        if (rawLength == -1) //'VarKey = "SISTEMAS-BANSUD"
                        {
                            while (offset < rawData.Length)
                            {
                                int length = Math.Min(chunkSize, rawData.Length - (int)offset);
                                byte[] chunk = new byte[length];
                                Array.Copy(rawData, offset, chunk, 0, length);

                                for (int i = 0; i < chunk.Length; i++)
                                {
                                    char keyChar = varKey[i % varKey.Length];
                                    chunk[i] ^= (byte)keyChar;
                                }

                                await memoryStream.WriteAsync(chunk, 0, chunk.Length);
                                offset += length;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < rawData.Length; i += chunkSize)
                            {
                                int length = Math.Min(chunkSize, rawData.Length - i);
                                byte[] chunk = new byte[length];
                                Array.Copy(rawData, i, chunk, 0, length);

                                for (int j = 0; j < chunk.Length; j++)
                                {
                                    char keyChar = varKey[(j + 1) % varKey.Length];
                                    chunk[j] ^= (byte)keyChar;
                                }
                                await memoryStream.WriteAsync(chunk, 0, chunk.Length);
                            }
                        }
                        rawDataDesincriptada = memoryStream.ToArray();
                    }
                    //await fileStream.WriteAsync(rawDataDesincriptada, 0, rawDataDesincriptada.Length);
                //}
                ////Eliminar el archivo después de haberlo procesado
                //if (File.Exists(archivo))
                //{
                //    File.Delete(archivo);
                //}

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = Convert.ToBase64String(rawDataDesincriptada);  

            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                result.Content = "False";
                _logger.LogError($"Error en VerFirma - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }
            return result;
        }
        public async Task<ServicesResult> EliminarFirma(string USR_ID)
        {
            _logger.LogInformation($"Eliminar Firma ({USR_ID})");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            try
            {
                CUENTAFIRMA? cuentaFirma = await _context.CUENTAFIRMA.FirstOrDefaultAsync(cf => cf.USR_ID == USR_ID.ToUpper());
                if (cuentaFirma == null)
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Content = JsonConvert.SerializeObject(false);
                    result.Message = $"No se encontró la cuenta de firma para el usuario {USR_ID}.";
                    return result;
                }
                else
                {
                    _context.CUENTAFIRMA.Remove(cuentaFirma);
                }

                FIRMAS? firma = await _context.FIRMAS.FirstOrDefaultAsync(f => f.USR_ID == USR_ID);
                if (firma != null)
                {
                    _context.FIRMAS.Remove(firma);
                }

                FIRMASENCRIPTADAS? firmaEncriptada = await _context.FIRMASENCRIPTADAS.FirstOrDefaultAsync(fe => fe.USR_ID == USR_ID);
                if (firmaEncriptada != null)
                {
                    _context.FIRMASENCRIPTADAS.Remove(firmaEncriptada);
                }

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.Firmas_Eliminar, Globals.user, $"{USR_ID.ToUpper()}", " ");
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(true);
                result.Message = HttpStatusCode.OK.ToString();
                return result;
            }
            catch (Exception ex)
            {
                    _logger.LogError($"Error en EliminarFirma - Origen:  - " +
                    $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                    $"{ex.InnerException.ToString() ?? string.Empty}");

                    await transaction.RollbackAsync();
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error al ejecutar la consulta. {ex.Message}";
                result.Content = "False";
            }
             return result;
        }
        public async Task<ServicesResult> EliminarFirmaCuenta(decimal cuenta, string usr)
        {
            _logger.LogInformation($"Eliminar Firma por Cuenta ({cuenta}, {usr})");

            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {

                        CUENTAFIRMA? cuentaFirma = await _context.CUENTAFIRMA
                            .FirstOrDefaultAsync(cf => cf.USR_ID == usr.ToUpper() && cf.CTA_SERVICIO == cuenta);

                        if (cuentaFirma == null)
                        {
                            throw new Exception($"No se encontró la cuenta de firma {cuenta} para el usuario {usr}.");
                        }
                     
                            _context.CUENTAFIRMA.Remove(cuentaFirma);

                            var ok = await _seguridadService.InsertarLog((int)CodigosTareas.FirmantesPorCuenta_Eliminar, Globals.user, " ", $"Cuenta= {cuentaFirma.CTA_SERVICIO}, Usuario= {usr.ToUpper()}");
                            if (ok.Content == null)
                                throw new Exception("Error al insertar log.");

                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();
                            result.Code = ((int)HttpStatusCode.OK).ToString();
                            result.Content = JsonConvert.SerializeObject(true);
                            result.Message = HttpStatusCode.OK.ToString();
                            return result;

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error en EliminarFirmaCuenta - Origen:  - " +
                        $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                        $"{ex.InnerException.ToString() ?? string.Empty}");
                        await transaction.RollbackAsync();
                        throw new Exception("Error durante la eliminación de la cuenta de firma", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                result.Content = "False";
                _logger.LogError($"Error - Usuario: {Globals.user} - FirmasService - EliminarFirmaCuenta: ");
                return result;
            }
        }
        public async Task<ServicesResult> ModificarCuentaFirma(CUENTAFIRMA cuentaFirmaCambiado)
        {
            _logger.LogInformation($"Modificar Cuenta Firma ({cuentaFirmaCambiado})");

            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {

                        CUENTAFIRMA? cuentaFirma = await _context.CUENTAFIRMA
                            .FirstOrDefaultAsync(cf => cf.CTA_SERVICIO == cuentaFirmaCambiado.CTA_SERVICIO && cf.USR_ID == cuentaFirmaCambiado.USR_ID);

                        if (cuentaFirma == null)
                        {
                            throw new Exception($"No se encontró la cuenta de firma {cuentaFirmaCambiado.CTA_SERVICIO} para el usuario {cuentaFirmaCambiado.USR_ID}.");
                        } 
 
                            cuentaFirma.CFR_COEFICIENTEPARTICIPACION = cuentaFirmaCambiado.CFR_COEFICIENTEPARTICIPACION;
                            cuentaFirma.CFR_MAR_AUTREQ = cuentaFirmaCambiado.CFR_MAR_AUTREQ;


                        var ok = await _seguridadService.InsertarLog((int)CodigosTareas.FirmantesPorCuenta_Modificar, Globals.user, $"Cuenta= {cuentaFirma.CTA_SERVICIO}, Usuario= {cuentaFirma.USR_ID}", $"Cuenta= {cuentaFirma.CTA_SERVICIO}, Usuario= {cuentaFirma.USR_ID}");
                        if (ok.Content == null)
                            throw new Exception("Error al insertar log.");

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        result.Code = ((int)HttpStatusCode.OK).ToString();
                        result.Content = JsonConvert.SerializeObject(true);
                        result.Message = HttpStatusCode.OK.ToString();
                        return result;
                          
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Error durante la modificación de la cuenta de firma", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                result.Content = "False";
                _logger.LogError($"Error en ModificarCuentaFirma - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                return result;
            }
        }
        public async Task<ServicesResult> Cuentas(int CTA_SERVICIO)
        {
            _logger.LogInformation($"Consultando Cuentas ({CTA_SERVICIO})");

            try
            {

                List<CuentafirmaDto>? cuentas = await _context.CUENTAFIRMA
                    .Where(cf => cf.CTA_SERVICIO == CTA_SERVICIO)
                    .Select(cf => new CuentafirmaDto
                    {
                        CTA_SERVICIO = cf.CTA_SERVICIO,
                        CFR_COEFICIENTEPARTICIPACION = cf.CFR_COEFICIENTEPARTICIPACION
                    })
                    .ToListAsync();

                List<RecordSetDto> recordset = new List<RecordSetDto>();
                foreach (var cuenta in cuentas)
                {
                    recordset.Add(new RecordSetDto
                    {
                        CTA_SERVICIO = cuenta.CTA_SERVICIO,
                        CFR_COEFICIENTEPARTICIPACION = cuenta.CFR_COEFICIENTEPARTICIPACION
                    });
                }

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(recordset);


            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                result.Content = "False";
                _logger.LogError($"Error en Cuentas - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }
            return result;
        }
        public async Task<ServicesResult> FirmasPorCuenta(decimal cuenta)
        {
            _logger.LogInformation($"Consultando Firmas Por Cuenta ({cuenta})");

            try
            {
                List<FirmasDto> firmas = await (from f in _context.FIRMAS
                                                join cf in _context.CUENTAFIRMA on f.USR_ID equals cf.USR_ID
                                                where cf.CTA_SERVICIO == cuenta
                                                orderby f.USR_ID
                                                select new FirmasDto
                                                {
                                                    USR_ID = f.USR_ID,
                                                    FIR_ESTADO = f.FIR_ESTADO,
                                                    FIR_FIRMA = f.FIR_FIRMA,
                                                    FIR_LEN_FIRMA = f.FIR_LEN_FIRMA,
                                                    CFR_COEFICIENTEPARTICIPACION = cf.CFR_COEFICIENTEPARTICIPACION
                                                }).ToListAsync();

                List<RecordSetDto> recordset = new List<RecordSetDto>();
                foreach (var firma in firmas)
                {

                    recordset.Add(new RecordSetDto
                    {
                        CTA_SERVICIO = firma.CTA_SERVICIO,
                        CFR_COEFICIENTEPARTICIPACION = firma.CFR_COEFICIENTEPARTICIPACION
                    });
                }

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(recordset);

            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                result.Content = string.Empty;
                _logger.LogError($"Error en FirmasPorCuenta - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }
            return result;
        }
        public async Task<ServicesResult> ModificarFirma(FIRMAS_nueva firmas)
        {
            _logger.LogInformation($"Modificar Firma ({firmas})");

            try
            {
                ServicesResult ok = await NuevaFirma(firmas);
                var ok2 = await _seguridadService.InsertarLog((int)CodigosTareas.Firmas_Modificar, Globals.user, $"{firmas.USR_ID.ToUpper()}", $"{firmas.USR_ID.ToUpper()}");


                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = ok.ToString();
            }
            catch (Exception ex)
            {
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";

                result.Content = string.Empty;
                _logger.LogError($"Error en ModificarFirma - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
            }
            return result;
        }
        public async Task<ServicesResult> ExistenFirmantesVacios(int tipo, decimal numero)
        {
            _logger.LogInformation($"Consultando si Existen Firmantes Vacios ({tipo}, {numero})");

            try
            {
                _logger.LogInformation($"Usuario: {Globals.user} - FirmasService - ExistenFirmantesVacios: ");

                List<string> firmantesVacios = await (
                    from firma in _context.FIRMAS
                    where (firma.FIR_LEN_FIRMA == 358 || firma.FIR_LEN_FIRMA == 74934) &&
                          (from usuario in _context.USUARIOS
                           where usuario.TDD_CLT_ID == tipo &&
                                 usuario.CLT_NUMDOC == numero &&
                                 (usuario.PRF_ID == 1 || usuario.PRF_ID == 8) &&
                                 usuario.USR_MAR_BAJA == 0
                           select usuario.USR_ID).Contains(firma.USR_ID) &&
                          (from cliente in _context.CLIENTEMODALIDADPAGO
                           where cliente.CLT_NUMDOCCLI == numero &&
                                 cliente.TDD_CLT_ID == tipo &&
                                 (cliente.MPG_ID == 1 || cliente.MPG_ID == 3 || cliente.MPG_ID == 7 || cliente.MPG_ID == 9 ||
                                  cliente.MPG_ID == 10 || cliente.MPG_ID == 14 || cliente.MPG_ID == 15 || cliente.MPG_ID == 16 ||
                                  cliente.MPG_ID == 17)
                           select cliente).Any()
                    select firma.USR_ID
                ).ToListAsync();

                result.Content = JsonConvert.SerializeObject(firmantesVacios);
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ExistenFirmantesVacios - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                result.Content = null;
            }

            return result;
        }
        private List<FIRMAS_nueva> setFirma(List<FIRMAS_nueva> firmas)
        {
            foreach (var x in firmas)
            {
                x.FIRMA = "Firma";               
            }
            return firmas;
        }
        public async Task<ServicesResult> NuevaCuentaFirma(CUENTAFIRMA cuentaFirma)
        {
            _logger.LogInformation($"Nueva Cuenta Firma ({cuentaFirma})"); 

            using (var transaction = await _context.Database.BeginTransactionAsync())
                try
                {
                    string usr = cuentaFirma.USR_ID;

                    CUENTAFIRMA cf = _mapper.Map<CUENTAFIRMA>(cuentaFirma);
                    _context.CUENTAFIRMA.Add(cf);
                                      
                    var ok = await _seguridadService.InsertarLog((int)CodigosTareas.FirmantesPorCuenta_Agregar, Globals.user, " ", $"Cuenta= {cuentaFirma.CTA_SERVICIO}, Usuario= {usr}");
                    if (ok.Content == null)
                        throw new Exception("Error al insertar log.");

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = JsonConvert.SerializeObject(true);
                    result.Message = HttpStatusCode.OK.ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error en NuevaCuentaFirma - Origen:  - " +
                    $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                    $"{ex.InnerException.ToString() ?? string.Empty}");
                    await transaction.RollbackAsync();
                    result.Code = ex.HResult.ToString();
                    result.Message = $"Ha ocurrido un error al ejecutar la consulta. {ex.Message}";
                    result.Content = "False";
                }

            return result;
        }




    }
}

   
