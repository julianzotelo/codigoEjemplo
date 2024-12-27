using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static pp3.dominio.Models.Globals;

namespace pp3.services.Services
{
    public class RelSucursalModPagoService: IRelSucursalModPagoRepository
    {
        private readonly Pp3roContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SeguridadService _seguridadService;
        private readonly ServicesResult result = new ServicesResult();
        private readonly ILogger<RelSucursalModPagoService> _logger;
        private readonly IMapper _mapper;

        public RelSucursalModPagoService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, SeguridadService seguridadService, ILogger<RelSucursalModPagoService> logger, IMapper mapper)
        {
            this._context = context;
            this._seguridadService = seguridadService;
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
            this._mapper = mapper;
        }

        public async Task<ServicesResult> ConsultaRelSucursalModPago(decimal sucursalId)
        {
            _logger.LogInformation($"Consulta Relacion Sucursal Modalidad Pago ({sucursalId})");

            try
            {
                var query = await (from relSucursalModPago in _context.REL_SUCURSAL_MODPAGO
                                   join modalidadPago in _context.MODALIDADPAGO
                                       on new { relSucursalModPago.MPG_ID }
                                       equals new { modalidadPago.MPG_ID } into modPago
                                   from mp in modPago.DefaultIfEmpty()
                                   where relSucursalModPago.SUC_ID == sucursalId
                                   orderby relSucursalModPago.MPG_ID
                                   select new 
                                   {
                                       MPG_ID = relSucursalModPago.MPG_ID,
                                       MPG_DESCRIPCION = mp != null ? mp.MPG_DESCRIPCION : null,
                                       SUC_ID = relSucursalModPago.SUC_ID
                                   }).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConsultaRelSucursalModPago - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> AltaRelSucursalModPago(RelSucursalModpago relSucursalModpago)
        {
            _logger.LogInformation($"Alta Relacion Sucursal Modalidad Pago ({relSucursalModpago})");

            // Iniciar la transacción
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.REL_SUCURSAL_MODPAGO.AddAsync(relSucursalModpago);
                await _context.SaveChangesAsync();

                result.Content = "true";
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.A_RelSucursalModalidadPago, Globals.user, "ALTA SUC: " + relSucursalModpago.SUC_ID + "  - MPG: " + relSucursalModpago.MPG_ID, "ALTA SUC: " + relSucursalModpago.SUC_ID + "  - MPG: " + relSucursalModpago.MPG_ID);
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await _context.SaveChangesAsync();
                // Confirmar la transacción
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en AltaRelSucursalModPago - Origen:  - " +
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
                await _context.DisposeAsync();
            }

            return result;
        }
        public async Task<ServicesResult> BajaRelSucursalModPago(decimal sucursalId, decimal modalidadPagoId)
        {
            _logger.LogInformation($"Baja Relacion Sucursal Modalidad Pago ({sucursalId}, {modalidadPagoId})");

            // Iniciar la transacción
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var relSucModPago = await _context.REL_SUCURSAL_MODPAGO
                            .FirstOrDefaultAsync(rsmp => rsmp.SUC_ID == sucursalId && rsmp.MPG_ID == modalidadPagoId);

                if (relSucModPago == null)
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Content = JsonConvert.SerializeObject(false);
                    result.Message = "No se encontro relacion sucursal modalidad pago";
                    return result;
                }

                _context.REL_SUCURSAL_MODPAGO.Remove(relSucModPago);

                result.Content = JsonConvert.SerializeObject(true);
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.B_RelSucursalModalidadPago, Globals.user, "BAJA SUC: " + sucursalId + "  - MPG: " + modalidadPagoId, "BAJA SUC: " + sucursalId + "  - MPG: " + modalidadPagoId);
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await _context.SaveChangesAsync();
                // Confirmar la transacción
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en BajaRelSucursalModPago - Origen:  - " +
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
                await _context.DisposeAsync();
            }

            return result;
        }
        public async Task<ServicesResult> ModalidadesPagoNoAsociadasSucursal(decimal sucursalId)
        {
            _logger.LogInformation($"Consulta Modalidades Pago No Asociadas Sucursal ({sucursalId})");

            try
            {
                var modalidadesPago = await _context.MODALIDADPAGO.Select(mp => new { mp.MPG_ID, mp.MPG_DESCRIPCION }).ToListAsync();

                var modalidadesPagoAsocSucursal = await _context.REL_SUCURSAL_MODPAGO
                    .Where(rsmp => rsmp.SUC_ID == sucursalId)
                    .Select(rsmp => rsmp.MPG_ID)
                    .ToListAsync();

                var modalidadNoAsociadas = modalidadesPago.Where(mp => !modalidadesPagoAsocSucursal.Contains(mp.MPG_ID)).ToList();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(modalidadNoAsociadas);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ModalidadesPagoNoAsociadasSucursal - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
    }
}
