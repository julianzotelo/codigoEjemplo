using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class Modalidadpago
{
    [Key]
    public decimal MPG_ID { get; set; }

    [ForeignKey(nameof(TDF_ID))]
    public decimal TDF_ID { get; set; }

    public string MPG_DESCRIPCION { get; set; } = null!;

    [ForeignKey(nameof(CCB_ID))]
    public decimal CCB_ID { get; set; }

    public string MPG_COMISIONESPORSUCURSAL { get; set; } = null!;

    public decimal MPG_PORCDISTRIB { get; set; }

 
}
