using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class EscalasDescuentoDto
    {
        public decimal TDD_CLT_ID { get; set; }

        public decimal CLT_NUMDOC { get; set; }

        public decimal ESD_HASTA { get; set; }

        public decimal MPG_ID { get; set; }

        public decimal ESD_ALICUOTA { get; set; }

        public decimal ESD_IMP_FIJO { get; set; }

        public decimal ESD_IMP_MINIMO { get; set; }

        public decimal ESD_IMP_MAXIMO { get; set; }
    }
}
