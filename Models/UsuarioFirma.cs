using System;
using System.Collections.Generic;

namespace pp3.dominio.Models;

public partial class UsuarioFirma
{
    public decimal? CLT_ID { get; set; }

    public decimal? CLT_NUMDOC { get; set; }

    public string? FIRMANTE { get; set; }

    public DateTime? HORA { get; set; }
}
