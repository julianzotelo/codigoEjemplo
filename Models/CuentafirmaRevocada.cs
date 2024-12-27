using System;
using System.Collections.Generic;

namespace pp3.dominio.Models;

public partial class CuentafirmaRevocada
{
    public decimal CTA_SERVICIO { get; set; }

    public string USR_ID { get; set; } = null!;

    public decimal CFR_COEFICIENTEPARTICIPACION { get; set; }

    public decimal CFR_MAR_AUTREQ { get; set; }

    public DateTime FECHA_REV { get; set; }
}
