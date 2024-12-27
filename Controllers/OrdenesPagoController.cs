using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{
    [ApiController]
    [Route("api/OrdenesPago")]
    public class OrdenesPagoController : ControllerBase
    {
        private readonly Pp3roContext context;
        private OrdenesPagoService ordenesPagoService;
        private readonly ILogger<OrdenesPagoService> _logger;
        private readonly IMapper _mapper;
        private EscalasServices escalasServices;

        public OrdenesPagoController(Pp3roContext context, ILogger<OrdenesPagoService> logger, SeguridadService seguridadService, IMapper mapper)
        {
            this.context = context;
            this._logger = logger;
            this._mapper = mapper;
            this.ordenesPagoService = new OrdenesPagoService(context, new HttpContextAccessor(), seguridadService, logger, mapper);
        }

        [HttpGet("OrdenesPorClienteNumero")]
        public async Task<ServicesResult> OrdenesPorClienteNumero(int tipoCli, double numCli, double numero)
        {
            return await ordenesPagoService.OrdenesPorClienteNumero(tipoCli, numCli, numero);
        }

        [HttpGet("OrdenesPorClienteBeneficiarioNumero")]
        public async Task<ServicesResult> OrdenesPorClienteBeneficiarioNumero(int tipoCli, double numCli, int tipoBen, double numBen, double numero)
        {
            return await ordenesPagoService.OrdenesPorClienteBeneficiarioNumero(tipoCli, numCli, tipoBen, numBen, numero);
        }

        [HttpGet("OrdenesPorBeneficiario")]
        public async Task<ServicesResult> OrdenesPorBeneficiario(int tipoBen, double numBen)
        {
            return await ordenesPagoService.OrdenesPorBeneficiario(tipoBen, numBen);
        }

        [HttpGet("OrdenesPorNumero")]
        public async Task<ServicesResult> OrdenesPorNumero(double id)
        {
            return await ordenesPagoService.OrdenesPorNumero(id);
        }

        [HttpGet("Orden")]
        public async Task<ServicesResult> Orden(double Id)
        {
            return await ordenesPagoService.Orden(Id);
        }

        [HttpGet("Cheque")]
        public async Task<ServicesResult> Cheque(double nro)
        {
            return await ordenesPagoService.Cheque(nro);
        }

        [HttpGet("ChequeAnuladoCOBIS")]
        public async Task<ServicesResult> ChequeAnuladoCOBIS(double nroCheque)
        {
            return await ordenesPagoService.ChequeAnuladoCOBIS(nroCheque);
        }

        [HttpPost("LogCheque")]
        public async Task<ServicesResult> LogCheque([FromBody] ChequeBajaCobis chequeBajaCobis)
        {
            return await ordenesPagoService.LogCheque(chequeBajaCobis);
        }

        [HttpGet("AnularOPG")]
        public async Task<ServicesResult> AnularOPG(decimal id)
        {
            return await ordenesPagoService.AnularOPG(id);
        }

        [HttpGet("AnularOPGporEnvio")]
        public async Task<ServicesResult> AnularOPGporEnvio(int tipoDoc, double numDoc, double envio)
        {
            return await ordenesPagoService.AnularOPGporEnvio(tipoDoc, numDoc, envio);
        }

        [HttpPost("NuevaOrden")]
        public async Task<ServicesResult> NuevaOrden([FromBody] OrdenespagoDto ordenesPagoDto)
        {
            return await ordenesPagoService.NuevaOrden(ordenesPagoDto);
        }

        [HttpDelete("EliminarOrdenPago")]
        public async Task<ServicesResult> EliminarOrdenPago(double id)
        {
            return await ordenesPagoService.EliminarOrdenPago(id);
        }

        [HttpPut("ModificarOrden")]
        public async Task<ServicesResult> ModificarOrden([FromBody] OrdenespagoDto ordenesPagoDto)
        {
            return await ordenesPagoService.ModificarOrden(ordenesPagoDto);
        }

        [HttpGet("CantOpgsEnvio")]
        public async Task<ServicesResult> CantOpgsEnvio(int tipoDoc, double numero, double envio)
        {
            return await ordenesPagoService.CantOpgsEnvio(tipoDoc, numero, envio);
        }

        [HttpGet("CantOpgsEnvioAnulables")]
        public async Task<ServicesResult> CantOpgsEnvioAnulables(int tipoDoc, double numero, double envio)
        {
            return await ordenesPagoService.CantOpgsEnvioAnulables(tipoDoc, numero, envio);
        }

        [HttpPost("OpgEmitidasRp")]
        public async Task<ServicesResult> OpgEmitidas_Rp([FromBody] FiltroOpgEmitidasRpDto filtroOpgEmitidasRpDto)
        {
            return await ordenesPagoService.OpgEmitidas_Rp(filtroOpgEmitidasRpDto);
        }

        [HttpGet("ReversarOPG")]
        public async Task<ServicesResult> ReversarOPG(double cOPG_ID, string sucEnt, string user)
        {
            return await ordenesPagoService.ReversarOPG(cOPG_ID, sucEnt, user);
        }
        [HttpGet("OpgPorEnvio")]
        public async Task<ServicesResult> OpgPorEnvio(decimal? tipoDoc, decimal? numDoc, decimal? numEnvio)
        {
            return await ordenesPagoService.OpgPorEnvio(tipoDoc, numDoc, numEnvio);
        }
        [HttpGet("ConsultaOrdenesPagoReporte")]
        public async Task<ServicesResult> ConsultaOrdenesPagoReporte([FromQuery]FiltrosReporteOpg filtros)
        {
            return await ordenesPagoService.ConsultaOrdenesPagoReporte(filtros);
        }

        [HttpPost("DetalleOpgAfectadasAComisionesCobradas")]
        public async Task<ServicesResult> DetalleOpgAfectadasAComisionesCobradas([FromBody] FiltroComisionesCobradasDto filtroComisionesCobradasDto)
        {
            return await ordenesPagoService.DetalleOpgAfectadasAComisionesCobradas(filtroComisionesCobradasDto);
        }
    }
}
