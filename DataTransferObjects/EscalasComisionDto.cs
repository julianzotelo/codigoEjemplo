using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class EscalasComisionDto
    {
        public decimal TDD_CLT_ID { get; set; }

        public decimal CLT_NUMDOC { get; set; }

        public decimal ESC_HASTA { get; set; }

        public decimal MPG_ID { get; set; }

        public decimal ESC_ALICUOTA { get; set; }

        public decimal ESC_IMP_FIJO { get; set; }

        public decimal ESC_IMP_MINIMO { get; set; }

        public decimal ESC_IMP_MAXIMO { get; set; }

        public string MPG_DESCRIPCION { get; set; }
    }
}
