using MySql.Data.MySqlClient;
using System.Data;

namespace SistemaVales.Data
{
    public class ConexionBD
    {
        // IMPORTANTE: Cambiar estos valores según tu configuración MySQL
        private static string servidor = "localhost";
        private static string usuario = "root";
        private static string contraseña = "";
        private static string baseDatos = "sistema_vales";

        public static string ObtenercadenConexion()
        {
            return $"Server={servidor};User Id={usuario};Password={contraseña};Database={baseDatos}";
        }

        public static MySqlConnection ObtenerConexion()
        {
            MySqlConnection conexion = new MySqlConnection(ObtenercadenConexion());
            return conexion;
        }

        public static bool VerificarConexion()
        {
            try
            {
                MySqlConnection conexion = ObtenerConexion();
                conexion.Open();
                conexion.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
