using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class ReciboExternoRpDto
    {
        public decimal OPG_IMP_PAGO { get; set; }
        public DateTime OPG_FEC_PAGO { get; set; }
        public decimal? OPG_NUMCOMPROBANTE { get; set; }
        public string? BNF_RAZONSOC { get; set; }
        public string? BNF_CALLE { get; set; }
        public decimal? BNF_NUMPUERTA { get; set; }
        public string? BNF_UNIDADFUNCIONAL { get; set; }
        public decimal CCP_ID { get; set; }
        public string CCP_LOCALIDAD { get; set; } = null!;
        public string PRV_DESCRIPCION { get; set; } = null!;

    }
}
