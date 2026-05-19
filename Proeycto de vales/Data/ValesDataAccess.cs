using MySql.Data.MySqlClient;
using SistemaVales.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SistemaVales.Data
{
    public class ValesDataAccess
    {
        // ==================== CLIENTES ====================
        public static List<Cliente> ObtenerClientes()
        {
            return ObtenerClientesPorValera(SesionActual.ValeraSeleccionada);
        }

        public static List<Cliente> ObtenerClientesPorValera(string valera)
        {
            List<Cliente> clientes = new List<Cliente>();
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                string query = "SELECT * FROM clientes WHERE valera = @valera";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@valera", valera);
                MySqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(new Cliente
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre = reader["nombre"].ToString(),
                        Telefono = reader["telefono"].ToString()
                    });
                }
                reader.Close();
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener clientes: " + ex.Message);
            }
            return clientes;
        }

        /// <summary>
        /// Busca clientes por nombre o teléfono en la valera actual
        /// </summary>
        public static List<Cliente> BuscarClientes(string busqueda, string valera)
        {
            List<Cliente> clientes = new List<Cliente>();
            
            // Si la búsqueda está vacía, retornar todos los clientes
            if (string.IsNullOrEmpty(busqueda) || string.IsNullOrWhiteSpace(busqueda))
            {
                return ObtenerClientesPorValera(valera);
            }

            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                
                string query = @"SELECT * FROM clientes 
                               WHERE valera = @valera 
                               AND (nombre LIKE @busqueda OR telefono LIKE @busqueda)
                               ORDER BY nombre";
                
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@valera", valera);
                comando.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");
                
                MySqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(new Cliente
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre = reader["nombre"].ToString(),
                        Telefono = reader["telefono"].ToString()
                    });
                }
                reader.Close();
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar clientes: " + ex.Message);
            }
            return clientes;
        }

        public static bool AgregarCliente(Cliente cliente)
        {
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                string query = "INSERT INTO clientes (nombre, telefono, valera) VALUES (@nombre, @telefono, @valera)";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
                comando.Parameters.AddWithValue("@telefono", cliente.Telefono);
                comando.Parameters.AddWithValue("@valera", SesionActual.ValeraSeleccionada);
                
                int resultado = comando.ExecuteNonQuery();
                conexion.Close();
                return resultado > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar cliente: " + ex.Message);
                return false;
            }
        }

        public static bool ActualizarCliente(Cliente cliente)
        {
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                string query = "UPDATE clientes SET nombre = @nombre, telefono = @telefono WHERE id = @id";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
                comando.Parameters.AddWithValue("@telefono", cliente.Telefono);
                comando.Parameters.AddWithValue("@id", cliente.Id);
                
                int resultado = comando.ExecuteNonQuery();
                conexion.Close();
                return resultado > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar cliente: " + ex.Message);
                return false;
            }
        }

        public static bool EliminarCliente(int clienteId)
        {
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                string query = "DELETE FROM clientes WHERE id = @id";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@id", clienteId);
                
                int resultado = comando.ExecuteNonQuery();
                conexion.Close();
                return resultado > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar cliente: " + ex.Message);
                return false;
            }
        }

        // ==================== VALES ====================
        public static List<Vale> ObtenerVales()
        {
            return ObtenerValesPorValera(SesionActual.ValeraSeleccionada);
        }

        public static List<Vale> ObtenerValesPorValera(string valera)
        {
            List<Vale> vales = new List<Vale>();
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                string query = @"SELECT v.*, 
                               COALESCE(SUM(p.monto_pagado), 0) AS total_pagado
                        FROM vales v
                        LEFT JOIN pagos p ON v.id = p.vale_id AND p.valera = v.valera
                        WHERE v.valera = @valera
                        GROUP BY v.id";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@valera", valera);
                MySqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    decimal monto = Convert.ToDecimal(reader["monto"]);
                    decimal totalPagado = Convert.ToDecimal(reader["total_pagado"]);
                    decimal deuda = monto - totalPagado;

                    vales.Add(new Vale
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        ClienteId = Convert.ToInt32(reader["cliente_id"]),
                        Monto = monto,
                        FechaPrestamo = Convert.ToDateTime(reader["fecha_prestamo"]),
                        FechaLimite = Convert.ToDateTime(reader["fecha_limite"]),
                        DeudaActual = deuda,
                        Estado = DeterminarEstado(Convert.ToDateTime(reader["fecha_limite"]), deuda)
                    });
                }
                reader.Close();
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener vales: " + ex.Message);
            }
            return vales;
        }

        public static bool AgregarVale(Vale vale)
        {
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                string query = @"INSERT INTO vales (cliente_id, monto, fecha_prestamo, fecha_limite, valera) 
                        VALUES (@cliente_id, @monto, @fecha_prestamo, @fecha_limite, @valera)";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cliente_id", vale.ClienteId);
                comando.Parameters.AddWithValue("@monto", vale.Monto);
                comando.Parameters.AddWithValue("@fecha_prestamo", vale.FechaPrestamo);
                comando.Parameters.AddWithValue("@fecha_limite", vale.FechaLimite);
                comando.Parameters.AddWithValue("@valera", SesionActual.ValeraSeleccionada);
                
                int resultado = comando.ExecuteNonQuery();
                conexion.Close();
                return resultado > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar vale: " + ex.Message);
                return false;
            }
        }

        public static bool ActualizarVale(Vale vale)
        {
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                string query = @"UPDATE vales 
                        SET cliente_id = @cliente_id, monto = @monto, 
                            fecha_prestamo = @fecha_prestamo, fecha_limite = @fecha_limite
                        WHERE id = @id";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cliente_id", vale.ClienteId);
                comando.Parameters.AddWithValue("@monto", vale.Monto);
                comando.Parameters.AddWithValue("@fecha_prestamo", vale.FechaPrestamo);
                comando.Parameters.AddWithValue("@fecha_limite", vale.FechaLimite);
                comando.Parameters.AddWithValue("@id", vale.Id);
                
                int resultado = comando.ExecuteNonQuery();
                conexion.Close();
                return resultado > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar vale: " + ex.Message);
                return false;
            }
        }

        public static bool EliminarVale(int valeId)
        {
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                
                // Primero eliminar los pagos asociados
                string queryPagos = "DELETE FROM pagos WHERE vale_id = @id";
                MySqlCommand comandoPagos = new MySqlCommand(queryPagos, conexion);
                comandoPagos.Parameters.AddWithValue("@id", valeId);
                comandoPagos.ExecuteNonQuery();

                // Luego eliminar el vale
                string query = "DELETE FROM vales WHERE id = @id";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@id", valeId);
                
                int resultado = comando.ExecuteNonQuery();
                conexion.Close();
                return resultado > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar vale: " + ex.Message);
                return false;
            }
        }

        // ==================== PAGOS ====================
        public static List<Pago> ObtenerPagosPorVale(int valeId)
        {
            List<Pago> pagos = new List<Pago>();
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                string query = "SELECT * FROM pagos WHERE vale_id = @vale_id ORDER BY fecha_pago DESC";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@vale_id", valeId);
                MySqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    pagos.Add(new Pago
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        ValeId = Convert.ToInt32(reader["vale_id"]),
                        MontoPagado = Convert.ToDecimal(reader["monto_pagado"]),
                        FechaPago = Convert.ToDateTime(reader["fecha_pago"])
                    });
                }
                reader.Close();
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener pagos: " + ex.Message);
            }
            return pagos;
        }

        public static bool AgregarPago(Pago pago)
        {
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                string query = @"INSERT INTO pagos (vale_id, monto_pagado, fecha_pago, valera) 
                        VALUES (@vale_id, @monto_pagado, @fecha_pago, @valera)";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@vale_id", pago.ValeId);
                comando.Parameters.AddWithValue("@monto_pagado", pago.MontoPagado);
                comando.Parameters.AddWithValue("@fecha_pago", pago.FechaPago);
                comando.Parameters.AddWithValue("@valera", SesionActual.ValeraSeleccionada);
                
                int resultado = comando.ExecuteNonQuery();
                conexion.Close();
                return resultado > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar pago: " + ex.Message);
                return false;
            }
        }

        public static bool EliminarPago(int pagoId)
        {
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                string query = "DELETE FROM pagos WHERE id = @id";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@id", pagoId);
                
                int resultado = comando.ExecuteNonQuery();
                conexion.Close();
                return resultado > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar pago: " + ex.Message);
                return false;
            }
        }

        // ==================== UTILIDADES ====================
        public static string DeterminarEstado(DateTime fechaLimite, decimal deuda)
        {
            if (deuda <= 0)
                return "Pagado";
            else if (DateTime.Now > fechaLimite)
                return "Atrasado";
            else
                return "En tiempo";
        }

        public static decimal ObtenerTotalPrestado()
        {
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                string query = "SELECT COALESCE(SUM(monto), 0) AS total FROM vales WHERE valera = @valera";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@valera", SesionActual.ValeraSeleccionada);
                decimal total = Convert.ToDecimal(comando.ExecuteScalar());
                conexion.Close();
                return total;
            }
            catch
            {
                return 0;
            }
        }

        public static decimal ObtenerTotalRecuperado()
        {
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                string query = "SELECT COALESCE(SUM(monto_pagado), 0) AS total FROM pagos WHERE valera = @valera";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@valera", SesionActual.ValeraSeleccionada);
                decimal total = Convert.ToDecimal(comando.ExecuteScalar());
                conexion.Close();
                return total;
            }
            catch
            {
                return 0;
            }
        }

        public static decimal ObtenerTotalPendiente()
        {
            return ObtenerTotalPrestado() - ObtenerTotalRecuperado();
        }

        public static DataTable ObtenerDashboard()
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();
                string query = @"SELECT 
                            c.nombre AS Cliente,
                            v.monto AS Monto,
                            COALESCE(SUM(p.monto_pagado), 0) AS Pagado,
                            v.monto - COALESCE(SUM(p.monto_pagado), 0) AS Deuda,
                            CASE 
                                WHEN v.monto - COALESCE(SUM(p.monto_pagado), 0) <= 0 THEN 'Pagado'
                                WHEN NOW() > v.fecha_limite THEN 'Atrasado'
                                ELSE 'En tiempo'
                            END AS Estado
                        FROM vales v
                        JOIN clientes c ON v.cliente_id = c.id AND c.valera = v.valera
                        LEFT JOIN pagos p ON v.id = p.vale_id AND p.valera = v.valera
                        WHERE v.valera = @valera
                        GROUP BY v.id
                        ORDER BY c.nombre";
                
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion);
                adapter.SelectCommand.Parameters.AddWithValue("@valera", SesionActual.ValeraSeleccionada);
                adapter.Fill(dt);
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener dashboard: " + ex.Message);
            }
            return dt;
        }

        // ==================== AUTENTICACIÓN ====================
        public static Usuario ObtenerUsuario(string nombreUsuario, string password)
        {
            try
            {
                MySqlConnection conexion = ConexionBD.ObtenerConexion();
                conexion.Open();

                string query = @"SELECT 
                                id AS usuario_id,
                                nombre_usuario,
                                password,
                                nombre_completo,
                                rol,
                                activo
                            FROM usuarios 
                            WHERE nombre_usuario = @usuario 
                            AND password = @password 
                            AND activo = TRUE";

                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@usuario", nombreUsuario);
                comando.Parameters.AddWithValue("@password", password);

                MySqlDataReader reader = comando.ExecuteReader();

                Usuario usuario = null;

                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["usuario_id"]),
                        NombreUsuario = reader["nombre_usuario"].ToString(),
                        Password = reader["password"].ToString(),
                        NombreCompleto = reader["nombre_completo"].ToString(),
                        Rol = reader["rol"].ToString(),
                        Activo = Convert.ToBoolean(reader["activo"])
                    };
                }

                reader.Close();
                conexion.Close();

                return usuario;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al autenticar usuario: " + ex.Message);
                return null;
            }
        }
    }
}
        
