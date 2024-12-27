using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.services.Repositories
{
    public interface ISeguridadRepository
    {
        Task<ServicesResult> InsertarLog(int tarea, string usuario, string anterior, string actual);
        Task<ServicesResult> UsuarioCliente(string USR_ID);
        Task<ServicesResult> UsuariosPorCliente(int tipo, double numero);
        Task<ServicesResult> usuarios(string id);
        Task<ServicesResult> UsuarIoD(string id);
        Task<ServicesResult> EliminarUsuario(string idUsuario);
        Task<ServicesResult> Perfiles();
        Task<ServicesResult> ModificarUsuario(UsuariosDto usuario);
        Task<ServicesResult> ActivarUsuario(string UserID);
        Task<ServicesResult> DesactivarUsuario(string UserID);
        Task<ServicesResult> NuevoUsuario(UsuariosDto usuario);
    }
}
