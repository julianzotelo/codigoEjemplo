using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class EstadosTransferencias
{
    [Key]
    public string CodEstado { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    [ForeignKey(nameof(EST_ID))]
    public string EST_ID { get; set; } = null!;

    public byte[] FechaAlta { get; set; } = null!;


}
