using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace pp3.dominio.Models;

public partial class Bancos
{
    [Key]
    public decimal BCO_ID { get; set; }

    public string BCO_DESCRIPCION { get; set; } = null!;

}
