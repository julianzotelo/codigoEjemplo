using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class Retenciones
{
    [Key]
    public decimal RETENCION_ID { get; set; }

    public string OPG_IDOPGCLI { get; set; } = null!;

    public decimal RET_TIPO_ID { get; set; }

    public decimal RET_ZONA_ID { get; set; }

    public decimal RET_SECUENCIA_ID { get; set; }

    public string RET_TEXTO { get; set; } = null!;


    [ForeignKey(nameof(USR_ID))]
    public string USR_ID { get; set; } = null!;


    [ForeignKey(nameof(EST_ID))]
    public string EST_ID { get; set; } = null!;


    [ForeignKey(nameof(ENV_ID))]
    public decimal ENV_ID { get; set; }

    public decimal? COM_ID { get; set; }

    public decimal? ID_REGISTRO { get; set; }


}
