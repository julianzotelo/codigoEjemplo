using System;
using System.Collections.Generic;

namespace pp3.dominio.Models;

public partial class Clientemodalidadpagocuenta
{
    public decimal TddCltId { get; set; }

    public decimal CltNumdoccli { get; set; }

    public decimal MpgId { get; set; }

    public decimal CmpEstimMensPagos { get; set; }

    public decimal CtaServicio { get; set; }

    public decimal MndCuentaservicio { get; set; }

    public decimal? CltMarAsignanrocheque { get; set; }
}
