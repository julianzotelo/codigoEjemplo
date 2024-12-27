using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using pp3.dominio.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace pp3.services.Repositories
{
    public interface IClientesRepository
    {
        public Task<ServicesResult> Nuevo(Clientes nuevoCliente);
        public Task<ServicesResult> Baja(decimal tipoDoc, decimal numDoc);
        public Task<ServicesResult> CondicionIva(string codIvaCobis);
        public Task<ServicesResult> NombreSucursal(string sucDescripcion);
        public Task<ServicesResult> NombreMoneda(decimal codMonedaCobis);
        public Task<ServicesResult> Cliente(decimal tipoDoc, decimal numDoc);
        public Task<ServicesResult> Modificar(Clientes clienteModificacion);
        public Task<ServicesResult> Proovedor(decimal tipoDoc, decimal numDoc, decimal bnfTipo, decimal bnfNumero);
        public Task<ServicesResult> Beneficiarios(decimal tipoDoc, decimal numDoc);
        public Task<ServicesResult> RangoTodosLosClientes();
        public Task<ServicesResult> ClientesRazonSocial(string razonSocial);
        public Task<ServicesResult> ClientesConOrdenesEnEstado(decimal? tipoDoc, decimal numDoc, string estadoAValidar);
        public Task<ServicesResult> ActivaDesactivaCliente(decimal tipoDoc, decimal numDoc, int operacion);
        public Task<ServicesResult> Clientes(decimal tipoDoc, decimal numDoc);
        public Task<ServicesResult> OrdenDepagoPendiente(decimal tipoDoc, decimal numDoc);
        public Task<ServicesResult> TodosLosClientes();
        public Task<ServicesResult> ClienteModalidadesPago(decimal tipoDoc, decimal numDoc);

    }
}
