using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class UsuariosDto
    {
        public string USR_ID { get; set; }
        public int PRF_ID { get; set; }
        public string USR_NOMBRE { get; set; }
        public string USR_APELLIDO { get; set; }
        public int TDD_USR_ID { get; set; }
        public double USR_NUMDOC { get; set; }
        public decimal SUC_ID { get; set; }
        public int TDD_CLT_ID { get; set; }
        public double CLT_NUMDOC { get; set; }
        public string USR_CARGO { get; set; }
        public double NROCOBIS { get; set; }

    }
}
