using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pp3.dominio.Context;
using pp3.dominio.Models;
using pp3.services.Handler;
using pp3.services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace pp3.services.Services
{
    public class HorarioService : IHorarioRepository
    {
        private readonly Pp3roContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ServicesResult result = new ServicesResult();
        private readonly ILogger<HorarioService> _logger;
        private readonly IMapper _mapper;
        

        public HorarioService(Pp3roContext context, IHttpContextAccessor httpContextAccessor, ILogger<HorarioService> logger, IMapper mapper)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
            this._mapper = mapper;
        }

        public async Task<ServicesResult> ConsultaHorarios()
        {
            _logger.LogInformation($"Consultando horarios");

            try
            {
                var query = await (from parametros in _context.PARAMETROS
                                      select new
                                      {
                                          PRM_HORARIOCORTE = parametros.PRM_HORARIOCORTE,
                                          PRM_HORARIOCONCENTRADOR = parametros.PRM_HORARIOCONCENTRADOR,
                                          PRM_HORARIO_CORTE_ECHEQ = parametros.PRM_HORARIO_CORTE_ECHEQ
                                      }).ToListAsync();

                var horarios = query.Select(h => new
                {
                    PRM_HORARIOCORTE = h.PRM_HORARIOCORTE.HasValue ? ConvertDecimalToTimeSpan(h.PRM_HORARIOCORTE.Value) : "",
                    PRM_HORARIOCONCENTRADOR = h.PRM_HORARIOCONCENTRADOR.HasValue ? h.PRM_HORARIOCONCENTRADOR.Value.ToString("HH:mm") : "",
                    PRM_HORARIO_CORTE_ECHEQ = h.PRM_HORARIO_CORTE_ECHEQ.HasValue ? h.PRM_HORARIO_CORTE_ECHEQ.Value.ToString("HH:mm") : ""
                }).ToList();

                result.Code = ((int)HttpStatusCode.OK).ToString();
                result.Content = JsonConvert.SerializeObject(horarios);
                result.Message = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ConsultaHorarios - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> ModificarHorarioCorte(TimeSpan horario)
        {
            _logger.LogInformation($"Modificando horario corte");

            try
            {
                var parametro = await _context.PARAMETROS.FirstOrDefaultAsync();

                if (parametro != null)
                {
                    parametro.PRM_HORARIOCORTE = ConvertTimeSpanToDecimal(horario);

                    await _context.SaveChangesAsync();

                    result.Content = JsonConvert.SerializeObject(parametro);
                    result.Message = HttpStatusCode.OK.ToString();
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                }
                else
                {
                    result.Message = "Registro no encontrado";
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ModificarHorarioCorte - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public async Task<ServicesResult> ModificarHorarioCorteEcheq(TimeSpan horario)
        {
            _logger.LogInformation($"Modificando horario corte ECHEQ");

            try
            {
                var parametro = await _context.PARAMETROS.FirstOrDefaultAsync();

                if (parametro != null)
                {
                    DateTime horarioCorteEcheq = new DateTime(2020, 01, 01, horario.Hours, horario.Minutes, 0, 0);

                    parametro.PRM_HORARIO_CORTE_ECHEQ = horarioCorteEcheq;

                    await _context.SaveChangesAsync();

                    result.Content = JsonConvert.SerializeObject(parametro);
                    result.Message = HttpStatusCode.OK.ToString();
                    result.Code = ((int)HttpStatusCode.OK).ToString();
                }
                else
                {
                    result.Message = "Registro no encontrado";
                    result.Code = ((int)HttpStatusCode.NotFound).ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ModificarHorarioCorteEcheq - Origen:  - " +
                $"{ex.Source.ToString() ?? string.Empty}" + $"- Mensaje de error: {ex.Message} - Excepción interna: " +
                $"{ex.InnerException.ToString() ?? string.Empty}");
                result.Code = ex.HResult.ToString();
                result.Message = $"Ha ocurrido un error: {ex.Message}";
            }

            return result;
        }

        public string ConvertDecimalToTimeSpan(decimal horario)
        {
            var valor = horario;
            valor = valor * 24;            
            int horas = (int)valor;
            var minutos = (valor - horas) * 60;

            DateTime horarioFormateado = new DateTime(1,1,1, horas, (int)Math.Round(minutos, 0, MidpointRounding.AwayFromZero), 0);

            return horarioFormateado.ToString("HH:mm");

        }

        public decimal ConvertTimeSpanToDecimal(TimeSpan horario)
        {
            var horas = horario.Hours;
            var minutos = horario.Minutes;

            var horarioDecimal = (horas + ((decimal)minutos / 60)) / 24;

            return horarioDecimal;
        }

    }
}
