using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Tarea
{
    [Key]
    public decimal TAR_ID { get; set; }

    public string TAR_DESCRIPCION { get; set; } = null!;

}
