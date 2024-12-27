using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pp3.dominio.Context;
using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using pp3.services.Handler;
using pp3.services.Repositories;
using System.Data;
using System.Linq;
using System.Net;

namespace pp3.services.Services
{
    public class ClientesService : IClientesRepository
    {
        private readonly Pp3roContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ServicesResult result = new ServicesResult();
        private readonly ILogger<ClientesService> _logger;
        private readonly IMapper _mapper;

        public ClientesService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, ILogger<ClientesService> logger, IMapper mapper)
        {
            this.context = context;

            this._httpContextAccessor = httpContextAccessor;

            this._logger = logger;

            this._mapper = mapper;
        }
        public async Task<ServicesResult> TodosLosClientes()
        {
            _logger.LogInformation($"Consultando Todos los Clientes");

            try
            {
                List<Clientes> listaClientes = await context.CLIENTES
                    .OrderBy(c => c.CLT_RAZONSOC)
                    .ToListAsync();

                List<ClientesDto> listaClientesDto = _mapper.Map<List<ClientesDto>>(listaClientes);

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(listaClientesDto);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en TodosLosClientes - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;

        }

        public async Task<ServicesResult> Nuevo(Clientes nuevoCliente)
        {
            _logger.LogInformation($"Nuevo Cliente ({nuevoCliente})");

            try
            {
                context.CLIENTES.Add(nuevoCliente);
                await context.SaveChangesAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Nuevo - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;

        }

        public async Task<ServicesResult> Baja(decimal tipoDoc, decimal numDoc)
        {
            _logger.LogInformation($"Baja Por Documento ({tipoDoc}, {numDoc})");

            bool clienteModalidadPago = await context.CLIENTEMODALIDADPAGO.Where(x => x.CLT_NUMDOCCLI == numDoc && x.TDD_CLT_ID == tipoDoc).AnyAsync();

            if(clienteModalidadPago)
            {
                //LoggingManager.LogObject(clienteModalidadPago, _logger);
                result.Code = ((int)HttpStatusCode.InternalServerError).ToString();
                result.Message = $"No se puede eliminar el cliente porque tiene modalidades de pago asociadas.";
                return result;
            }

            // Iniciar la transacción
            var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var cliente = await context.CLIENTES.FirstOrDefaultAsync(clt => clt.TDD_CLT_ID == tipoDoc
                    && clt.CLT_NUMDOC == numDoc);

                if (cliente != null)
                {
                    context.CLIENTES.Remove(cliente);

                    await context.SaveChangesAsync();

                    // Confirmar la transacción
                    await transaction.CommitAsync();

                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Content = "true";
                    result.Message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                    result.Message = "No se encontro cliente";
                    result.Content = "false";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Baja - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");

                // Revertir transacción
                await transaction.RollbackAsync();
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            finally
            {
                // Liberar recursos del contexto y transacción
                await transaction.DisposeAsync();
                await context.DisposeAsync();
            }

            return result;

        }

        public async Task<ServicesResult> CondicionIva(string codIvaCobis)
        {
            _logger.LogInformation($"Consultando CondicionIva ({codIvaCobis})");
            try
            {

                string codIvaCobisTrimmed = codIvaCobis.Trim();

                var query = from ci in context.CONDICIONESIVA
                            join circ in context.CONDICIONESIVA_RELACIONCOBIS on ci.CNI_ID equals circ.CNI_ID
                            where circ.COBIS_ID.Trim() == codIvaCobisTrimmed
                            select new CondicionesivaDto
                            {
                                CNI_ID = ci.CNI_ID,
                                CNI_DESCRIPCION = ci.CNI_DESCRIPCION
                            };

                var queryResult = await query.ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(queryResult);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en CondicionIva - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> NombreSucursal(string sucDescripcion)
        {
            //este método es raro, pide un código Cobis que no está en la tabla y lo compara con la descripción,
            //al mismo tiempo la descripción de la sucursal es lo que el método retorna
            _logger.LogInformation($"Consultando Nombre Sucursales ({sucDescripcion})");

            try
            {
                string codSucursalCobisTrimmed = sucDescripcion.Trim();

                var query = from s in context.SUCURSALES
                            where s.SUC_DESCRIPCION.Trim() == sucDescripcion
                            select new SucursalesDto
                            {
                                SUC_DESCRIPCION = s.SUC_DESCRIPCION
                            };

                var sucursal = await query.FirstOrDefaultAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(sucursal);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en NombreSucursal - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> NombreMoneda(decimal codMonedaCobis)
        {
            _logger.LogInformation($"Consultando Nombre Moneda ({codMonedaCobis})");

            try
            {
                var query = from m in context.MONEDAS
                            where m.MND_COBIS == codMonedaCobis
                            select new MonedaDto
                            {
                                MND_DESCRIPCION = m.MND_DESCRIPCION
                            };

                var monedaDto = await query.FirstOrDefaultAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(monedaDto);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en NombreMoneda - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> Cliente(decimal tipoDoc, decimal numDoc)
        {
            _logger.LogInformation($"Consultando Cliente ({numDoc})");

            try
            {
                List<Clientes> cliente = await context.CLIENTES
                    .Where(c => c.TDD_CLT_ID == tipoDoc && c.CLT_NUMDOC == numDoc)
                    .ToListAsync();

                List<ClientesDto> clienteDto = _mapper.Map<List<ClientesDto>>(cliente);

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(clienteDto);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Cliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> ClienteCompleto(decimal tipoDoc, decimal numDoc)
        {
            _logger.LogInformation($"Consultando Cliente ({numDoc})");

            try
            {
                List<Clientes> cliente = await context.CLIENTES
                    .Where(c => c.TDD_CLT_ID == tipoDoc && c.CLT_NUMDOC == numDoc)
                    .ToListAsync();


                result.Content = JsonConvert.SerializeObject(cliente);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Cliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> Modificar(Clientes clienteModificacion)
        {
            _logger.LogInformation($"Modificar Clientes ({clienteModificacion})");


            try
            {
                var cliente = await context.CLIENTES
         .FirstOrDefaultAsync(c => c.TDD_CLT_ID == clienteModificacion.TDD_CLT_ID && c.CLT_NUMDOC == clienteModificacion.CLT_NUMDOC);

                if (cliente == null)
                {
                    result.Message = "Cliente no encontrado.";
                    return result;
                }

                if (cliente.CLT_MAR_VALIDAUSOFIRMASCOBIS != clienteModificacion.CLT_MAR_VALIDAUSOFIRMASCOBIS)
                {
                    string estadosAValidar = clienteModificacion.CLT_MAR_VALIDAUSOFIRMASCOBIS == 0 ? "'I','PC'" : "'I'";

                    int cantidadOpgEnEstado = 1;

                    if (cantidadOpgEnEstado > 0)
                    {
                        result.Message = $"No se puede cambiar el ESQUEMA DE VALIDACION DE FIRMAS porque existen {cantidadOpgEnEstado} OPG en alguno de los siguientes estados ({estadosAValidar}).";
                        return result;
                    }
                }

                context.Entry(cliente).CurrentValues.SetValues(clienteModificacion);

                await context.SaveChangesAsync();
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Modificar - Origen:  - " +
            $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
            $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }


            return result;
        }

        public async Task<ServicesResult> Proovedor(decimal cltTipoDoc, decimal cltNumDoc, decimal bnfTipoDoc, decimal bnfNumDoc)
        {
            _logger.LogInformation($"Consultando Provedores ({cltNumDoc}, {bnfNumDoc})");

            try
            {

                BENEFICIARIOS? beneficiario = await context.BENEFICIARIOS
                .FirstOrDefaultAsync(b => b.TDD_CLT_ID == cltTipoDoc &&
                b.CLT_NUMDOC == cltNumDoc &&
                b.TDD_BNF_ID == bnfTipoDoc &&
                b.BNF_NUMDOC == bnfNumDoc);

                BeneficiariosDto beneficiariosDto = _mapper.Map<BeneficiariosDto>(beneficiario);

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(beneficiariosDto);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Proovedor - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> Beneficiarios(decimal tipoDoc, decimal numDoc)
        {
            _logger.LogInformation($"Consultando Beneficiarios ({numDoc}) ");

            try
            {
                List<BENEFICIARIOS> listabeneficiarios = await context.BENEFICIARIOS
                .Where(b => b.CLT_NUMDOC == tipoDoc && b.CLT_NUMDOC == numDoc)
                .Select(b => new BENEFICIARIOS
                {
                    TDD_BNF_ID = b.TDD_BNF_ID,
                    BNF_NUMDOC = b.BNF_NUMDOC,
                    BNF_RAZONSOC = b.BNF_RAZONSOC,
                    BNF_CALLE = b.BNF_CALLE,
                    BNF_NUMPUERTA = b.BNF_NUMPUERTA
                })
                .ToListAsync();

                List<BeneficiariosDto> listabeneficiariosDto = _mapper.Map<List<BeneficiariosDto>>(listabeneficiarios);

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(listabeneficiariosDto);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Beneficiarios - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> RangoTodosLosClientes()
        {
            _logger.LogInformation($"Consultando Rango Todos Los Clientes");

            try
            {

                List<Clientes> listaClientes = await context.CLIENTES
                    .Where(c => context.CLIENTEMODALIDADPAGO
                    .Any(cmp => (new decimal[] { 1, 3, 6, 8, 9, 10, 14, 15, 16, 17 }).Contains(cmp.MPG_ID) &&
                    cmp.TDD_CLT_ID == c.TDD_CLT_ID &&
                    cmp.CLT_NUMDOCCLI == c.CLT_NUMDOC) &&
                    c.CLT_ESTADO != "DA")
                    .OrderBy(c => c.CLT_RAZONSOC)
                    .ToListAsync();

                List<ClientesDto> listaClientesDto = _mapper.Map<List<ClientesDto>>(listaClientes);

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(listaClientesDto);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en RangoTodosLosClientes - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> ClientesRazonSocial(string razonSocial)
        {
            _logger.LogInformation($"Consultando Clientes Razon Social ({razonSocial})");

            try
            {
                var query = from c in context.CLIENTES
                            join t in context.TIPODOC on c.TDD_CLT_ID equals t.TDD_ID
                            join ci in context.CONDICIONESIVA on c.CNI_ID equals ci.CNI_ID
                            where c.CLT_RAZONSOC.ToUpper().Contains(razonSocial.ToUpper())
                            orderby c.CLT_RAZONSOC
                            select new ClientesDto
                            {
                                CLT_NUMDOC = c.CLT_NUMDOC,
                                TDD_CLT_ID = c.TDD_CLT_ID,
                                CLT_RAZONSOC = c.CLT_RAZONSOC,
                                CNI_ID = c.CNI_ID,
                                CLT_CALLE = c.CLT_CALLE,
                                CLT_NUMPUERTA = c.CLT_NUMPUERTA,
                                CLT_UNIDADFUNCIONAL = c.CLT_UNIDADFUNCIONAL,
                                CCP_ID = c.CCP_ID,
                                SUC_CUENTACOMISION = c.SUC_CUENTACOMISION,
                                CLT_FEC_INGRESO = c.CLT_FEC_INGRESO,
                                TipoDocumento = t.TDD_DESCRIPCION,
                                CondicionIVA = ci.CNI_DESCRIPCION,
                                CLT_ESTADO = c.CLT_ESTADO
                            };

                var clientes = await query.ToListAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(clientes);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ClientesRazonSocial - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }
            return result;
        }

        public async Task<ServicesResult> ActivaDesactivaCliente(decimal tipoDoc, decimal numDoc, int operacion)
        {
            _logger.LogInformation($"Activa Desactiva Cliente ({numDoc}, {operacion})");

            try
            {
                var W_FUNCION = new SqlParameter("@W_FUNCION", operacion);
                var W_TDD_CLT_ID = new SqlParameter("@W_TDD_CLT_ID", tipoDoc);
                var W_CLT_NUMDOC = new SqlParameter("@W_CLT_NUMDOC", numDoc);
                var okParam = new SqlParameter("@W_OK", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var descParam = new SqlParameter("@W_DESC", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };

                var execSP = await context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_DESACTIVA_ACTIVA_CLIENTE @W_FUNCION, @W_TDD_CLT_ID, @W_CLT_NUMDOC, @W_OK OUTPUT, @W_DESC OUTPUT",
                    W_FUNCION,
                    W_TDD_CLT_ID,
                    W_CLT_NUMDOC,
                    okParam,
                    descParam
                );

                if (execSP > 0)
                {
                    int codErr = (int)okParam.Value;
                    string descErr = (string)descParam.Value;

                    result.Code = codErr.ToString();
                    result.Message = descErr;
                    result.Content = "True";
                }
                else
                {
                    result.Content = "False";
                    result.Message = "No se pudo ejecutar el SP.";
                    result.Code = HttpStatusCode.NotFound.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ActivaDesactivaCliente - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> ClientesConOrdenesEnEstado(decimal? tipoDoc, decimal numDoc, string estadoAValidar)
        {
            _logger.LogInformation($"Consultando Clientes Con Ordenes En Estado ({numDoc}, {estadoAValidar})");

            try
            {
                var estadoList = estadoAValidar
                    .ToUpper()
                    .Split(',')
                    .Select(e => e
                    .Trim()
                    .Trim('\''))
                    .ToList();

                var cantidad = await context.ORDENESPAGO
                    .Where(op => op.TDD_CLT_ID == tipoDoc &&
                                 op.CLT_NUMDOC == numDoc &&
                                 op.OPG_FEC_PAGO >= System.DateTime.Now &&
                                 estadoList.Contains(op.EST_ESTADOACTUAL.ToUpper()))
                    .CountAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = cantidad.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ClientesConOrdenesEnEstado - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> Clientes(decimal tipoDoc, decimal numDoc)
        {

            _logger.LogInformation($"Consultando Clientes en Lista ({numDoc})");

            try
            {
                List<ClientesDto> listaClientes = await (from c in context.CLIENTES
                                                         join t in context.TIPODOC on c.TDD_CLT_ID equals t.TDD_ID
                                                         join ci in context.CONDICIONESIVA on c.CNI_ID equals ci.CNI_ID
                                                         where c.TDD_CLT_ID == tipoDoc && c.CLT_NUMDOC >= numDoc
                                                         orderby c.CLT_NUMDOC
                                                         select new ClientesDto
                                                         {
                                                             CLT_NUMDOC = c.CLT_NUMDOC,
                                                             TDD_CLT_ID = c.TDD_CLT_ID,
                                                             CLT_RAZONSOC = c.CLT_RAZONSOC,
                                                             CNI_ID = c.CNI_ID,
                                                             CLT_CALLE = c.CLT_CALLE,
                                                             CLT_NUMPUERTA = c.CLT_NUMPUERTA,
                                                             CLT_UNIDADFUNCIONAL = c.CLT_UNIDADFUNCIONAL,
                                                             CCP_ID = c.CCP_ID,
                                                             CLT_CUENTACOMISION = c.CLT_CUENTACOMISION,
                                                             CLT_FEC_INGRESO = c.CLT_FEC_INGRESO,
                                                             TipoDocumento = t.TDD_DESCRIPCION,
                                                             CondicionIVA = ci.CNI_DESCRIPCION,
                                                             CLT_ESTADO = c.CLT_ESTADO

                                                         }).ToListAsync();

                List<ClientesDto> listaclientesDto = _mapper.Map<List<ClientesDto>>(listaClientes);

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(listaclientesDto);
                result.Message = HttpStatusCode.OK.ToString();
            }

            catch (Exception ex)

            {
                _logger.LogError($"Error en Clientes - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> OrdenDepagoPendiente(decimal tipoDoc, decimal numDoc)
        {

            _logger.LogInformation($"Consultando si el cliente con tipo de documento {tipoDoc} y número {numDoc} tiene órdenes de pago pendientes.");


            try
            {

                var existeOrden = await context.ORDENESPAGO
                    .Where(op => op.TDD_CLT_ID == tipoDoc &&
                                 op.CLT_NUMDOC == numDoc &&
                                 new[] { 2, 4 }.Contains((int)op.MPG_ID) &&
                                 new[] { "I", "A", "N" }.Contains(op.EST_ESTADOACTUAL))
                    .Select(op => 1)
                    .FirstOrDefaultAsync();

                if (existeOrden != 0)
                {
                    result.Content = JsonConvert.SerializeObject(true);
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Message = "El cliente tiene órdenes de pago pendientes";
                }
                else
                {
                    result.Content = JsonConvert.SerializeObject(false);
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                    result.Message = "El cliente no tiene órdenes de pago pendientes";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en OrdenDepagoPendiente - Origen: {ex.Source ?? string.Empty} - " +
                                 $"Mensaje de error: {ex.Message} - Excepción interna: {ex.InnerException?.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> ClienteModalidadesPago(decimal tipoDoc, decimal numDoc)
        {
            try
            {
                int modalidadesPago = await context.CLIENTEMODALIDADPAGO
                .Where(x => x.TDD_CLT_ID == tipoDoc && x.CLT_NUMDOCCLI == numDoc)
                .CountAsync();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Message = HttpStatusCode.OK.ToString();
                result.Content = modalidadesPago.ToString();
            }
            catch (Exception e)
            {
                result.Code = ((int)HttpStatusCode.InternalServerError).ToString();
                result.Message = $"Error en la consulta de Modalidades de Pago {e.Message}, {e.Data}";
            }

            return result;
        }

    }
}
