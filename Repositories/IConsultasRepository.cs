using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.services.Repositories
{
    public interface IConsultasRepository
    {
        Task<ServicesResult> tipoDoc();
        Task<ServicesResult> Tipo(int Id);
        Task<ServicesResult> Modalidades();
        Task<ServicesResult> sucursales();
        Task<ServicesResult> Sucursal(int Id);
        Task<ServicesResult> IVAs();
        Task<ServicesResult> iva(int Id);
        Task<ServicesResult> ExisteCodigoPostal(int cod);
        Task<ServicesResult> ExisteSucursal(int cod);
        Task<ServicesResult> estados();
        Task<ServicesResult> EstadosVisibles();
        Task<ServicesResult> ConIB();
        Task<ServicesResult> ConIG();
        Task<ServicesResult> Motivos();
        Task<ServicesResult> tipoCuenta(int idTipoCuenta);
        Task<ServicesResult> TiposDeCuentas();
        Task<ServicesResult> TiposDeCuentasCobis();
        Task<ServicesResult> Monedas();
        Task<ServicesResult> Provincias();
        Task<ServicesResult> TipoConvenio(int? Id);
        Task<ServicesResult> TiposConvenio();
        Task<ServicesResult> ConsultaAcreditacionesInterbanking(string? TipoDocCLT, string? NumDocCLT, string? MpgId, string? EstadoActual, string? OPGID, string? EnvId, string? FechaDesde, string? FechaHasta);
        Task<ServicesResult> ValidarConsultaHistorialEstados(decimal? opgId, decimal? numComprobante);
        Task<ServicesResult> ConsultaHistorialEstado(decimal? opgId, decimal? nroCheque);
        Task<ServicesResult> ConsultaHistorialEstadoTabla(decimal opgId);
        Task<ServicesResult> ConsultaComisionesCobradas(FiltroComisionesCobradasDto filtroComisionesCobradasDto);
    }
}
