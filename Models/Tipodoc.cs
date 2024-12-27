using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Tipodoc
{
    [Key]
    public decimal? TDD_ID { get; set; }

    public string? TDD_DESCRIPCION { get; set; } = null!;

    public string? TDD_DESCRIPCIONABREV { get; set; } = null!;

    public string? TDD_CODIGOSIAF { get; set; } = null!;

    public string? TDD_MASCARA { get; set; }

    public string? TDD_COBIS { get; set; }

    public Tipodoc(decimal? tDD_ID, string? tDD_DESCRIPCION, string? tDD_DESCRIPCIONABREV, string? tDD_CODIGOSIAF, string? tDD_MASCARA, string? tDD_COBIS)
    {
        TDD_ID = tDD_ID;
        TDD_DESCRIPCION = tDD_DESCRIPCION;
        TDD_DESCRIPCIONABREV = tDD_DESCRIPCIONABREV;
        TDD_CODIGOSIAF = tDD_CODIGOSIAF;
        TDD_MASCARA = tDD_MASCARA;
        TDD_COBIS = tDD_COBIS;
    }
}
