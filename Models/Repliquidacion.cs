using System;
using System.Collections.Generic;

namespace pp3.dominio.Models;

public partial class Repliquidacion
{
    public string REPLIQ_CLT_RAZONSOC { get; set; } = null!;

    public decimal REPLIQ_CLT_NUMDOC { get; set; }

    public string REPLIQ_CLT_DOMICILIO { get; set; } = null!;

    public string REPLIQ_FEC_PAGO { get; set; } = null!;

    public string? REPLIQ_FEC_RECEP { get; set; }

    public string REPLIQ_NUMERO_COMPROBANTE { get; set; } = null!;

    public decimal REPLIQ_IMPORTE { get; set; }

    public string REPLIQ_BNF_RAZONSOC { get; set; } = null!;

    public decimal REPLIQ_BNF_NUMDOC { get; set; }

    public string REPLIQ_BNF_DOMICILIO { get; set; } = null!;

    public string REPLIQ_TIPO_COMPROBANTE { get; set; } = null!;

    public string? REPLIQ_REFERENCIA { get; set; }

    public string? REPLIQ_OPER_ORIGINAL { get; set; }

    public string? REPLIQ_OBSERVACION { get; set; }

    public decimal REPLIQ_OPG_ID { get; set; }

    public string REPLIQ_OPG_IDOPGCLI { get; set; } = null!;

    public decimal REPLIQ_OPG_IMP_PAGO { get; set; }

    public string? REPLIQ_CUENTAPAGO_NROCHEQUE { get; set; }

    public string? REPLIQ_OPG_MONTOLETRAS { get; set; }

    public decimal? REPLIQ_SEC_IMPRESION { get; set; }
}
