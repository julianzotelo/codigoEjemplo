using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pp3.dominio.Models;

public partial class ORDENESPAGO
{
    [Key]
    public decimal OPG_ID { get; set; }


    [ForeignKey(nameof(TDD_BNF_ID))]
    public decimal TDD_BNF_ID { get; set; }


    [ForeignKey(nameof(BNF_NUMDOC))]
    public decimal BNF_NUMDOC { get; set; }


    [ForeignKey(nameof(TDD_CLT_ID))]
    public decimal? TDD_CLT_ID { get; set; }


    [ForeignKey(nameof(CLT_NUMDOC))]
    public decimal CLT_NUMDOC { get; set; }


    [ForeignKey(nameof(SUC_ENTREGA))]
    public decimal SUC_ENTREGA { get; set; }


    [ForeignKey(nameof(OPG_IDOPGCLI))]
    public string? OPG_IDOPGCLI { get; set; }



    public string? OPG_ORDENALT { get; set; }

    public decimal? OPG_IMP_DEBITO { get; set; }

    public decimal OPG_IMP_PAGO { get; set; }


    [ForeignKey(nameof(SUC_CUENTADEBITO))]
    public decimal SUC_CUENTADEBITO { get; set; }


    [ForeignKey(nameof(TDC_DEBITO))]
    public decimal TDC_DEBITO { get; set; }


    [ForeignKey(nameof(MND_DEBITO))]
    public decimal MND_DEBITO { get; set; }


    [ForeignKey(nameof(CTA_CUENTADEBITO))]
    public decimal CTA_CUENTADEBITO { get; set; }


    [ForeignKey(nameof(BCO_CUENTAPAGO))]
    public decimal? BCO_CUENTAPAGO { get; set; }


    [ForeignKey(nameof(SUC_CUENTAPAGO))]
    public decimal? SUC_CUENTAPAGO { get; set; }


    [ForeignKey(nameof(TDC_PAGO))]
    public decimal? TDC_PAGO { get; set; }


    [ForeignKey(nameof(MND_PAGO))]
    public decimal MND_PAGO { get; set; }



    public decimal? OPG_CUENTAPAGO { get; set; }

    public decimal? OPG_AUXCBU { get; set; }


    [ForeignKey(nameof(MPG_ID))]
    public decimal MPG_ID { get; set; }



    public decimal? OPG_MAR_REGCHQ { get; set; }

    public DateTime OPG_FEC_PAGO { get; set; }

    public DateTime? OPG_FEC_PAGODIFERIDO { get; set; }

    public decimal? OPG_NUMCOMPROBANTE { get; set; }

    public decimal? COM_ID { get; set; }


    [ForeignKey(nameof(USR_FIRMANTE1))]
    public string? USR_FIRMANTE1 { get; set; }


    [ForeignKey(nameof(USR_FIRMANTE2))]
    public string? USR_FIRMANTE2 { get; set; }


    [ForeignKey(nameof(USR_FIRMANTE3))]
    public string? USR_FIRMANTE3 { get; set; }



    public string EST_ESTADOACTUAL { get; set; } = null!;

    public decimal? OPG_NUM_ENVIO { get; set; }


    [ForeignKey(nameof(ENV_ID))]
    public decimal? ENV_ID { get; set; }

    public decimal? OPG_NRORECIBO { get; set; }

    public string? PATH_DOCU { get; set; }

    public decimal? ID_REGISTRO { get; set; }

    public string? ID_ECHEQ { get; set; }

    public string? FIRMANTES_ECHEQ { get; set; }

    public decimal? RAN_ID { get; set; }

 
}
