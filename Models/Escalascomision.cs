using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pp3.dominio.Models;
[PrimaryKey(nameof(TDD_CLT_ID), nameof(CLT_NUMDOC), nameof(ESC_HASTA), nameof(MPG_ID))]
public partial class Escalascomision
{
    [ForeignKey(nameof(TDD_CLT_ID))]
    public decimal TDD_CLT_ID { get; set; }

    [ForeignKey(nameof(CLT_NUMDOC))]
    public decimal CLT_NUMDOC { get; set; }
    [Column(TypeName = "decimal(15,2)")]
    public decimal ESC_HASTA { get; set; }

    [ForeignKey(nameof(MPG_ID))]
    public decimal MPG_ID { get; set; }
    [Column(TypeName = "decimal(8,5)")]
    public decimal ESC_ALICUOTA { get; set; }
    [Column(TypeName = "decimal(15,2)")]
    public decimal ESC_IMP_FIJO { get; set; }
    [Column(TypeName = "decimal(15,2)")]
    public decimal ESC_IMP_MINIMO { get; set; }
    [Column(TypeName = "decimal(15,2)")]
    public decimal ESC_IMP_MAXIMO { get; set; }

}







