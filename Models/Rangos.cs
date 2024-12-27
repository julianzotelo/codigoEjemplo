using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class Rangos
{
    [Key]
    public decimal RAN_ID { get; set; }


    [ForeignKey(nameof(TDD_CLT_ID))]
    public decimal TDD_CLT_ID { get; set; }


    [ForeignKey(nameof(CLT_NUMDOCCLI))]
    public decimal CLT_NUMDOCCLI { get; set; }


    [ForeignKey(nameof(CTA_SERVICIO))]
    public decimal CTA_SERVICIO { get; set; }


    [ForeignKey(nameof(MPG_ID))]
    public decimal MPG_ID { get; set; }



    public decimal RAN_DESDE { get; set; }

    public decimal RAN_HASTA { get; set; }

    public decimal RAN_ACTUAL { get; set; }


    [ForeignKey(nameof(EST_ID))]
    public string EST_ID { get; set; } = null!;



    public decimal RAN_CANTCHEQUES { get; set; }


}
