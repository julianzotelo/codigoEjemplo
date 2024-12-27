using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class TiposFactura
{
    [Key]
    public decimal TFA_ID { get; set; }

    public string TFA_DESCRIPCION { get; set; } = null!;

    public string TFA_COMPROBANTE { get; set; } = null!;

}
