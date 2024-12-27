using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Condicionesiva
{
    [Key]
    public decimal? CNI_ID { get; set; }

    public string? CNI_DESCRIPCION { get; set; } = null!;

    public decimal? CNI_TASA { get; set; }

    public decimal? CNI_SOBRETASA { get; set; }

    public Condicionesiva(decimal? cNI_ID, string? cNI_DESCRIPCION, decimal? cNI_TASA, decimal? cNI_SOBRETASA)
    {
        CNI_ID = cNI_ID;
        CNI_DESCRIPCION = cNI_DESCRIPCION;
        CNI_TASA = cNI_TASA;
        CNI_SOBRETASA = cNI_SOBRETASA;
    }
}
