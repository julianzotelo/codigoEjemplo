using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.services.Repositories
{
    public interface IOrdenesPagoRepository
    {
        Task<ServicesResult> OrdenesPorClienteNumero(int tipoCli, double numCli, double numero);
        Task<ServicesResult> OrdenesPorClienteBeneficiarioNumero(int tipoCli, double numCli, int tipoBen, double numBen, double numero);
        Task<ServicesResult> OrdenesPorBeneficiario(int tipoBen, double numBen);
        Task<ServicesResult> OrdenesPorNumero(double id);
        Task<ServicesResult> Orden(double Id);
        Task<ServicesResult> Cheque(double nro);
        Task<ServicesResult> ChequeAnuladoCOBIS(double nroCheque);
        Task<ServicesResult> LogCheque(ChequeBajaCobis chequeBajaCobis);
        Task<ServicesResult> AnularOPG(decimal id);
        Task<ServicesResult> AnularOPGporEnvio(int tipoDoc, double numDoc, double envio);
        Task<ServicesResult> NuevaOrden(OrdenespagoDto ordenesPagoDto);
        Task<ServicesResult> EliminarOrdenPago(double id);
        Task<ServicesResult> ModificarOrden(OrdenespagoDto ordenesPagoDto);
        Task<ServicesResult> CantOpgsEnvio(int tipoDoc, double numero, double envio);
        Task<ServicesResult> CantOpgsEnvioAnulables(int tipoDoc, double numero, double envio);
        Task<ServicesResult> OpgEmitidas_Rp(FiltroOpgEmitidasRpDto filtroOpgEmitidasRpDto);
        Task<ServicesResult> ReversarOPG(double cOPG_ID, string sucEnt, string user);
        Task<ServicesResult> OpgPorEnvio(decimal? tipoDoc, decimal? numDoc, decimal? numEnvio);
        Task<ServicesResult> ConsultaOrdenesPagoReporte(FiltrosReporteOpg filtros);
        Task<ServicesResult> DetalleOpgAfectadasAComisionesCobradas(FiltroComisionesCobradasDto filtroComisionesCobradasDto);
    }
}
