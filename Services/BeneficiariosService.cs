using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pp3.dominio.Context;
using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using pp3.services.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using pp3.services.Handler;

namespace pp3.services.Services
{
    public class BeneficiariosService : IBeneficiariosRepository
    {
        private readonly Pp3roContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ServicesResult result = new ServicesResult();
        private readonly ILogger<BeneficiariosService> _logger;
        private readonly IMapper _mapper;

        public BeneficiariosService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, ILogger<BeneficiariosService> logger, IMapper mapper)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
            this._mapper = mapper;
        }

        public async Task<ServicesResult> EliminarBeneficiario(decimal tipoCli, decimal numCli, decimal tipoBen, decimal numBen)
        {
            _logger.LogInformation($"Eliminar Beneficiario ({tipoCli}, {numCli}, {tipoBen}, {numBen}) ");
            try
            {
                List<BENEFICIARIOS> beneficiarios = await _context.BENEFICIARIOS
                    .Where(b => b.TDD_CLT_ID == tipoCli && b.CLT_NUMDOC == numCli 
                    && b.TDD_BNF_ID == tipoBen && b.BNF_NUMDOC == numBen)
                    .ToListAsync();

                if (beneficiarios[0] != null)
                {
                    foreach (var beneficiario in beneficiarios)
                    {
                        beneficiario.BNF_MAR_BAJA = 1;
                    }

                    await _context.SaveChangesAsync();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Message = "No se encontró ningún beneficiario.";
                    result.Content = "False";

                    return result;
                }

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message= HttpStatusCode.OK.ToString();
                result.Content = "True";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en EliminarBeneficiario - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> NuevoBeneficiario(BeneficiariosDto newBeneficiario)
        {
            _logger.LogInformation($"Usuario: {Globals.user} - BeneficiariosService - NuevoBeneficiario");
 
            bool validCCP = bool.Parse(VerificarCodigoPostal(newBeneficiario.CCP_ID).Result.Content);
                try
                {
                    if(validCCP)
                    {
                        await _context.BENEFICIARIOS.AddAsync(_mapper.Map<BENEFICIARIOS>(newBeneficiario));
                        await _context.SaveChangesAsync();

                        result.Code = ((int)HttpStatusCode.OK).ToString();
                        result.Message = HttpStatusCode.OK.ToString();
                        result.Content = "True";
                    }
                    else
                    {
                        result.Code = ((int)HttpStatusCode.NotFound).ToString();
                        result.Message ="Codigo postal invalido";
                        result.Content = "False";
                    }
                }
                 catch (Exception ex)
	            {
	                _logger.LogError($"Error en NuevoBeneficiario - Origen:  - " +
	                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
	                $"{ex.InnerException.ToString() ?? string.Empty}");
	                result.Code = ex.HResult.ToString();
	                result.Message = $"Ha ocurrido un error: {ex.Message}";
	                result.Content = "False";
	            }

                
            return result;
        }

        public async Task<ServicesResult> BeneficiariosPorCliente(decimal tipoCli, decimal numero, string razon)
        {
            _logger.LogInformation($"Consultando Beneficiarios por Cliente ({tipoCli}, {numero}, {razon})");

            try
            {
                List<BeneficiariosPorClienteDto> listaBenefPorCliente = await
                            _context.BENEFICIARIOS
                            .Join(_context.TIPODOC,
                            beneficiarios => beneficiarios.TDD_BNF_ID,
                            tipoDoc => tipoDoc.TDD_ID,
                            (b, td) => new { b, td })
                            .Join(_context.CONDICIONESIVA,
                            b_td => b_td.b.CNI_ID,
                            ci => ci.CNI_ID,
                            (b_td, ci) => new { b_td.b, b_td.td, ci })
                            .Join(_context.CONDICIONESIB,
                            b_td_ci => b_td_ci.b.CIB_ID,
                            cib => cib.CIB_ID,
                            (b_td_ci, cib) => new { b_td_ci.b, b_td_ci.td, b_td_ci.ci, cib })
                            .Join(_context.CONDICIONESIG,
                            b_td_ci_cib => b_td_ci_cib.b.CNG_ID,
                            cig => cig.CNG_ID,
                            (b_td_ci_cib, cig) => new { b_td_ci_cib.b, b_td_ci_cib.td, b_td_ci_cib.ci, b_td_ci_cib.cib, cig })
                            .Where(result => result.b.TDD_CLT_ID == tipoCli &&
                            result.b.CLT_NUMDOC == numero)
                            .OrderBy(result => result.b.BNF_RAZONSOC)
                            .Select(result => new BeneficiariosPorClienteDto
                            {
                                TDD_BNF_ID = result.b.TDD_BNF_ID,
                                BNF_NUMDOC = result.b.BNF_NUMDOC,
                                CIB_ID = result.cib.CIB_ID,
                                CNG_ID = result.cig.CNG_ID,
                                CNI_ID = result.ci.CNI_ID,
                                BNF_RAZONSOC = result.b.BNF_RAZONSOC,
                                BNF_CALLE = result.b.BNF_CALLE,
                                BNF_NUMPUERTA = result.b.BNF_NUMPUERTA,
                                BNF_UNIDADFUNCIONAL = result.b.BNF_UNIDADFUNCIONAL,
                                CCP_ID = result.b.CCP_ID,
                                BNF_NUMIB = result.b.BNF_NUMIB,
                                BNF_MAR_CLIENTEBANSUD = result.b.BNF_MAR_CLIENTEBANSUD,
                                TDD_DESCRIPCION = result.td.TDD_DESCRIPCION,
                                TDD_CLT_ID = result.b.TDD_CLT_ID,
                                CLT_NUMDOC = result.b.CLT_NUMDOC,
                                CNI_DESCRIPCION = result.ci.CNI_DESCRIPCION,
                                CIB_DESCRIPCION = result.cib.CIB_DESCRIPCION,
                                CNG_DESCRIPCION = result.cig.CNG_DESCRIPCION,
                                BNF_MAR_BAJA = result.b.BNF_MAR_BAJA
                            }).ToListAsync();

                if (razon != null && razon != "")
                {
                    listaBenefPorCliente = listaBenefPorCliente
                    .Where(b => b.BNF_RAZONSOC.ToUpper().Contains(razon.ToUpper()))
                    .ToList();
                }

                result.Content = JsonConvert.SerializeObject(listaBenefPorCliente);
                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message= HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en BeneficiariosPorCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                result.Content = "False";
            }

            return result;
        }

        public async Task<ServicesResult> Beneficiario(int tipoDocCli, decimal numDocCli, decimal tipoDocBnf, decimal numDocBnf)
        {
            _logger.LogInformation($"Consultando Beneficiarios ({tipoDocCli}, {numDocCli}, {tipoDocBnf}, {numDocBnf})");

            try
            {
                BeneficiariosDto? beneficiario = await (from BENEFICIARIOS in _context.BENEFICIARIOS
                            join tipodoc in _context.TIPODOC on BENEFICIARIOS.TDD_BNF_ID equals tipodoc.TDD_ID
                            where BENEFICIARIOS.TDD_CLT_ID == tipoDocCli
                               && BENEFICIARIOS.CLT_NUMDOC == numDocCli
                               && BENEFICIARIOS.BNF_NUMDOC == numDocBnf
                               && BENEFICIARIOS.TDD_BNF_ID == tipoDocBnf
                            select new BeneficiariosDto
                            {
                                TDD_BNF_ID = BENEFICIARIOS.TDD_BNF_ID,
                                BNF_NUMDOC = BENEFICIARIOS.BNF_NUMDOC,
                                CIB_ID = BENEFICIARIOS.CIB_ID,
                                CNG_ID = BENEFICIARIOS.CNG_ID,
                                CNI_ID = BENEFICIARIOS.CNI_ID,
                                BNF_RAZONSOC = BENEFICIARIOS.BNF_RAZONSOC,
                                BNF_CALLE = BENEFICIARIOS.BNF_CALLE,
                                BNF_NUMPUERTA = BENEFICIARIOS.BNF_NUMPUERTA,
                                BNF_UNIDADFUNCIONAL = BENEFICIARIOS.BNF_UNIDADFUNCIONAL,
                                CCP_ID = BENEFICIARIOS.CCP_ID,
                                BNF_NUMIB = BENEFICIARIOS.BNF_NUMIB,
                                BNF_MAR_CLIENTEBANSUD = BENEFICIARIOS.BNF_MAR_CLIENTEBANSUD,
                                TDD_DESCRIPCION= tipodoc.TDD_DESCRIPCION
                            })
                            .FirstOrDefaultAsync();

                if (beneficiario != null)
                {
                    result.Content = JsonConvert.SerializeObject(beneficiario);
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Message= HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Content = "False";
                    result.Code = HttpStatusCode.NotFound.ToString();
                    result.Message = "No se encontró el beneficiario.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Beneficiario - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                result.Content = "False";
            }

            return result;
        }

        public async Task<ServicesResult> VerificarCodigoPostal(decimal codigoPostal)
        {
            try
            {
                bool existe = await _context.CODIGOSPOSTALES.Where(x => x.CCP_ID == codigoPostal).AnyAsync();

                if (existe)
                {
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Message = HttpStatusCode.OK.ToString();
                    result.Content = "true";
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = HttpStatusCode.NotFound.ToString();
                    result.Content = "false";
                }
            }
            catch (Exception e)
            {
                result.Code = e.HResult.ToString();
                result.Message = e.Message;
                result.Content = "false";
            }

            return result;
        }

        public async Task<ServicesResult> ModificarBeneficiario(BeneficiariosDto beneficiarioModif)
        {
            _logger.LogInformation($"Modificar Beneficiario ({beneficiarioModif})");

            try
                {
                    // Comenzar una transacción

                    // Buscar el beneficiario correspondiente
                    BENEFICIARIOS? beneficiario = await _context.BENEFICIARIOS
                        .FirstOrDefaultAsync(b => b.TDD_BNF_ID == beneficiarioModif.TDD_BNF_ID && b.BNF_NUMDOC == beneficiarioModif.BNF_NUMDOC
                        && b.TDD_CLT_ID == beneficiarioModif.TDD_CLT_ID && b.CLT_NUMDOC == beneficiarioModif.CLT_NUMDOC);

                    if (beneficiario != null)
                    {
                        // Actualizar los campos del beneficiario
                        beneficiario.BNF_RAZONSOC = beneficiarioModif.BNF_RAZONSOC;
                        beneficiario.CIB_ID = beneficiarioModif.CIB_ID;
                        beneficiario.CNG_ID = beneficiarioModif.CNG_ID;
                        beneficiario.BNF_CALLE = beneficiarioModif.BNF_CALLE;
                        beneficiario.BNF_NUMPUERTA = beneficiarioModif.BNF_NUMPUERTA;
                        beneficiario.CNI_ID = beneficiarioModif.CNI_ID;
                        beneficiario.BNF_UNIDADFUNCIONAL = beneficiarioModif.BNF_UNIDADFUNCIONAL;
                        beneficiario.CCP_ID = beneficiarioModif.CCP_ID;
                        beneficiario.BNF_NUMIB = beneficiarioModif.BNF_NUMIB;

                        // Guardar los cambios en la base de datos
                        await _context.SaveChangesAsync();


                        //await transaction.CommitAsync();

                        result.Code = ((int)HttpStatusCode.OK).ToString();
                        result.Message = HttpStatusCode.OK.ToString();
                        result.Content = "True";
                    }
                    else
                    {
                        result.Code = HttpStatusCode.NotFound.ToString();
                        result.Message = "Beneficiario no encontrado";
                        result.Content = "False";
                    }
                }            
                catch (Exception ex)
                {
                    _logger.LogError($"Error en ModificarBeneficiario - Origen:  - " +
                    $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                    $"{ex.InnerException.ToString() ?? string.Empty}");
                    result.Code = ex.HResult.ToString();
                    result.Message = $"Ha ocurrido un error: {ex.Message}";
                    result.Content = "False";
                }

            return result;
        }
        public async Task<ServicesResult> EsCliente(int tipoDoc, decimal numDoc)
        {
            _logger.LogInformation($"Consulta EsCliente ({numDoc}, {tipoDoc})");

            try
            {
                Clientes? clienteExiste = await _context.CLIENTES
                    .Where(cliente =>
                        cliente.TDD_CLT_ID == tipoDoc
                        && cliente.CLT_NUMDOC == numDoc)
                    .FirstOrDefaultAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message= HttpStatusCode.OK.ToString();
                result.Content = (clienteExiste != null).ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en EsCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
                result.Content = "False";
            }

            return result;
        }

        public async Task<ServicesResult> ReciboInternoRp(string filtro, Paginado paginado)
        {
            _logger.LogInformation($"Recibo Interno Rp ({filtro}) ");

            try
            {
                List<ReciboInternoRpDto> query = await (from op in _context.ORDENESPAGO
                            join b in _context.BENEFICIARIOS on new { op.TDD_BNF_ID, op.BNF_NUMDOC, op.TDD_CLT_ID, op.CLT_NUMDOC } equals new { b.TDD_BNF_ID, b.BNF_NUMDOC, b.TDD_CLT_ID, b.CLT_NUMDOC }
                            join cp in _context.CODIGOSPOSTALES on b.CCP_ID equals cp.CCP_ID
                            join p in _context.PROVINCIAS on cp.PRV_ID equals p.PRV_ID
                            where op.TDD_BNF_ID == b.TDD_BNF_ID 
                            && op.BNF_NUMDOC == b.BNF_NUMDOC
                            && op.TDD_CLT_ID == b.TDD_CLT_ID 
                            && op.CLT_NUMDOC == b.CLT_NUMDOC
                            && b.CCP_ID == cp.CCP_ID
                            && cp.PRV_ID == p.PRV_ID

                            select new ReciboInternoRpDto
                            {
                                OPG_FEC_PAGO = op.OPG_FEC_PAGO,
                                BNF_RAZONSOC = b.BNF_RAZONSOC,
                                BNF_CALLE = b.BNF_CALLE,
                                BNF_NUMPUERTA = b.BNF_NUMPUERTA,
                                BNF_UNIDADFUNCIONAL = b.BNF_UNIDADFUNCIONAL,
                                CCP_ID = b.CCP_ID,
                                CCP_LOCALIDAD = cp.CCP_LOCALIDAD,
                                PRV_DESCRIPCION = p.PRV_DESCRIPCION
                            }).Skip(paginado.Skip).Take(paginado.Take).ToListAsync();

                            

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query = query.OrderBy(r => r.BNF_RAZONSOC).ToList();
                }

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message= HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(query);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ReciboInternoRp - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> ReciboExternoRp(string filtro, Paginado paginado)
        {
            _logger.LogInformation($"Recibo Externo Rp ({filtro})");

            try
            {
                List< ReciboExternoRpDto> query = await (from op in _context.ORDENESPAGO
                            join b in _context.BENEFICIARIOS on new { op.TDD_BNF_ID, op.BNF_NUMDOC, op.TDD_CLT_ID, op.CLT_NUMDOC } equals new { b.TDD_BNF_ID, b.BNF_NUMDOC, b.TDD_CLT_ID, b.CLT_NUMDOC }
                            join cp in _context.CODIGOSPOSTALES on b.CCP_ID equals cp.CCP_ID
                            join p in _context.PROVINCIAS on cp.PRV_ID equals p.PRV_ID
                            where op.TDD_BNF_ID == b.TDD_BNF_ID &&
                                  op.BNF_NUMDOC == b.BNF_NUMDOC &&
                                  op.TDD_CLT_ID == b.TDD_CLT_ID &&
                                  op.CLT_NUMDOC == b.CLT_NUMDOC &&
                                  b.CCP_ID == cp.CCP_ID &&
                                  cp.PRV_ID == p.PRV_ID
                            select new ReciboExternoRpDto
                            {
                                OPG_IMP_PAGO = op.OPG_IMP_PAGO,
                                OPG_FEC_PAGO = op.OPG_FEC_PAGO,
                                OPG_NUMCOMPROBANTE = op.OPG_NUMCOMPROBANTE,
                                BNF_RAZONSOC = b.BNF_RAZONSOC,
                                BNF_CALLE = b.BNF_CALLE,
                                BNF_NUMPUERTA = b.BNF_NUMPUERTA,
                                BNF_UNIDADFUNCIONAL = b.BNF_UNIDADFUNCIONAL,
                                CCP_ID = b.CCP_ID,
                                CCP_LOCALIDAD = cp.CCP_LOCALIDAD,
                                PRV_DESCRIPCION = p.PRV_DESCRIPCION
                            }).Skip(paginado.Skip).Take(paginado.Take).ToListAsync();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query = query.OrderBy(r => r.BNF_RAZONSOC).ToList(); 
                }

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message= HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(query);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ReciboExternoRp - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> BeneficiariosClienteRp(string filtro, Paginado paginado)
        {
            _logger.LogInformation($"Consultando Beneficiarios por Cliente Rp ({filtro})");


            try
            {
                List<BeneficiariosClienteRpDto> query = await (from b in _context.BENEFICIARIOS
                            join c in _context.CLIENTES on new { b.TDD_CLT_ID, b.CLT_NUMDOC } equals new { c.TDD_CLT_ID, c.CLT_NUMDOC }
                            join t in _context.TIPODOC on b.TDD_BNF_ID equals t.TDD_ID
                            join cp in _context.CODIGOSPOSTALES on b.CCP_ID equals cp.CCP_ID
                            select new BeneficiariosClienteRpDto
                            {
                                BNF_NUMDOC = b.BNF_NUMDOC,
                                BNF_RAZONSOC = b.BNF_RAZONSOC,
                                BNF_MAR_CLIENTEBANSUD = b.BNF_MAR_CLIENTEBANSUD,
                                TDD_BNF_ID = b.TDD_BNF_ID,
                                TDD_CLT_ID = b.TDD_CLT_ID,
                                CLT_NUMDOC = b.CLT_NUMDOC,
                                BNF_CALLE = b.BNF_CALLE,
                                BNF_NUMPUERTA = b.BNF_NUMPUERTA,
                                BNF_UNIDADFUNCIONAL = b.BNF_UNIDADFUNCIONAL,
                                CCP_ID = b.CCP_ID,
                                CLT_RAZONSOC = c.CLT_RAZONSOC,
                                TDD_DESCRIPCIONABREV = t.TDD_DESCRIPCIONABREV,
                                CCP_LOCALIDAD = cp.CCP_LOCALIDAD
                            }).Skip(paginado.Skip).Take(paginado.Take).ToListAsync();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query = query.OrderBy(r => r.TDD_DESCRIPCIONABREV).ThenBy(b => b.BNF_NUMDOC).ToList();
                }

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message= HttpStatusCode.OK.ToString();
                result.Content = JsonConvert.SerializeObject(query);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en BeneficiariosClienteRp - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}"; ;
            }

            return result;
        }

    }
}
