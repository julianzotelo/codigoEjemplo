using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class Sucursales
{
    [Key]
    public decimal SUC_ID { get; set; }


    [ForeignKey(nameof(BCO_ID))]
    public decimal BCO_ID { get; set; }


    [ForeignKey(nameof(CDC_ID))]
    public decimal CDC_ID { get; set; }


    [ForeignKey(nameof(CCP_ID))]
    public decimal CCP_ID { get; set; }

    public string SUC_DESCRIPCION { get; set; } = null!;

    public string SUC_CALLE { get; set; } = null!;

    public string SUC_UNIDADFUNCIONAL { get; set; } = null!;

    public decimal SUC_MAR_MIGRACION { get; set; }

    public decimal SUC_MIGRADA { get; set; }

    public decimal SUC_MAR_BAJA { get; set; }

}
