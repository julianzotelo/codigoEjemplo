using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class CuentafirmaDto
    {
        public decimal CTA_SERVICIO { get; set; }

        public string USR_ID { get; set; } = null!;

        public decimal? CFR_COEFICIENTEPARTICIPACION { get; set; }

        public decimal CFR_MAR_AUTREQ { get; set; }
    }
}
