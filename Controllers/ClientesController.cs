using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{


    [ApiController]
    [Route("api/Cliente")]
    public class ClientesController : ControllerBase
    {

        private readonly Pp3roContext context;
        private ClientesService clientesService;
        private readonly ILogger<ClientesService> _logger;
        private readonly IMapper _mapper;

        public ClientesController(Pp3roContext context, ILogger<ClientesService> logger, IMapper mapper)
        {
            this.context = context;
            this._logger = logger;
            this._mapper = mapper;
            this.clientesService = new ClientesService(context, new HttpContextAccessor(), logger, mapper);
        }

        [HttpPost]
        public async Task<ActionResult<ServicesResult>> AgregarCliente(Clientes nuevoCliente)
        {
            return await clientesService.Nuevo(nuevoCliente);
        }

        [HttpDelete("EliminarCliente")]
        public async Task<ServicesResult> EliminarCliente(decimal tipoDoc, decimal numDoc)
        {
            return await clientesService.Baja(tipoDoc, numDoc);
        }

        [HttpGet("CondicionIva")]
        public async Task<ServicesResult> CondicionIva(string codIvaCobis)
        {
            return await clientesService.CondicionIva(codIvaCobis);
        }

        [HttpPut]
        public async Task<ServicesResult> ModificarCliente(Clientes clienteModificacion)
        {
            return await clientesService.Modificar(clienteModificacion);
        }

        [HttpGet("NombreSucursal")]
        public async Task<ServicesResult> NombreSucursal(string sucDescripcion)
        {
            return await clientesService.NombreSucursal(sucDescripcion);
        }

        [HttpGet("NombreMoneda")]
        public async Task<ServicesResult> NombreMoneda(decimal codMonedaCobis)
        {
            return await clientesService.NombreMoneda(codMonedaCobis);
        }

        [HttpGet("Cliente")]
        public async Task<ServicesResult> Cliente(decimal tipoDoc, decimal numDoc)
        {
            return await clientesService.Cliente(tipoDoc, numDoc);
        }


        [HttpGet("ClienteCompleto")]
        public async Task<ServicesResult> ClienteCompleto(decimal tipoDoc, decimal numDoc)
        {
            return await clientesService.ClienteCompleto(tipoDoc, numDoc);
        }

        [HttpGet("Proveedor")]
        public async Task<ServicesResult> Proveedor(decimal cltTipoDoc, decimal cltNumDoc, decimal bnfTipoDoc, decimal bnfNumDoc)
        {
            return await clientesService.Proovedor(cltTipoDoc, cltNumDoc, bnfTipoDoc, bnfNumDoc);
        }

        [HttpGet("RangoTodosLosClientes")]
        public async Task<ServicesResult> RangoTodosLosClientes()
        {
            return await clientesService.RangoTodosLosClientes();
        }
        [HttpGet("TodosLosClientes")]
        public async Task<ServicesResult> TodosLosClientes()
        {
            return await clientesService.TodosLosClientes();
        }

        [HttpGet("ClientesRazonSocial")]
        public async Task<ServicesResult> ClientesRazonSocial(string razonSocial)
        {
            return await clientesService.ClientesRazonSocial(razonSocial);
        }

        [HttpGet("ActivaDesactivaCliente")]
        public async Task<ServicesResult> ActivaDesactivaCliente(decimal tipoDoc, decimal numDoc, int operacion)
        {
            return await clientesService.ActivaDesactivaCliente(tipoDoc, numDoc, operacion);
        }

        [HttpGet("ClientesConOrdenesEnEstado")]
        public async Task<ServicesResult> ClientesConOrdenesEnEstado(decimal tipoDoc, decimal numDoc, string estadoAValidar)
        {
            return await clientesService.ClientesConOrdenesEnEstado(tipoDoc, numDoc, estadoAValidar);
        }

        [HttpGet("Clientes")]
        public async Task<ServicesResult> Clientes(decimal tipoDoc, decimal numDoc)
        {
            return await clientesService.Clientes(tipoDoc, numDoc);
        }
        [HttpGet("OrdenDepagoPendiente")]
        public async Task<ServicesResult> OrdenDepagoPendiente(decimal tipoDoc, decimal numDoc)
        {
            return await clientesService.OrdenDepagoPendiente(tipoDoc, numDoc);
        }

        [HttpGet("ClientesModalidadesPago")]
        public async Task<ServicesResult> ClienteModalidadesPago(decimal tipoDoc, decimal numDoc)
        {
            return await clientesService.ClienteModalidadesPago(tipoDoc, numDoc);
        }
    }

}
