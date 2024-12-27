using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class FiltroComisionesCobradasDto
    {
        public DateTime? comFechaDesde { get; set; }
        public DateTime? comFechaHasta { get; set; }
        public decimal? tipoDocClt { get; set; }
        public decimal? numDocClt { get; set; }
    }
}
