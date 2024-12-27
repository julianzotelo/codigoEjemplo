using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class OrdenespagoDto
    {
        public decimal OPG_ID { get; set; }

        public decimal TDD_BNF_ID { get; set; }

        public decimal BNF_NUMDOC { get; set; }

        public decimal TDD_CLT_ID { get; set; }

        public decimal CLT_NUMDOC { get; set; }

        public decimal SUC_ENTREGA { get; set; }

        public string? OPG_IDOPGCLI { get; set; }

        public string? OPG_ORDENALT { get; set; }

        public decimal? OPG_IMP_DEBITO { get; set; }

        public decimal OPG_IMP_PAGO { get; set; }

        public decimal SUC_CUENTADEBITO { get; set; }

        public decimal TDC_DEBITO { get; set; }

        public decimal MND_DEBITO { get; set; }

        public decimal CTA_CUENTADEBITO { get; set; }

        public decimal? BCO_CUENTAPAGO { get; set; }

        public decimal? SUC_CUENTAPAGO { get; set; }

        public decimal? TDC_PAGO { get; set; }

        public decimal MND_PAGO { get; set; }

        public decimal? OPG_CUENTAPAGO { get; set; }

        public decimal? OPG_AUXCBU { get; set; }

        public decimal MPG_ID { get; set; }

        public decimal? OPG_MAR_REGCHQ { get; set; }

        public DateTime OPG_FEC_PAGO { get; set; }

        public DateTime? OPG_FEC_PAGODIFERIDO { get; set; }

        public decimal? OPG_NUMCOMPROBANTE { get; set; }

        public decimal? COM_ID { get; set; }

        public string? USR_FIRMANTE1 { get; set; }

        public string? USR_FIRMANTE2 { get; set; }

        public string? USR_FIRMANTE3 { get; set; }

        public string? EST_ESTADOACTUAL { get; set; }

    }
}
