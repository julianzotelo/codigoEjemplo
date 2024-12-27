using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp3.dominio.Models;

public partial class FIRMASENCRIPTADAS
{
    [Key, ForeignKey(nameof(USR_ID))]
    public string USR_ID { get; set; } = null!;

    public byte[] FIR_FIRMA_ENCRIPTADA { get; set; } = null!;

}
