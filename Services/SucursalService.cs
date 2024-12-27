using accusys.connector_keycloak.dominio.Manager;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace pp3.services.Services
{
    public class SucursalService: ISucursalRepository
    {
        private readonly Pp3roContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SeguridadService _seguridadService;
        private readonly ServicesResult result = new ServicesResult();
        private readonly ILogger<SucursalService> _logger;
        private readonly IMapper _mapper;

        public SucursalService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, SeguridadService seguridadService, ILogger<SucursalService> logger, IMapper mapper)
        {
            this._context = context;
            this._seguridadService = seguridadService;
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
            this._mapper = mapper;
        }

        public Task<ServicesResult> AltaSucursal(Sucursales nuevaSucursal)
        {
            throw new NotImplementedException();
        }

        public async Task<ServicesResult> ConsultaSucursales(int provinciaId)
        {
            _logger.LogInformation($"Consulta Sucursales ({provinciaId})");

            try
            {
                var query = await (from sucursales in _context.SUCURSALES
                                   join codigosPostales in _context.CODIGOSPOSTALES on sucursales.CCP_ID equals codigosPostales.CCP_ID
                                   where codigosPostales.PRV_ID == provinciaId
                                   orderby sucursales.SUC_DESCRIPCION
                                   select new
                                   {
                                       SUC_ID = sucursales.SUC_ID,
                                       BCO_ID = sucursales.BCO_ID,
                                       CDC_ID = sucursales.CDC_ID,
                                       CCP_ID = sucursales.CCP_ID,
                                       SUC_DESCRIPCION = sucursales.SUC_DESCRIPCION,
                                       SUC_CALLE = sucursales.SUC_CALLE,
                                       SUC_UNIDADFUNCIONAL = sucursales.SUC_UNIDADFUNCIONAL,
                                       SUC_MAR_MIGRACION = sucursales.SUC_MAR_MIGRACION,
                                       SUC_MIGRADA = sucursales.SUC_MIGRADA,
                                       SUC_MAR_BAJA = sucursales.SUC_MAR_BAJA
                                   }).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConsultaSucursales - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: ");
                LoggingManager.LogException(_logger, ex);
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> EliminarSucursal(decimal sucursalId)
        {
            _logger.LogInformation($"Eliminando Sucursal con id: ({sucursalId})");
            try
            {
                Sucursales? sucursalEliminar = await _context.SUCURSALES
                    .Where(x => x.SUC_ID == sucursalId)
                    .FirstOrDefaultAsync();

                if(sucursalEliminar == null)
                {
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Message = $"No se encontró la sucursal con Id {sucursalId}";

                    return result;
                }

                _context.SUCURSALES.Remove(sucursalEliminar);
                await _context.SaveChangesAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConsultaSucursales - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: ");
                LoggingManager.LogException(_logger, ex);
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";

                return result;
            }
        }

        public async Task<ServicesResult> ModificarSucursal(Sucursales modSucursal)
        {
            _logger.LogInformation($"Modificando Sucursal: ({modSucursal.SUC_DESCRIPCION})");
            try
            {
                Sucursales? sucursalModif = await _context.SUCURSALES
                    .Where(x => x.SUC_ID == modSucursal.SUC_ID)
                    .FirstOrDefaultAsync();

                if (sucursalModif == null)
                {
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Message = $"No se encontró la sucursal";

                    return result;
                }

                _context.SUCURSALES.Remove(sucursalModif);
                await _context.SaveChangesAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConsultaSucursales - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: ");
                LoggingManager.LogException(_logger, ex);
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";

                return result;
            }
        }
    }
}
