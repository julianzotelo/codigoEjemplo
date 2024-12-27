using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pp3.dominio.Models;

namespace pp3.dominio.DataTransferObjects
{
    public class ClientesDto
    {
        public decimal CLT_NUMDOC { get; set; }
        public decimal? TDD_CLT_ID { get; set; }
        public string? CLT_RAZONSOC { get; set; }
        public decimal? CNI_ID { get; set; }
        public string? CLT_CALLE { get; set; }
        public decimal? CLT_NUMPUERTA { get; set; }
        public string? CLT_UNIDADFUNCIONAL { get; set; }
        public decimal CCP_ID { get; set; }
        public decimal SUC_CUENTACOMISION { get; set; }
        public decimal  CLT_CUENTACOMISION { get; set; }
        public DateTime? CLT_FEC_INGRESO { get; set; }
        public string? TipoDocumento { get; set; }
        public string? CondicionIVA { get; set; }
        public string? CLT_ESTADO { get; set; }
    }
}
