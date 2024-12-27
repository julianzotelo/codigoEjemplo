using pp3.dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.services.Repositories
{
    public interface ISucursalRepository
    {
        Task<ServicesResult> ConsultaSucursales(int provinciaId);
        Task<ServicesResult> AltaSucursal(Sucursales nuevaSucursal);
        Task<ServicesResult> EliminarSucursal(decimal sucursalId);
        Task<ServicesResult> ModificarSucursal(Sucursales modSucursal);
    }
}
