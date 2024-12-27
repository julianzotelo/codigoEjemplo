using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class USUARIOS
{
    [Key]
    public string USR_ID { get; set; } = null!;


    [ForeignKey(nameof(TDD_BNF_ID))]
    public decimal? TDD_BNF_ID { get; set; }


    [ForeignKey(nameof(BNF_NUMDOC))]
    public decimal? BNF_NUMDOC { get; set; }


    [ForeignKey(nameof(TDD_CLT_ID))]
    public decimal? TDD_CLT_ID { get; set; }


    [ForeignKey(nameof(CLT_NUMDOC))]
    public decimal? CLT_NUMDOC { get; set; }


    [ForeignKey(nameof(SUC_ID))]
    public decimal SUC_ID { get; set; }



    public string? USR_CLAVE { get; set; }


    [ForeignKey(nameof(TDD_USR_ID))]
    public decimal TDD_USR_ID { get; set; }



    public decimal USR_NUMDOC { get; set; }

    public string USR_NOMBRE { get; set; } = null!;

    public string USR_APELLIDO { get; set; } = null!;


    [ForeignKey(nameof(PRF_ID))]
    public decimal PRF_ID { get; set; }

    public decimal USR_MAR_ACTIVO { get; set; }

    public decimal USR_INTENTOSFALLIDOS { get; set; }

    public decimal USR_MAR_BAJA { get; set; }

    public string? USR_CARGO { get; set; }

    public decimal USR_MAR_GENCLAVE { get; set; }

    public decimal USR_MAR_CAMBIACLAVE { get; set; }

    public decimal? NROCOBIS { get; set; }

}
