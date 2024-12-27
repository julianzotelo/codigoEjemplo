using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class FirmasRetenciones
{
    [Key]
    public string USR_ID { get; set; } = null!;

    public byte[]? FIR_FIRMA { get; set; }

    public decimal? FIR_LEN_FIRMA { get; set; }
}
