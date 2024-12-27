using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace pp3.dominio.Models;
[PrimaryKey(nameof(TDD_BNF_ID), nameof(BNF_NUMDOC), nameof(TDD_CLT_ID), nameof(CLT_NUMDOC))]
public partial class BENEFICIARIOS
{
    [ForeignKey(nameof(TDD_BNF_ID))]
    public decimal TDD_BNF_ID { get; set; }
    
    public decimal BNF_NUMDOC { get; set; }

    public decimal? TDD_CLT_ID { get; set; }

    public decimal CLT_NUMDOC { get; set; }

    [ForeignKey(nameof(CIB_ID))]
    public decimal CIB_ID { get; set; }
    [ForeignKey(nameof(CNG_ID))]
    public decimal CNG_ID { get; set; }
    [ForeignKey(nameof(CNI_ID))]
    public decimal CNI_ID { get; set; }

    public string? BNF_RAZONSOC { get; set; }

    public string? BNF_CALLE { get; set; }

    public decimal? BNF_NUMPUERTA { get; set; } = 0;

    public string? BNF_UNIDADFUNCIONAL { get; set; }
    [ForeignKey(nameof(CCP_ID))]
    public decimal CCP_ID { get; set; }

    public string? BNF_NUMIB { get; set; }

    public decimal BNF_MAR_CLIENTEBANSUD { get; set; }

    public string? BNF_MAIL { get; set; }

    public decimal BNF_MAR_BAJA { get; set; }

    public string? BNF_MAILS { get; set; }

    public string? BNF_PISO { get; set; }

    public string? BNF_CIUDAD { get; set; }

    public string? BNF_PROVINCIA { get; set; }

    public string? BNF_PAIS { get; set; }

    public decimal? ENV_ID { get; set; }

    public decimal? ID_REGISTRO { get; set; }


}
