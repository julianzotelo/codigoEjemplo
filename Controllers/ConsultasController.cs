using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{
    [ApiController]
    [Route("api/Consultas")]
    public class ConsultasController : ControllerBase
    {
        private readonly Pp3roContext context;
        private ConsultasService consultasService;
        private readonly ILogger<ConsultasService> _logger;
        private readonly IMapper _mapper;

        public ConsultasController(Pp3roContext context, ILogger<ConsultasService> logger, IMapper mapper)
        {
            this.context = context;
            this._logger = logger;
            this._mapper = mapper;
            this.consultasService = new ConsultasService(context, new HttpContextAccessor(), logger, mapper);
        }

        [HttpGet("tipoDocumento")]
        public async Task<ServicesResult> tipoDoc()
        {
            return await consultasService.tipoDoc();
        }

        [HttpGet("Tipo")]
        public async Task<ServicesResult> Tipo(int Id)
        {
            return await consultasService.Tipo(Id);
        }

        [HttpGet("Modalidades")]
        public async Task<ServicesResult> Modalidades()
        {
            return await consultasService.Modalidades();
        }

        [HttpGet("Sucursales")]
        public async Task<ServicesResult> sucursales()
        {
            return await consultasService.sucursales();
        }

        [HttpGet("Sucursal")]
        public async Task<ServicesResult> Sucursal(int Id)
        {
            return await consultasService.Sucursal(Id);
        }

        [HttpGet("Ivas")]
        public async Task<ServicesResult> IVAs()
        {
            return await consultasService.IVAs();
        }

        [HttpGet("Iva")]
        public async Task<ServicesResult> iva(int Id)
        {
            return await consultasService.iva(Id);
        }

        [HttpGet("ExisteCodigoPostal")]
        public async Task<ServicesResult> ExisteCodigoPostal(int cod)
        {
            return await consultasService.ExisteCodigoPostal(cod);
        }

        [HttpGet("ExisteSucursal")]
        public async Task<ServicesResult> ExisteSucursal(int cod)
        {
            return await consultasService.ExisteSucursal(cod);
        }

        [HttpGet("Estados")]
        public async Task<ServicesResult> estados()
        {
            return await consultasService.estados();
        }

        [HttpGet("EstadosVisibles")]
        public async Task<ServicesResult> EstadosVisibles()
        {
            return await consultasService.EstadosVisibles();
        }

        [HttpGet("ConIB")]
        public async Task<ServicesResult> ConIB()
        {
            return await consultasService.ConIB();
        }

        [HttpGet("ConIG")]
        public async Task<ServicesResult> ConIG()
        {
            return await consultasService.ConIG();
        }

        [HttpGet("Motivos")]
        public async Task<ServicesResult> Motivos()
        {
            return await consultasService.Motivos();
        }

        [HttpGet("tipoCuenta")]
        public async Task<ServicesResult> tipoCuenta(int idTipoCuenta)
        {
            return await consultasService.tipoCuenta(idTipoCuenta);
        }

        [HttpGet("TiposDeCuentas")]
        public async Task<ServicesResult> TiposDeCuentas()
        {
            return await consultasService.TiposDeCuentas();
        }

        [HttpGet("TiposDeCuentasCobis")]
        public async Task<ServicesResult> TiposDeCuentasCobis()
        {
            return await consultasService.TiposDeCuentasCobis();
        }

        [HttpGet("Monedas")]
        public async Task<ServicesResult> Monedas()
        {
            return await consultasService.Monedas();
        }

        [HttpGet("Provincias")]
        public async Task<ServicesResult> Provincias()
        {
            return await consultasService.Provincias();
        }
        [HttpGet("TiposConvenio")]
        public async Task<ServicesResult> TiposConvenio()
        {
            return await consultasService.TiposConvenio();
        }
        [HttpGet("TipoConvenio")]
        public async Task<ServicesResult> TipoConvenio(int? Id)
        {
            return await consultasService.TipoConvenio(Id);
        }

        [HttpGet("ConsultaAcreditacionesInterbanking")]
        public async Task<ServicesResult> ConsultaAcreditacionesInterbanking(string? TipoDocCLT, string? NumDocCLT, string? MpgId, string? EstadoActual, string? OPGID, string? EnvId, string? FechaDesde, string? FechaHasta)
        {
            return await consultasService.ConsultaAcreditacionesInterbanking(TipoDocCLT, NumDocCLT, MpgId, EstadoActual, OPGID, EnvId, FechaDesde, FechaHasta);
        }

        [HttpGet("ValidarConsultaHistorialEstados")]
        public async Task<ServicesResult> ValidarConsultaHistorialEstados(decimal? opgId, decimal? numComprobante)
        {
            return await consultasService.ValidarConsultaHistorialEstados(opgId, numComprobante);
        }

        [HttpGet("ConsultaHistorialEstado")]
        public async Task<ServicesResult> ConsultaHistorialEstado(decimal? opgId, decimal? numComprobante)
        {
            return await consultasService.ConsultaHistorialEstado(opgId, numComprobante);
        }

        [HttpGet("ConsultaHistorialEstadoTabla")]
        public async Task<ServicesResult> ConsultaHistorialEstadoTabla(decimal opgId)
        {
            return await consultasService.ConsultaHistorialEstadoTabla(opgId);
        }

        [HttpPost("ConsultaComisionesCobradas")]
        public async Task<ServicesResult> ConsultaComisionesCobradas([FromBody] FiltroComisionesCobradasDto filtroComisionesCobradasDto)
        {
            return await consultasService.ConsultaComisionesCobradas(filtroComisionesCobradasDto);
        }
    }
}
