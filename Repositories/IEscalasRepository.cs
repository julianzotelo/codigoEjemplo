using pp3.dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.services.Repositories
{
    public interface IEscalasRepository
    {
        public  Task<ServicesResult> ComisionesPorClienteModalidad(int tipo, decimal numero, int moda);
        public  Task<ServicesResult> ComisionesPorCliente(decimal tipo, decimal numero);
        public  Task<ServicesResult> NuevaComision(Escalascomision escalascomision);
        public  Task<ServicesResult> Comision(int tipo, decimal numero, int moda, decimal hasta);
        public  Task<ServicesResult> EliminarComision(int tipo, decimal numero, int moda, decimal hasta);
        public  Task<ServicesResult> EliminarDescuento(int tipo, decimal numero, int moda, decimal hasta);
        public  Task<ServicesResult> ModificarComision(Escalascomision escalascomision);
        public  Task<ServicesResult> ModificarDescuento(Escalasdescuento escalasdescuento);
        public  Task<ServicesResult> DescuentosPorClienteModalidad(int tipo, decimal numero, int moda);
        public  Task<ServicesResult> NuevoDescuento(Escalasdescuento escalasdescuento);
        public  Task<ServicesResult> Descuento(int tipo, decimal numero, int moda, decimal hasta);


    }
}
