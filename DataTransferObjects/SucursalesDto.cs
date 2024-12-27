using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class SucursalesDto
    {
        public decimal SUC_ID { get; set; }

        public decimal BCO_ID { get; set; }

        public decimal CDC_ID { get; set; }

        public decimal CCP_ID { get; set; }

        public string SUC_DESCRIPCION { get; set; } = null!;

        public string SUC_CALLE { get; set; } = null!;

        public string SUC_UNIDADFUNCIONAL { get; set; } = null!;

        public decimal SUC_MAR_MIGRACION { get; set; }

        public decimal SUC_MIGRADA { get; set; }

        public decimal SUC_MAR_BAJA { get; set; }
    }
}
