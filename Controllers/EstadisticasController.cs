using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{
    [ApiController]
    [Route("api/Estadisticas")]
    public class EstadisticasController : ControllerBase
    {
        private readonly Pp3roContext context;
        private EstadisticasService estadisticasService;
        private readonly ILogger<EstadisticasService> _logger;
        private readonly IMapper _mapper;

        public EstadisticasController(Pp3roContext context, ILogger<EstadisticasService> logger, IMapper mapper)
        {
            this.context = context;
            this._logger = logger;
            this._mapper = mapper;
            this.estadisticasService = new EstadisticasService(context, new HttpContextAccessor(), logger, mapper);
        }

        [HttpGet("CantidadECheqPorCliente")]
        public async Task<ServicesResult> CantidadECheqPorCliente(string fecha)
        {
            return await estadisticasService.CantidadECheqPorCliente(fecha);
        }

        [HttpGet("CantidadTeftPorCliente")]
        public async Task<ServicesResult> CantidadTeftPorCliente(string fecha)
        {
            return await estadisticasService.CantidadTeftPorCliente(fecha);
        }

        [HttpGet("CantidadEfectivoPorCliente")]
        public async Task<ServicesResult> CantidadEfectivoPorCliente(string fecha)
        {
            return await estadisticasService.CantidadEfectivoPorCliente(fecha);
        }

        [HttpGet("CantidadEmpresasActivasEmitiendoECheqPorCliente")]
        public async Task<ServicesResult> CantidadEmpresasActivasEmitiendoECheqPorCliente(string fecha)
        {
            return await estadisticasService.CantidadEmpresasActivasEmitiendoECheqPorCliente(fecha);
        }

        [HttpGet("CantidadEmpresasActivasEmitiendoTeftPorCliente")]
        public async Task<ServicesResult> CantidadEmpresasActivasEmitiendoTeftPorCliente(string fecha)
        {
            return await estadisticasService.CantidadEmpresasActivasEmitiendoTeftPorCliente(fecha);
        }

        [HttpGet("ConveniosAperturadosTodasModalidadesPago")]
        public async Task<ServicesResult> ConveniosAperturadosTodasModalidadesPago(string fecha)
        {
            return await estadisticasService.ConveniosAperturadosTodasModalidadesPago(fecha);
        }
    }
}
