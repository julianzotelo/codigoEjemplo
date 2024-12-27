using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class ReporteOpgDto
    {
        [Key]
        public decimal Id { get; set; }

        public string? Tipo_Doc_Beneficiario { get; set; }

        public decimal Beneficiario { get; set; }

        public string? Tipo_Doc_Cliente { get; set; }

        public decimal Cliente { get; set; }

        public string? Sucursal { get; set; }

        public decimal? Cuenta_Pago { get; set; }

        public string? Mod_Pago { get; set; }

        public string Estado { get; set; } = null!;

        public string? Monto { get; set; }

    }
}
