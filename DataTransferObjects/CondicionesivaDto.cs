using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class CondicionesivaDto
    {
        public decimal? CNI_ID { get; set; }

        public string CNI_DESCRIPCION { get; set; } = null!;

        public decimal CNI_TASA { get; set; }

        public decimal CNI_SOBRETASA { get; set; }
    }
}
