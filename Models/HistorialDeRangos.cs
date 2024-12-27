using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pp3.dominio.Models;
[PrimaryKey(nameof(RAN_ID), nameof(EST_ID))]
public partial class HistorialDeRangos
{
    [ForeignKey(nameof(RAN_ID))]
    public decimal RAN_ID { get; set; }

    [ForeignKey(nameof(EST_ID))]
    public string EST_ID { get; set; } = null!;

    public DateTime HRA_FEC { get; set; }

}
