using pp3.dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.services.Repositories
{
    public interface IModalidadRepository
    {
        public Task<ServicesResult> ModalidadesPorCliente(decimal Id, decimal NumDoc);
        public  Task<ServicesResult> Modalidad(int tipo, decimal numero, int id);
        public Task<ServicesResult> NuevaModalidad(Clientemodalidadpago clientemodalidadpago);
        public  Task<ServicesResult> EliminarModalidad(int Tipo, decimal numero, int Id);
        public  Task<ServicesResult> ModificarModalidad(Clientemodalidadpago clientemodalidadpago);

    }
}
