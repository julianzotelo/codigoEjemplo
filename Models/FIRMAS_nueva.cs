using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.Models
{
    public class FIRMAS_nueva
    {
        public string USR_ID { get; set; } = null!;

        public string FIR_ESTADO { get; set; } = null!;

        //public byte[] FIR_FIRMA { get; set; } = null!;
        public string? FIR_FIRMA { get; set; } = null!;

        public decimal? FIR_LEN_FIRMA { get; set; }
        public string? FIRMA { get; set; }
    }
}
