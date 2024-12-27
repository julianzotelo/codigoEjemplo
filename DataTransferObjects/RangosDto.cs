using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class RangosDto
    {
        public decimal? RAN_ID { get; set; }

        public decimal? TDD_CLT_ID { get; set; }

        public decimal? CLT_NUMDOCCLI { get; set; }

        public decimal? CTA_SERVICIO { get; set; }

        public decimal? MPG_ID { get; set; }

        public decimal? RAN_DESDE { get; set; }

        public decimal? RAN_HASTA { get; set; }

        public decimal? RAN_ACTUAL { get; set; }

        public string? EST_ID { get; set; } 

        public DateTime? HRA_FEC { get; set; } 
        public string? CLT_RAZONSOC { get; set; }
        public string? MPG_DESCRIPCION { get; set; }
        public string? TDD_DESCRIPCIONABREV { get; set; }
        public string? EST_DESCRIPCION { get; set; }
        public string? Icono { get; set; }


    }
}
