using System;

namespace SistemaVales
{
    /// <summary>
    /// Clase estática para guardar información de la sesión actual del usuario
    /// </summary>
    public static class SesionActual
    {
        public static int UsuarioId { get; set; }
        public static string NombreUsuario { get; set; }
        public static string NombreCompleto { get; set; }
        public static string Rol { get; set; }
        public static string ValeraSeleccionada { get; set; }

        /// <summary>
        /// Limpia toda la información de sesión
        /// </summary>
        public static void LimpiarSesion()
        {
            UsuarioId = 0;
            NombreUsuario = string.Empty;
            NombreCompleto = string.Empty;
            Rol = string.Empty;
            ValeraSeleccionada = string.Empty;
        }

        /// <summary>
        /// Retorna una cadena con la información actual de sesión
        /// </summary>
        public static string ObtenerInfoSesion()
        {
            return $"Usuario: {NombreCompleto} ({Rol}) | Valera: {ValeraSeleccionada}";
        }
    }
}
