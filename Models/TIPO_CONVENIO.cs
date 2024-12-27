using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class TIPO_CONVENIO
{
    [ForeignKey(nameof(TIPO_CONVENIO_ID))]
    public int TIPO_CONVENIO_ID { get; set; }
    public string DESC_CONVENIO { get; set; } = null!;
}
