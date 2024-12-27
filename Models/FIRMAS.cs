using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class FIRMAS
{
    [Key, ForeignKey(nameof(USR_ID))]
    public string USR_ID { get; set; } = null!;

    public string FIR_ESTADO { get; set; } = null!;

    public byte[] FIR_FIRMA { get; set; } = null!;

    public decimal FIR_LEN_FIRMA { get; set; }


}
