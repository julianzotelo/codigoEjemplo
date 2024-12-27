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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace pp3.services.Services
{
    public class CodigoPostalService: ICodigoPostalRepository
    {
        private readonly Pp3roContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SeguridadService _seguridadService;
        private readonly ServicesResult result = new ServicesResult();
        private readonly ILogger<CodigoPostalService> _logger;
        private readonly IMapper _mapper;

        public CodigoPostalService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, SeguridadService seguridadService, ILogger<CodigoPostalService> logger, IMapper mapper)
        {
            this._context = context;
            this._seguridadService = seguridadService;
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
            this._mapper = mapper;
        }

        public async Task<ServicesResult> Traer(decimal pciaId)
        {
            _logger.LogInformation($"Consulta Codigos Postales por Provincia ({pciaId})");

            try
            {
                var query = await _context.CODIGOSPOSTALES
                    .Where(cp => cp.PRV_ID == pciaId)
                    .OrderBy(cp => cp.CCP_LOCALIDAD)
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Traer - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> CodigoPostal(decimal pCod)
        {
            _logger.LogInformation($"Consulta Codigos Postales por Id ({pCod})");

            try
            {
                var query = await _context.CODIGOSPOSTALES
                    .Where(cp => cp.CCP_ID == pCod)
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en CodigoPostal - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> NuevoCodigoPostal(Codigospostale codigoPostal)
        {
            _logger.LogInformation($"Alta Codigo Postal ({codigoPostal.CCP_ID}, {codigoPostal.PRV_ID}, {codigoPostal.CCP_LOCALIDAD})");

            // Iniciar la transacción
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.CODIGOSPOSTALES.AddAsync(codigoPostal);
                await _context.SaveChangesAsync();

                result.Content = "true";
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.AltaCodigoPostal, Globals.user, " ", "PRV_ID= " + codigoPostal.PRV_ID + " CCP_LOCALIDAD= " + codigoPostal.CCP_LOCALIDAD);
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await _context.SaveChangesAsync();
                // Confirmar la transacción
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en NuevoCodigoPostal - Origen:  - " +
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

        public async Task<ServicesResult> ModificarCodigoPostal(Codigospostale codigoPostal)
        {
            _logger.LogInformation($"Modificacion Codigo Postal ({codigoPostal.CCP_ID}, {codigoPostal.PRV_ID}, {codigoPostal.CCP_LOCALIDAD})");

            // Iniciar la transacción
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var codPostal = await _context.CODIGOSPOSTALES
                            .FirstOrDefaultAsync(cp => cp.CCP_ID == codigoPostal.CCP_ID);

                if (codPostal == null)
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Content = JsonConvert.SerializeObject(false);
                    result.Message = "No se encontro codigo postal";
                    return result;
                }

                codPostal.PRV_ID = codigoPostal.PRV_ID;
                codPostal.CCP_LOCALIDAD = codigoPostal.CCP_LOCALIDAD;

                _context.CODIGOSPOSTALES.Update(codPostal);

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.ModificacionCodigoPostal, Globals.user, "PRV_ID= " + codigoPostal.PRV_ID + " CCP_LOCALIDAD= " + codigoPostal.CCP_LOCALIDAD, "PRV_ID= " + codigoPostal.PRV_ID + " CCP_LOCALIDAD= " + codigoPostal.CCP_LOCALIDAD);
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await _context.SaveChangesAsync();
                // Confirmar la transacción
                await transaction.CommitAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(true);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ModificarCodigoPostal - Origen:  - " +
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

        public async Task<ServicesResult> EliminarCodigoPostal(decimal pCcpId)
        {
            _logger.LogInformation($"Baja Codigo Postal ({pCcpId})");

            // Iniciar la transacción
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var codPostal = await _context.CODIGOSPOSTALES
                            .FirstOrDefaultAsync(cp => cp.CCP_ID == pCcpId);

                if (codPostal == null)
                {
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = JsonConvert.SerializeObject(false);
                    result.Message = "No se encontró código postal";
                    return result;
                }

                var codPostalBenef = await _context.BENEFICIARIOS.FirstOrDefaultAsync(cpb => cpb.CCP_ID.Equals(pCcpId));
                if (codPostalBenef != null)
                {
                    result.Code = ((int)HttpStatusCode.OK) .ToString();
                    result.Content = JsonConvert.SerializeObject(false);
                    result.Message = "No se puede borrar el código postal por que existen beneficiarios cargados con este código";
                    return result;
                }

                _context.CODIGOSPOSTALES.Remove(codPostal);

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.BajaCodigoPostal, Globals.user, "CCP_ID= " + pCcpId, " ");
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await _context.SaveChangesAsync();
                // Confirmar la transacción
                await transaction.CommitAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(true);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en EliminarCodigoPostal - Origen:  - " +
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
    }
}
