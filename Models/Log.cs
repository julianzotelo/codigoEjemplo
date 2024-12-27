using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pp3.dominio.Models;
public partial class Log
{
    [Key]
    public decimal LOG_ID { get; set; }

    [ForeignKey(nameof(TAR_ID))]
    public decimal TAR_ID { get; set; }

    [ForeignKey(nameof(USR_ID))]
    public string USR_ID { get; set; } = null!;

    public DateTime LOG_FEC { get; set; }

    public string LOG_VALORANTERIOR { get; set; } = null!;

    public string LOG_VALORACTUAL { get; set; } = null!;

   
}
