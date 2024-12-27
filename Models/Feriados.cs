using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Feriados
{
    [Key]
    public DateTime FerFecha { get; set; }

    public decimal FerEsferiado { get; set; }

    public decimal FerJulian { get; set; }
}
