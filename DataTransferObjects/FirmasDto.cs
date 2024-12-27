

namespace pp3.dominio.DataTransferObjects
{
    public class FirmasDto
    {
        public decimal CTA_SERVICIO { get; set; }

        public decimal CFR_MAR_AUTREQ { get; set; }
        public string? USR_ID { get; set; }

        public string? FIR_ESTADO { get; set; }

        public byte[]? FIR_FIRMA { get; set; }

        public decimal? FIR_LEN_FIRMA { get; set; }
        public decimal? CFR_COEFICIENTEPARTICIPACION { get; set; }

    }
}
