using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pp3.dominio.Models;
[PrimaryKey(nameof(CTA_SERVICIO), nameof(USR_ID))]
public partial class CUENTAFIRMA
{
    [ForeignKey(nameof(CTA_SERVICIO))]
    [Column(TypeName = "decimal(25,0)")]
    public decimal CTA_SERVICIO { get; set; }
   
    public string USR_ID { get; set; } = null!;

    [Column(TypeName = "decimal(6,5)")]
    public decimal? CFR_COEFICIENTEPARTICIPACION { get; set; }

    public decimal CFR_MAR_AUTREQ { get; set; }

}
