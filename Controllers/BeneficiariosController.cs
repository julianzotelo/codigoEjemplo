using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{
    [ApiController]
    [Route("api/Beneficiarios")]
    public class BeneficiariosController
    {
        private readonly Pp3roContext _context;
        private BeneficiariosService beneficiariosService;
        private readonly ILogger<BeneficiariosService> _logger;
        private readonly IMapper _mapper;

        public BeneficiariosController(Pp3roContext context, ILogger<BeneficiariosService> logger, IMapper mapper)
        {
            this._context = context;
            this._logger = logger;
            this._mapper = mapper;
            this.beneficiariosService = new BeneficiariosService(context, new HttpContextAccessor(), logger, mapper);
        }

        [HttpGet("codigospostales")]
        public async Task<ServicesResult> VerificarCodigoPostal(decimal codigoPostal)
        {
            return await beneficiariosService.VerificarCodigoPostal(codigoPostal);
        }

        [HttpPost]
        public async Task<ServicesResult> NuevoBeneficiario(BeneficiariosDto newBeneficiario)
        {
            return await beneficiariosService.NuevoBeneficiario(newBeneficiario);
        }

        [HttpDelete]
        public async Task<ServicesResult> EliminarBeneficiario(decimal tipoCli, decimal NumCli, decimal tipoBen, decimal numBen)
        {
            return await beneficiariosService.EliminarBeneficiario(tipoCli, NumCli, tipoBen, numBen);
        }

        [HttpGet("BeneficiariosPorCliente")]
        public async Task<ServicesResult> BeneficiariosPorCliente(decimal tipoCli, decimal numero, string? razon)
        {
            return await beneficiariosService.BeneficiariosPorCliente(tipoCli, numero, razon);
        }
        [HttpGet("Beneficiario")]
        public async Task<ServicesResult> Beneficiario(int tipoDocCli, decimal numDocCli, decimal tipoDocBnf, decimal numDocBnf)
        {
            return await beneficiariosService.Beneficiario(tipoDocCli, numDocCli, tipoDocBnf, numDocBnf);
        }
        [HttpPut("ModificarBeneficiario")]
        public async Task<ServicesResult> ModificarBeneficiario(BeneficiariosDto beneficiarioModif)
        {
            return await beneficiariosService.ModificarBeneficiario(beneficiarioModif);
        }
        [HttpGet("EsCliente")]
        public async Task<ServicesResult> EsCliente(int tipoDoc, decimal numDoc)
        {
            return await beneficiariosService.EsCliente(tipoDoc, numDoc);
        }
        [HttpGet("ReciboInternoRp")]
        public async Task<ServicesResult> ReciboInternoRp(string filtro, [FromQuery]Paginado paginado)
        {
            return await beneficiariosService.ReciboInternoRp(filtro, paginado);
        }
        [HttpGet("ReciboExternoRp")]
        public async Task<ServicesResult> ReciboExternoRp(string filtro, [FromQuery]Paginado paginado)
        {
            return await beneficiariosService.ReciboExternoRp(filtro, paginado);
        }
        [HttpGet("BeneficiariosClienteRp")]
        public async Task<ServicesResult> BeneficiariosClienteRp(string filtro, [FromQuery]Paginado paginado)

        {
            return await beneficiariosService.BeneficiariosClienteRp(filtro, paginado);
        }
    }
}
