using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class Cuenta
{
    [Key]
    public decimal CtaServicio { get; set; }

    public decimal SucCuentaservicio { get; set; }

    public decimal MndCuentaservicio { get; set; }

    public decimal TdcCuentaservicio { get; set; }

    public decimal SucCuentacliente { get; set; }

    public decimal MndCuentacliente { get; set; }

    public decimal TdcCuentacliente { get; set; }

    public decimal CtaCliente { get; set; }

    public decimal TddCltId { get; set; }

    public decimal CltNumdoc { get; set; }

    public decimal CtaRelacionsiaf { get; set; }

    public decimal CtaInactiva { get; set; }

    public decimal CtaConvenio { get; set; }

    public DateTime? CtaFechapertura { get; set; }

    public decimal? CtaMarFirmaelbanco { get; set; }

    public byte? TipoConvenio { get; set; }

    public bool? CtaComision { get; set; }

    public string? CtaConvenioBma { get; set; }

 
}
