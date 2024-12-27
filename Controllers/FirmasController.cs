using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{

    [ApiController]
    [Route("api/Firmas")]
    public class FirmasController
    {
        private readonly Pp3roContext context;
        private FirmasService firmasService;
        private readonly ILogger<FirmasService> _logger;
        private readonly IMapper _mapper;

        public FirmasController(Pp3roContext context, SeguridadService seguridadService,IMapper mapper, ILogger<FirmasService> logger)
        {
            this.context = context;
            this._logger = logger;
            this._mapper = mapper;
            this.firmasService = new FirmasService(context, new HttpContextAccessor(), seguridadService, mapper, logger);
        }
        [HttpGet("FirmasPorId")]
        public async Task<ServicesResult> FirmasPorId(string USR_ID)
        {
            return await firmasService.FirmasPorId(USR_ID);
        }

        [HttpGet("FirmasPorIdDoc")]
        public async Task<ServicesResult> FirmasPorIdDoc(string id, int tipo, decimal numero)
        {
            return await firmasService.FirmasPorIdDoc(id, tipo, numero);
        }
        [HttpGet("FirmasPorDocumento")]
        public async Task<ServicesResult> FirmasPorDocumento(int tipo, decimal numero)
        {
            return await firmasService.FirmasPorDocumento(tipo, numero);
        }

        [HttpGet("FirmasPorCliente")]
        public async Task<ServicesResult> FirmasPorCliente(int tipo, decimal numero)
        {
            return await firmasService.FirmasPorCliente(tipo, numero);
        }

        [HttpGet("FirmantesPorCliente")]
        public async Task<ServicesResult> FirmantesPorCliente(int tipo, decimal numero)
        {
            return await firmasService.FirmantesPorCliente(tipo, numero);
        }

        [HttpGet("FirmasPorApellido")]
        public async Task<ServicesResult> FirmasPorApellido(string apellido)
        {
            return await firmasService.FirmasPorApellido(apellido);
        }

        [HttpGet("Firma")]
        public async Task<ServicesResult> Firma(string id)
        {
            return await firmasService.Firma(id);
        }
        [HttpPost("NuevaFirma")]
        public async Task<ServicesResult> NuevaFirma([FromBody] FIRMAS_nueva firmas)
        {
            return await firmasService.NuevaFirma(firmas);
        }
        [HttpGet("VerFirma")]
        public async Task<ServicesResult> VerFirma(string firma)
        {
            return await firmasService.VerFirma(firma);
        }
        [HttpDelete("EliminarFirma")]
        public async Task<ServicesResult> EliminarFirma(string id)
        {
            return await firmasService.EliminarFirma(id);
        }
        [HttpDelete("EliminarFirmaCuenta")]
        public async Task<ServicesResult> EliminarFirmaCuenta(decimal cuenta, string usr)
        {
            return await firmasService.EliminarFirmaCuenta(cuenta, usr);
        }
        [HttpPut("ModificarCuentaFirma")]
        public async Task<ServicesResult> ModificarCuentaFirma([FromBody]  CUENTAFIRMA cuentaFirmaCambiado)
        {
            return await firmasService.ModificarCuentaFirma(cuentaFirmaCambiado);
        }
        [HttpGet("Cuentas")]
        public async Task<ServicesResult> Cuentas(int id)
        {
            return await firmasService.Cuentas(id);
        }
        [HttpGet("FirmasPorCuenta")]
        public async Task<ServicesResult> FirmasPorCuenta(decimal cuenta)
        {
            return await firmasService.FirmasPorCuenta(cuenta);
        }
        [HttpPut("ModificarFirma")]
        public async Task<ServicesResult> ModificarFirma([FromBody] FIRMAS_nueva firmas)
        {
            return await firmasService.ModificarFirma(firmas); 
        }
        [HttpGet("ExistenFirmantesVacios")]
        public async Task<ServicesResult> ExistenFirmantesVacios(int tipo, decimal numero)
        {
            return await firmasService.ExistenFirmantesVacios(tipo, numero);
        }
        [HttpPost("NuevaCuentaFirma")]
        public async Task<ServicesResult> NuevaCuentaFirma([FromBody] CUENTAFIRMA cuentaFirma)
        {
            return await firmasService.NuevaCuentaFirma(cuentaFirma);
        }
    }
}
