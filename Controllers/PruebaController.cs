
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;
using pp3.dominio.Models;
using pp3.dominio.Context;
using pp3.services.Services;
using System.Reflection.Metadata.Ecma335;
//using pp3.dominio.M;
//pp3.dominio.Context

namespace pp3.api.Controllers
{

    [ApiController]
    [Route("api/pruebas")]
    public class PruebaController : ControllerBase
    {

        private readonly Pp3roContext context;
        private PruebaService pruebaService;

        public PruebaController(Pp3roContext context)
        {
            this.context = context;
            this.pruebaService = new PruebaService(context, new HttpContextAccessor());
        }

        [HttpGet]
        public async Task<Object> ConsultaIncidente()
        {
            return await pruebaService.PruebaGet();
        }

        //[HttpGet("consultaincidenteporid")]
        //[RequiredScope(RequiredScopesConfigurationKey = "sidef:incidentes.consultar")]
        //public async Task<v_Incidente> ConsultaIncidentePorId(int id)
        //{
        //    return await incidentesService.ConsultaIncidentePorId(id);
        //}
    }
}
