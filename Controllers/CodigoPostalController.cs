using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{
    [ApiController]
    [Route("api/CodigoPostal")]
    public class CodigoPostalController : ControllerBase
    {
        private readonly Pp3roContext context;
        private CodigoPostalService codigoPostalService;

        public CodigoPostalController(Pp3roContext context, SeguridadService seguridadService, ILogger<CodigoPostalService> logger, IMapper mapper)
        {
            this.context = context;
            this.codigoPostalService = new CodigoPostalService(context, new HttpContextAccessor(), seguridadService, logger, mapper);
        }

        [HttpGet("ConsultarCodigosPostalesPorProvincia")]
        public async Task<ServicesResult> Traer(decimal pciaId)
        {
            return await codigoPostalService.Traer(pciaId);
        }

        [HttpGet("ConsultarCodigosPostalesPorCodigo")]
        public async Task<ServicesResult> CodigoPostal(decimal pCod)
        {
            return await codigoPostalService.CodigoPostal(pCod);
        }

        [HttpPost("AltaCodigoPostal")]
        public async Task<ServicesResult> NuevoCodigoPostal([FromBody] Codigospostale codigoPostal)
        {
            return await codigoPostalService.NuevoCodigoPostal(codigoPostal);
        }

        [HttpPut("ModificacionCodigoPostal")]
        public async Task<ServicesResult> ModificarCodigoPostal([FromBody] Codigospostale codigoPostal)
        {
            return await codigoPostalService.ModificarCodigoPostal(codigoPostal);
        }

        [HttpDelete("BajaCodigoPostal")]
        public async Task<ServicesResult> EliminarCodigoPostal(decimal pCcpId)
        {
            return await codigoPostalService.EliminarCodigoPostal(pCcpId);
        }
    }
}
