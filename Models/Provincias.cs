using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Provincias
{
    [Key]
    public decimal PRV_ID { get; set; }

    public string PRV_DESCRIPCION { get; set; } = null!;

}
