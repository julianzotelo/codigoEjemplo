using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class FiltroOpgEmitidasRpDto
    {
        public DateTime? FECHA_DESDE_PAGO { get; set; }
        public DateTime? FECHA_HASTA_PAGO { get; set; }
        public bool? TODOS_CLIENTES { get; set; }
        public decimal? TDD_CLIENTE_ID { get; set; }
        public decimal? CLIENTE_NUMDOC { get; set; }
        public bool? TODAS_MODALIDADES { get; set; }
        public decimal? MPG_ID { get; set; }
        public bool? TODAS_SUCURSALES { get; set; }
        public decimal? SUC_ENTREGA { get; set; }
        public bool? TODOS_ESTADOS { get; set; }
        public string? EST_ESTADOACTUAL { get; set; }
        public decimal? OPG_ID { get; set; }
        public decimal? ENV_ID { get; set; }
        public decimal? OPG_NUMCOMPROBANTE { get; set; }
    }
}
