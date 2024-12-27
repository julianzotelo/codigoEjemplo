using pp3.dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.services.Repositories
{
    public interface ICodigoPostalRepository
    {
        Task<ServicesResult> Traer(decimal pciaId);
        Task<ServicesResult> CodigoPostal(decimal pCod);
        Task<ServicesResult> NuevoCodigoPostal(Codigospostale codigoPostal);
        Task<ServicesResult> ModificarCodigoPostal(Codigospostale codigoPostal);
        Task<ServicesResult> EliminarCodigoPostal(decimal pCcpId);
    }
}
