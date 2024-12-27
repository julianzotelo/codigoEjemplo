using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pp3.dominio.Models;

[PrimaryKey(nameof(TDD_CLT_ID), nameof(CLT_NUMDOC))]
public partial class Clientes
{
    [ForeignKey(nameof(TDD_CLT_ID))]
    public decimal? TDD_CLT_ID { get; set; }
    public decimal CLT_NUMDOC { get; set; }

    public string? CLT_RAZONSOC { get; set; }
    [ForeignKey(nameof(CCP_ID))]
    public decimal CCP_ID { get; set; }

    public string? CLT_CALLE { get; set; }

    public decimal? CLT_NUMPUERTA { get; set; }

    public string? CLT_UNIDADFUNCIONAL { get; set; }
    [ForeignKey(nameof(CNI_ID))]
    public decimal? CNI_ID { get; set; }
    [ForeignKey(nameof(MTV_ID))]
    public decimal MTV_ID { get; set; }

    public DateTime? CLT_FEC_INGRESO { get; set; }

    public string? CLT_ESTADO { get; set; }

    public DateTime? CLT_FEC_ESTADO { get; set; }
    [ForeignKey(nameof(SUC_CUENTACOMISION))]
    public decimal SUC_CUENTACOMISION { get; set; }
    [ForeignKey(nameof(TDC_CUENTACOMISION))]
    public decimal TDC_CUENTACOMISION { get; set; }
    [ForeignKey(nameof(MND_CUENTACOMISION))]
    public decimal MND_CUENTACOMISION { get; set; }

    public decimal CLT_CUENTACOMISION { get; set; }

    public decimal CLT_MAR_MIGRACION { get; set; }

    public decimal CLT_MAR_SOYELBANCO { get; set; }

    public decimal CLT_DUP_CHK { get; set; }

    public decimal CLT_MAR_TRANSMITIENDO { get; set; }

    public string? CLT_MAILS { get; set; }

    public decimal? CLT_MAR_PERMITECHEQUEMISMODIAFECHAPAGO { get; set; }

    public decimal? CLT_MAR_EMITECHEQUECUANDOSEAUTORIZA { get; set; }

    public decimal? CLT_MAR_TRANSMITEARCHIVOSENCRIPTADOS { get; set; }

    public decimal? CLT_MAR_VALIDAUSOFIRMASCOBIS { get; set; }

    public decimal? CLT_MAR_EMITERETENCIONCENTRALIZADA { get; set; }

    public decimal? CLT_MAR_EMITECARATULA { get; set; }

    public decimal? CLT_MAR_ASIGNANROCHEQUE { get; set; }

    public byte? CLT_MAR_ACRED_CONCENTRADOR { get; set; }

    public byte? CLT_MAR_ACTIVA_CHEQUES { get; set; }

    public byte? CLT_SFTP_AUT { get; set; }

    public byte? CTA_MISMO_TIT { get; set; }

    public string? CLT_MAR_ENROLAMIENTO { get; set; }

    public bool? CLT_MAR_BMA { get; set; }


}
