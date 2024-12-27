using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pp3.dominio.Models;
[PrimaryKey(nameof(COM_ID),nameof(MPG_ID))]
public partial class Comisiones
{
    
    public decimal COM_ID { get; set; }
    [ForeignKey(nameof(MPG_ID))]
    public decimal MPG_ID { get; set; }
    [ForeignKey(nameof(TDD_CLT_ID))]
    public decimal? TDD_CLT_ID { get; set; }
    [ForeignKey(nameof(CLT_NUMDOC))]
    public decimal CLT_NUMDOC { get; set; }

    public decimal COM_IMP_COMISION { get; set; }

    public DateTime COM_FECHA { get; set; }

    public decimal COM_CANTIDADOPGS { get; set; }
    [ForeignKey(nameof(EST_ID))]
    public string EST_ID { get; set; } = null!;

    public decimal? COM_CANTIDADRETS { get; set; }

}
