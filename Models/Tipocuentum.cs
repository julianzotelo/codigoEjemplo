using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Tipocuentum
{
    [Key]
    public decimal TdcId { get; set; }

    public string TdcDescripcion { get; set; } = null!;

    public byte? TdcCobis { get; set; }

}
