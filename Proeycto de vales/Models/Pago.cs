using System;

namespace SistemaVales.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int ValeId { get; set; }
        public decimal MontoPagado { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
