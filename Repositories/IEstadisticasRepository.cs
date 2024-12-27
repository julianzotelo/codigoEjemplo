using pp3.dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.services.Repositories
{
    public interface IEstadisticasRepository
    {
        Task<ServicesResult> CantidadECheqPorCliente(string fecha);
        Task<ServicesResult> CantidadTeftPorCliente(string fecha);
        Task<ServicesResult> CantidadEfectivoPorCliente(string fecha);
        Task<ServicesResult> CantidadEmpresasActivasEmitiendoECheqPorCliente(string fecha);
        Task<ServicesResult> CantidadEmpresasActivasEmitiendoTeftPorCliente(string fecha);
        Task<ServicesResult> ConveniosAperturadosTodasModalidadesPago(string fecha);

    }
}
