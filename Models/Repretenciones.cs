using System;
using System.Collections.Generic;

namespace pp3.dominio.Models;

public partial class Repretenciones
{
    public string REPRET_REGIMEN { get; set; } = null!;

    public string REPRET_CLT_RAZONSOC { get; set; } = null!;

    public decimal REPRET_CLT_NUMDOC { get; set; }

    public string REPRET_CLT_DOMICILIO { get; set; } = null!;

    public string REPRET_ID_COMPROBANTE { get; set; } = null!;

    public string REPRET_FEC_PAGO { get; set; } = null!;

    public string REPRET_TIPO_COMPROBANTE { get; set; } = null!;

    public string REPRET_NUMERO_FACTURA { get; set; } = null!;

    public decimal REPRET_MONTO_COMPROBANTE { get; set; }

    public decimal REPRET_MONTO_RETENCION { get; set; }

    public string REPRET_MONTORETLETRAS { get; set; } = null!;

    public string REPRET_FIRMA { get; set; } = null!;

    public string REPRET_ACLARACION { get; set; } = null!;

    public string REPRET_BNF_RAZONSOC { get; set; } = null!;

    public string REPRET_BNF_DOMICILIO { get; set; } = null!;

    public decimal REPRET_BNF_NUMDOC { get; set; }

    public string? REPRET_OBSERVACION { get; set; }

    public string? REPRET_OBSERVACION1 { get; set; }

    public string? REPRET_OBSERVACION2 { get; set; }

    public string? REPRET_OBSERVACION3 { get; set; }

    public string REPRET_CARGO { get; set; } = null!;

    public string? REPRET_PROVINCIA { get; set; }

    public string REPRET_TIPO_RETENCION { get; set; } = null!;
}
