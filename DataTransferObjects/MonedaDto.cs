using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.DataTransferObjects
{
    public class MonedaDto
    {
        public decimal MND_ID { get; set; }

        public string MND_DESCRIPCION { get; set; } = null!;

        public string MND_SIMBOLO { get; set; } = null!;

        public byte? MND_COBIS { get; set; }
    }
}
