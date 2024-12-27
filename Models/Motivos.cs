using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Motivos
{
    [Key]
    public decimal MTV_ID { get; set; }

    public string MTV_DESCRIPCION { get; set; } = null!;

}
