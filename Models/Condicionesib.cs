using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Condicionesib
{
    [Key]
    public decimal CIB_ID { get; set; }

    public string CIB_DESCRIPCION { get; set; } = null!;

}
