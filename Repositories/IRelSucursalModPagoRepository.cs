using pp3.dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.services.Repositories
{
    public interface IRelSucursalModPagoRepository
    {
        Task<ServicesResult> ConsultaRelSucursalModPago(decimal sucursalId);
        Task<ServicesResult> AltaRelSucursalModPago(RelSucursalModpago relSucursalModpago);
        Task<ServicesResult> BajaRelSucursalModPago(decimal sucursalId, decimal modalidadPagoId);
        Task<ServicesResult> ModalidadesPagoNoAsociadasSucursal(decimal sucursalId);
    }
}
