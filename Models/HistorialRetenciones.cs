using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pp3.dominio.Models;
[PrimaryKey(nameof(RETENCION_ID), nameof(EST_ID))]
public partial class HistorialRetenciones
{
    [ForeignKey(nameof(RETENCION_ID))]
    public decimal RETENCION_ID { get; set; }

    [ForeignKey(nameof(EST_ID))]
    public string EST_ID { get; set; } = null!;

    public DateTime HIS_FEC { get; set; }


}
