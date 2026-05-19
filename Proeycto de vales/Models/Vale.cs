using System;

namespace SistemaVales.Models
{
    public class Vale
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaLimite { get; set; }
        public string Estado { get; set; } // "Pagado", "Atrasado", "En tiempo"
        public decimal DeudaActual { get; set; }
    }
}
