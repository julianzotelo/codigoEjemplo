using pp3.dominio.Models;

namespace pp3.services.Repositories
{
    public interface IFirmasRepository
    {
        public Task<ServicesResult> FirmasPorId(string USR_ID);
        public Task<ServicesResult> FirmasPorIdDoc(string id, int tipo, decimal numero);
        public Task<ServicesResult> FirmasPorDocumento(int tipo, decimal numero);
        public Task<ServicesResult> FirmasPorCliente(int tipo, decimal numero);
        public Task<ServicesResult> FirmantesPorCliente(int tipo, decimal numero);
        public Task<ServicesResult> FirmasPorApellido(string apellido);
        public Task<ServicesResult> Firma(string id);
        public Task<ServicesResult> NuevaFirma (FIRMAS_nueva firmas);
        public Task<ServicesResult> VerFirma(string firma);
        public Task<ServicesResult> EliminarFirma(string id);
        public Task<ServicesResult> EliminarFirmaCuenta(decimal cuenta, string usr);
        public Task<ServicesResult> ModificarCuentaFirma(CUENTAFIRMA cuentaFirmaCambiado);
        public Task<ServicesResult> Cuentas(int id);
        public Task<ServicesResult> FirmasPorCuenta(decimal cuenta);
        public Task<ServicesResult> ModificarFirma(FIRMAS_nueva firmas);
        public Task<ServicesResult> ExistenFirmantesVacios(int tipo, decimal numero);
        public Task<ServicesResult> NuevaCuentaFirma(CUENTAFIRMA cuentaFirma);

    }
}
