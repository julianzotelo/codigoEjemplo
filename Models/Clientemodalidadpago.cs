using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pp3.dominio.Models;
[PrimaryKey(nameof(MPG_ID), nameof(TDD_CLT_ID), nameof(CLT_NUMDOCCLI))]
public partial class Clientemodalidadpago
{
    [ForeignKey(nameof(MPG_ID))]
    public decimal MPG_ID { get; set; }
    [ForeignKey(nameof(TDD_CLT_ID))]
    public decimal TDD_CLT_ID { get; set; }
    [ForeignKey(nameof(CLT_NUMDOCCLI))]
    public decimal CLT_NUMDOCCLI { get; set; }


    public decimal CMP_PERIODOCOBRO { get; set; }

    public decimal CMP_MOMENTOPERIODOCOBRO { get; set; }

    public decimal CMP_MAR_EVENTOCOBRO { get; set; }

    public decimal CMP_MAR_ESCALA_X_IMPORTE { get; set; }

    public DateTime CMP_FEC_ULTIMALIQUIDACION { get; set; }

    public decimal CMP_IMP_MONTOMAXIMO { get; set; }

    public decimal CMP_ESTIM_MENS_PAGOS { get; set; }

    public decimal CMP_DIAS_DEBITO { get; set; }


}
