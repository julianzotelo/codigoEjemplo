using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{

    [ApiController]
    [Route("api/Modalidad")]
    public class ModalidadController : ControllerBase
    {

        private readonly Pp3roContext context;
        private ModalidadService modalidadService;
        private readonly ILogger<ModalidadService> _logger;
        private readonly IMapper _mapper;
        public ModalidadController(Pp3roContext context,SeguridadService seguridadService, HttpContextAccessor httpContextAccessor, ILogger<ModalidadService> logger, IMapper mapper)
        {
            this.context = context;
            this.modalidadService = new ModalidadService(context, httpContextAccessor, seguridadService, logger, mapper);
        }

        [HttpGet("ModalidadesPorCliente")]
        public async Task<ServicesResult> ModalidadesPorCliente(decimal id, decimal Numdoc)
        {
            return await modalidadService.ModalidadesPorCliente(id, Numdoc);
        }

        [HttpGet("Modalidad")]
        public async Task<ServicesResult> Modalidad(int tipo, decimal numero, int id)
        {
            return await modalidadService.Modalidad(tipo, numero, id);
        }

        [HttpPost("NuevaModalidad")]
        public async Task<ServicesResult> NuevaModalidad([FromBody] Clientemodalidadpago clientemodalidadpago)
        {
            return await modalidadService.NuevaModalidad(clientemodalidadpago);
        }

        [HttpDelete("EliminarModalidad")]
        public async Task<ServicesResult> EliminarModalidad(int Tipo, decimal numero, int Id)
        {
            return await modalidadService.EliminarModalidad(Tipo,  numero, Id);
        }

        [HttpPut("ActualizarModalidad")]
        public async Task<ServicesResult> ModificarModalidad([FromBody] Clientemodalidadpago clientemodalidadpago)
        {
            return await modalidadService.ModificarModalidad(clientemodalidadpago);
        }
    }

}
