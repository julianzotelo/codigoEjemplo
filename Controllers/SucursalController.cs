using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{
    [ApiController]
    [Route("api/Sucursal")]
    public class SucursalController : ControllerBase
    {
        private readonly Pp3roContext context;
        private SucursalService sucursalService;

        public SucursalController(Pp3roContext context, SeguridadService seguridadService, ILogger<SucursalService> logger, IMapper mapper)
        {
            this.context = context;
            this.sucursalService = new SucursalService(context, new HttpContextAccessor(), seguridadService, logger, mapper);
        }

        [HttpGet("ConsultaSucursales")]
        public async Task<ServicesResult> ConsultaSucursales(int provinciaId)
        {
            return await sucursalService.ConsultaSucursales(provinciaId);
        }

        [HttpDelete("EliminarSucursal")]
        public async Task<ServicesResult> EliminarSucursal(int sucursalId)
        {
            return await sucursalService.EliminarSucursal(sucursalId);
        }

        [HttpPut("ModificarSucursal")]
        public async Task<ServicesResult> ModificarSucursal(Sucursales sucursalModif)
        {
            return await sucursalService.ModificarSucursal(sucursalModif);
        }
    }
}
