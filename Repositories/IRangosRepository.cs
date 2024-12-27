using pp3.dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.services.Repositories
{
    public interface IRangosRepository
    {
        Task <ServicesResult> Consulta(decimal Tdd_Clt_Id, decimal Clt_Numdoccli, decimal Mpg_id, string? Est_id);
        Task<ServicesResult> SeleccionoClienteConRP(decimal Tdd_Clt_Id, decimal Clt_Numdoccli);
        Task<ServicesResult> SeleccionoReporteRango(decimal? Tdd_Clt_Id, decimal? Clt_Numdoccli);
        Task <ServicesResult> ConfirmarRango(int rango_id);
        Task<ServicesResult> ConsultaCliRangos(int rango_id);
        Task<ServicesResult> ConsultaRangoCheques(decimal? Tdd_Clt_Id, decimal? Clt_Numdoccli, decimal? Mpg_id, string? Est_id);

    }
}
