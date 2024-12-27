using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{

    [ApiController]
    [Route("api/Escalas")]
    public class EscalasController : ControllerBase
    {

        private readonly Pp3roContext context;
        private EscalasServices escalasServices;



        public EscalasController(Pp3roContext context, SeguridadService seguridadService, IMapper mapper, ILogger<EscalasServices> logger)
        {
            this.context = context;
            this.escalasServices = new EscalasServices(context, new HttpContextAccessor(), seguridadService,mapper, logger);
        }

        [HttpGet]
        public async Task<Object> ConsultaIncidente()
        {
            //return await escalasServices;
            return null;
        }
        [HttpGet("ComisionesPorClienteModalidad")]
        public async Task<ServicesResult> ComisionesPorClienteModalidad(int tipo, decimal numero, int moda)
        {
            return await escalasServices.ComisionesPorClienteModalidad(tipo,numero,moda);

        }
        [HttpGet("ComisionesPorCliente")]
        public async Task<ServicesResult> ComisionesPorCliente(decimal tipo, decimal numero)
        {
            return await escalasServices.ComisionesPorCliente(tipo, numero);

        }
        [HttpPost("NuevaComision")]
        public async Task<ServicesResult> NuevaComision([FromBody]Escalascomision escalascomision)
        {
            return await escalasServices.NuevaComision(escalascomision);

        }
        [HttpGet("Comision")]
        public async Task<ServicesResult> Comision(int tipo, decimal numero, int moda, decimal hasta)
        {
            return await escalasServices.Comision(tipo, numero, moda,hasta);

        }
        [HttpDelete("EliminarComision")]
        public async Task<ServicesResult> EliminarComision(int tipo, decimal numero, int moda, decimal hasta)
        {
            return await escalasServices.EliminarComision(tipo, numero, moda,hasta);

        }
        [HttpDelete("EliminarDescuento")]
        public async Task<ServicesResult> EliminarDescuento(int tipo, decimal numero, int moda, decimal hasta)
        {
            return await escalasServices.EliminarDescuento(tipo, numero, moda, hasta);

        }
        [HttpPut("ModificarComision")]
        public async Task<ServicesResult> ModificarComision([FromBody] Escalascomision escalascomision)
        {
            return await escalasServices.ModificarComision(escalascomision);

        }
        [HttpPut("ModificarDescuento")]
        public async Task<ServicesResult> ModificarDescuento([FromBody] Escalasdescuento escalasdescuento)
        {
            return await escalasServices.ModificarDescuento(escalasdescuento);

        }
        [HttpGet("DescuentosPorClienteModalidad")]
        public async Task<ServicesResult> DescuentosPorClienteModalidad(int tipo, decimal numero, int moda)
        {
            return await escalasServices.DescuentosPorClienteModalidad(tipo, numero, moda);

        }
        [HttpPost("NuevoDescuento")]
        public async Task<ServicesResult> NuevoDescuento([FromBody] Escalasdescuento escalasdescuento)
        {
            return await escalasServices.NuevoDescuento(escalasdescuento);

        }
        [HttpGet("Descuento")]
        public async Task<ServicesResult> Descuento(int tipo, decimal numero, int moda, decimal hasta)
        {
            return await escalasServices.Descuento(tipo, numero, moda,hasta);

        }
    }

}
