using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class ChequeBajaCobis
{
    [Key] // No es PK en base de datos
    public decimal OPG_NUMERO_CHEQUE { get; set; }

    public decimal CTA_CUENTADEBITO { get; set; }

    public byte MND_COBIS { get; set; }
}
