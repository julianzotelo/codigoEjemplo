using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using accusys.connector_keycloak.dominio.Manager;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Azure.Management.Network.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Repositories;
using static pp3.dominio.Models.Globals;

namespace pp3.services.Services
{
    public class ConvenioService : IConvenioRepository
    {
        private Pp3roContext _context;
        private ILogger<ConvenioService> _logger;
        private readonly SeguridadService _seguridadService;

        private ServicesResult result = new ServicesResult();
        public ConvenioService(Pp3roContext context, SeguridadService seguridadService, ILogger<ConvenioService> logger)
        {
            this._context = context;
            this._seguridadService = seguridadService;
            this._logger = logger;
        }

        public async Task<ServicesResult> AltaConvenio(TIPO_CONVENIO nuevoConvenio)
        {
            _logger.LogInformation($"Alta Convenio ({nuevoConvenio.TIPO_CONVENIO_ID}, {nuevoConvenio.DESC_CONVENIO})");
            try
            {
                bool existe = await _context.TIPO_CONVENIO
                    .Where(x => x.TIPO_CONVENIO_ID == nuevoConvenio.TIPO_CONVENIO_ID
                    || x.DESC_CONVENIO.ToUpper() == nuevoConvenio.DESC_CONVENIO.ToUpper())
                    .AnyAsync();

                if (existe)
                {
                    result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                    result.Message = "Ya existe un tipo de convenio con este código";

                    return result;
                }

                //Le asigno un ID auto-incremental ya que no es llave primaria en la tabla
                int idConvenio = await ObtenerMaximoTipoConvenioId();
                nuevoConvenio.TIPO_CONVENIO_ID = idConvenio + 1;

                await _context.TIPO_CONVENIO.AddAsync(nuevoConvenio);
                await _context.SaveChangesAsync();

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.A_RelTConvModalidadPago, Globals.user, "ALTA TCONV:" + nuevoConvenio.TIPO_CONVENIO_ID + " - DESC:" + nuevoConvenio.DESC_CONVENIO, "ALTA TCONV:" + nuevoConvenio.TIPO_CONVENIO_ID + " - DESC:" + nuevoConvenio.DESC_CONVENIO);
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await _context.SaveChangesAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = (HttpStatusCode.OK).ToString();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Convenio - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: ");
                LoggingManager.LogException(_logger, ex);

                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";

                return result;
            }
        }

