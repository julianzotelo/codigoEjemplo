using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pp3.dominio.DataTransferObjects;
using pp3.dominio.Models;

namespace pp3.services.Repositories
{
    public interface IBeneficiariosRepository
    {
        public Task<ServicesResult> EliminarBeneficiario(decimal tipoCli, decimal NumCli, decimal tipoBen, decimal numBen);
        public Task<ServicesResult> BeneficiariosPorCliente(decimal tipoCli, decimal numero, string? razon);
        public Task<ServicesResult> Beneficiario(int tipoDocCli, decimal numDocCli, decimal tipoDocBnf, decimal numDocBnf);
        public Task<ServicesResult> ModificarBeneficiario(BeneficiariosDto beneficiarioModif);
        public Task<ServicesResult> EsCliente(int tipoDoc, decimal numDoc);
        public Task<ServicesResult> ReciboInternoRp(string filtro, Paginado paginado);
        public Task<ServicesResult> ReciboExternoRp(string filtro, Paginado paginado);
        public Task<ServicesResult> BeneficiariosClienteRp(string filtro, Paginado paginado);
    }
}
