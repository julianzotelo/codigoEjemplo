using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp3.dominio.Models
{
    public class ServicesResult
    {
        public string? Code { get; set; }
        public string? Message { get; set; }
        public string? Content { get; set; }


        public static string OKMessage = "OK";
    }

}
