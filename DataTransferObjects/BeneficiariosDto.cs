using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class BeneficiariosDto
    {
        public decimal TDD_BNF_ID { get; set; }

        public decimal BNF_NUMDOC { get; set; }

        public decimal CIB_ID { get; set; }

        public decimal CNG_ID { get; set; }

        public decimal CNI_ID { get; set; }

        public string? BNF_RAZONSOC { get; set; }

        public string? BNF_CALLE { get; set; }

        public decimal? BNF_NUMPUERTA { get; set; }

        public string? BNF_UNIDADFUNCIONAL { get; set; }

        public decimal CCP_ID { get; set; }

        public string? BNF_NUMIB { get; set; }

        public decimal BNF_MAR_CLIENTEBANSUD { get; set; }
        public string? TDD_DESCRIPCION { get;set; }
        public decimal TDD_CLT_ID { get; set; }
        public decimal CLT_NUMDOC { get; set; }
    }
}
