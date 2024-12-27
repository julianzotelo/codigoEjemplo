using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pp3.dominio.Context;
using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using pp3.services.Handler;
using pp3.services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static pp3.dominio.Models.Globals;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace pp3.services.Services
{
    public class CuentasService : ICuentasRepository
    {
        private readonly Pp3roContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SeguridadService _seguridadService;
        private readonly IMapper _mapper;
        private readonly ILogger<CuentasService> _logger;
        private ServicesResult result = new ServicesResult();
        

        public CuentasService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<CuentasService> logger, SeguridadService seguridadService)
        {
            this.context = context;

            this._httpContextAccessor = httpContextAccessor;

            this._mapper = mapper;

            this._logger = logger;

            this._seguridadService = seguridadService;
        }
        public async Task<ServicesResult> Cuentas(decimal cuentaServicio)
        {
            _logger.LogInformation($"Consultando Cuentas ({cuentaServicio})");

            try
            {
                List<CUENTAS> listaCuentas = await (from cuentas in context.CUENTAS
                                                    join tipoDocumentos in context.TIPODOC on cuentas.TDD_CLT_ID equals tipoDocumentos.TDD_ID
                                                    where cuentas.CTA_SERVICIO == cuentaServicio && cuentas.TDD_CLT_ID == tipoDocumentos.TDD_ID
                                                    select cuentas)
                                   .ToListAsync();

                List<CuentasDto> cuentasDto = _mapper.Map<List<CuentasDto>>(listaCuentas);

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(cuentasDto);
                result.Message = HttpStatusCode.OK.ToString();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Cuentas - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> CuentasPorCliente(decimal tipoDocumento, decimal numDocumento)
        {
            _logger.LogInformation($"Consultando Cuentas Por Cliente ({tipoDocumento}, {numDocumento})");

            try
            {
                var listaCuentasPorCliente = await 
                                (from cuentas in context.CUENTAS
                                 join tipodoc in context.TIPODOC on cuentas.TDD_CLT_ID equals tipodoc.TDD_ID
                                 join sucursal in context.SUCURSALES on cuentas.SUC_CUENTACLIENTE equals sucursal.SUC_ID
                                 where cuentas.TDD_CLT_ID == tipoDocumento && cuentas.CLT_NUMDOC == numDocumento
                                 orderby cuentas.CTA_SERVICIO
                                 select new
                                 {
                                     CTA_SERVICIO = cuentas.CTA_SERVICIO.ToString(),
                                     SUC_CUENTASERVICIO = cuentas.SUC_CUENTASERVICIO,
                                     MND_CUENTASERVICIO = cuentas.MND_CUENTASERVICIO,
                                     TDC_CUENTASERVICIO = cuentas.TDC_CUENTASERVICIO,
                                     SUC_CUENTACLIENTE = cuentas.SUC_CUENTACLIENTE,
                                     MND_CUENTACLIENTE = cuentas.MND_CUENTACLIENTE,
                                     TDC_CUENTACLIENTE = cuentas.TDC_CUENTACLIENTE,
                                     CTA_CLIENTE = cuentas.CTA_CLIENTE.ToString(),
                                     TDD_CLT_ID = cuentas.TDD_CLT_ID,
                                     CLT_NUMDOC = cuentas.CLT_NUMDOC,
                                     CTA_RELACIONSIAF = cuentas.CTA_RELACIONSIAF,
                                     CTA_INACTIVA = cuentas.CTA_INACTIVA,
                                     CTA_CONVENIO = cuentas.CTA_CONVENIO,
                                     CTA_FECHAPERTURA = cuentas.CTA_FECHAPERTURA,
                                     CTA_MAR_FIRMAELBANCO = cuentas.CTA_MAR_FIRMAELBANCO,
                                     TIPO_CONVENIO = cuentas.TIPO_CONVENIO,
                                     CTA_COMISION = cuentas.CTA_COMISION,
                                     CTA_CONVENIO_BMA = cuentas.CTA_CONVENIO_BMA,
                                     TDD_DESCRIPCION = tipodoc.TDD_DESCRIPCION
                                 }).ToListAsync();

                result.Content = JsonConvert.SerializeObject(listaCuentasPorCliente);
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en CuentasPorCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> Cuenta(decimal cuentaServicio)
        {
            _logger.LogInformation($"Consultando Cuenta ({cuentaServicio})");

            try
            {
                var query = from c in context.CUENTAS
                            join t in context.TIPODOC on c.TDD_CLT_ID equals t.TDD_ID
                            where c.CTA_SERVICIO == cuentaServicio
                            select new
                            {
                                c.CTA_SERVICIO,
                                c.SUC_CUENTASERVICIO,
                                c.MND_CUENTASERVICIO,
                                c.TDC_CUENTASERVICIO,
                                c.SUC_CUENTACLIENTE,
                                c.MND_CUENTACLIENTE,
                                c.TDC_CUENTACLIENTE,
                                c.CTA_CLIENTE,
                                c.TDD_CLT_ID,
                                c.CLT_NUMDOC,
                                c.CTA_RELACIONSIAF,
                                c.CTA_INACTIVA,
                                c.CTA_CONVENIO,
                                c.CTA_FECHAPERTURA,
                                c.CTA_MAR_FIRMAELBANCO,
                                c.TIPO_CONVENIO,
                                c.CTA_COMISION,
                                c.CTA_CONVENIO_BMA,
                                t.TDD_DESCRIPCION 
                            };

                var cuentaExistente = await query.ToListAsync();

                if (cuentaExistente != null)
                {
                    result.Content = JsonConvert.SerializeObject(cuentaExistente);
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No se encontró la cuenta.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Cuenta - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException?.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> NuevaCuenta(CuentasDto cuentasDto)
        {
            _logger.LogInformation($"Nueva Cuenta ({cuentasDto})");

            var transaction = await context.Database.BeginTransactionAsync();

            try
            {

                CUENTAS cuenta = _mapper.Map<CUENTAS>(cuentasDto);
                //cuenta.
                context.CUENTAS.Add(cuenta);
                context.SaveChanges();
                await _seguridadService.InsertarLog((int)CodigosTareas.Cuentas_Agregar, Globals.user, $"", $"{cuentasDto.CTA_SERVICIO}");

                // Confirmar la transacción
                await transaction.CommitAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en NuevaCuenta - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
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




        public async Task<ServicesResult> ModificarCuenta(CuentasDto cuentasDto)
        {
            _logger.LogInformation($"Modificar Cuenta ({cuentasDto})");


            try
            {
                bool existe = await context.CUENTAS.AnyAsync(x => x.TDC_CUENTASERVICIO == cuentasDto.TDC_CUENTASERVICIO);

                if (existe)
                {
                    cuentasDto.MND_CUENTACLIENTE = 0;
                    cuentasDto.MND_CUENTASERVICIO = 0;
                    CUENTAS actualizar = _mapper.Map<CUENTAS>(cuentasDto);
                    context.CUENTAS.Update(actualizar);
                    context.SaveChanges();
                    await _seguridadService.InsertarLog((int)CodigosTareas.Cuentas_Modificar, Globals.user, $"{cuentasDto.CTA_SERVICIO}", $"{cuentasDto.CTA_SERVICIO}");


                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No se encontró la cuenta.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ModificarCuenta - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " 
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            finally
            {
   
                await context.DisposeAsync();
            }

            return result;
        }
        public async Task<ServicesResult> EliminarCuenta(string cuentaServicio)
        {
            _logger.LogInformation($"Eliminar Cuenta");

            var transaction = await context.Database.BeginTransactionAsync();
            ServicesResult resultado = new ServicesResult();
            try
            {
                CUENTAS? cuentaExistente = await context.CUENTAS.Where(x => x.CTA_SERVICIO.Equals(Convert.ToDecimal(cuentaServicio))).FirstOrDefaultAsync();

                if (cuentaExistente != null)
                {
                    cuentaExistente.CTA_INACTIVA = 1;

                    context.CUENTAS.Update(cuentaExistente);
                    context.SaveChanges();

                    var cuentaFirma = context.CUENTAFIRMA.Where(ctaFirma => ctaFirma.CTA_SERVICIO == Convert.ToDecimal(cuentaServicio));

                    context.CUENTAFIRMA.RemoveRange(cuentaFirma);
                    context.SaveChanges();
                    resultado = await _seguridadService.InsertarLog((int)CodigosTareas.Cuentas_Eliminar, Globals.user, $"{cuentaServicio}", $"{cuentaServicio}");
                    if (resultado.Content == "True")
                    {
                        var cuentaDelete = context.CUENTAS.Where(ctaDelete => ctaDelete.CTA_SERVICIO == Convert.ToDecimal(cuentaServicio));

                        context.CUENTAS.RemoveRange(cuentaDelete);
                        context.SaveChanges();

                    }

                    // Confirmar la transacción
                    await transaction.CommitAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Message = HttpStatusCode.OK.ToString();

                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No se encontró la cuenta.";
                }

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                _logger.LogError($"Error en EliminarCuenta - Origen:  - " +
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
        public async Task<ServicesResult> Firmantes(decimal cuenta)
        {
            _logger.LogInformation($"Consultando Firmantes");

            try
            {
                List<CUENTAFIRMA> listaCuentas = await (from cuentaFirma in context.CUENTAFIRMA
                                                        join usuarios in context.USUARIOS on cuentaFirma.USR_ID equals usuarios.USR_ID
                                                        where usuarios.PRF_ID == 1 && cuentaFirma.CTA_SERVICIO == cuenta
                                                        select cuentaFirma)
                                   .Distinct()
                                   .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(listaCuentas);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Firmantes - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> ExisteConvenioModif(decimal convenio, decimal cuenta)
        {
            _logger.LogInformation($"Consultando existe convenio");
            try
            {
                var existeConvenio = await context.CUENTAS
                      .AnyAsync(c => c.CTA_INACTIVA == 0 && c.CTA_CONVENIO == convenio && c.CTA_SERVICIO != cuenta);

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(existeConvenio);
                result.Message = HttpStatusCode.OK.ToString();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ExisteConvenioModif - Origen:  - " +
             $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
             $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }
    }
}