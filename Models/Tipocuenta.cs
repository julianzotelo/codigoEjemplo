using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Tipocuenta
{
    [Key]
    public decimal TDC_ID { get; set; }

    public string TDC_DESCRIPCION { get; set; } = null!;

    public byte? TDC_COBIS { get; set; }

}
