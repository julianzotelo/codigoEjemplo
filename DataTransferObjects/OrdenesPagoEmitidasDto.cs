using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class OrdenesPagoEmitidasDto
    {
        public decimal? OPG_ID { get; set; }
        public decimal? TDD_CLT_ID { get; set; }
        public decimal? CLT_NUMDOC { get; set; }
        public decimal? SUC_ENTREGA { get; set; }
        public decimal? OPG_IMP_PAGO { get; set; }
        public decimal? OPG_CUENTAPAGO { get; set; }
        public decimal? MPG_ID { get; set; }
        public DateTime OPG_FEC_PAGO { get; set; }
        public decimal? OPG_NUMCOMPROBANTE { get; set; }
        public string? BNF_RAZONSOC { get; set; }
        public string? MPG_DESCRIPCION { get; set; }
        public string? CLT_RAZONSOC { get; set; }
        public string? EST_DESCRIPCION { get; set; }
        public string? EST_ESTADOACTUAL { get; set; }
        public decimal? ENV_ID { get; set; }
    }
}
