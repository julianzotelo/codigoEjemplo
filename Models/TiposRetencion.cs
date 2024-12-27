using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class TiposRetencion
{
    [Key]
    public decimal TRE_ID { get; set; }

    public string TRE_DESCRIPCION { get; set; } = null!;
}
