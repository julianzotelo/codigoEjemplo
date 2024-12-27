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
using System.Net;
using static pp3.dominio.Models.Globals;

namespace pp3.services.Services
{
    public class EscalasServices : IEscalasRepository
    {
        private readonly Pp3roContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SeguridadService _seguridadService;
        private readonly ServicesResult result = new ServicesResult();
        private readonly ILogger<EscalasServices> _logger;
        private readonly IMapper _mapper;
        

        public EscalasServices(Pp3roContext context, IHttpContextAccessor httpContextAccessor, SeguridadService seguridadService, IMapper mapper, ILogger<EscalasServices> logger)
        {
            this.context = context;
            this._mapper = mapper;
            this._logger = logger;
            this._seguridadService = seguridadService;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServicesResult> ComisionesPorClienteModalidad(int tipo, decimal numero, int moda)
        {
            _logger.LogInformation($"Consultando Comisiones Por Cliente Modalidad ({tipo} ,{numero}, {moda})");

            try
            {
                var Escalas = await context.ESCALASCOMISION
                                         .Where(ec => ec.TDD_CLT_ID == tipo && ec.CLT_NUMDOC == numero && ec.MPG_ID == moda)
                                         .ToListAsync();
                List<EscalasComisionDto> EscalasDto = _mapper.Map<List<EscalasComisionDto>>(Escalas);
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(EscalasDto);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ComisionesPorClienteModalidad - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }
        public async Task<ServicesResult> ComisionesPorCliente(decimal tipo, decimal numero)
        {
            _logger.LogInformation($"Consultando Comisiones Por Cliente ({tipo}, {numero})");

            try
            {
                var Escalas = await context.ESCALASCOMISION
                                         .Join(context.MODALIDADPAGO,
                                               ec => ec.MPG_ID,
                                               mp => mp.MPG_ID,
                                               (ec, mp) => new { EscalaComision = ec, ModalidadPago = mp })
                                         .Where(joined => joined.EscalaComision.TDD_CLT_ID == tipo && joined.EscalaComision.CLT_NUMDOC == numero)
                                         .Select(joined => new EscalasComisionDto
                                         {
                                             TDD_CLT_ID = joined.EscalaComision.TDD_CLT_ID,
                                             CLT_NUMDOC = joined.EscalaComision.CLT_NUMDOC,
                                             MPG_ID = joined.EscalaComision.MPG_ID,
                                             MPG_DESCRIPCION = joined.ModalidadPago.MPG_DESCRIPCION,
                                             ESC_HASTA = joined.EscalaComision.ESC_HASTA,
                                             ESC_ALICUOTA = joined.EscalaComision.ESC_ALICUOTA,
                                             ESC_IMP_FIJO = joined.EscalaComision.ESC_IMP_FIJO,
                                             ESC_IMP_MINIMO = joined.EscalaComision.ESC_IMP_MINIMO,
                                             ESC_IMP_MAXIMO = joined.EscalaComision.ESC_IMP_MAXIMO
                                         })
                                         .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(Escalas);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ComisionesPorCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }
        public async Task<ServicesResult> NuevaComision(Escalascomision nuevaComision)
        {
            _logger.LogInformation($"Nueva Comision ({nuevaComision})");

            {
                try
                {
                    context.ESCALASCOMISION.Add(nuevaComision);
                    await context.SaveChangesAsync();
                    var ok = await _seguridadService.InsertarLog((int)CodigosTareas.EscalasComision_Agregar, Globals.user, " ", $"Cliente-Modalidad={nuevaComision.TDD_CLT_ID}-{nuevaComision.CLT_NUMDOC}-{nuevaComision.MPG_ID}");
                    if (ok.Content == null)
                    {
                        result.Message = HttpStatusCode.NotAcceptable.ToString();
                        result.Content = "false";
                        result.Code = ((int)HttpStatusCode.NotAcceptable).ToString();
                        return result;
                    }

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = JsonConvert.SerializeObject(true);
                    result.Message = HttpStatusCode.OK.ToString();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error en NuevaComision - Origen:  - " +
                    $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                    $"{ex.InnerException.ToString() ?? string.Empty}");
                    result.Code = ex.HResult.ToString();
                    result.Message = $"Ha ocurrido un error: {ex.Message}";
                    result.Content = "false";
                }
                return result;
            }
        }
        public async Task<ServicesResult> Comision(int tipo, decimal numero, int moda, decimal hasta)
        {
            _logger.LogInformation($"Consultando Comision ({tipo}, {numero}, {moda}, {hasta})");

            try
            {
                Escalascomision? Escalas = await context.ESCALASCOMISION
                    .FirstOrDefaultAsync(ec => ec.TDD_CLT_ID == tipo &&
                                               ec.CLT_NUMDOC == numero &&
                                               ec.MPG_ID == moda &&
                                               ec.ESC_HASTA == hasta);
                EscalasComisionDto EscalasDto = _mapper.Map<EscalasComisionDto>(Escalas);
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(EscalasDto);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Comision - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }
        public async Task<ServicesResult> EliminarComision(int tipo, decimal numero, int moda, decimal hasta)
        {
            _logger.LogInformation($"Eliminar Comision ({tipo}, {numero}, {moda}, {hasta})");

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var comision = await context.ESCALASCOMISION
                    .FirstOrDefaultAsync(ec => ec.TDD_CLT_ID == tipo &&
                                               ec.CLT_NUMDOC == numero &&
                                               ec.MPG_ID == moda &&
                                               ec.ESC_HASTA == hasta);

                if (comision == null)
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No se encontro comision";
                    result.Content = "false";
                    return result;
                }

                context.ESCALASCOMISION.Remove(comision);
                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.EscalasComision_Eliminar, Globals.user, $"Cliente-Modalidad={tipo}-{numero}-{moda}", " ");
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(true);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en EliminarComision - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                await transaction.RollbackAsync();
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                result.Content = "false";
            }
            return result;
        }
        public async Task<ServicesResult> EliminarDescuento(int tipo, decimal numero, int moda, decimal hasta)
        {
            _logger.LogInformation($"Eliminar Descuento ({tipo}, {numero}, {moda}, {hasta})");
            //var escalasDescuento = context.ESCALASDESCUENTO
            //        .Where(e => e.TDD_CLT_ID == Tipo && e.CLT_NUMDOC == numero && e.MPG_ID == Id);
            //context.ESCALASDESCUENTO.RemoveRange(escalasDescuento);
            try
            {
                var descuento = await context.ESCALASDESCUENTO
                    .Where(ed => ed.TDD_CLT_ID == tipo &&
                                               ed.CLT_NUMDOC == numero &&
                                               ed.MPG_ID == moda &&
                                               ed.ESD_HASTA == hasta).ToListAsync();

                if (descuento == null)
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Content = JsonConvert.SerializeObject(false);
                    result.Message = "No se encontro descuento";
                    return result;
                }
                context.ESCALASDESCUENTO.RemoveRange(descuento);

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.EscalasDescuento_Eliminar, Globals.user, $"Cliente-Modalidad={tipo}-{numero}-{moda}", " ");
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await context.SaveChangesAsync();
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(true);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en EliminarDescuento - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }
        public async Task<ServicesResult> ModificarComision(Escalascomision escalascomision)
        {
            _logger.LogInformation($"Modificar Comision ({escalascomision})");

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var comision = await context.ESCALASCOMISION
                    .FirstOrDefaultAsync(ec => ec.TDD_CLT_ID == escalascomision.TDD_CLT_ID &&
                                               ec.CLT_NUMDOC == escalascomision.CLT_NUMDOC &&
                                               ec.MPG_ID == escalascomision.MPG_ID &&
                                               ec.ESC_HASTA == escalascomision.ESC_HASTA);

                if (comision == null)
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Content = JsonConvert.SerializeObject(false);
                    result.Message = "No se encontro comision";
                    return result;
                }

