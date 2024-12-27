using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pp3.dominio.Models;

public partial class PERFILES
{
    [Key]
    public decimal PRF_ID { get; set; }

    public string PRF_DESCRIPCION { get; set; } = null!;

    public string PRF_TIPOUSUARIO { get; set; } = null!;


}
