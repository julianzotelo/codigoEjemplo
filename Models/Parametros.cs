using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class Parametros
{
    [Key]
    public decimal PRM_ID { get; set; }



    [ForeignKey(nameof(SUC_TESORERIA))]
    public decimal SUC_TESORERIA { get; set; }


    [ForeignKey(nameof(CCB_TESORERIA))]
    public decimal CCB_TESORERIA { get; set; }


    [ForeignKey(nameof(CDC_TESORERIA))]
    public decimal CDC_TESORERIA { get; set; }


    [ForeignKey(nameof(MTV_COMISIONES))]
    public decimal MTV_COMISIONES { get; set; }

    public decimal PRM_NUMASIENTOUSR { get; set; }

    public string PRM_USUARIOCONTABILIDAD { get; set; } = null!;

    public string PRM_USUARIOGATEWAY { get; set; } = null!;

    public string PRM_ARCHIVOCOTIZACIONES { get; set; } = null!;

    public string PRM_ARCHIVOCUENTAS { get; set; } = null!;

    public string PRM_ARCHIVOCLIENTES { get; set; } = null!;

    public string PRM_ARCHIVOFUT { get; set; } = null!;

    public string PRM_ARCHIVORETFUT { get; set; } = null!;

    public decimal PRM_ULTIMOCHEQUE { get; set; }

    public decimal PRM_CHEQUEACTUAL { get; set; }

    public string PRM_ARCHIVOSUCURSALES { get; set; } = null!;

    public string PRM_ARCHIVOCHQPAG { get; set; } = null!;

    public string PRM_ARCHIVOCHQPAG2 { get; set; } = null!;

    public string PRM_ARCHIVOCHQRECH { get; set; } = null!;

    public string PRM_ARCHIVOCHQCAMARA { get; set; } = null!;

    public string PRM_ARCHIVOCALENDARIO { get; set; } = null!;

    public string PRM_ARCHIVOCONTABLE { get; set; } = null!;

    public string PRM_ARCHIVOCHEQUERAS { get; set; } = null!;

    public string PRM_ARCHIVOENTREGAINP { get; set; } = null!;

    public string PRM_ARCHIVOENTREGAOUT { get; set; } = null!;

    public string PRM_ARCHIVOUSUARIOSHB { get; set; } = null!;

    public string PRM_ARCHIVOCOELSAENV { get; set; } = null!;

    public string PRM_ARCHIVOCOELSARECH { get; set; } = null!;
    [Column(TypeName = "decimal(16,15)")]
    public decimal? PRM_HORARIOCORTE { get; set; }

    public int? PRM_TIEMPOMAXIMOPREFIRMA { get; set; }

    public DateTime? PRM_HORARIOCONCENTRADOR { get; set; }

    public decimal? PRM_MIN_NROCHEQUE { get; set; }

    public DateTime? PRM_HORARIO_CORTE_ECHEQ { get; set; }


}
