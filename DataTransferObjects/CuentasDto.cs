using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class CuentasDto
    {
        public decimal? CTA_SERVICIO { get; set; }
                      
        public decimal? SUC_CUENTASERVICIO { get; set; }

        public decimal? MND_CUENTASERVICIO { get; set; }

        public decimal? TDC_CUENTASERVICIO { get; set; }

        public decimal? SUC_CUENTACLIENTE { get; set; }

        public decimal? MND_CUENTACLIENTE { get; set; }

        public decimal? TDC_CUENTACLIENTE { get; set; }

        public decimal? CTA_CLIENTE { get; set; }

        public decimal? TDD_CLT_ID { get; set; }

        public decimal? CLT_NUMDOC { get; set; }

        public decimal? CTA_RELACIONSIAF { get; set; }

        public decimal? CTA_INACTIVA { get; set; }

        public decimal? CTA_CONVENIO { get; set; }

        public DateTime? CTA_FECHAPERTURA { get; set; }

        public decimal? CTA_MAR_FIRMAELBANCO { get; set; }

        public byte? TIPO_CONVENIO { get; set; }

        public bool? CTA_COMISION { get; set; }

        public string? CTA_CONVENIO_BMA { get; set; }
        public string? TDD_DESCRIPCION { get; set; }
        public string? SUC_DESCRIPCION { get; set; }
    }
}
