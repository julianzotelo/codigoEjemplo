using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Envios
{
    [Key]
    public decimal ENV_ID { get; set; }

    public decimal TDD_CLT_ID { get; set; }

    public decimal CLT_NUMDOC { get; set; }

    public DateTime ENV_FEC { get; set; }

    public bool? ENV_ONLINE { get; set; }

    public bool? ENV_CONTROLADO { get; set; }

    public int? ENV_CANAL { get; set; }

    public string? ENV_ESTID { get; set; }

    public string? ENV_TIPOARCHIVO { get; set; }

    public decimal? ENV_NROLOTE_BPM { get; set; }

    public string? ENV_ARCHIVO { get; set; }

    public decimal? ENV_CANTREGTOTALES { get; set; }

    public decimal? ENV_CANTREGACEPTADOS { get; set; }

    public decimal? ENV_CANTREGRECHAZADOS { get; set; }

    public decimal? ENV_INICIOLOTE { get; set; }

    public decimal? ENV_FINLOTE { get; set; }

}
