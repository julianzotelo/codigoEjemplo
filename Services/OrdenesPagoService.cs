using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Management.Network.Models;
using Microsoft.Data.SqlClient;
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
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static pp3.dominio.Models.Globals;

namespace pp3.services.Services
{
    public class OrdenesPagoService : IOrdenesPagoRepository
    {
        private readonly Pp3roContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SeguridadService _seguridadService;
        private readonly IMapper _mapper;
        ServicesResult result = new ServicesResult();
        private readonly ILogger<OrdenesPagoService> _logger;
        


        public OrdenesPagoService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, SeguridadService seguridadService, ILogger<OrdenesPagoService> logger, IMapper mapper)
        {
            this.context = context;

            this._httpContextAccessor = httpContextAccessor;

            this._logger = logger;

            this._mapper = mapper;

            this._seguridadService = seguridadService;
        }

        public async Task<ServicesResult> OrdenesPorClienteNumero(int tipoCli, double numCli, double numero)
        {
            _logger.LogInformation($"Consultando Ordenes por cliente numero ({tipoCli}, {numCli}, {numero})");

            try
            {
                var query = await (from ordenesPago in context.ORDENESPAGO
                                   join sucursales in context.SUCURSALES on ordenesPago.SUC_ENTREGA equals sucursales.SUC_ID
                                   join modalidadPago in context.MODALIDADPAGO on ordenesPago.MPG_ID equals modalidadPago.MPG_ID
                                   join estados in context.ESTADOS on ordenesPago.EST_ESTADOACTUAL equals estados.EST_ID
                                   where ordenesPago.TDD_CLT_ID == tipoCli && ordenesPago.CLT_NUMDOC == Convert.ToDecimal(numCli) && ordenesPago.OPG_ID >= Convert.ToDecimal(numero)
                                   orderby (ordenesPago.OPG_ID)
                                   select new
                                   {
                                       OPG_ID = ordenesPago.OPG_ID,
                                       TDD_BNF_ID = ordenesPago.TDD_BNF_ID,
                                       BNF_NUMDOC = ordenesPago.BNF_NUMDOC,
                                       TDD_CLT_ID = ordenesPago.TDD_CLT_ID,
                                       CLT_NUMDOC = ordenesPago.CLT_NUMDOC,
                                       SUC_ENTREGA = ordenesPago.SUC_ENTREGA,
                                       OPG_IDOPGCLI = ordenesPago.OPG_IDOPGCLI,
                                       OPG_ORDENALT = ordenesPago.OPG_ORDENALT,
                                       OPG_IMP_DEBITO = ordenesPago.OPG_IMP_DEBITO,
                                       OPG_IMP_PAGO = ordenesPago.OPG_IMP_PAGO,
                                       SUC_CUENTADEBITO = ordenesPago.SUC_CUENTADEBITO,
                                       TDC_DEBITO = ordenesPago.TDC_DEBITO,
                                       MND_DEBITO = ordenesPago.MND_DEBITO,
                                       CTA_CUENTADEBITO = ordenesPago.CTA_CUENTADEBITO,
                                       BCO_CUENTAPAGO = ordenesPago.BCO_CUENTAPAGO,
                                       SUC_CUENTAPAGO = ordenesPago.SUC_CUENTAPAGO,
                                       TDC_PAGO = ordenesPago.TDC_PAGO,
                                       MND_PAGO = ordenesPago.MND_PAGO,
                                       OPG_CUENTAPAGO = ordenesPago.OPG_CUENTAPAGO,
                                       OPG_AUXCBU = ordenesPago.OPG_AUXCBU,
                                       MPG_ID = ordenesPago.MPG_ID,
                                       OPG_MAR_REGCHQ = ordenesPago.OPG_MAR_REGCHQ,
                                       OPG_FEC_PAGO = ordenesPago.OPG_FEC_PAGO,
                                       OPG_FEC_PAGODIFERIDO = ordenesPago.OPG_FEC_PAGODIFERIDO,
                                       OPG_NUMCOMPROBANTE = ordenesPago.OPG_NUMCOMPROBANTE,
                                       COM_ID = ordenesPago.COM_ID,
                                       USR_FIRMANTE1 = ordenesPago.USR_FIRMANTE1,
                                       USR_FIRMANTE2 = ordenesPago.USR_FIRMANTE2,
                                       USR_FIRMANTE3 = ordenesPago.USR_FIRMANTE3,
                                       EST_ESTADOACTUAL = ordenesPago.EST_ESTADOACTUAL,
                                       OPG_NUM_ENVIO = ordenesPago.OPG_NUM_ENVIO,
                                       ENV_ID = ordenesPago.ENV_ID,
                                       OPG_NRORECIBO = ordenesPago.OPG_NRORECIBO,
                                       PATH_DOCU = ordenesPago.PATH_DOCU,
                                       ID_REGISTRO = ordenesPago.ID_REGISTRO,
                                       ID_ECHEQ = ordenesPago.ID_ECHEQ,
                                       FIRMANTES_ECHEQ = ordenesPago.FIRMANTES_ECHEQ,
                                       RAN_ID = ordenesPago.RAN_ID,
                                       SUC_DESCRIPCION = sucursales.SUC_DESCRIPCION,
                                       MPG_DESCRIPCION = modalidadPago.MPG_DESCRIPCION,
                                       EST_DESCRIPCION = estados.EST_DESCRIPCION
                                   }).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en OrdenesPorClienteNumero - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> OrdenesPorClienteBeneficiarioNumero(int tipoCli, double numCli, int tipoBen, double numBen, double numero)
        {
            _logger.LogInformation($"Consultando Ordenes por cliente beneficiario numero ({tipoCli}, {numCli}, {tipoBen}, {numBen}, {numero})");

            try
            {
                var query = await (from ordenesPago in context.ORDENESPAGO
                                   join sucursales in context.SUCURSALES on ordenesPago.SUC_ENTREGA equals sucursales.SUC_ID
                                   join modalidadPago in context.MODALIDADPAGO on ordenesPago.MPG_ID equals modalidadPago.MPG_ID
                                   join estados in context.ESTADOS on ordenesPago.EST_ESTADOACTUAL equals estados.EST_ID
                                   where ordenesPago.TDD_CLT_ID == tipoCli
                                   && ordenesPago.CLT_NUMDOC == Convert.ToDecimal(numCli)
                                   && ordenesPago.TDD_BNF_ID == Convert.ToDecimal(tipoBen)
                                   && ordenesPago.BNF_NUMDOC == Convert.ToDecimal(numBen)
                                   && ordenesPago.OPG_ID >= Convert.ToDecimal(numero)
                                   orderby (ordenesPago.OPG_ID)
                                   select new
                                   {
                                       OPG_ID = ordenesPago.OPG_ID,
                                       TDD_BNF_ID = ordenesPago.TDD_BNF_ID,
                                       BNF_NUMDOC = ordenesPago.BNF_NUMDOC,
                                       TDD_CLT_ID = ordenesPago.TDD_CLT_ID,
                                       CLT_NUMDOC = ordenesPago.CLT_NUMDOC,
                                       SUC_ENTREGA = ordenesPago.SUC_ENTREGA,
                                       OPG_IDOPGCLI = ordenesPago.OPG_IDOPGCLI,
                                       OPG_ORDENALT = ordenesPago.OPG_ORDENALT,
                                       OPG_IMP_DEBITO = ordenesPago.OPG_IMP_DEBITO,
                                       OPG_IMP_PAGO = ordenesPago.OPG_IMP_PAGO,
                                       SUC_CUENTADEBITO = ordenesPago.SUC_CUENTADEBITO,
                                       TDC_DEBITO = ordenesPago.TDC_DEBITO,
                                       MND_DEBITO = ordenesPago.MND_DEBITO,
                                       CTA_CUENTADEBITO = ordenesPago.CTA_CUENTADEBITO,
                                       BCO_CUENTAPAGO = ordenesPago.BCO_CUENTAPAGO,
                                       SUC_CUENTAPAGO = ordenesPago.SUC_CUENTAPAGO,
                                       TDC_PAGO = ordenesPago.TDC_PAGO,
                                       MND_PAGO = ordenesPago.MND_PAGO,
                                       OPG_CUENTAPAGO = ordenesPago.OPG_CUENTAPAGO,
                                       OPG_AUXCBU = ordenesPago.OPG_AUXCBU,
                                       MPG_ID = ordenesPago.MPG_ID,
                                       OPG_MAR_REGCHQ = ordenesPago.OPG_MAR_REGCHQ,
                                       OPG_FEC_PAGO = ordenesPago.OPG_FEC_PAGO,
                                       OPG_FEC_PAGODIFERIDO = ordenesPago.OPG_FEC_PAGODIFERIDO,
                                       OPG_NUMCOMPROBANTE = ordenesPago.OPG_NUMCOMPROBANTE,
                                       COM_ID = ordenesPago.COM_ID,
                                       USR_FIRMANTE1 = ordenesPago.USR_FIRMANTE1,
                                       USR_FIRMANTE2 = ordenesPago.USR_FIRMANTE2,
                                       USR_FIRMANTE3 = ordenesPago.USR_FIRMANTE3,
                                       EST_ESTADOACTUAL = ordenesPago.EST_ESTADOACTUAL,
                                       OPG_NUM_ENVIO = ordenesPago.OPG_NUM_ENVIO,
                                       ENV_ID = ordenesPago.ENV_ID,
                                       OPG_NRORECIBO = ordenesPago.OPG_NRORECIBO,
                                       PATH_DOCU = ordenesPago.PATH_DOCU,
                                       ID_REGISTRO = ordenesPago.ID_REGISTRO,
                                       ID_ECHEQ = ordenesPago.ID_ECHEQ,
                                       FIRMANTES_ECHEQ = ordenesPago.FIRMANTES_ECHEQ,
                                       RAN_ID = ordenesPago.RAN_ID,
                                       SUC_DESCRIPCION = sucursales.SUC_DESCRIPCION,
                                       MPG_DESCRIPCION = modalidadPago.MPG_DESCRIPCION,
                                       EST_DESCRIPCION = estados.EST_DESCRIPCION
                                   }).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en OrdenesPorClienteBeneficiarioNumero - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> OrdenesPorBeneficiario(int tipoBen, double numBen)
        {
            _logger.LogInformation($"Consultando Ordenes por beneficiario ({tipoBen}, {numBen})");

            try
            {
                var ordenes = await context.ORDENESPAGO
                    .Where(opg => opg.TDD_BNF_ID == tipoBen && opg.BNF_NUMDOC == Convert.ToDecimal(numBen)).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(ordenes);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en OrdenesPorBeneficiario - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> OrdenesPorNumero(double id)
        {
            _logger.LogInformation($"Consultando Ordenes por numero ({id})");

            try
            {
                var ordenes = await context.ORDENESPAGO
                    .Where(opg => opg.OPG_ID >= Convert.ToDecimal(id)).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(ordenes);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en OrdenesPorNumero - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> Orden(double Id)
        {
            _logger.LogInformation($"Consultando Orden ({Id})");

            try
            {
                var query = await (from ordenesPago in context.ORDENESPAGO
                                   join monedas in context.MONEDAS on ordenesPago.MND_DEBITO equals monedas.MND_ID
                                   where ordenesPago.OPG_ID == Convert.ToDecimal(Id)
                                   select new
                                   {
                                       OPG_ID = ordenesPago.OPG_ID,
                                       TDD_BNF_ID = ordenesPago.TDD_BNF_ID,
                                       BNF_NUMDOC = ordenesPago.BNF_NUMDOC,
                                       TDD_CLT_ID = ordenesPago.TDD_CLT_ID,
                                       CLT_NUMDOC = ordenesPago.CLT_NUMDOC,
                                       SUC_ENTREGA = ordenesPago.SUC_ENTREGA,
                                       OPG_IDOPGCLI = ordenesPago.OPG_IDOPGCLI,
                                       OPG_ORDENALT = ordenesPago.OPG_ORDENALT,
                                       OPG_IMP_DEBITO = ordenesPago.OPG_IMP_DEBITO,
                                       OPG_IMP_PAGO = ordenesPago.OPG_IMP_PAGO,
                                       SUC_CUENTADEBITO = ordenesPago.SUC_CUENTADEBITO,
                                       TDC_DEBITO = ordenesPago.TDC_DEBITO,
                                       MND_DEBITO = ordenesPago.MND_DEBITO,
                                       CTA_CUENTADEBITO = ordenesPago.CTA_CUENTADEBITO,
                                       BCO_CUENTAPAGO = ordenesPago.BCO_CUENTAPAGO,
                                       SUC_CUENTAPAGO = ordenesPago.SUC_CUENTAPAGO,
                                       TDC_PAGO = ordenesPago.TDC_PAGO,
                                       MND_PAGO = ordenesPago.MND_PAGO,
                                       OPG_CUENTAPAGO = ordenesPago.OPG_CUENTAPAGO,
                                       OPG_AUXCBU = ordenesPago.OPG_AUXCBU,
                                       MPG_ID = ordenesPago.MPG_ID,
                                       OPG_MAR_REGCHQ = ordenesPago.OPG_MAR_REGCHQ,
                                       OPG_FEC_PAGO = ordenesPago.OPG_FEC_PAGO,
                                       OPG_FEC_PAGODIFERIDO = ordenesPago.OPG_FEC_PAGODIFERIDO,
                                       OPG_NUMCOMPROBANTE = ordenesPago.OPG_NUMCOMPROBANTE,
                                       COM_ID = ordenesPago.COM_ID,
                                       USR_FIRMANTE1 = ordenesPago.USR_FIRMANTE1,
                                       USR_FIRMANTE2 = ordenesPago.USR_FIRMANTE2,
                                       USR_FIRMANTE3 = ordenesPago.USR_FIRMANTE3,
                                       EST_ESTADOACTUAL = ordenesPago.EST_ESTADOACTUAL,
                                       OPG_NUM_ENVIO = ordenesPago.OPG_NUM_ENVIO,
                                       ENV_ID = ordenesPago.ENV_ID,
                                       OPG_NRORECIBO = ordenesPago.OPG_NRORECIBO,
                                       PATH_DOCU = ordenesPago.PATH_DOCU,
                                       ID_REGISTRO = ordenesPago.ID_REGISTRO,
                                       ID_ECHEQ = ordenesPago.ID_ECHEQ,
                                       FIRMANTES_ECHEQ = ordenesPago.FIRMANTES_ECHEQ,
                                       RAN_ID = ordenesPago.RAN_ID,
                                       MND_ID = monedas.MND_ID,
                                       MND_DESCRIPCION = monedas.MND_DESCRIPCION,
                                       MND_SIMBOLO = monedas.MND_SIMBOLO,
                                       MND_COBIS = monedas.MND_COBIS
                                   }).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Orden - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> Cheque(double nro)
        {
            _logger.LogInformation($"Consultando Cheque ({nro})");

            try
            {
                var opg = await (from ordenesPago in context.ORDENESPAGO
                                 join monedas in context.MONEDAS
                                 on ordenesPago.MND_DEBITO equals monedas.MND_ID
                                 where ordenesPago.OPG_NUMCOMPROBANTE == Convert.ToDecimal(nro)
                                 select new
                                 {
                                     OPG_ID = ordenesPago.OPG_ID,
                                     TDD_BNF_ID = ordenesPago.TDD_BNF_ID,
                                     BNF_NUMDOC = ordenesPago.BNF_NUMDOC,
                                     TDD_CLT_ID = ordenesPago.TDD_CLT_ID,
                                     CLT_NUMDOC = ordenesPago.CLT_NUMDOC,
                                     SUC_ENTREGA = ordenesPago.SUC_ENTREGA,
                                     OPG_IDOPGCLI = ordenesPago.OPG_IDOPGCLI,
                                     OPG_ORDENALT = ordenesPago.OPG_ORDENALT,
                                     OPG_IMP_DEBITO = ordenesPago.OPG_IMP_DEBITO,
                                     OPG_IMP_PAGO = ordenesPago.OPG_IMP_PAGO,
                                     SUC_CUENTADEBITO = ordenesPago.SUC_CUENTADEBITO,
                                     TDC_DEBITO = ordenesPago.TDC_DEBITO,
                                     MND_DEBITO = ordenesPago.MND_DEBITO,
                                     CTA_CUENTADEBITO = ordenesPago.CTA_CUENTADEBITO,
                                     BCO_CUENTAPAGO = ordenesPago.BCO_CUENTAPAGO,
                                     SUC_CUENTAPAGO = ordenesPago.SUC_CUENTAPAGO,
                                     TDC_PAGO = ordenesPago.TDC_PAGO,
                                     MND_PAGO = ordenesPago.MND_PAGO,
                                     OPG_CUENTAPAGO = ordenesPago.OPG_CUENTAPAGO,
                                     OPG_AUXCBU = ordenesPago.OPG_AUXCBU,
                                     MPG_ID = ordenesPago.MPG_ID,
                                     OPG_MAR_REGCHQ = ordenesPago.OPG_MAR_REGCHQ,
                                     OPG_FEC_PAGO = ordenesPago.OPG_FEC_PAGO,
                                     OPG_FEC_PAGODIFERIDO = ordenesPago.OPG_FEC_PAGODIFERIDO,
                                     OPG_NUMCOMPROBANTE = ordenesPago.OPG_NUMCOMPROBANTE,
                                     COM_ID = ordenesPago.COM_ID,
                                     USR_FIRMANTE1 = ordenesPago.USR_FIRMANTE1,
                                     USR_FIRMANTE2 = ordenesPago.USR_FIRMANTE2,
                                     USR_FIRMANTE3 = ordenesPago.USR_FIRMANTE3,
                                     EST_ESTADOACTUAL = ordenesPago.EST_ESTADOACTUAL,
                                     OPG_NUM_ENVIO = ordenesPago.OPG_NUM_ENVIO,
                                     ENV_ID = ordenesPago.ENV_ID,
                                     OPG_NRORECIBO = ordenesPago.OPG_NRORECIBO,
                                     PATH_DOCU = ordenesPago.PATH_DOCU,
                                     ID_REGISTRO = ordenesPago.ID_REGISTRO,
                                     ID_ECHEQ = ordenesPago.ID_ECHEQ,
                                     FIRMANTES_ECHEQ = ordenesPago.FIRMANTES_ECHEQ,
                                     RAN_ID = ordenesPago.RAN_ID,
                                     MND_ID = monedas.MND_ID,
                                     MND_DESCRIPCION = monedas.MND_DESCRIPCION,
                                     MND_SIMBOLO = monedas.MND_SIMBOLO,
                                     MND_COBIS = monedas.MND_COBIS
                                 }).ToListAsync();


                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(opg);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Cheque - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> ChequeAnuladoCOBIS(double nroCheque)
        {
            _logger.LogInformation($"Consultando Cheque Anulado COBIS ({nroCheque})");

            try
            {
                bool chequeAnuladoCobis = await context.CHEQUE_BAJA_COBIS.AnyAsync(chq => chq.OPG_NUMERO_CHEQUE == Convert.ToDecimal(nroCheque));

                if (chequeAnuladoCobis)
                {
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = "true";
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = "false";
                    result.Message = HttpStatusCode.OK.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ChequeAnuladoCOBIS - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> LogCheque(ChequeBajaCobis chequeBajaCobis)
        {
            _logger.LogInformation($"Insertando Log Cheque ({chequeBajaCobis})");

            try
            {
                ServicesResult resultChequeAnuladoCOBIS = await ChequeAnuladoCOBIS(Convert.ToDouble(chequeBajaCobis.OPG_NUMERO_CHEQUE));

                if (resultChequeAnuladoCOBIS.Content == "false")
                {
                    await context.CHEQUE_BAJA_COBIS.AddAsync(chequeBajaCobis);
                    await context.SaveChangesAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = "true";
                    result.Message = HttpStatusCode.OK.ToString();

                    return result;
                }

                result.Code = ((int)HttpStatusCode.NotFound).ToString();
                result.Content = "false";
                result.Message = "No se pudo realizar la inserción";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en LogCheque - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> AnularOPG(decimal id)
        {
            _logger.LogInformation($"Anulando OPG ({id})");

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var opg = await context.ORDENESPAGO
                    .FirstOrDefaultAsync(orden => orden.OPG_ID == id);

                if (opg != null)
                {
                    opg.EST_ESTADOACTUAL = "BB";

                    context.ORDENESPAGO.Update(opg);
                    await context.SaveChangesAsync();

                    Historial historial = new Historial();

                    historial.OPG_ID = id;
                    historial.EST_ID = "BB";
                    historial.HIS_FEC = DateTime.Now;

                    await context.HISTORIAL.AddAsync(historial);
                    await context.SaveChangesAsync();

                    var ok = await _seguridadService.InsertarLog((int)CodigosTareas.OrdenesPago_Anulacion, Globals.user, " ", $"Opg : {id}");
                    if (ok.Content == null)
                        throw new Exception("Error al insertar log.");

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = JsonConvert.SerializeObject(true);
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Content = "false";
                    result.Message = "No se encontro orden de pago";

                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Error en AnularOPG - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            finally
            {
                await transaction.DisposeAsync();
                await context.DisposeAsync();
            }

            return result;
        }
        public async Task<ServicesResult> AnularOPGporEnvio(int tipoDoc, double numDoc, double envio)
        {
            _logger.LogInformation($"Anulando OPG por envio ({tipoDoc}, {numDoc}, {envio})");

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                Historial historial = new Historial();

                var opg = await context.ORDENESPAGO
                    .Where(op =>
                        op.ENV_ID == Convert.ToDecimal(envio) &&
                        op.TDD_CLT_ID == tipoDoc &&
                        op.CLT_NUMDOC == Convert.ToDecimal(numDoc) &&
                        (op.EST_ESTADOACTUAL == (op.MPG_ID == 1 || op.MPG_ID == 9 ? "L" : "A") ||
                         op.EST_ESTADOACTUAL == (op.MPG_ID == 1 || op.MPG_ID == 9 ? "L" : "I"))
                    ).ToListAsync();

                if (opg != null)
                {
                    foreach (var op in opg)
                    {
                        historial.OPG_ID = op.OPG_ID;
                        historial.EST_ID = "BB";
                        historial.HIS_FEC = DateTime.Now;

                        await context.HISTORIAL.AddAsync(historial);
                        await context.SaveChangesAsync();

                        op.EST_ESTADOACTUAL = "BB";

                        context.ORDENESPAGO.Update(op);
                        await context.SaveChangesAsync();
                    };

                    var ok = await _seguridadService.InsertarLog((int)CodigosTareas.OrdenesPago_Anulacion, Globals.user, " ", $"Envío : {envio}");
                    if (ok.Content == null)
                        throw new Exception("Error al insertar log.");

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = JsonConvert.SerializeObject(true);
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Content = "false";
                    result.Message = "No se encontro orden de pago";
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Error en AnularOPGporEnvio - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            finally
            {
                await transaction.DisposeAsync();
                await context.DisposeAsync();
            }

            return result;
        }
        public async Task<ServicesResult> NuevaOrden(OrdenespagoDto ordenesPagoDto)
        {
            _logger.LogInformation($"Agregando orden ({ordenesPagoDto})");

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {

                decimal Corriente = 0;


                if (ordenesPagoDto.OPG_MAR_REGCHQ == null)
                {
                    ordenesPagoDto.OPG_MAR_REGCHQ = null;
                }

                if (ordenesPagoDto.OPG_FEC_PAGODIFERIDO == null)
                {
                    ordenesPagoDto.OPG_FEC_PAGODIFERIDO = null;
                }
                else
                {
                    ordenesPagoDto.OPG_FEC_PAGODIFERIDO = ordenesPagoDto.OPG_FEC_PAGODIFERIDO;
                }

                Corriente = context.Database.SqlQuery<int>($"select sq_opg_id.nextval from PARAMETROS where PRM_ID=1").FirstOrDefault();


                if (ordenesPagoDto.OPG_CUENTAPAGO == null)
                {
                    ordenesPagoDto.OPG_CUENTAPAGO = null;
                    ordenesPagoDto.BCO_CUENTAPAGO = null;
                    ordenesPagoDto.SUC_CUENTAPAGO = null;
                    ordenesPagoDto.TDC_PAGO = null;
                    //ordenesPagoDto.MND_PAGO = null;
                }

                ORDENESPAGO opg = _mapper.Map<ORDENESPAGO>(ordenesPagoDto);

                context.ORDENESPAGO.Add(opg);

                var ok = await _seguridadService.InsertarLog((int)CodigosTareas.OrdenesPago_Agregar, Globals.user, " ", $"{Corriente}");
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
                await transaction.RollbackAsync();
                _logger.LogError($"Error en NuevaOrden - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            finally
            {
                await transaction.DisposeAsync();
                await context.DisposeAsync();
            }

            return result;
        }
        public async Task<ServicesResult> EliminarOrdenPago(double id)
        {
            _logger.LogInformation($"Eliminando orden ({id})");

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var orden = await context.ORDENESPAGO.FirstOrDefaultAsync(opg => opg.OPG_ID == Convert.ToDecimal(id));

                if (orden != null)
                {
                    context.ORDENESPAGO.Remove(orden);

                    var ok = await _seguridadService.InsertarLog((int)CodigosTareas.OrdenesPago_Eliminar, Globals.user, $"{id}", " ");
                    if (ok.Content == null)
                        throw new Exception("Error al insertar log.");

                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = JsonConvert.SerializeObject(true);
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Content = "false";
                    result.Message = "No se encontro orden de pago";
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Error en EliminarOrdenPago - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            finally
            {
                await transaction.DisposeAsync();
                await context.DisposeAsync();
            }

            return result;
        }
        public async Task<ServicesResult> ModificarOrden(OrdenespagoDto ordenesPagoDto)
        {
            _logger.LogInformation($"Modificando orden ({ordenesPagoDto})");

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var orden = await context.ORDENESPAGO.FirstOrDefaultAsync(opg => opg.OPG_ID == ordenesPagoDto.OPG_ID);

                if (orden != null)
                {
                    if (ordenesPagoDto.OPG_MAR_REGCHQ == null)
                    {
                        ordenesPagoDto.OPG_MAR_REGCHQ = null;
                    }

                    if (ordenesPagoDto.OPG_FEC_PAGODIFERIDO == null)
                    {
                        ordenesPagoDto.OPG_FEC_PAGODIFERIDO = null;
                    }
                    else
                    {
                        ordenesPagoDto.OPG_FEC_PAGODIFERIDO = ordenesPagoDto.OPG_FEC_PAGODIFERIDO;
                    }

                    if (ordenesPagoDto.OPG_CUENTAPAGO == null)
                    {
                        ordenesPagoDto.OPG_CUENTAPAGO = null;
                        ordenesPagoDto.BCO_CUENTAPAGO = null;
                        ordenesPagoDto.SUC_CUENTAPAGO = null;
                        ordenesPagoDto.TDC_PAGO = null;
                        //ordenesPagoDto.MND_PAGO = null;
                    }

                    orden.OPG_IDOPGCLI = ordenesPagoDto.OPG_IDOPGCLI;
                    orden.SUC_ENTREGA = ordenesPagoDto.SUC_ENTREGA;
                    orden.OPG_ORDENALT = ordenesPagoDto.OPG_ORDENALT;
                    orden.OPG_IMP_DEBITO = ordenesPagoDto.OPG_IMP_DEBITO;
                    orden.OPG_IMP_PAGO = ordenesPagoDto.OPG_IMP_PAGO;
                    orden.MPG_ID = ordenesPagoDto.MPG_ID;
                    orden.EST_ESTADOACTUAL = ordenesPagoDto.EST_ESTADOACTUAL;
                    orden.OPG_CUENTAPAGO = ordenesPagoDto.OPG_CUENTAPAGO;
                    orden.OPG_FEC_PAGO = ordenesPagoDto.OPG_FEC_PAGO;
                    orden.USR_FIRMANTE1 = ordenesPagoDto.USR_FIRMANTE1;
                    orden.USR_FIRMANTE2 = ordenesPagoDto.USR_FIRMANTE2;
                    orden.USR_FIRMANTE3 = ordenesPagoDto.USR_FIRMANTE3;
                    orden.OPG_FEC_PAGODIFERIDO = ordenesPagoDto.OPG_FEC_PAGODIFERIDO;
                    orden.OPG_MAR_REGCHQ = ordenesPagoDto.OPG_MAR_REGCHQ;
                    orden.SUC_CUENTADEBITO = ordenesPagoDto.SUC_CUENTADEBITO;
                    orden.TDC_DEBITO = ordenesPagoDto.TDC_DEBITO;
                    orden.MND_DEBITO = ordenesPagoDto.MND_DEBITO;
                    orden.BCO_CUENTAPAGO = ordenesPagoDto.BCO_CUENTAPAGO;
                    orden.SUC_CUENTAPAGO = ordenesPagoDto.SUC_CUENTAPAGO;
                    orden.TDC_PAGO = ordenesPagoDto.TDC_PAGO;
                    orden.MND_PAGO = ordenesPagoDto.MND_PAGO;

                    ORDENESPAGO opg = _mapper.Map<ORDENESPAGO>(ordenesPagoDto);

                    context.ORDENESPAGO.Update(opg);

                    var ok = await _seguridadService.InsertarLog((int)CodigosTareas.OrdenesPago_Modificar, Globals.user, $"{ordenesPagoDto.OPG_ID}", $"{ordenesPagoDto.OPG_ID}");
                    if (ok.Content == null)
                        throw new Exception("Error al insertar log.");

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = JsonConvert.SerializeObject(true);
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Content = "false";
                    result.Message = "No se encontro orden de pago";
                }

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Error en ModificarOrden - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            finally
            {
                await transaction.DisposeAsync();
                await context.DisposeAsync();
            }

            return result;
        }
        public async Task<ServicesResult> CantOpgsEnvio(int tipoDoc, double numero, double envio)
        {
            _logger.LogInformation($"Consultando Cantidad Opgs Envio ({tipoDoc}, {numero}, {envio})");

            try
            {
                var opg = await context.ORDENESPAGO
                    .FirstOrDefaultAsync(opg => opg.TDD_CLT_ID == tipoDoc && opg.CLT_NUMDOC == Convert.ToDecimal(numero) && opg.ENV_ID == Convert.ToDecimal(envio));

                if (opg != null)
                {
                    int cantidad = await context.ORDENESPAGO
                    .CountAsync(opg => opg.TDD_CLT_ID == tipoDoc && opg.CLT_NUMDOC == Convert.ToDecimal(numero) && opg.ENV_ID == Convert.ToDecimal(envio));

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = $"{cantidad}";
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Content = "false";
                    result.Message = "No se encontro orden de pago";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en CantOpgsEnvio - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> CantOpgsEnvioAnulables(int tipoDoc, double numero, double envio)
        {
            _logger.LogInformation($"Consultando cantidad opgs envio anulables ({tipoDoc}, {numero}, {envio})");

            try
            {
                var cantOpgsEnvioAnulables = await (from ordenesPago in context.ORDENESPAGO
                                                    join monedas in context.MONEDAS on ordenesPago.MND_DEBITO equals monedas.MND_ID
                                                    join cuentas in context.CUENTAS on ordenesPago.CTA_CUENTADEBITO equals cuentas.CTA_SERVICIO
                                                    where ordenesPago.TDD_CLT_ID == Convert.ToDecimal(tipoDoc)
                                                    && ordenesPago.CLT_NUMDOC == Convert.ToDecimal(numero)
                                                    && ordenesPago.ENV_ID == Convert.ToDecimal(envio)
                                                    && (
                                                         (new[] { 1m, 9m, 5m }.Contains(ordenesPago.MPG_ID) && new[] { "PI", "I", "A", "L" }.Contains(ordenesPago.EST_ESTADOACTUAL))
                                                         || (new[] { 2m, 4m }.Contains(ordenesPago.MPG_ID) && new[] { "PI", "I" }.Contains(ordenesPago.EST_ESTADOACTUAL))
                                                         || (new[] { 6m, 8m }.Contains(ordenesPago.MPG_ID) && new[] { "PI", "I", "PE" }.Contains(ordenesPago.EST_ESTADOACTUAL))
                                                       )
                                                    select ordenesPago).CountAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = cantOpgsEnvioAnulables.ToString();
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en CantOpgsEnvioAnulables - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> OpgEmitidas_Rp(FiltroOpgEmitidasRpDto filtroOpgEmitidasRpDto)
        {
            _logger.LogInformation($"Consultando opg emitidas ({filtroOpgEmitidasRpDto})");

            try
            {
                IQueryable<OrdenesPagoEmitidasDto> query = from ordenesPago in context.ORDENESPAGO
                                                           join beneficiarios in context.BENEFICIARIOS on ordenesPago.TDD_BNF_ID equals beneficiarios.TDD_BNF_ID
                                                           join modalidadPago in context.MODALIDADPAGO on ordenesPago.MPG_ID equals modalidadPago.MPG_ID
                                                           join clientes in context.CLIENTES on beneficiarios.TDD_CLT_ID equals clientes.TDD_CLT_ID
                                                           join estados in context.ESTADOS on ordenesPago.EST_ESTADOACTUAL equals estados.EST_ID
                                                           where ordenesPago.BNF_NUMDOC == beneficiarios.BNF_NUMDOC
                                                           && ordenesPago.TDD_CLT_ID == beneficiarios.TDD_CLT_ID
                                                           && ordenesPago.CLT_NUMDOC == beneficiarios.CLT_NUMDOC
                                                           && beneficiarios.CLT_NUMDOC == clientes.CLT_NUMDOC
                                                           orderby (ordenesPago.OPG_NUMCOMPROBANTE)
                                                           select new OrdenesPagoEmitidasDto
                                                           {
                                                               OPG_ID = ordenesPago.OPG_ID,
                                                               TDD_CLT_ID = ordenesPago.TDD_CLT_ID,
                                                               CLT_NUMDOC = ordenesPago.CLT_NUMDOC,
                                                               SUC_ENTREGA = ordenesPago.SUC_ENTREGA,
                                                               OPG_IMP_PAGO = ordenesPago.OPG_IMP_PAGO,
                                                               OPG_CUENTAPAGO = ordenesPago.OPG_CUENTAPAGO,
                                                               MPG_ID = ordenesPago.MPG_ID,
                                                               OPG_FEC_PAGO = ordenesPago.OPG_FEC_PAGO,
                                                               OPG_NUMCOMPROBANTE = ordenesPago.OPG_NUMCOMPROBANTE,
                                                               BNF_RAZONSOC = beneficiarios.BNF_RAZONSOC,
                                                               MPG_DESCRIPCION = modalidadPago.MPG_DESCRIPCION,
                                                               CLT_RAZONSOC = clientes.CLT_RAZONSOC,
                                                               EST_DESCRIPCION = estados.EST_DESCRIPCION,
                                                               EST_ESTADOACTUAL = ordenesPago.EST_ESTADOACTUAL,
                                                               ENV_ID = ordenesPago.ENV_ID
                                                           };

                if (filtroOpgEmitidasRpDto.FECHA_DESDE_PAGO.HasValue && filtroOpgEmitidasRpDto.FECHA_HASTA_PAGO.HasValue)
                {
                    query = query.Where(x => x.OPG_FEC_PAGO >= filtroOpgEmitidasRpDto.FECHA_DESDE_PAGO && x.OPG_FEC_PAGO <= filtroOpgEmitidasRpDto.FECHA_HASTA_PAGO);
                }

                if (!string.IsNullOrEmpty(filtroOpgEmitidasRpDto.EST_ESTADOACTUAL))
                {
                    query = query.Where(x => x.EST_ESTADOACTUAL == filtroOpgEmitidasRpDto.EST_ESTADOACTUAL);
                }
                else
                {
                    query = query.Where(x => x.EST_ESTADOACTUAL != "BC" && x.EST_ESTADOACTUAL != "BB");
                }

                if (filtroOpgEmitidasRpDto.TDD_CLIENTE_ID.HasValue && filtroOpgEmitidasRpDto.TDD_CLIENTE_ID.Value != 0 && filtroOpgEmitidasRpDto.CLIENTE_NUMDOC.HasValue && filtroOpgEmitidasRpDto.CLIENTE_NUMDOC.Value != 0)
                {
                    query = query.Where(x => x.TDD_CLT_ID == filtroOpgEmitidasRpDto.TDD_CLIENTE_ID && x.CLT_NUMDOC == filtroOpgEmitidasRpDto.CLIENTE_NUMDOC);
                }

                if (filtroOpgEmitidasRpDto.SUC_ENTREGA.HasValue && filtroOpgEmitidasRpDto.SUC_ENTREGA.Value != 0)
                {
                    query = query.Where(x => x.SUC_ENTREGA == filtroOpgEmitidasRpDto.SUC_ENTREGA);
                }

                if (filtroOpgEmitidasRpDto.MPG_ID.HasValue && filtroOpgEmitidasRpDto.MPG_ID.Value != 0)
                {
                    query = query.Where(x => x.MPG_ID == filtroOpgEmitidasRpDto.MPG_ID);

                    if (filtroOpgEmitidasRpDto.MPG_ID.Value == 6 || filtroOpgEmitidasRpDto.MPG_ID.Value == 8)
                    {
                        query = query.OrderBy(x => x.OPG_NUMCOMPROBANTE);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.MPG_ID)
                                    .ThenBy(op => op.SUC_ENTREGA)
                                    .ThenBy(op => op.TDD_CLT_ID)
                                    .ThenBy(op => op.CLT_NUMDOC)
                                    .ThenBy(op => op.OPG_ID);
                    }
                }

                if (filtroOpgEmitidasRpDto.OPG_ID.HasValue && filtroOpgEmitidasRpDto.OPG_ID.Value != 0)
                {
                    query = query.Where(x => x.OPG_ID == filtroOpgEmitidasRpDto.OPG_ID);
                }

                if (filtroOpgEmitidasRpDto.ENV_ID.HasValue && filtroOpgEmitidasRpDto.ENV_ID.Value != 0)
                {
                    query = query.Where(x => x.ENV_ID == filtroOpgEmitidasRpDto.ENV_ID);
                }

                if (filtroOpgEmitidasRpDto.OPG_NUMCOMPROBANTE.HasValue && filtroOpgEmitidasRpDto.OPG_NUMCOMPROBANTE.Value != 0)
                {
                    query = query.Where(x => x.OPG_NUMCOMPROBANTE == filtroOpgEmitidasRpDto.OPG_NUMCOMPROBANTE);
                }

                var queryFiltered = await query.ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(queryFiltered);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en OpgEmitidas_Rp - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> ReversarOPG(double cOPG_ID, string sucEnt, string user)
        {
            _logger.LogInformation($"Reversando OPG ({cOPG_ID}, {sucEnt}, {user}");

            try
            {
                var sucEntregaParam = new SqlParameter("@SUC_ENTREGA", sucEnt.Trim());
                var operadorCajaParam = new SqlParameter("@ID_OPERADOR_CAJA", user.Length > 30 ? user.Substring(0, 30) : user);
                var opgIdParam = new SqlParameter("@OPG_ID", cOPG_ID);
                var nroOperacionCajaParam = new SqlParameter("@NRO_OPERACION_CAJA", DBNull.Value);
                var funcionOpParam = new SqlParameter("@FUNCION_OP", "2");
                var datetimeCajaParam = new SqlParameter("@DATETIME_CAJA", DBNull.Value);

                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "EXEC SP_FUNCION_OP_PP3RO_EFT @SUC_ENTREGA, @ID_OPERADOR_CAJA, @OPG_ID, @NRO_OPERACION_CAJA, @FUNCION_OP, @DATETIME_CAJA";
                    command.Parameters.Add(sucEntregaParam);
                    command.Parameters.Add(operadorCajaParam);
                    command.Parameters.Add(opgIdParam);
                    command.Parameters.Add(nroOperacionCajaParam);
                    command.Parameters.Add(funcionOpParam);
                    command.Parameters.Add(datetimeCajaParam);

                    context.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var resultList = new List<Dictionary<string, object>>();

                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            resultList.Add(row);
                        }

                        foreach (var row in resultList)
                        {
                            int errId = Convert.ToInt32(row.Values.ElementAt(0));
                            string descripcion = row.Values.ElementAt(1).ToString();

                            if (errId != 0)
                            {
                                result.Code = errId.ToString();
                                result.Message = descripcion;
                                result.Content = "True";
                            }
                            else
                            {
                                result.Code = ((int)HttpStatusCode.OK).ToString();
                                result.Message = HttpStatusCode.OK.ToString();
                                result.Content = "True";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ReversarOPG - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }
        public async Task<ServicesResult> OpgPorEnvio(decimal? tipoDoc, decimal? numDoc, decimal? numEnvio)
        {
            _logger.LogInformation($"Consultando OPG por envio ({tipoDoc}, {numDoc}, {numEnvio}");
            try
            {
                var opgs = await context.ORDENESPAGO
                .Where(op => op.CLT_NUMDOC == numDoc && op.ENV_ID == numEnvio && op.TDD_CLT_ID == tipoDoc)
                .Select(op => new
                {
                    op.OPG_ID,
                    op.TDD_BNF_ID,
                    op.BNF_NUMDOC,
                    op.TDD_CLT_ID,
                    op.CLT_NUMDOC,
                    op.SUC_ENTREGA,
                    op.OPG_IDOPGCLI,
                    op.OPG_ORDENALT,
                    op.OPG_IMP_DEBITO,
                    op.OPG_IMP_PAGO,
                    op.SUC_CUENTADEBITO,
                    op.TDC_DEBITO,
                    op.MND_DEBITO,
                    op.CTA_CUENTADEBITO,
                    op.BCO_CUENTAPAGO,
                    op.SUC_CUENTAPAGO,
                    op.TDC_PAGO,
                    op.MND_PAGO,
                    op.OPG_CUENTAPAGO,
                    op.OPG_AUXCBU,
                    op.MPG_ID,
                    op.OPG_MAR_REGCHQ,
                    op.OPG_FEC_PAGO,
                    op.OPG_FEC_PAGODIFERIDO,
                    op.OPG_NUMCOMPROBANTE,
                    op.COM_ID,
                    op.USR_FIRMANTE1,
                    op.USR_FIRMANTE2,
                    op.USR_FIRMANTE3,
                    op.EST_ESTADOACTUAL,
                    op.OPG_NUM_ENVIO,
                    op.ENV_ID,
                    op.OPG_NRORECIBO,
                    op.PATH_DOCU,
                    op.ID_REGISTRO,
                    op.ID_ECHEQ,
                    op.FIRMANTES_ECHEQ,
                    op.RAN_ID
                }).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(opgs);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en OpgPorEnvio - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }

        public async Task<ServicesResult> ConsultaOrdenesPagoReporte(FiltrosReporteOpg filtros)
        {
            _logger.LogInformation($"ConsultaOrdenesPagoReporte - Consultando OPGs");

            try
            {
                var aux = context.ORDENESPAGO.AsQueryable();

                
                if (filtros.tipoDoc != null)
                {
                    aux = aux.Where(x => x.TDD_CLT_ID == filtros.tipoDoc);
                }

                if (filtros.documentoDesde != null)
                {
                    aux = aux.Where(x => x.CLT_NUMDOC >= filtros.documentoDesde);
                }

                if (filtros.documentoHasta != null)
                {
                    aux = aux.Where(x => x.CLT_NUMDOC <= filtros.documentoHasta);
                }

                if (filtros.modalidad != null)
                {
                    aux = aux.Where(x => x.MPG_ID == filtros.modalidad);
                }

                if (filtros.sucursalDeEntrega != null)
                {
                    aux = aux.Where(x => x.SUC_ENTREGA == filtros.sucursalDeEntrega);
                }

                if (filtros.cuentaPagoDesde != null)
                {
                    aux = aux.Where(x => x.OPG_CUENTAPAGO >= filtros.cuentaPagoDesde);
                }

                if (filtros.cuentaPagoHasta != null)
                {
                    aux = aux.Where(x => x.OPG_CUENTAPAGO <= filtros.cuentaPagoHasta);
                }

                
                var resultado = from op in aux
                                join hist in context.HISTORIAL
                                    .Where(h => (filtros.fechaDesde.HasValue ? h.HIS_FEC >= filtros.fechaDesde.Value.Date : true) &&
                                                (filtros.fechaHasta.HasValue ? h.HIS_FEC <= filtros.fechaHasta.Value.Date : true))
                                on op.OPG_ID equals hist.OPG_ID into op_hist
                                from hist in op_hist.DefaultIfEmpty() 

                                    
                                join tdClt in context.TIPODOC on op.TDD_CLT_ID equals tdClt.TDD_ID
                                join tdBnf in context.TIPODOC on op.TDD_BNF_ID equals tdBnf.TDD_ID
                                join suc in context.SUCURSALES on op.SUC_ENTREGA equals suc.SUC_ID
                                join est in context.ESTADOS on op.EST_ESTADOACTUAL equals est.EST_ID
                                join mp in context.MODALIDADPAGO on op.MPG_ID equals mp.MPG_ID

                                
                                where string.IsNullOrEmpty(filtros.estado) || op.EST_ESTADOACTUAL == filtros.estado

                                
                                group new { op, hist, tdClt, tdBnf, suc, est, mp } by op.OPG_ID into grouped
                                select new ReporteOpgDto
                                {
                                    Id = grouped.Key,
                                    Tipo_Doc_Cliente = grouped.Select(g => g.tdClt.TDD_DESCRIPCIONABREV).FirstOrDefault(),
                                    Cliente = grouped.Select(g => g.op.CLT_NUMDOC).FirstOrDefault(),
                                    Tipo_Doc_Beneficiario = grouped.Select(g => g.tdBnf.TDD_DESCRIPCIONABREV).FirstOrDefault(),
                                    Beneficiario = grouped.Select(g => g.op.BNF_NUMDOC).FirstOrDefault(),
                                    Sucursal = grouped.Select(g => g.suc.SUC_DESCRIPCION).FirstOrDefault(),
                                    Monto = "$" + grouped.Select(g => g.op.OPG_IMP_PAGO).FirstOrDefault().ToString("0.00"),
                                    Cuenta_Pago = grouped.Select(g => g.op.OPG_CUENTAPAGO).FirstOrDefault(),
                                    Mod_Pago = grouped.Select(g => g.mp.MPG_DESCRIPCION).FirstOrDefault(),
                                    Estado = grouped.Select(g => g.est.EST_DESCRIPCION).FirstOrDefault()
                                };

                resultado = resultado.OrderBy(x => x.Id);
                var ordenesPago = await resultado.ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(ordenesPago);

            }
            catch (Exception e)
            {
                _logger.LogError($"Error en la consulta de OPGs - {e.Message}");
                result.Code = ((int)HttpStatusCode.InternalServerError).ToString();
                result.Message = HttpStatusCode.InternalServerError.ToString();
            }

            return result;
        }

        public async Task<ServicesResult> DetalleOpgAfectadasAComisionesCobradas(FiltroComisionesCobradasDto filtroComisionesCobradasDto)
        {
            _logger.LogInformation($"Obteniendo detalle opg afectadas a comisiones cobradas");

            try
            {

                var listComisiones = await context.COMISIONES
                    .Where(c => c.COM_FECHA >= filtroComisionesCobradasDto.comFechaDesde
                                         && c.COM_FECHA <= filtroComisionesCobradasDto.comFechaHasta && c.TDD_CLT_ID == filtroComisionesCobradasDto.tipoDocClt
                                          && c.CLT_NUMDOC == filtroComisionesCobradasDto.numDocClt)
                    .Select(c => c.COM_ID)
                    .ToListAsync();

                var query = await (from ordenesPago in context.ORDENESPAGO
                                   join historial in context.HISTORIAL on ordenesPago.OPG_ID equals historial.OPG_ID
                                   join cliente in context.CLIENTES on ordenesPago.CLT_NUMDOC equals cliente.CLT_NUMDOC
                                   join mp in context.MODALIDADPAGO on ordenesPago.MPG_ID equals mp.MPG_ID
                                   join beneficiario in context.BENEFICIARIOS
                                       on new { ordenesPago.TDD_BNF_ID, ordenesPago.BNF_NUMDOC, ordenesPago.TDD_CLT_ID, ordenesPago.CLT_NUMDOC }
                                       equals new { beneficiario.TDD_BNF_ID, beneficiario.BNF_NUMDOC, beneficiario.TDD_CLT_ID, beneficiario.CLT_NUMDOC } into ben
                                   from b in ben.DefaultIfEmpty() // Left join
                                   where historial.EST_ID == "A"                                        
                                        && listComisiones.Contains(ordenesPago.COM_ID.Value)
                                   orderby ordenesPago.COM_ID, ordenesPago.MPG_ID, ordenesPago.OPG_ID
                                   select new
                                   {
                                       OPG_ID = ordenesPago.OPG_ID,
                                       TDD_BNF_ID = ordenesPago.TDD_BNF_ID,
                                       BNF_NUMDOC = ordenesPago.BNF_NUMDOC,
                                       TDD_CLT_ID = ordenesPago.TDD_CLT_ID,
                                       CLT_NUMDOC = ordenesPago.CLT_NUMDOC,
                                       SUC_ENTREGA = ordenesPago.SUC_ENTREGA,
                                       OPG_IDOPGCLI = ordenesPago.OPG_IDOPGCLI,
                                       OPG_ORDENALT = ordenesPago.OPG_ORDENALT,
                                       OPG_IMP_DEBITO = ordenesPago.OPG_IMP_DEBITO,
                                       OPG_IMP_PAGO = ordenesPago.OPG_IMP_PAGO,
                                       SUC_CUENTADEBITO = ordenesPago.SUC_CUENTADEBITO,
                                       TDC_DEBITO = ordenesPago.TDC_DEBITO,
                                       MND_DEBITO = ordenesPago.MND_DEBITO,
                                       CTA_CUENTADEBITO = ordenesPago.CTA_CUENTADEBITO,
                                       BCO_CUENTAPAGO = ordenesPago.BCO_CUENTAPAGO,
                                       SUC_CUENTAPAGO = ordenesPago.SUC_CUENTAPAGO,
                                       TDC_PAGO = ordenesPago.TDC_PAGO,
                                       MND_PAGO = ordenesPago.MND_PAGO,
                                       OPG_CUENTAPAGO = ordenesPago.OPG_CUENTAPAGO,
                                       OPG_AUXCBU = ordenesPago.OPG_AUXCBU,
                                       MPG_ID = ordenesPago.MPG_ID,
                                       OPG_MAR_REGCHQ = ordenesPago.OPG_MAR_REGCHQ,
                                       OPG_FEC_PAGO = ordenesPago.OPG_FEC_PAGO,
                                       OPG_FEC_PAGODIFERIDO = ordenesPago.OPG_FEC_PAGODIFERIDO,
                                       OPG_NUMCOMPROBANTE = ordenesPago.OPG_NUMCOMPROBANTE,
                                       COM_ID = ordenesPago.COM_ID,
                                       USR_FIRMANTE1 = ordenesPago.USR_FIRMANTE1,
                                       USR_FIRMANTE2 = ordenesPago.USR_FIRMANTE2,
                                       USR_FIRMANTE3 = ordenesPago.USR_FIRMANTE3,
                                       EST_ESTADOACTUAL = ordenesPago.EST_ESTADOACTUAL,
                                       OPG_NUM_ENVIO = ordenesPago.OPG_NUM_ENVIO,
                                       ENV_ID = ordenesPago.ENV_ID,
                                       OPG_NRORECIBO = ordenesPago.OPG_NRORECIBO,
                                       PATH_DOCU = ordenesPago.PATH_DOCU,
                                       ID_REGISTRO = ordenesPago.ID_REGISTRO,
                                       ID_ECHEQ = ordenesPago.ID_ECHEQ,
                                       FIRMANTES_ECHEQ = ordenesPago.FIRMANTES_ECHEQ,
                                       RAN_ID = ordenesPago.RAN_ID,
                                       MPG_DESCRIPCION = mp.MPG_DESCRIPCION,
                                       BNF_RAZONSOC = b != null ? b.BNF_RAZONSOC : null
                                   }).ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(query);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error en DetalleOpgAfectadasAComisionesCobradas - {e.Message}");
                result.Code = ((int)HttpStatusCode.InternalServerError).ToString();
                result.Message = HttpStatusCode.InternalServerError.ToString();
            }

            return result;
        }
    }
}
