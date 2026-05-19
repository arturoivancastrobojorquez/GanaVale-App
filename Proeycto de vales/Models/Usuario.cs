namespace SistemaVales.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; } // "Administrador", "Operador"
        public bool Activo { get; set; }
    }
}
