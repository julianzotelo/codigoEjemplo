using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class Facturas
{
    [Key]
    public decimal FAC_ID { get; set; }

    [ForeignKey(nameof(EST_ID))]
    public string EST_ID { get; set; } = null!;

    public string FAC_ID_CLIENTE { get; set; } = null!;

    public decimal FAC_IMP_NETO { get; set; }

    public DateTime? FAC_FEC_RECEPCION { get; set; }

    public decimal? FAC_IMP_IVA1 { get; set; }

    public decimal? FAC_IMP_IVA2 { get; set; }

    public string? FAC_OBSERVACION { get; set; }

    [ForeignKey(nameof(TFA_ID))]
    public decimal TFA_ID { get; set; }

    [ForeignKey(nameof(OPG_ID))]
    public decimal OPG_ID { get; set; }

    [ForeignKey(nameof(OPG_ID_ORIGINAL))]
    public decimal? OPG_ID_ORIGINAL { get; set; }

    [ForeignKey(nameof(USR_ID))]
    public string USR_ID { get; set; } = null!;

    [ForeignKey(nameof(ENV_ID))]
    public decimal ENV_ID { get; set; }

    public string FAC_CARGO { get; set; } = null!;

    public string? FAC_REFERENCIA { get; set; }

    public string? FAC_OPER_ORIGINAL { get; set; }


}
