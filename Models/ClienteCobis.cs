using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace pp3.dominio.Models
{
    public class ClienteCobis
    {
        public string? Nombre { get; set; } 
        public string? Edad { get; set; } 
        public string? Cedula { get; set; } 
        public int? Telefono { get; set; }
        public string? Direccion { get; set; } 
        public string? Puerta { get; set; }
        public string? CodPostal { get; set; }
        public string? Piso { get; set; }
        public string? Depto { get; set; }
        public int? CodBanca { get; set; } 
        public string? Banca { get; set; }
        public int? CodSegmento { get; set; }
        public string? Segmento { get; set; } 
        public int? CodSubsegmento { get; set; } 
        public string? Subsegmento { get; set; }
        public int? CodCanalCaptacion { get; set; } 
        public string? CanalCaptacion { get; set; } 
        public int? CodCategoria { get; set; } 
        public string? Categoria { get; set; }
        public int? CodOficial { get; set; }
        public string? Oficial { get; set; } 
        public int? NumeroCobis { get; set; }
        public string? CondicionIva { get; set; }
        public string? IvaC { get; set; }
    }
}
