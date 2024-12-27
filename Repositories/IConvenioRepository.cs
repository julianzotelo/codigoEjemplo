using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pp3.dominio.Models;

namespace pp3.services.Repositories
{
    public interface IConvenioRepository
    {
        Task<ServicesResult> AltaConvenio(TIPO_CONVENIO nuevoConvenio);
        Task<ServicesResult> EliminarConvenio(int convenioId);
        Task<ServicesResult> ModificarConvenio(TIPO_CONVENIO convenioModif);
        Task<ServicesResult> ObtenerModalidadesAsociadas(int convenioId);
        Task<ServicesResult> ObtenerModalidadesNoAsociadas(int convenioId);
        Task<ServicesResult> AsociarModalidadPago(REL_TIPOCONVENIO_MODPAGO relConvenioModPago);
        Task<ServicesResult> DesasociarModalidadPago(decimal convenioId, decimal modalidadId);
    }
}
