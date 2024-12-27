using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{
    [ApiController]
    [Route("api/Cuentas")]
    public class CuentasController : ControllerBase
    {

        private readonly Pp3roContext context;
        private CuentasService cuentasService;
        private IMapper _mapper;
        private readonly ILogger<CuentasService> _logger;
        public CuentasController(Pp3roContext context, IMapper mapper, ILogger<CuentasService> logger, SeguridadService seguridadService)
        {
            this.context = context;
            this._mapper = mapper;
            this._logger = logger;
            this.cuentasService = new CuentasService(context, new HttpContextAccessor(), mapper, logger, seguridadService);

        }

        [HttpGet("cuentas")]
        public async Task<ServicesResult> Cuentas(decimal cuentaServicio)
        {
            return await cuentasService.Cuentas(cuentaServicio);
        }

        [HttpGet("cuentasPorCliente")]
        public async Task<ServicesResult> CuentasPorCliente(decimal tipoDoc, decimal numDoc)
        {
            return await cuentasService.CuentasPorCliente(tipoDoc, numDoc);
        }

        [HttpGet("cuenta")]
        public async Task<ServicesResult> Cuenta(decimal cuentaServicio)
        {
            return await cuentasService.Cuenta(cuentaServicio);
        }


        [HttpPost]
        public async Task<ServicesResult> NuevaCuenta(CuentasDto cuentasDto)
        {
            return await cuentasService.NuevaCuenta(cuentasDto);
        }

        [HttpPut]
        public async Task<ServicesResult> ModificarCuenta(CuentasDto cuentasDto)
        {
            return await cuentasService.ModificarCuenta(cuentasDto);
        }

        [HttpDelete]
        public async Task<ServicesResult> EliminarCuenta(string cuentaServicio)
        {
            return await cuentasService.EliminarCuenta(cuentaServicio);
        }

        [HttpGet("firmantes")]
        public async Task<ServicesResult> Firmantes(decimal cuenta)
        {
            return await cuentasService.Firmantes(cuenta);
        }
        
        [HttpGet("ExisteConvenioModif")]
        public async Task<ServicesResult> ExisteConvenioModif(decimal convenio, decimal cuenta)
        {
            return await cuentasService.ExisteConvenioModif(convenio,cuenta);
        }

    }
}
