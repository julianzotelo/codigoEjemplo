using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Handler;
using pp3.services.Repositories;
using System.Net;
using static pp3.dominio.Models.Globals;

namespace pp3.services.Services
{
    public class ModalidadService : IModalidadRepository
    {
        private readonly Pp3roContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SeguridadService _seguridadService;
        private readonly ServicesResult result = new ServicesResult();
        private readonly ILogger<ModalidadService> _logger;
        private readonly IMapper _mapper;
        


        public ModalidadService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, SeguridadService seguridadService, ILogger<ModalidadService> logger, IMapper mapper)
        {
            this.context = context;
            this._seguridadService = seguridadService;
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
            this._mapper = mapper;
        }
        public async Task<ServicesResult> ModalidadesPorCliente(decimal Id, decimal NumDoc)
        {
            _logger.LogInformation($"Consultando Modalidades Por Cliente ({Id}, {NumDoc})");

            try
            {
                var query = await context.CLIENTEMODALIDADPAGO
                                 .Join(
                                      context.MODALIDADPAGO,
                                      clientModalidad => clientModalidad.MPG_ID,
                                      modalidad => modalidad.MPG_ID,
                                      (clientModalidad, modalidad) => new
                                      {
                                          clientModalidad.TDD_CLT_ID,
                                          clientModalidad.CLT_NUMDOCCLI,
                                          clientModalidad.CMP_PERIODOCOBRO,
                                          clientModalidad.CMP_MOMENTOPERIODOCOBRO,
                                          clientModalidad.CMP_MAR_EVENTOCOBRO,
                                          clientModalidad.CMP_MAR_ESCALA_X_IMPORTE,
                                          clientModalidad.CMP_FEC_ULTIMALIQUIDACION,
                                          clientModalidad.CMP_IMP_MONTOMAXIMO,
                                          clientModalidad.CMP_ESTIM_MENS_PAGOS,
                                          clientModalidad.CMP_DIAS_DEBITO,
                                          clientModalidad.MPG_ID,
                                          modalidad.MPG_DESCRIPCION
                                      }
                                  )
                                  .Where(cm => cm.TDD_CLT_ID == Id && cm.CLT_NUMDOCCLI == NumDoc)
                                  .OrderBy(cm => cm.MPG_ID)
                                  .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ModalidadesPorCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";

            }
            return result;
        }
        public async Task<ServicesResult> Modalidad(int tipo, decimal numero, int id)
        {
            _logger.LogInformation($"Consultando Modalidad ({tipo}, {numero}, {id})");

            try
            {

                Clientemodalidadpago? modalidad = await context.CLIENTEMODALIDADPAGO
                .FirstOrDefaultAsync(cmp => cmp.TDD_CLT_ID == tipo && cmp.CLT_NUMDOCCLI == numero && cmp.MPG_ID == id);

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(modalidad);
                result.Message = HttpStatusCode.OK.ToString();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Modalidad - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }
        public async Task<ServicesResult> NuevaModalidad(Clientemodalidadpago clientemodalidadpago)
        {
            _logger.LogInformation($"Nueva Modalidad ({clientemodalidadpago})");

            try
            {
                await context.CLIENTEMODALIDADPAGO.AddAsync(clientemodalidadpago);
                await context.SaveChangesAsync();
                result.Content = "true";
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();

                var retencionIds = (from r in context.RETENCIONES 
                                    join e in context.ENVIOS on r.ENV_ID equals e.ENV_ID
                                    where e.TDD_CLT_ID == clientemodalidadpago.TDD_CLT_ID && e.CLT_NUMDOC == clientemodalidadpago.CLT_NUMDOCCLI && r.COM_ID == 0
                                    select r.RETENCION_ID).ToList();

                // Actualiza las retenciones
                var retencionesToUpdate = context.RETENCIONES
                    .Where(r => retencionIds.Contains(r.RETENCION_ID));

                foreach (var retencion in retencionesToUpdate)
                {
                    retencion.COM_ID = null;
                }
                //    var ok = _securityService.InsertarLog(40, usuario.ToUpper(), " ", $"Cli y Mod={tipo} {numero} {id}");
                //    if (!ok)
                //    {
                //        throw new Exception("Error al insertar en el log.");
                //    }

                //    if (id == 99)
                //    {
                //        var sql = "UPDATE RETENCIONES SET COM_ID=NULL " +
                //                  "WHERE RETENCION_ID IN " +
                //                  "(SELECT R.RETENCION_ID FROM RETENCIONES R, ENVIOS E " +
                //                  "WHERE E.TDD_CLT_ID = @Tipo AND E.CLT_NUMDOC = @Numero AND E.ENV_ID = R.ENV_ID AND R.COM_ID = 0)";

                //        _context.Database.ExecuteSqlRaw(sql, new SqlParameter("@Tipo", tipo), new SqlParameter("@Numero", numero));
                //    }

                //    transaction.Commit();
                //    return true;
                await _seguridadService.InsertarLog((int)CodigosTareas.ModalidadesPago_Agregar, Globals.user, " ", "Cli y Mod=" + clientemodalidadpago.TDD_CLT_ID + " " + clientemodalidadpago.CLT_NUMDOCCLI + " " + clientemodalidadpago.MPG_ID);
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en NuevaModalidad - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}"); ;
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;

        }
        public async Task<ServicesResult> EliminarModalidad(int Tipo, decimal numero, int Id)
        {
            _logger.LogInformation($"EliminarModalidad ({Tipo}, {numero}, {Id})");

            // Iniciar la transacción
            var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Eliminar registros de ESCALASCOMISION
                var escalasComision = await context.ESCALASCOMISION
                    .Where(ec => ec.TDD_CLT_ID == Tipo && ec.CLT_NUMDOC == numero && ec.MPG_ID == Id)
                    .ToListAsync();

                context.ESCALASCOMISION.RemoveRange(escalasComision);

                await context.SaveChangesAsync();

                // Eliminar registros de ESCALASDESCUENTO
                var escalasDescuento = await context.ESCALASDESCUENTO
                    .Where(ed => ed.TDD_CLT_ID == Tipo && ed.CLT_NUMDOC == numero && ed.MPG_ID == Id)
                    .ToListAsync();

                context.ESCALASDESCUENTO.RemoveRange(escalasDescuento);

                await context.SaveChangesAsync();

                // Eliminar registros de CLIENTEMODALIDADPAGO
                var clienteModalidadPago = await context.CLIENTEMODALIDADPAGO
                    .Where(cmp => cmp.TDD_CLT_ID == Tipo && cmp.CLT_NUMDOCCLI == numero && cmp.MPG_ID == Id)
                    .ToListAsync();

                context.CLIENTEMODALIDADPAGO.RemoveRange(clienteModalidadPago);


                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.ModalidadesPago_Eliminar, Globals.user, "Cli y Mod=" + Tipo + " " + numero + " " + Id, " h");
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await context.SaveChangesAsync();

                // Confirmar la transacción
                await transaction.CommitAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = "true";
                result.Message = HttpStatusCode.OK.ToString();
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en EliminarModalidad - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                // Revertir transacción
                await transaction.RollbackAsync();        
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            finally
            {
                // Liberar recursos del contexto y transacción
                await transaction.DisposeAsync();
                await context.DisposeAsync();
            }

            return result;
        }
        public async Task<ServicesResult> ModificarModalidad(Clientemodalidadpago clientemodalidadpago)
        {
            _logger.LogInformation($"Modificar Modalidad ({clientemodalidadpago})");

            try
            {
                Clientemodalidadpago modalidad = await context.CLIENTEMODALIDADPAGO.FirstOrDefaultAsync(cmp => cmp.TDD_CLT_ID == clientemodalidadpago.TDD_CLT_ID && cmp.CLT_NUMDOCCLI == clientemodalidadpago.CLT_NUMDOCCLI && cmp.MPG_ID == clientemodalidadpago.MPG_ID);

                if (modalidad != null)
                {
                    modalidad.CMP_PERIODOCOBRO = clientemodalidadpago.CMP_PERIODOCOBRO;
                    modalidad.CMP_MOMENTOPERIODOCOBRO = clientemodalidadpago.CMP_MOMENTOPERIODOCOBRO;
                    modalidad.CMP_MAR_EVENTOCOBRO = clientemodalidadpago.CMP_MAR_EVENTOCOBRO;
                    modalidad.CMP_MAR_ESCALA_X_IMPORTE = clientemodalidadpago.CMP_MAR_ESCALA_X_IMPORTE;
                    modalidad.CMP_FEC_ULTIMALIQUIDACION = clientemodalidadpago.CMP_FEC_ULTIMALIQUIDACION;
                    modalidad.CMP_IMP_MONTOMAXIMO = clientemodalidadpago.CMP_IMP_MONTOMAXIMO;
                    modalidad.CMP_ESTIM_MENS_PAGOS = clientemodalidadpago.CMP_ESTIM_MENS_PAGOS;
                    modalidad.CMP_DIAS_DEBITO = clientemodalidadpago.CMP_DIAS_DEBITO;

                    context.SaveChanges();
                    await _seguridadService.InsertarLog((int)CodigosTareas.ModalidadesPago_Modificar, Globals.user, "Cli y Mod=" + clientemodalidadpago.TDD_CLT_ID + " " + clientemodalidadpago.CLT_NUMDOCCLI + " " + clientemodalidadpago.MPG_ID, "Cli y Mod=" + clientemodalidadpago.TDD_CLT_ID + " " + clientemodalidadpago.CLT_NUMDOCCLI + " " + clientemodalidadpago.MPG_ID);
                    
                    result.Content = JsonConvert.SerializeObject(modalidad);
                    result.Message = HttpStatusCode.OK.ToString();
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                }
                else
                {
                    result.Message = "Registro no encontrado";
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ModificarModalidad - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }
    }
}
