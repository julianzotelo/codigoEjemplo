using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.dominio.DataTransferObjects;
using pp3.services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.Logging;
using System.Net;
using pp3.services.Handler;

namespace pp3.services.Services
{
    public class RangosService : IRangosRepository
    {
        private readonly Pp3roContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        ServicesResult result = new ServicesResult();
        private readonly ILogger<RangosService> _logger;
        

        public RangosService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, ILogger<RangosService> logger, IMapper mapper)
        {
            this.context = context;

            this._httpContextAccessor = httpContextAccessor;

            this._logger = logger;

            this._mapper = mapper;
        }
        public async Task<ServicesResult> Consulta(decimal Tdd_Clt_Id, decimal Clt_Numdoccli, decimal Mpg_id, string? Est_id)
        {
            _logger.LogInformation($"Consultando Rango ({Tdd_Clt_Id}, {Clt_Numdoccli}, {Mpg_id}, {Est_id})");

            try
            {
                List<RangosDto> listaRangos = await (from rangos in context.RANGOS
                            join historialRangos in context.HISTORIAL_DE_RANGOS
                            on rangos.RAN_ID equals historialRangos.RAN_ID
                            where rangos.TDD_CLT_ID == Tdd_Clt_Id &&
                                  rangos.CLT_NUMDOCCLI == Clt_Numdoccli &&
                                  rangos.MPG_ID == Mpg_id
                            select new RangosDto {

                                RAN_ID = rangos.RAN_ID,
                                TDD_CLT_ID = rangos.TDD_CLT_ID,
                                CLT_NUMDOCCLI = rangos.CLT_NUMDOCCLI,
                                CTA_SERVICIO = rangos.CTA_SERVICIO,
                                MPG_ID = rangos.MPG_ID,
                                RAN_DESDE = rangos.RAN_DESDE,
                                RAN_HASTA = rangos.RAN_HASTA,
                                RAN_ACTUAL = rangos.RAN_ACTUAL,
                                EST_ID = rangos.EST_ID,
                                HRA_FEC = historialRangos.HRA_FEC,

                            })
                            .ToListAsync();

                if (!string.IsNullOrEmpty(Est_id))
                {
                    listaRangos = listaRangos.Where(q => q.EST_ID == Est_id).ToList();
                }

                listaRangos = listaRangos.OrderBy(q => q.RAN_ID).ToList();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(listaRangos);
                result.Message= HttpStatusCode.OK.ToString();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Consulta - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> SeleccionoClienteConRP(decimal Tdd_Clt_Id, decimal Clt_Numdoccli)
        {
            _logger.LogInformation($"Consultando Selecciono Cliente Con RP ({Tdd_Clt_Id}, {Clt_Numdoccli})");

            try
            {
                List<Clientes> listaClientes = await context.CLIENTES
                .Where(cliente => cliente.TDD_CLT_ID == Tdd_Clt_Id && cliente.CLT_NUMDOC == Clt_Numdoccli)
                .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(listaClientes);
                result.Message= HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en SeleccionoClienteConRP - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> SeleccionoReporteRango(decimal? Tdd_Clt_Id, decimal? Clt_Numdoccli)
        {
            _logger.LogInformation($"Consultando Selecciono Reporte Rango ({Tdd_Clt_Id}, {Clt_Numdoccli})");
            try
            {
                if (Tdd_Clt_Id != null && Clt_Numdoccli != null)
                {
                    var query = await (from r in context.RANGOS
                                       join c in context.CLIENTES on r.TDD_CLT_ID equals c.TDD_CLT_ID
                                       join m in context.MODALIDADPAGO on r.MPG_ID equals m.MPG_ID
                                       where r.EST_ID == "RP"
                                             && c.TDD_CLT_ID == Tdd_Clt_Id
                                             && c.CLT_NUMDOC == Clt_Numdoccli
                                             && r.CLT_NUMDOCCLI == Clt_Numdoccli
                                       orderby r.RAN_ID ascending
                                       select new RangosDto
                                       {
                                           RAN_ID = r.RAN_ID,
                                           CTA_SERVICIO = r.CTA_SERVICIO,
                                           RAN_DESDE = r.RAN_DESDE,
                                           RAN_HASTA = r.RAN_HASTA,
                                           CLT_RAZONSOC = c.CLT_RAZONSOC,
                                           MPG_DESCRIPCION = m.MPG_DESCRIPCION
                                       })
                                 .ToListAsync();

                    result.Content = JsonConvert.SerializeObject(query);
                }
                else
                {
                    List<decimal> valores = new List<decimal> { 1, 3, 6, 8, 9, 10, 14, 15, 16, 17 };

                    List<RangosDto> listaRangos = await (from rango in context.RANGOS
                                       join cliente in context.CLIENTES on rango.TDD_CLT_ID equals cliente.TDD_CLT_ID
                                       join modalidad in context.MODALIDADPAGO on rango.MPG_ID equals modalidad.MPG_ID
                                       where rango.EST_ID == "RP"
                                       && rango.TDD_CLT_ID == cliente.TDD_CLT_ID
                                       && rango.CLT_NUMDOCCLI == cliente.CLT_NUMDOC
                                       && valores.Contains(rango.MPG_ID)
                                       orderby rango.RAN_ID ascending
                                       select new RangosDto
                                       {
                                           RAN_ID = rango.RAN_ID,
                                           CTA_SERVICIO = rango.CTA_SERVICIO,
                                           RAN_DESDE = rango.RAN_DESDE,
                                           RAN_HASTA = rango.RAN_HASTA,
                                           CLT_RAZONSOC = cliente.CLT_RAZONSOC,
                                           MPG_DESCRIPCION = modalidad.MPG_DESCRIPCION
                                       })
                                .ToListAsync();

                    result.Content = JsonConvert.SerializeObject(listaRangos);
                }

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message= HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en SeleccionoReporteRango - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> ConsultaCliRangos(int rango_id)
        {
            _logger.LogInformation($"Consultando Clientes por Rangos ({rango_id})");
            try
            {
                var query = await (from rangos in context.RANGOS
                                   join clientes in context.CLIENTES on rangos.CLT_NUMDOCCLI equals clientes.CLT_NUMDOC
                                   join modalidadPagos in context.MODALIDADPAGO on rangos.MPG_ID equals modalidadPagos.MPG_ID
                                   join tipoDocumentos in context.TIPODOC on rangos.TDD_CLT_ID equals tipoDocumentos.TDD_ID
                                   join estados in context.ESTADOS on rangos.EST_ID equals estados.EST_ID
                                   where rangos.RAN_ID == rango_id
                                   select new RangosDto
                                   {
                                       EST_DESCRIPCION = estados.EST_DESCRIPCION,
                                       CLT_RAZONSOC = clientes.CLT_RAZONSOC,
                                       CLT_NUMDOCCLI = clientes.CLT_NUMDOC,
                                       MPG_DESCRIPCION = modalidadPagos.MPG_DESCRIPCION,
                                       TDD_DESCRIPCIONABREV = tipoDocumentos.TDD_DESCRIPCIONABREV
                                   })
                                  .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message= HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConsultaCliRangos - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> ConfirmarRango(int rango_id)
        {
            _logger.LogInformation($"Confirmar Rango ({rango_id})");
            // Iniciar la transacción
            var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var rango = await context.RANGOS.FirstOrDefaultAsync(rango => rango.RAN_ID == rango_id);

                if (rango != null)
                {

                    rango.EST_ID = "RC";
                    context.RANGOS.Update(rango);
                    await context.SaveChangesAsync();

                    HistorialDeRangos historial = new HistorialDeRangos
                    {
                        RAN_ID = rango_id,
                        EST_ID = "RC",
                        HRA_FEC = DateTime.Now
                    };
                    context.HISTORIAL_DE_RANGOS.Add(historial);
                    await context.SaveChangesAsync();

                    // Confirmar la transacción
                    await transaction.CommitAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Message= HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No se encontró el rango.";
                }
            }
            catch (Exception ex)
            {
                
                await transaction.RollbackAsync();

                _logger.LogError($"Error en ConfirmarRango - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
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
        public async Task<ServicesResult> ConsultaRangoCheques(decimal? Tdd_Clt_Id, decimal? Clt_Numdoccli, decimal? Mpg_id, string? Est_id)
        {
            _logger.LogInformation($"Consultando Rangos Cheques ({Tdd_Clt_Id}, {Clt_Numdoccli}, {Mpg_id}, {Est_id})");

            try
            {
                List<RangosDto>? listaRangoCheques = await (from rangos in context.RANGOS
                            join historialRangos in context.HISTORIAL_DE_RANGOS
                            on new { rangos.RAN_ID, rangos.EST_ID } equals new { historialRangos.RAN_ID, historialRangos.EST_ID }
                            join clientes in context.CLIENTES on rangos.CLT_NUMDOCCLI equals clientes.CLT_NUMDOC
                            join modalidadPagos in context.MODALIDADPAGO on rangos.MPG_ID equals modalidadPagos.MPG_ID
                            join tipoDocumentos in context.TIPODOC on rangos.TDD_CLT_ID equals tipoDocumentos.TDD_ID
                            join estados in context.ESTADOS on rangos.EST_ID equals estados.EST_ID
                            select new RangosDto 
                            {
                                RAN_ID = rangos.RAN_ID,
                                TDD_CLT_ID = rangos.TDD_CLT_ID,
                                CLT_NUMDOCCLI = rangos.CLT_NUMDOCCLI,
                                CTA_SERVICIO = rangos.CTA_SERVICIO,
                                MPG_ID = rangos.MPG_ID,
                                RAN_DESDE = rangos.RAN_DESDE,
                                RAN_HASTA = rangos.RAN_HASTA,
                                RAN_ACTUAL = rangos.RAN_ACTUAL,
                                EST_ID = rangos.EST_ID,
                                HRA_FEC = historialRangos.HRA_FEC,
                                CLT_RAZONSOC = clientes.CLT_RAZONSOC,
                                MPG_DESCRIPCION = modalidadPagos.MPG_DESCRIPCION,
                                TDD_DESCRIPCIONABREV = tipoDocumentos.TDD_DESCRIPCIONABREV,
                                EST_DESCRIPCION = estados.EST_DESCRIPCION
                            })
                            .ToListAsync();

                if (Tdd_Clt_Id.HasValue && Tdd_Clt_Id > 0 && Clt_Numdoccli.HasValue && Clt_Numdoccli > 0)
                {
                    listaRangoCheques = listaRangoCheques.Where(q => q.TDD_CLT_ID == Tdd_Clt_Id && q.CLT_NUMDOCCLI == Clt_Numdoccli).ToList();
                }
                if (Mpg_id.HasValue && Mpg_id > 0)
                {
                    listaRangoCheques = listaRangoCheques.Where(q => q.MPG_ID == Mpg_id).ToList();
                }
                if (!string.IsNullOrEmpty(Est_id))
                {
                    listaRangoCheques = listaRangoCheques.Where(q => q.EST_ID == Est_id.ToUpper()).ToList();
                }

                listaRangoCheques = listaRangoCheques
                    .OrderBy(q => q.TDD_CLT_ID)     //En el caso de Imprimir todos clientes se los agrupa primero por Clientes
                    .ThenBy(q => q.CLT_NUMDOCCLI)   //En el caso de Imprimir todos clientes se los agrupa primero por Clientes
                    .ThenBy(q => q.MPG_ID)
                    .ThenBy(q => q.EST_ID)
                    .ThenBy(q => q.RAN_ID)
                    .ToList();


                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content =  JsonConvert.SerializeObject(setIcon(listaRangoCheques));
                result.Message= HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConsultaRangoCheques - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        private List<RangosDto> setIcon(List<RangosDto> rangos)
        {
            foreach (var x in rangos)
            {
                if (x.EST_ID == "RE")
                {
                    x.Icono = "Expirado";
                }
                if (x.EST_ID == "RC")
                {
                    x.Icono = "Confirmado";
                }
                if (x.EST_ID == "RP")
                {
                    x.Icono = "Pendiente";
                }
            }

            return rangos;
        }
    }
}
