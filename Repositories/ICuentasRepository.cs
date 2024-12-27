using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.services.Repositories
{
    public interface ICuentasRepository
    {
        Task<ServicesResult> Cuentas(decimal cuentaServicio);
        Task<ServicesResult> CuentasPorCliente(decimal tipoDoc, decimal numDoc);
        Task<ServicesResult> Cuenta(decimal cuentaServicio);
        Task<ServicesResult> NuevaCuenta(CuentasDto cuentasDto);
        Task<ServicesResult> ModificarCuenta(CuentasDto cuentasDto);
        Task<ServicesResult> EliminarCuenta(string cuentaServicio);
        Task<ServicesResult> Firmantes(decimal cuenta);
        Task<ServicesResult> ExisteConvenioModif(decimal convenio, decimal cuenta);
    }
}
