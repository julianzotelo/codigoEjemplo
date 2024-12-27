using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class Historial
{
    [ForeignKey(nameof(OPG_ID))]
    public decimal OPG_ID { get; set; }

    [ForeignKey(nameof(EST_ID))]
    public string EST_ID { get; set; } = null!;

    public DateTime HIS_FEC { get; set; }

    public int? NRO_OPERACION_CAJA { get; set; }

    public string? ID_OPERADOR_CAJA { get; set; }


}
