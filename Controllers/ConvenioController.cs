using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{
    [ApiController]
    [Route("api/Convenio")]
    public class ConvenioController : ControllerBase
    {
        private readonly Pp3roContext _context;
        private ConvenioService _convenioService;
        //private IMapper _mapper;
        private readonly ILogger<ConvenioService> _logger;
        public ConvenioController(Pp3roContext context, SeguridadService seguridadService, ILogger<ConvenioService> logger)
        {
            this._context = context;
            this._logger = logger;
            this._convenioService = new ConvenioService(context, seguridadService, logger);
        }

        [HttpPost("AltaConvenio")]
        public async Task<ServicesResult> AltaConvenio(TIPO_CONVENIO nuevoConvenio)
        {
            return await _convenioService.AltaConvenio(nuevoConvenio);
        }

        [HttpDelete("EliminarConvenio")]
        public async Task<ServicesResult> EliminarConvenio(int convenioId)
        {
            return await _convenioService.EliminarConvenio(convenioId);
        }

        [HttpPut("ModificarConvenio")]
        public async Task<ServicesResult> ModificarConvenio(TIPO_CONVENIO convenioModif)
        {
            return await _convenioService.ModificarConvenio(convenioModif);
        }

        [HttpGet("ModalidadesAsociadas")]
        public async Task<ServicesResult> ObtenerModalidadesAsociadas(int convenioId)
        {
            return await _convenioService.ObtenerModalidadesAsociadas(convenioId);
        }

        [HttpGet("ModalidadesNoAsociadas")]
        public async Task<ServicesResult> ObtenerModalidadesNoAsociadas(int convenioId)
        {
            return await _convenioService.ObtenerModalidadesNoAsociadas(convenioId);
        }

        [HttpPost("AsociarModalidadPago")]
        public async Task<ServicesResult> AsociarModalidadPago(REL_TIPOCONVENIO_MODPAGO relConvenioModPago)
        {
            return await _convenioService.AsociarModalidadPago(relConvenioModPago);
        }

        [HttpDelete("DesasociarModalidadPago")]
        public async Task<ServicesResult> DesasociarModalidadPago(decimal convenioId, decimal modalidadId)
        {
            return await _convenioService.DesasociarModalidadPago(convenioId, modalidadId);
        }
    }
}
