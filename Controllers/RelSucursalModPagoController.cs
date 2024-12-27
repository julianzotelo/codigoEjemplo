using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{
    [ApiController]
    [Route("api/RelSucursalModPago")]
    public class RelSucursalModPagoController : ControllerBase
    {
        private readonly Pp3roContext context;
        private RelSucursalModPagoService relSucursalModPagoService;

        public RelSucursalModPagoController(Pp3roContext context, SeguridadService seguridadService, ILogger<RelSucursalModPagoService> logger, IMapper mapper)
        {
            this.context = context;
            this.relSucursalModPagoService = new RelSucursalModPagoService(context, new HttpContextAccessor(), seguridadService, logger, mapper);
        }

        [HttpGet("ConsultaRelSucursalModPago")]
        public async Task<ServicesResult> ConsultaRelSucursalModPago(decimal sucursalId)
        {
            return await relSucursalModPagoService.ConsultaRelSucursalModPago(sucursalId);
        }

        [HttpPost("AltaRelSucursalModPago")]
        public async Task<ServicesResult> AltaRelSucursalModPago([FromBody] RelSucursalModpago relSucursalModpago)
        {
            return await relSucursalModPagoService.AltaRelSucursalModPago(relSucursalModpago);
        }

        [HttpDelete("BajaRelSucursalModPago")]
        public async Task<ServicesResult> BajaRelSucursalModPago(decimal sucursalId, decimal modalidadPagoId)
        {
            return await relSucursalModPagoService.BajaRelSucursalModPago(sucursalId, modalidadPagoId);
        }

        [HttpGet("ModalidadesPagoNoAsociadasSucursal")]
        public async Task<ServicesResult> ModalidadesPagoNoAsociadasSucursal(decimal sucursalId)
        {
            return await relSucursalModPagoService.ModalidadesPagoNoAsociadasSucursal(sucursalId);
        }
    }
}
