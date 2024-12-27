using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class Monedas
{
    [Key]
    public decimal MND_ID { get; set; }

    public string MND_DESCRIPCION { get; set; } = null!;

    public string MND_SIMBOLO { get; set; } = null!;

    public byte MND_COBIS { get; set; }


}
