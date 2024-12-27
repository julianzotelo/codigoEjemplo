using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.Models
{
    public class FiltrosReporteOpg
    {
        public decimal tipoDoc {  get; set; }
        public decimal? documentoDesde { get; set; }
        public decimal? documentoHasta { get; set; }
        public decimal? modalidad { get; set; }
        public string? estado { get; set; }
        public DateTime? fechaDesde { get; set; }
        public DateTime? fechaHasta { get; set; }
        public decimal? sucursalDeEntrega { get; set; }
        public decimal? cuentaPagoDesde { get; set; }
        public decimal? cuentaPagoHasta { get; set; }
    }
}
