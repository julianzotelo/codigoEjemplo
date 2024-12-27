using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class BeneficiariosClienteRpDto
    {
        public decimal BNF_NUMDOC { get; set; }
        public string? BNF_RAZONSOC { get; set; }
        public decimal BNF_MAR_CLIENTEBANSUD { get; set; }
        public decimal TDD_BNF_ID { get; set; }
        public decimal? TDD_CLT_ID { get; set; }
        public decimal CLT_NUMDOC { get; set; }
        public string? BNF_CALLE { get; set; }
        public decimal? BNF_NUMPUERTA { get; set; }
        public string? BNF_UNIDADFUNCIONAL { get; set; }
        public decimal CCP_ID { get; set; }
        public string? CLT_RAZONSOC { get; set; }
        public string? TDD_DESCRIPCIONABREV { get; set; } = null!;
        public string CCP_LOCALIDAD { get; set; } = null!;

    }
}
