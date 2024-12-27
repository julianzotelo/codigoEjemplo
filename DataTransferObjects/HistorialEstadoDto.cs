using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class HistorialEstadoDto
    {
        public decimal OPG_ID { get; set; }
        public decimal? OPG_NUMCOMPROBANTE { get; set; }
        public string MPG_DESCRIPCION { get; set; }
        public decimal SUC_ENTREGA { get; set; }
        public string SUC_DESCRIPCION { get; set; }
        public string? BNF_RAZONSOC { get; set; }
        public decimal TDD_BNF_ID { get; set; }
        public decimal BNF_NUMDOC { get; set; }
        public string? CLT_RAZONSOC { get; set; }
        public decimal? TDD_CLT_ID { get; set; }
        public decimal CLT_NUMDOC { get; set; }
    }
}
