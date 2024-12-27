using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pp3.dominio.Context;
using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using pp3.services.Handler;
using pp3.services.Repositories;
using System.Data;
using System.Net;
using System.Security.Cryptography;

namespace pp3.services.Services
{
    public class ConsultasService : IConsultasRepository
    {
        private readonly Pp3roContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ServicesResult result = new ServicesResult();
        private readonly ILogger<ConsultasService> _logger;
        private readonly IMapper _mapper;
        


        public ConsultasService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, ILogger<ConsultasService> logger, IMapper mapper)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
            this._mapper = mapper;
        }

        public async Task<ServicesResult> tipoDoc()
        {
            _logger.LogInformation($"Consulta TIpoDoc");

            try
            {
                var query = await _context.TIPODOC
                    .OrderBy(tipoDoc => tipoDoc.TDD_DESCRIPCION)
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString(); ;
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en tipoDoc - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> Tipo(int Id)
        {
            _logger.LogInformation($"Consulta por Tipo ({Id})");

            try
            {
                bool existe = await _context.TIPODOC.AnyAsync(tipoDoc => tipoDoc.TDD_ID == Id);

                if (existe)
                {
                    var query = await _context.TIPODOC
                        .Where(tipoDoc => tipoDoc.TDD_ID == Id)
                        .ToListAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = JsonConvert.SerializeObject(query);
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No existe el tipo de documento.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Tipo - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> Modalidades()
        {
            _logger.LogInformation($"Consulta por Modalidades");

            try
            {
                var query = await _context.MODALIDADPAGO
                    .OrderBy(modPago => modPago.MPG_ID)
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Modalidades - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> sucursales()
        {
            _logger.LogInformation($"Consultando Sucursales:");


            try
            {
                var query = await _context.SUCURSALES
                    .Where(suc => suc.SUC_MAR_MIGRACION == 0 && suc.SUC_MAR_BAJA == 0)
                    .OrderBy(suc => suc.SUC_DESCRIPCION)
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en sucursales - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> Sucursal(int Id)
        {
            _logger.LogInformation($"Consulta Sucursal ({Id}) ");

            try
            {
                bool existe = await _context.SUCURSALES.AnyAsync(suc => suc.SUC_ID == Id);

                if (existe)
                {
                    var query = await _context.SUCURSALES
                        .Where(suc => suc.SUC_ID == Id)
                        .ToListAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = JsonConvert.SerializeObject(query);
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No existe la sucursal.";
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error en Sucursal - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> IVAs()
        {
            _logger.LogInformation($"Consulta IVAs");

            try
            {
                var query = await _context.CONDICIONESIVA
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en IVAs - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> iva(int Id)
        {
            _logger.LogInformation($"Consulta iva ({Id})");

            try
            {
                bool existe = await _context.CONDICIONESIVA.AnyAsync(condIva => condIva.CNI_ID == Id);

                if (existe)
                {
                    var query = await _context.CONDICIONESIVA
                        .Where(condIva => condIva.CNI_ID == Id)
                        .ToListAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = JsonConvert.SerializeObject(query);
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No existe la condicion iva.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en iva - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> ExisteCodigoPostal(int cod)
        {
            _logger.LogInformation($"Consulta Existe Codigo Postal ({cod}) ");

            try
            {
                bool existe = await _context.CODIGOSPOSTALES.AnyAsync(codPostales => codPostales.CCP_ID == cod);

                if (existe)
                {
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = "True";
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = "False";
                    result.Message = HttpStatusCode.OK.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ExisteCodigoPostal - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> ExisteSucursal(int cod)
        {
            _logger.LogInformation($"Consulta Existe Sucursal ({cod})");

            try
            {
                bool existe = await _context.SUCURSALES.AnyAsync(suc => suc.SUC_ID == cod);

                if (existe)
                {
                    var query = await _context.SUCURSALES
                        .Where(suc => suc.SUC_ID == cod)
                        .ToListAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = JsonConvert.SerializeObject(query);
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No existe la sucursal.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ExisteSucursal - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> estados()
        {
            _logger.LogInformation($"Consulta estados");

            try
            {
                var query = await _context.ESTADOS
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en estados - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> EstadosVisibles()
        {
            _logger.LogInformation($"Consulta Estados Visibles");

            try
            {
                var query = await _context.ESTADOS
                    .Where(estados => estados.EST_MAR_VISIBLE == 1)
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en EstadosVisibles - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> ConIB()
        {
            _logger.LogInformation($"Consulta ConIB");

            try
            {
                var query = await _context.CONDICIONESIB
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConIB - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> ConIG()
        {
            _logger.LogInformation($"Consulta ConIG");

            try
            {
                var query = await _context.CONDICIONESIG
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConIG - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> Motivos()
        {
            _logger.LogInformation($"Consulta Motivos");

            try
            {
                var query = await _context.MOTIVOS
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Motivos - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> tipoCuenta(int idTipoCuenta)
        {
            _logger.LogInformation($"Consulta Tipo Cuenta ({idTipoCuenta})");

            try
            {
                var query = await _context.TIPOCUENTA.Where(id => id.TDC_ID == idTipoCuenta)

                    .OrderBy(tipoCuenta => tipoCuenta.TDC_DESCRIPCION)
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en tipoCuenta - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> TiposDeCuentas()
        {
            _logger.LogInformation($"Consulta Tipos de Cuentas");

            try
            {
                var query = await _context.TIPOCUENTA
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en TiposDeCuentas - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> TiposDeCuentasCobis()
        {
            _logger.LogInformation($"Consulta Tipos De Cuentas Cobis");

            try
            {
                var query = await _context.TIPOCUENTA
                    .Where(tipoCuentaCobis => tipoCuentaCobis.TDC_ID > 2)
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en TiposDeCuentasCobis - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> Monedas()
        {
            _logger.LogInformation($"Consulta Monedas");

            try
            {
                var query = await _context.MONEDAS
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Monedas - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> Provincias()
        {
            _logger.LogInformation($"Consulta provincias");

            try
            {
                var query = await _context.PROVINCIAS
                    .OrderBy(provincias => provincias.PRV_DESCRIPCION)
                    .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Provincias - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> TiposConvenio()
        {
            _logger.LogInformation($"Consulta Tipos Convenio");

            try
            {
                var query = await _context.TIPO_CONVENIO
                .OrderBy(tipoConvenio => tipoConvenio.DESC_CONVENIO).ToListAsync(); ;
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = "OK";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en TiposConvenio - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> TipoConvenio(int? Id)
        {
            _logger.LogInformation($"Consulta Tipo Convenio ({Id})");

            try
            {
                IQueryable<TIPO_CONVENIO> query;

                if (Id == 0 || Id == null)
                {
                    query = _context.TIPO_CONVENIO
                        .OrderBy(tipoConvenio => tipoConvenio.DESC_CONVENIO);

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = JsonConvert.SerializeObject(await query.ToListAsync());
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    bool existe = await _context.TIPO_CONVENIO.AnyAsync(tipoConvenio => tipoConvenio.TIPO_CONVENIO_ID == Id);

                    if (existe)
                    {
                        query = _context.TIPO_CONVENIO
                            .Where(tipoConvenio => tipoConvenio.TIPO_CONVENIO_ID == Id)
                            .OrderBy(tipoConvenio => tipoConvenio.DESC_CONVENIO);

                        result.Code = ((int)HttpStatusCode.OK).ToString();
                        result.Content = JsonConvert.SerializeObject(await query.ToListAsync());
                        result.Message = HttpStatusCode.OK.ToString();
                    }
                    else
                    {
                        result.Code = ((int)HttpStatusCode.NotFound).ToString();
                        result.Message = "No existe el tipo de convenio.";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en TipoConvenio - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> ConsultaAcreditacionesInterbanking(string? TipoDocCLT, string? NumDocCLT, string? MpgId, string? EstadoActual, string? OPGID, string? EnvId, string? FechaDesde, string? FechaHasta)
        {
            _logger.LogInformation($"Consulta Acreditaciones Interbanking ({TipoDocCLT}, {NumDocCLT}, {MpgId}, {EstadoActual}, {OPGID}, {EnvId}, {FechaDesde}, {FechaHasta})");

            try
            {
                var param0 = new SqlParameter("@TDD_CLT_ID", string.IsNullOrEmpty(TipoDocCLT) ? (object)DBNull.Value : TipoDocCLT);
                var param1 = new SqlParameter("@CLT_NUMDOC", string.IsNullOrEmpty(NumDocCLT) ? (object)DBNull.Value : NumDocCLT);
                var param2 = new SqlParameter("@MPG_ID", string.IsNullOrEmpty(MpgId) ? (object)DBNull.Value : MpgId);
                var param3 = new SqlParameter("@EST_ESTADOACTUAL", string.IsNullOrEmpty(EstadoActual) ? (object)DBNull.Value : EstadoActual);
                var param4 = new SqlParameter("@OPG_ID", string.IsNullOrEmpty(OPGID) ? (object)DBNull.Value : OPGID);
                var param5 = new SqlParameter("@ENV_ID", string.IsNullOrEmpty(EnvId) ? (object)DBNull.Value : EnvId);
                var param6 = new SqlParameter("@FECHA_DESDE", string.IsNullOrEmpty(FechaDesde) ? (object)DBNull.Value : FechaDesde);
                var param7 = new SqlParameter("@FECHA_HASTA", string.IsNullOrEmpty(FechaHasta) ? (object)DBNull.Value : FechaHasta);
                var param8 = new SqlParameter("@W_OK", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var param9 = new SqlParameter("@W_DESC", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };

                var execSp = await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_CONSULTA_ACREDITACIONES_INTERBANKING @TDD_CLT_ID, @CLT_NUMDOC, @MPG_ID, @EST_ESTADOACTUAL, @OPG_ID, @ENV_ID, @FECHA_DESDE, @FECHA_HASTA, @W_OK OUTPUT, @W_DESC OUTPUT",
                    param0, param1, param2, param3, param4, param5, param6, param7, param8, param9);

                if ((int)param8.Value == 0)
                {
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = (string)param8.Value;
                    result.Message = (string)param9.Value;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConsultaAcreditacionesInterbanking - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> ValidarConsultaHistorialEstados(decimal? opgId, decimal? numComprobante)
        {
            _logger.LogInformation($"Validando Consulta Historial Estados ({opgId}, {numComprobante})");

            try
            {
                IQueryable<OrdenespagoDto> query = from ordenesPago in _context.ORDENESPAGO
                                                   select new OrdenespagoDto
                                                   {
                                                       OPG_ID = ordenesPago.OPG_ID,
                                                       OPG_NUMCOMPROBANTE = ordenesPago.OPG_NUMCOMPROBANTE
                                                   };

                if (opgId.HasValue)
                {
                    query = query.Where(x => x.OPG_ID == opgId);

                    if (query.Count() == 0)
                    {
                        result.Code = ((int)HttpStatusCode.OK).ToString();
                        result.Message = "No existe la orden de pago solicitada";

                        return result;
                    }
                }

                if (numComprobante.HasValue)
                {
                    query = query.Where(x => x.OPG_NUMCOMPROBANTE == numComprobante);

                    if (query.Count() == 0)
                    {
                        result.Code = ((int)HttpStatusCode.OK).ToString();
                        result.Message = "No existe el número de cheque solicitado";

                        return result;
                    }
                }

                if (opgId.HasValue && numComprobante.HasValue)
                {
                    query = query.Where(x => x.OPG_ID == opgId && x.OPG_NUMCOMPROBANTE == numComprobante);

                    if (query.Count() == 0)
                    {
                        result.Code = ((int)HttpStatusCode.OK).ToString();
                        result.Message = "El número de cheque no corresponde a la orden de Pago";

                        return result;
                    }
                }

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(await query.ToListAsync());
                result.Message = "OK";

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ValidarConsultaHistorialEstados - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> ConsultaHistorialEstado(decimal? opgId, decimal? numComprobante)
        {
            _logger.LogInformation($"Consultando Historial Estados ({opgId}, {numComprobante})");

            try
            {
                IQueryable<HistorialEstadoDto> query = from ordenesPago in _context.ORDENESPAGO
                                                       join clientes in _context.CLIENTES on ordenesPago.TDD_CLT_ID equals clientes.TDD_CLT_ID
                                                       join beneficiarios in _context.BENEFICIARIOS on ordenesPago.TDD_BNF_ID equals beneficiarios.TDD_BNF_ID
                                                       join modalidadPago in _context.MODALIDADPAGO on ordenesPago.MPG_ID equals modalidadPago.MPG_ID
                                                       join sucursales in _context.SUCURSALES on ordenesPago.SUC_ENTREGA equals sucursales.SUC_ID
                                                       where ordenesPago.CLT_NUMDOC == clientes.CLT_NUMDOC
                                                       && ordenesPago.BNF_NUMDOC == beneficiarios.BNF_NUMDOC
                                                       && ordenesPago.TDD_CLT_ID == beneficiarios.TDD_CLT_ID
                                                       && ordenesPago.CLT_NUMDOC == beneficiarios.CLT_NUMDOC
                                                       select new HistorialEstadoDto
                                                       {
                                                           OPG_ID = ordenesPago.OPG_ID,
                                                           OPG_NUMCOMPROBANTE = ordenesPago.OPG_NUMCOMPROBANTE,
                                                           MPG_DESCRIPCION = modalidadPago.MPG_DESCRIPCION,
                                                           SUC_ENTREGA = ordenesPago.SUC_ENTREGA,
                                                           SUC_DESCRIPCION = sucursales.SUC_DESCRIPCION,
                                                           BNF_RAZONSOC = beneficiarios.BNF_RAZONSOC,
                                                           TDD_BNF_ID = ordenesPago.TDD_BNF_ID,
                                                           BNF_NUMDOC = ordenesPago.BNF_NUMDOC,
                                                           CLT_RAZONSOC = clientes.CLT_RAZONSOC,
                                                           TDD_CLT_ID = ordenesPago.TDD_CLT_ID,
                                                           CLT_NUMDOC = ordenesPago.CLT_NUMDOC
                                                       };

                if (opgId.HasValue && numComprobante.HasValue)
                {
                    query = query.Where(x => x.OPG_ID == opgId && x.OPG_NUMCOMPROBANTE == numComprobante);
                }

                if (opgId.HasValue && !numComprobante.HasValue)
                {
                    query = query.Where(x => x.OPG_ID == opgId);
                }

                if (!opgId.HasValue && numComprobante.HasValue)
                {
                    query = query.Where(x => x.OPG_NUMCOMPROBANTE == numComprobante);
                }

                var resultQuery = await query.ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(resultQuery);
                result.Message = "OK";

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConsultaHistorialEstado - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> ConsultaHistorialEstadoTabla(decimal opgId)
        {
            _logger.LogInformation($"Consultando Historial Estados Tabla ({opgId})");

            try
            {
                var query = await (from historial in _context.HISTORIAL
                                   join estados in _context.ESTADOS on historial.EST_ID equals estados.EST_ID
                                   where estados.EST_MAR_VISIBLE == 1
                                   && historial.OPG_ID == opgId
                                   orderby historial.OPG_ID, historial.HIS_FEC
                                   select new
                                   {
                                       EST_ID = historial.EST_ID,
                                       EST_DESCRIPCION = estados.EST_DESCRIPCION,
                                       HIS_FEC = historial.HIS_FEC
                                   }).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = "OK";

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConsultaHistorialEstadoTabla - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> ConsultaComisionesCobradas(FiltroComisionesCobradasDto filtroComisionesCobradasDto)
        {
            _logger.LogInformation($"Consultando Comisiones Cobradas");

            try
            {
                var query = await (from comisiones in _context.COMISIONES
                            join mp in _context.MODALIDADPAGO on comisiones.MPG_ID equals mp.MPG_ID
                            where comisiones.COM_FECHA >= filtroComisionesCobradasDto.comFechaDesde
                                  && comisiones.COM_FECHA <= filtroComisionesCobradasDto.comFechaHasta
                                  && comisiones.TDD_CLT_ID == filtroComisionesCobradasDto.tipoDocClt
                                  && comisiones.CLT_NUMDOC == filtroComisionesCobradasDto.numDocClt
                            select new
                            {
                                COM_ID = comisiones.COM_ID,
                                MPG_ID = comisiones.MPG_ID,
                                TDD_CLT_ID = comisiones.TDD_CLT_ID,
                                CLT_NUMDOC = comisiones.CLT_NUMDOC,
                                COM_IMP_COMISION = comisiones.COM_IMP_COMISION,
                                COM_FECHA = comisiones.COM_FECHA,
                                COM_CANTIDADOPGS = comisiones.COM_CANTIDADOPGS,
                                EST_ID = comisiones.EST_ID,
                                COM_CANTIDADRETS = comisiones.COM_CANTIDADRETS,
                                CTA_CUENTADEBITO = (from orden in _context.ORDENESPAGO
                                                    where orden.COM_ID == comisiones.COM_ID
                                                    select orden.CTA_CUENTADEBITO).FirstOrDefault(),
                                MPG_DESCRIPCION = mp.MPG_DESCRIPCION
                            }).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = "OK";

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConsultaComisionesCobradas - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
    }
}