        public async Task<ServicesResult> EliminarConvenio(int convenioId)
        {
            _logger.LogInformation($"Eliminar tipo de Convenio ({convenioId})");
            try
            {
                TIPO_CONVENIO? convenioAEliminar = await _context.TIPO_CONVENIO
                    .Where(x => x.TIPO_CONVENIO_ID == convenioId)
                    .FirstOrDefaultAsync();

                if(convenioAEliminar == null)
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No se encontró el convenio seleccionado.";

                    return result;
                }
                    _context.TIPO_CONVENIO.Remove(convenioAEliminar);
                    await _context.SaveChangesAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = (HttpStatusCode.OK).ToString();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Convenio - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");

                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";

                return result;
            };
        }

        public async Task<ServicesResult> ModificarConvenio(TIPO_CONVENIO convenioModif)
        {
            _logger.LogInformation($"Modificar tipo de Convenio: ({convenioModif.TIPO_CONVENIO_ID}, {convenioModif.DESC_CONVENIO})");
            try
            {
                TIPO_CONVENIO? convenioAModificar = await _context.TIPO_CONVENIO
                            .Where(x => x.TIPO_CONVENIO_ID == convenioModif.TIPO_CONVENIO_ID)
                            .FirstOrDefaultAsync();

                if(convenioAModificar == null)
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No se encontró el convenio seleccionado.";

                    return result;
                }

                //Remplazo la descripción del convenio ya que es lo único que se debería poder modificar
                convenioAModificar.DESC_CONVENIO = convenioModif.DESC_CONVENIO;

                _context.TIPO_CONVENIO.Update(convenioAModificar);
                await _context.SaveChangesAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = (HttpStatusCode.OK).ToString();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Convenio - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: ");
                LoggingManager.LogException(_logger, ex);

                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";

                return result;
            }
        }

        public async Task<ServicesResult> ObtenerModalidadesAsociadas(int convenioId)
        {
            _logger.LogInformation($"Obtener modalidades de Pago asociadas al convenio con Id: ({convenioId})");
            try
            {
                var resultado = await (from mp in _context.MODALIDADPAGO
                                      join rtm in _context.REL_TIPOCONVENIO_MODPAGO on mp.MPG_ID equals rtm.MPG_ID
                                      where rtm.TIPO_CONVENIO_ID == convenioId // Asume que tienes el valor en tipoConvenioId
                                      select mp).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK ).ToString();
                result.Message = (HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(resultado);

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Convenio - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: ");
                LoggingManager.LogException(_logger, ex);

                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";

                return result;
            }
        }

        public async Task<ServicesResult> ObtenerModalidadesNoAsociadas(int convenioId)
        {
            _logger.LogInformation($"Obtener modalidades de Pago NO asociadas al convenio con Id: ({convenioId})");

            try
            {
                var resultado = await _context.MODALIDADPAGO
                                .Where(mp => !_context.REL_TIPOCONVENIO_MODPAGO
                                .Any(rtm => rtm.MPG_ID == mp.MPG_ID && rtm.TIPO_CONVENIO_ID == convenioId))
                                .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = (HttpStatusCode.OK).ToString();
                result.Content= JsonConvert.SerializeObject(resultado);

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Convenio - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: ");
                LoggingManager.LogException(_logger, ex);

                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";

                return result;
            }
        }

        public async Task<ServicesResult> DesasociarModalidadPago(decimal convenioId, decimal modalidadId)
        {
            _logger.LogInformation($"Asociando Modalidad al Convenio Id: ({convenioId})");
            try
            {
                REL_TIPOCONVENIO_MODPAGO? relacion = await _context.REL_TIPOCONVENIO_MODPAGO
                                                    .Where(x => x.TIPO_CONVENIO_ID == convenioId
                                                    && x.MPG_ID == modalidadId)
                                                    .FirstOrDefaultAsync();

                if(relacion == null)
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No se encontró el relación seleccionada.";

                    return result;
                }

                _context.REL_TIPOCONVENIO_MODPAGO.Remove(relacion);
                await _context.SaveChangesAsync();

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.B_RelTConvModalidadPago, Globals.user, "BAJA TCONV:" + convenioId + " - MPG:" + modalidadId, "BAJA TCONV:" + convenioId + " - MPG:" + modalidadId);
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await _context.SaveChangesAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = (HttpStatusCode.OK).ToString();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Convenio - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: ");
                LoggingManager.LogException(_logger, ex);

                return result;
            }
        }

        public async Task<ServicesResult> AsociarModalidadPago(REL_TIPOCONVENIO_MODPAGO relConvenioModPago)
        {
            try
            {
                //REL_TIPOCONVENIO_MODPAGO? relacion = await _context.REL_TIPOCONVENIO_MODPAGO
                //                                    .Where(x => x.TIPO_CONVENIO_ID == relConvenioModPago.TIPO_CONVENIO_ID
                //                                    && x.MPG_ID == relConvenioModPago.MPG_ID)
                //                                    .FirstOrDefaultAsync();

                await _context.REL_TIPOCONVENIO_MODPAGO.AddAsync(relConvenioModPago);
                await _context.SaveChangesAsync();

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.A_RelTConvModalidadPago, Globals.user, "ALTA TCONV:" + relConvenioModPago.TIPO_CONVENIO_ID + " - MPG:" + relConvenioModPago.MPG_ID, "ALTA TCONV:" + relConvenioModPago.TIPO_CONVENIO_ID + " - MPG:" + relConvenioModPago.MPG_ID);
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await _context.SaveChangesAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = (HttpStatusCode.OK).ToString();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Convenio - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: ");
                LoggingManager.LogException(_logger, ex);

                return result;
            }
        }

        private async Task<int> ObtenerMaximoTipoConvenioId()
        {
            var maxTipoConvenioId = await _context.TIPO_CONVENIO.MaxAsync(tc => tc.TIPO_CONVENIO_ID);
            return maxTipoConvenioId;
        }

    }
}
