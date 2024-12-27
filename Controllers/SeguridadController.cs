using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pp3.dominio.Context;
using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using pp3.services.Services;

namespace pp3.api.Controllers
{
    [ApiController]
    [Route("api/Seguridad")]
    public class SeguridadController : ControllerBase
    {

        private readonly Pp3roContext context;
        private SeguridadService seguridadService;
        private readonly ILogger<SeguridadService> _logger;
        private readonly IMapper _mapper;

        public SeguridadController(Pp3roContext context, ILogger<SeguridadService> logger, IMapper mapper)
        {
            this.context = context;
            this._logger = logger;
            this._mapper = mapper;
            this.seguridadService = new SeguridadService(context, new HttpContextAccessor(), logger, mapper);
        }

        [HttpPost]
        public async Task<ServicesResult> InsertarLog(int tarea, string usuario, string anterior, string actual)
        {
            return await seguridadService.InsertarLog(tarea, usuario, anterior, actual);
        }


        [HttpGet("UsuarioCliente")]
        public async Task<ServicesResult> UsuarioCliente(string USR_ID)
        {
            return await seguridadService.UsuarioCliente(USR_ID);
        }
        
        [HttpGet("usuariosPorCliente")]
        public async Task<ServicesResult> UsuariosPorCliente(int tipo, double numero)
        {
            return await seguridadService.UsuariosPorCliente(tipo, numero);
        }

        [HttpGet("usuarios")]
        public async Task<ServicesResult> usuarios(string id)
        {
            return await seguridadService.usuarios(id);
        }

        [HttpGet("usuarIoD")]
        public async Task<ServicesResult> UsuarIoD(string id)
        {
            return await seguridadService.UsuarIoD(id);
        }

        [HttpDelete("eliminarUsuario")]
        public async Task<ServicesResult> EliminarUsuario(string idUsuario)
        {
            return await seguridadService.EliminarUsuario(idUsuario);
        }

        [HttpGet("perfiles")]
        public async Task<ServicesResult> Perfiles()
        {
            return await seguridadService.Perfiles();
        }

        [HttpPut("modificarUsuario")]
        public async Task<ServicesResult> ModificarUsuario(UsuariosDto usuario)
        {
            return await seguridadService.ModificarUsuario(usuario);
        }

        [HttpGet("activarUsuario")]
        public async Task<ServicesResult> ActivarUsuario(string UserID)
        {
            return await seguridadService.ActivarUsuario(UserID);
        }

        [HttpGet("desactivarUsuario")]
        public async Task<ServicesResult> DesactivarUsuario(string UserID)
        {
            return await seguridadService.DesactivarUsuario(UserID);
        }

        [HttpPost("nuevoUsuario")]
        public async Task<ServicesResult> NuevoUsuario(UsuariosDto usuario)
        {
            return await seguridadService.NuevoUsuario(usuario);
        }
    }
}
