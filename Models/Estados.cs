using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Estados
{
    [Key]
    public string EST_ID { get; set; } = null!;

    public string EST_DESCRIPCION { get; set; } = null!;

    public decimal EST_MAR_VISIBLE { get; set; }

  
}
