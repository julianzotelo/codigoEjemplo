using pp3.dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.services.Repositories
{
    public interface IHorarioRepository
    {
        Task<ServicesResult> ConsultaHorarios();
        Task<ServicesResult> ModificarHorarioCorte(TimeSpan horario);
        Task<ServicesResult> ModificarHorarioCorteEcheq(TimeSpan horario);
    }
}
