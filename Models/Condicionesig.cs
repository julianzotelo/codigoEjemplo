using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Condicionesig
{
    [Key]
    public decimal CNG_ID { get; set; }

    public string CNG_DESCRIPCION { get; set; } = null!;

}
