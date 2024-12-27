using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{

    [ApiController]
    [Route("api/Rangos")]
    public class RangosController : ControllerBase
    {

        private readonly Pp3roContext context;
        private RangosService rangosService;
        private readonly ILogger<RangosService> _logger;
        private readonly IMapper _mapper;
        public RangosController(Pp3roContext context, ILogger<RangosService> logger, IMapper mapper)
        {
            this.context = context; 
            this._logger = logger;
            this._mapper = mapper;
            this.rangosService = new RangosService(context, new HttpContextAccessor(), logger, mapper);
        }

        [HttpGet("consulta")]
        public async Task<ServicesResult> Consulta(decimal Tdd_Clt_Id, decimal Clt_Numdoccli, decimal Mpg_id, string? Est_id)
        {
            return await rangosService.Consulta(Tdd_Clt_Id, Clt_Numdoccli, Mpg_id, Est_id);
        }
        [HttpGet("consultaRangoCheques")]
        public async Task<ServicesResult> ConsultaRangoCheques(decimal? Tdd_Clt_Id, decimal? Clt_Numdoccli, decimal? Mpg_id, string? Est_id)
        {
            return await rangosService.ConsultaRangoCheques(Tdd_Clt_Id, Clt_Numdoccli, Mpg_id, Est_id);
        }

        [HttpGet("seleccionClienteConRp")]
        public async Task<ServicesResult> SeleccionoClienteConRP(decimal Tdd_Clt_Id, decimal Clt_Numdoccli)
        {
            return await rangosService.SeleccionoClienteConRP(Tdd_Clt_Id, Clt_Numdoccli);
        }

        [HttpGet("seleccionoReporteRango")]
        public async Task<ServicesResult> SeleccionoReporteRango(decimal? Tdd_Clt_Id, decimal? Clt_Numdoccli)
        {
            return await rangosService.SeleccionoReporteRango(Tdd_Clt_Id, Clt_Numdoccli);
        }

        [HttpGet("consultaClienteRangos")]
        public async Task<ServicesResult> ConsultaCliRangos(int rango_id)
        {
            return await rangosService.ConsultaCliRangos(rango_id);
        }

        [HttpPut("confirmarRango")]
        public async Task<ServicesResult> ConfirmarRango([FromBody] int rango_id)
        {
            return await rangosService.ConfirmarRango(rango_id);
        }

    }

}
