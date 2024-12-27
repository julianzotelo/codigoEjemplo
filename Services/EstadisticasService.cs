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

namespace pp3.services.Services
{
    public class EstadisticasService : IEstadisticasRepository
    {
        private readonly Pp3roContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ServicesResult result = new ServicesResult();
        private readonly ILogger<EstadisticasService> _logger;
        private readonly IMapper _mapper;

        public EstadisticasService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, ILogger<EstadisticasService> logger, IMapper mapper)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
            this._mapper = mapper;
        }

        public async Task<ServicesResult> CantidadECheqPorCliente(string fecha)
        {
            _logger.LogInformation($"Consultando Cantidad ECheq Por Cliente ({fecha})");

            try
            {
                var fechas = getDates(fecha);

                DateTime fechaDesde = fechas.dateFrom;
                DateTime fechaHasta = fechas.dateTo;


                var query = await (from ordenesPago in _context.ORDENESPAGO
                                   join clientes in _context.CLIENTES on ordenesPago.CLT_NUMDOC equals clientes.CLT_NUMDOC
                                   join modalidadPago in _context.MODALIDADPAGO on ordenesPago.MPG_ID equals modalidadPago.MPG_ID
                                   where ((new[] { 8m, 6m }.Contains(ordenesPago.MPG_ID) && new[] { "P", "L", "E" }.Contains(ordenesPago.EST_ESTADOACTUAL)))
                                   && ordenesPago.OPG_FEC_PAGO >= fechaDesde && ordenesPago.OPG_FEC_PAGO <= fechaHasta
                                   group ordenesPago by new
                                   {
                                       ordenesPago.CLT_NUMDOC,
                                       ClienteRazonSocial = clientes.CLT_RAZONSOC,
                                       ordenesPago.MPG_ID,
                                       MetodoPagoDescripcion = modalidadPago.MPG_DESCRIPCION
                                   } into grouped
                                   select new
                                   {
                                       CLT_NUMDOC = grouped.Key.CLT_NUMDOC,
                                       CLT_RAZONSOC = grouped.Key.ClienteRazonSocial,
                                       MPG_ID = grouped.Key.MPG_ID,
                                       MPG_DESCRIPCION = grouped.Key.MetodoPagoDescripcion,
                                       CANT_ECHEQS = grouped.Count(),
                                       IMP_TOTAL = grouped.Sum(x => x.OPG_IMP_PAGO)
                                   })
                                   .OrderBy(r => r.CLT_NUMDOC)
                                   .ThenBy(r => r.CLT_RAZONSOC)
                                   .ThenBy(r => r.MPG_ID)
                                   .ThenBy(r => r.MPG_DESCRIPCION)
                                   .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en CantidadECheqPorCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> CantidadTeftPorCliente(string fecha)
        {
            _logger.LogInformation($"Consultando Cantidad Teft Por Cliente ({fecha})");

            try
            {
                var fechas = getDates(fecha);

                DateTime fechaDesde = fechas.dateFrom;
                DateTime fechaHasta = fechas.dateTo;


                var query = await (from ordenesPago in _context.ORDENESPAGO
                                   join clientes in _context.CLIENTES on ordenesPago.CLT_NUMDOC equals clientes.CLT_NUMDOC
                                   join modalidadPago in _context.MODALIDADPAGO on ordenesPago.MPG_ID equals modalidadPago.MPG_ID
                                   where ((new[] { 2m, 4m }.Contains(ordenesPago.MPG_ID) && new[] { "P", "L", "E" }.Contains(ordenesPago.EST_ESTADOACTUAL)))
                                   && ordenesPago.OPG_FEC_PAGO >= fechaDesde && ordenesPago.OPG_FEC_PAGO <= fechaHasta
                                   group ordenesPago by new
                                   {
                                       ordenesPago.CLT_NUMDOC,
                                       ClienteRazonSocial = clientes.CLT_RAZONSOC,
                                       ordenesPago.MPG_ID,
                                       MetodoPagoDescripcion = modalidadPago.MPG_DESCRIPCION
                                   } into grouped
                                   select new
                                   {
                                       CLT_NUMDOC = grouped.Key.CLT_NUMDOC,
                                       CLT_RAZONSOC = grouped.Key.ClienteRazonSocial,
                                       MPG_ID = grouped.Key.MPG_ID,
                                       MPG_DESCRIPCION = grouped.Key.MetodoPagoDescripcion,
                                       CANT_ECHEQS = grouped.Count(),
                                       IMP_TOTAL = grouped.Sum(x => x.OPG_IMP_PAGO)
                                   })
                                   .OrderBy(r => r.CLT_NUMDOC)
                                   .ThenBy(r => r.CLT_RAZONSOC)
                                   .ThenBy(r => r.MPG_ID)
                                   .ThenBy(r => r.MPG_DESCRIPCION)
                                   .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en CantidadTeftPorCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> CantidadEfectivoPorCliente(string fecha)
        {
            _logger.LogInformation($"Consultando Cantidad Efectivo Por Cliente ({fecha})");

            try
            {
                var fechas = getDates(fecha);

                DateTime fechaDesde = fechas.dateFrom;
                DateTime fechaHasta = fechas.dateTo;


                var query = await (from ordenesPago in _context.ORDENESPAGO
                                   join clientes in _context.CLIENTES on ordenesPago.CLT_NUMDOC equals clientes.CLT_NUMDOC
                                   join modalidadPago in _context.MODALIDADPAGO on ordenesPago.MPG_ID equals modalidadPago.MPG_ID
                                   where ((new[] { 5m }.Contains(ordenesPago.MPG_ID) && new[] { "P", "L", "E" }.Contains(ordenesPago.EST_ESTADOACTUAL)))
                                   && ordenesPago.OPG_FEC_PAGO >= fechaDesde && ordenesPago.OPG_FEC_PAGO <= fechaHasta
                                   group ordenesPago by new
                                   {
                                       ordenesPago.CLT_NUMDOC,
                                       ClienteRazonSocial = clientes.CLT_RAZONSOC,
                                       ordenesPago.MPG_ID,
                                       MetodoPagoDescripcion = modalidadPago.MPG_DESCRIPCION
                                   } into grouped
                                   select new
                                   {
                                       CLT_NUMDOC = grouped.Key.CLT_NUMDOC,
                                       CLT_RAZONSOC = grouped.Key.ClienteRazonSocial,
                                       MPG_ID = grouped.Key.MPG_ID,
                                       MPG_DESCRIPCION = grouped.Key.MetodoPagoDescripcion,
                                       CANT_ECHEQS = grouped.Count(),
                                       IMP_TOTAL = grouped.Sum(x => x.OPG_IMP_PAGO)
                                   })
                                   .OrderBy(r => r.CLT_NUMDOC)
                                   .ThenBy(r => r.CLT_RAZONSOC)
                                   .ThenBy(r => r.MPG_ID)
                                   .ThenBy(r => r.MPG_DESCRIPCION)
                                   .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en CantidadEfectivoPorCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> CantidadEmpresasActivasEmitiendoECheqPorCliente(string fecha)
        {
            _logger.LogInformation($"Consultando Cantidad Empresas Activas Emitiendo ECheq Por Cliente ({fecha})");

            try
            {
                var fechas = getDates(fecha);

                DateTime fechaDesde = fechas.dateFrom;
                DateTime fechaHasta = fechas.dateTo;


                var query = await (from ordenesPago in _context.ORDENESPAGO
                                   join clientes in _context.CLIENTES on ordenesPago.CLT_NUMDOC equals clientes.CLT_NUMDOC
                                   join modalidadPago in _context.MODALIDADPAGO on ordenesPago.MPG_ID equals modalidadPago.MPG_ID
                                   where ((new[] { 8m, 6m }.Contains(ordenesPago.MPG_ID) && clientes.CLT_ESTADO == "AC" && new[] { "P", "L", "E" }.Contains(ordenesPago.EST_ESTADOACTUAL)))
                                   && ordenesPago.OPG_FEC_PAGO >= fechaDesde && ordenesPago.OPG_FEC_PAGO <= fechaHasta
                                   group ordenesPago by new
                                   {
                                       ordenesPago.CLT_NUMDOC,
                                       ClienteRazonSocial = clientes.CLT_RAZONSOC,
                                       ordenesPago.MPG_ID,
                                       MetodoPagoDescripcion = modalidadPago.MPG_DESCRIPCION,
                                       FechaIngreso = clientes.CLT_FEC_INGRESO
                                   } into grouped
                                   select new
                                   {
                                       CLT_NUMDOC = grouped.Key.CLT_NUMDOC,
                                       CLT_RAZONSOC = grouped.Key.ClienteRazonSocial,
                                       MPG_ID = grouped.Key.MPG_ID,
                                       MPG_DESCRIPCION = grouped.Key.MetodoPagoDescripcion,
                                       CLT_FEC_INGRESO = grouped.Key.FechaIngreso,
                                       CANT_ECHEQS = grouped.Count()
                                   })
                                   .OrderBy(r => r.CLT_NUMDOC)
                                   .ThenBy(r => r.CLT_RAZONSOC)
                                   .ThenBy(r => r.MPG_ID)
                                   .ThenBy(r => r.MPG_DESCRIPCION)
                                   .ThenBy(r => r.CLT_FEC_INGRESO)
                                   .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en CantidadEmpresasActivasEmitiendoECheqPorCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> CantidadEmpresasActivasEmitiendoTeftPorCliente(string fecha)
        {
            _logger.LogInformation($"Consultando Cantidad Empresas Activas Emitiendo Teft Por Cliente ({fecha})");

            try
            {
                var fechas = getDates(fecha);

                DateTime fechaDesde = fechas.dateFrom;
                DateTime fechaHasta = fechas.dateTo;


                var query = await (from ordenesPago in _context.ORDENESPAGO
                                   join clientes in _context.CLIENTES on ordenesPago.CLT_NUMDOC equals clientes.CLT_NUMDOC
                                   join modalidadPago in _context.MODALIDADPAGO on ordenesPago.MPG_ID equals modalidadPago.MPG_ID
                                   where ((new[] { 2m, 4m }.Contains(ordenesPago.MPG_ID) && clientes.CLT_ESTADO == "AC" && new[] { "P", "L", "E" }.Contains(ordenesPago.EST_ESTADOACTUAL)))
                                   && ordenesPago.OPG_FEC_PAGO >= fechaDesde && ordenesPago.OPG_FEC_PAGO <= fechaHasta
                                   group ordenesPago by new
                                   {
                                       ordenesPago.CLT_NUMDOC,
                                       ClienteRazonSocial = clientes.CLT_RAZONSOC,
                                       ordenesPago.MPG_ID,
                                       MetodoPagoDescripcion = modalidadPago.MPG_DESCRIPCION,
                                       FechaIngreso = clientes.CLT_FEC_INGRESO
                                   } into grouped
                                   select new
                                   {
                                       CLT_NUMDOC = grouped.Key.CLT_NUMDOC,
                                       CLT_RAZONSOC = grouped.Key.ClienteRazonSocial,
                                       MPG_ID = grouped.Key.MPG_ID,
                                       MPG_DESCRIPCION = grouped.Key.MetodoPagoDescripcion,
                                       CLT_FEC_INGRESO = grouped.Key.FechaIngreso,
                                       CANT_TEFT = grouped.Count()
                                   })
                                   .OrderBy(r => r.CLT_NUMDOC)
                                   .ThenBy(r => r.CLT_RAZONSOC)
                                   .ThenBy(r => r.MPG_ID)
                                   .ThenBy(r => r.MPG_DESCRIPCION)
                                   .ThenBy(r => r.CLT_FEC_INGRESO)
                                   .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en CantidadEmpresasActivasEmitiendoTeftPorCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> ConveniosAperturadosTodasModalidadesPago(string fecha)
        {
            _logger.LogInformation($"Consultando Convenios Aperturados en el periodo con todas Modalidades Pago ({fecha})");

            try
            {
                var fechas = getDates(fecha);

                DateTime fechaDesde = fechas.dateFrom;
                DateTime fechaHasta = fechas.dateTo;


                var query = await (from cuentas in _context.CUENTAS
                                   join clientes in _context.CLIENTES on cuentas.CLT_NUMDOC equals clientes.CLT_NUMDOC
                                   join ordenesPago in _context.ORDENESPAGO on cuentas.CLT_NUMDOC equals ordenesPago.CLT_NUMDOC
                                   join envios in _context.ENVIOS on ordenesPago.ENV_ID equals envios.ENV_ID
                                   join modalidadPago in _context.MODALIDADPAGO on ordenesPago.MPG_ID equals modalidadPago.MPG_ID
                                   where clientes.CLT_ESTADO == "AC"
                                         && (new[] { "P", "L", "E" }.Contains(ordenesPago.EST_ESTADOACTUAL))
                                         && cuentas.CTA_FECHAPERTURA >= fechaDesde && cuentas.CTA_FECHAPERTURA <= fechaHasta
                                   group new { cuentas, clientes, ordenesPago, envios, modalidadPago } by new
                                   {
                                       clientes.CLT_NUMDOC,
                                       clientes.CLT_RAZONSOC,
                                       clientes.CLT_FEC_INGRESO,
                                       cuentas.SUC_CUENTASERVICIO,
                                       cuentas.CTA_SERVICIO,
                                       cuentas.CTA_CONVENIO,
                                       cuentas.CTA_INACTIVA,
                                       cuentas.CTA_FECHAPERTURA,
                                       ordenesPago.MPG_ID,
                                       modalidadPago.MPG_DESCRIPCION
                                   } into grouped
                                   select new
                                   {
                                       CUIT = grouped.Key.CLT_NUMDOC,
                                       RAZON_SOCIAL = grouped.Key.CLT_RAZONSOC,
                                       FECHA_INGRESO = grouped.Key.CLT_FEC_INGRESO,
                                       SUCURSAL = grouped.Key.SUC_CUENTASERVICIO,
                                       CUENTA_SERVICIO = grouped.Key.CTA_SERVICIO,
                                       CONVENIO = grouped.Key.CTA_CONVENIO,
                                       ESTADO = grouped.Key.CTA_INACTIVA,
                                       FECHA_APERTURA = grouped.Key.CTA_FECHAPERTURA,
                                       MODALIDAD_PAGO_ID = grouped.Key.MPG_ID,
                                       MODALIDAD_PAGO_DESC = grouped.Key.MPG_DESCRIPCION,
                                       ULT_FECHA_ENV = grouped.Max(e => e.envios.ENV_FEC),
                                       CANTIDAD = grouped.Count(),
                                       IMP_TOTAL = grouped.Sum(o => o.ordenesPago.OPG_IMP_PAGO)
                                   })
                                   .OrderBy(r => r.CUIT)
                                   .ThenBy(r => r.RAZON_SOCIAL)
                                   .ThenBy(r => r.FECHA_INGRESO)
                                   .ThenBy(r => r.SUCURSAL)
                                   .ThenBy(r => r.CUENTA_SERVICIO)
                                   .ThenBy(r => r.CONVENIO)
                                   .ThenBy(r => r.ESTADO)
                                   .ThenBy(r => r.FECHA_APERTURA)
                                   .ThenBy(r => r.MODALIDAD_PAGO_ID)
                                   .ThenBy(r => r.MODALIDAD_PAGO_DESC)
                                   .ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConveniosAperturadosTodasModalidadesPago - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public (DateTime dateFrom, DateTime dateTo) getDates(string date)
        {
            var parts = date.Split('/');
            int month = int.Parse(parts[0]);
            int year = int.Parse(parts[1]);

            DateTime dateFrom = new DateTime(year, month, 1);

            int lastDay = DateTime.DaysInMonth(year, month);
            DateTime dateTo = new DateTime(year, month, lastDay);

            return (dateFrom, dateTo);
        }
    }
}