                comision.ESC_ALICUOTA = escalascomision.ESC_ALICUOTA;
                comision.ESC_IMP_FIJO = escalascomision.ESC_IMP_FIJO;
                comision.ESC_IMP_MINIMO = escalascomision.ESC_IMP_MINIMO;
                comision.ESC_IMP_MAXIMO = escalascomision.ESC_IMP_MAXIMO;

                context.ESCALASCOMISION.Update(comision);

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.EscalasComision_Modificar, Globals.user, $"Cliente-Modalidad={escalascomision.TDD_CLT_ID}-{escalascomision.CLT_NUMDOC}-{escalascomision.MPG_ID}", $"Cliente-Modalidad={escalascomision.TDD_CLT_ID}-{escalascomision.CLT_NUMDOC}-{escalascomision.MPG_ID}");
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(true);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ModificarComision - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                await transaction.RollbackAsync();
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }
        public async Task<ServicesResult> ModificarDescuento(Escalasdescuento escalasdescuento)
        {
            _logger.LogInformation($"Modificar Descuento ({escalasdescuento})");

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var descuento = await context.ESCALASDESCUENTO
                    .FirstOrDefaultAsync(ed => ed.TDD_CLT_ID == escalasdescuento.TDD_CLT_ID &&
                                               ed.CLT_NUMDOC == escalasdescuento.CLT_NUMDOC &&
                                               ed.MPG_ID == escalasdescuento.MPG_ID &&
                                               ed.ESD_HASTA == escalasdescuento.ESD_HASTA);
                if (descuento == null)
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Content = JsonConvert.SerializeObject(false);
                    result.Message = "No se encontro descuento";
                    return result;
                }
                descuento.ESD_ALICUOTA = escalasdescuento.ESD_ALICUOTA;
                descuento.ESD_IMP_FIJO = escalasdescuento.ESD_IMP_FIJO;
                descuento.ESD_IMP_MINIMO = escalasdescuento.ESD_IMP_MINIMO;
                descuento.ESD_IMP_MAXIMO = escalasdescuento.ESD_IMP_MAXIMO;

                context.ESCALASDESCUENTO.Update(descuento);

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.EscalasDescuento_Modificar, Globals.user, $"Cliente-Modalidad={escalasdescuento.TDD_CLT_ID}-{escalasdescuento.CLT_NUMDOC}-{escalasdescuento.MPG_ID}", $"Cliente-Modalidad={escalasdescuento.TDD_CLT_ID}-{escalasdescuento.CLT_NUMDOC}-{escalasdescuento.MPG_ID}");
                if (ok.Content == null)
                    throw new Exception("Error al insertar log.");

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(true);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ModificarDescuento - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                await transaction.RollbackAsync();
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }
        public async Task<ServicesResult> DescuentosPorClienteModalidad(int tipo, decimal numero, int moda)
        {
            _logger.LogInformation($"Consultando Descuentos Por Cliente Modalidad");
            try
            {
                List<Escalasdescuento> Escalas = await context.ESCALASDESCUENTO
                    .Where(ed => ed.TDD_CLT_ID == tipo && ed.CLT_NUMDOC == numero && ed.MPG_ID == moda)
                    .ToListAsync();
                List<EscalasDescuentoDto> EscalasDto = _mapper.Map<List<EscalasDescuentoDto>>(Escalas);
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(EscalasDto);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en DescuentosPorClienteModalidad - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }
        public async Task<ServicesResult> NuevoDescuento(Escalasdescuento nuevoDescuento)
        {
            _logger.LogInformation($"Nuevo Descuento ({nuevoDescuento})");

            try
            {

                context.ESCALASDESCUENTO.Add(nuevoDescuento);
                await context.SaveChangesAsync();

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.EscalasDescuento_Agregar, Globals.user, " ", $"Cliente-Modalidad={nuevoDescuento.TDD_CLT_ID}-{nuevoDescuento.CLT_NUMDOC}-{nuevoDescuento.MPG_ID}");
                if (ok.Content == null)
                {
                    result.Code = ((int)HttpStatusCode.NotAcceptable).ToString();
                    result.Message = HttpStatusCode.NotAcceptable.ToString();
                    result.Content = "false";
                    return result;
                }

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(true);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en NuevoDescuento - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }
        public async Task<ServicesResult> Descuento(int tipo, decimal numero, int moda, decimal hasta)
        {
            _logger.LogInformation($"Consultando Descuento ({tipo}, {numero}, {moda}, {hasta})");

            try
            {
                Escalasdescuento? Escalas = await context.ESCALASDESCUENTO
                    .FirstOrDefaultAsync(ed => ed.TDD_CLT_ID == tipo && ed.CLT_NUMDOC == numero && ed.MPG_ID == moda && ed.ESD_HASTA == hasta);

                EscalasDescuentoDto EscalasDto = _mapper.Map<EscalasDescuentoDto>(Escalas);
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(EscalasDto);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Descuento - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }
        //        'Se crea la función para reemplazar la consulta que se realizaba en Crystal Report Report: Estadísticas1.rpt    -- Maria 11/06/2004
        //Public Function EstadisticasRp1(Filtro As String) As Recordset
        //On Error Resume Next
        //Dim sql As String
        //Dim recEst As New Recordset

        //sql = "SELECT " & _
        //    " CLIENTES.TDD_CLT_ID, CLIENTES.CLT_NUMDOC, CLIENTES.CLT_RAZONSOC," & _
        //    " ORDENESPAGO.OPG_ID, ORDENESPAGO.OPG_IMP_PAGO, ORDENESPAGO.MPG_ID," & _
        //    " HISTORIAL.HIS_FEC, MODALIDADPAGO.MPG_DESCRIPCION " & _
        //"From " & _
        //    " CLIENTES CLIENTES, " & _
        //    " ORDENESPAGO ORDENESPAGO, " & _
        //    " HISTORIAL HISTORIAL, " & _
        //    " MODALIDADPAGO MODALIDADPAGO " & _
        //"Where " & _
        //    " CLIENTES.TDD_CLT_ID = ORDENESPAGO.TDD_CLT_ID AND " & _
        //    " CLIENTES.CLT_NUMDOC = ORDENESPAGO.CLT_NUMDOC AND " & _
        //    " ORDENESPAGO.OPG_ID = HISTORIAL.OPG_ID AND " & _
        //    " ORDENESPAGO.MPG_ID = MODALIDADPAGO.MPG_ID " & _
        //    IIf(Trim(Filtro) <> "", " AND " & Filtro, "") & _
        //" Order by " & _
        //    " CLIENTES.TDD_CLT_ID,CLIENTES.CLT_NUMDOC,MODALIDADPAGO.MPG_ID"

        //recEst.CursorLocation = adUseClient 'para que funcione el AbsolutePosition
        //recEst.Open sql, Con, adOpenStatic
        //Set EstadisticasRp1 = recEst
        //Set recEst = Nothing

        //End Function
        public async Task<ServicesResult> EstadisticasRp1(string filtro)
        {
            _logger.LogInformation($"Consultando Estadisticas Rp1 ({filtro})");

            try
            {
                var query = from cliente in context.CLIENTES
                            join orden in context.ORDENESPAGO on new { cliente.TDD_CLT_ID, cliente.CLT_NUMDOC } equals new { orden.TDD_CLT_ID, orden.CLT_NUMDOC }
                            join historial in context.HISTORIAL on orden.OPG_ID equals historial.OPG_ID
                            join modalidad in context.MODALIDADPAGO on orden.MPG_ID equals modalidad.MPG_ID
                            where string.IsNullOrEmpty(filtro)
                            orderby cliente.TDD_CLT_ID, cliente.CLT_NUMDOC, modalidad.MPG_ID
                            select new
                            {
                                cliente.TDD_CLT_ID,
                                cliente.CLT_NUMDOC,
                                cliente.CLT_RAZONSOC,
                                orden.OPG_ID,
                                orden.OPG_IMP_PAGO,
                                orden.MPG_ID,
                                historial.HIS_FEC,
                                modalidad.MPG_DESCRIPCION
                            };
                var results = query.ToList();
                var resultado = await query.OrderBy(e => e.TDD_CLT_ID).ThenBy(e => e.CLT_NUMDOC).ThenBy(e => e.MPG_ID).ToListAsync();
                result.Content = JsonConvert.SerializeObject(resultado);
                result.Message = HttpStatusCode.OK.ToString();
                result.Code = ((int)HttpStatusCode.OK).ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en EstadisticasRp1 - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;

        }
        //'Se crea la función para reemplazar la consulta que se realizaba en Crystal Report Report: Estadísticas2.rpt    -- Maria 11/06/2004
        //Public Function EstadisticasRp2(Filtro As String) As Recordset  'Comisiones
        //On Error Resume Next
        //Dim sql As String
        //Dim recEst As New Recordset
        //recEst.CursorLocation = adUseClient
        //sql = "SELECT " & _
        //   " MODALIDADPAGO.MPG_DESCRIPCION," & _
        //   " COMISIONES.MPG_ID, COMISIONES.COM_IMP_COMISION," & _
        //   " COMISIONES.COM_FECHA," & _
        //   " CLIENTES.TDD_CLT_ID, CLIENTES.CLT_NUMDOC, " & _
        //   " CLIENTES.CLT_RAZONSOC " & _
        //" FROM " & _
        //   " MODALIDADPAGO MODALIDADPAGO, " & _
        //   " COMISIONES COMISIONES, " & _
        //   " CLIENTES CLIENTES " & _
        //" WHERE " & _
        //   " MODALIDADPAGO.MPG_ID = COMISIONES.MPG_ID AND " & _
        //   " COMISIONES.TDD_CLT_ID = CLIENTES.TDD_CLT_ID AND " & _
        //   " COMISIONES.CLT_NUMDOC = CLIENTES.CLT_NUMDOC " & _
        //    IIf(Trim(Filtro) <> "", " AND " & Filtro, "") & _
        //" Order by " & _
        //    " CLIENTES.TDD_CLT_ID,CLIENTES.CLT_NUMDOC,MODALIDADPAGO.MPG_ID"

        //recEst.Open sql, Con, adOpenStatic
        //Set EstadisticasRp2 = recEst

        //Set recEst = Nothing
        //End Function
        public async Task<ServicesResult> EstadisticasRp2(string filtro)
        {
            _logger.LogInformation($"Consultando EstadisticasRp2 ({filtro})");

            try
            {
                var query = from modalidadPago in context.MODALIDADPAGO
                            join comision in context.COMISIONES on modalidadPago.MPG_ID equals comision.MPG_ID
                            join cliente in context.CLIENTES on new { comision.TDD_CLT_ID, comision.CLT_NUMDOC } equals new { cliente.TDD_CLT_ID, cliente.CLT_NUMDOC }
                            select new
                            {
                                MpgDescripcion = modalidadPago.MPG_DESCRIPCION,
                                MpgId = comision.MPG_ID,
                                ComImporteComision = comision.COM_IMP_COMISION,
                                ComFecha = comision.COM_FECHA,
                                TddCltId = cliente.TDD_CLT_ID,
                                CltNumDoc = cliente.CLT_NUMDOC,
                                CltRazonSocial = cliente.CLT_RAZONSOC
                            };
                var resultado = await query.OrderBy(e => e.TddCltId).ThenBy(e => e.CltNumDoc).ThenBy(e => e.MpgId).ToListAsync();
                result.Content = JsonConvert.SerializeObject(resultado);
                result.Message = HttpStatusCode.OK.ToString();
                result.Code = ((int)HttpStatusCode.OK).ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en EstadisticasRp2 - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;

        }
    }
}
