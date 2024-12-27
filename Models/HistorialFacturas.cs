using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pp3.dominio.Models;
[PrimaryKey(nameof(FAC_ID), nameof(EST_ID))]
public partial class HistorialFacturas
{
    [ForeignKey(nameof(FAC_ID))]
    public decimal FAC_ID { get; set; }

    [ForeignKey(nameof(EST_ID))]
    public string EST_ID { get; set; } = null!;

    public DateTime HIS_FEC { get; set; }


}
