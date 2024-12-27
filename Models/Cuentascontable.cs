using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Cuentascontable
{
    [Key]
    public decimal CCB_ID { get; set; }

    public string CCB_DESCRIPCION { get; set; } = null!;


}
