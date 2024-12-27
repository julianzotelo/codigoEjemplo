using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class Codigospostale
{
    [Key]
    public decimal CCP_ID { get; set; }
    [ForeignKey(nameof(PRV_ID))]
    public decimal PRV_ID { get; set; }

    public string CCP_LOCALIDAD { get; set; } = null!;


}
