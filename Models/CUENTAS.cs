using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public class CUENTAS
{
    [Key]
    public decimal? CTA_SERVICIO { get; set; }
    [ForeignKey(nameof(SUC_CUENTASERVICIO))]
    public decimal? SUC_CUENTASERVICIO { get; set; }
    [ForeignKey(nameof(MND_CUENTASERVICIO))]
    public decimal? MND_CUENTASERVICIO { get; set; } = 0;
    [ForeignKey(nameof(TDC_CUENTASERVICIO))]
    public decimal? TDC_CUENTASERVICIO { get; set; }
    [ForeignKey(nameof(SUC_CUENTACLIENTE))]
    public decimal? SUC_CUENTACLIENTE { get; set; }
    [ForeignKey(nameof(MND_CUENTACLIENTE))]
    public decimal? MND_CUENTACLIENTE { get; set; } = 0;
    [ForeignKey(nameof(TDC_CUENTACLIENTE))]
    public decimal? TDC_CUENTACLIENTE { get; set; }

    public decimal? CTA_CLIENTE { get; set; }

    public decimal TDD_CLT_ID { get; set; }

    public decimal? CLT_NUMDOC { get; set; }

    public decimal CTA_RELACIONSIAF { get; set; }

    public decimal? CTA_INACTIVA { get; set; }

    public decimal CTA_CONVENIO { get; set; }

    public DateTime? CTA_FECHAPERTURA { get; set; }

    public decimal? CTA_MAR_FIRMAELBANCO { get; set; }

    public byte? TIPO_CONVENIO { get; set; }

    public bool? CTA_COMISION { get; set; }

    public string? CTA_CONVENIO_BMA { get; set; }

 
}
