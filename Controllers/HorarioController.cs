using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{
    [ApiController]
    [Route("api/Horario")]
    public class HorarioController : ControllerBase
    {
        private readonly Pp3roContext context;
        private HorarioService horarioService;
        private readonly ILogger<HorarioService> _logger;
        private readonly IMapper _mapper;

        public HorarioController(Pp3roContext context, ILogger<HorarioService> logger, IMapper mapper)
        {
            this.context = context;
            this._logger = logger;
            this._mapper = mapper;
            this.horarioService = new HorarioService(context, new HttpContextAccessor(), logger, mapper);
        }

        [HttpGet("ConsultaHorarios")]
        public async Task<ServicesResult> ConsultaHorarios()
        {
            return await horarioService.ConsultaHorarios();
        }

        [HttpGet("ModificarHorarioCorte")]
        public async Task<ServicesResult> ModificarHorarioCorte(TimeSpan horario)
        {
            return await horarioService.ModificarHorarioCorte(horario);
        }

        [HttpGet("ModificarHorarioCorteEcheq")]
        public async Task<ServicesResult> ModificarHorarioCorteEcheq(TimeSpan horario)
        {
            return await horarioService.ModificarHorarioCorteEcheq(horario);
        }
    }
}
