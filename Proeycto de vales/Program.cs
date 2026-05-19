using System;
using System.Windows.Forms;

namespace SistemaVales
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
            // Mostrar login primero
            Forms.FrmLogin loginForm = new Forms.FrmLogin();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // Si el login es exitoso, mostrar selección de valera
                Forms.FrmSeleccionValera valeraForm = new Forms.FrmSeleccionValera();
                if (valeraForm.ShowDialog() == DialogResult.OK)
                {
                    // Si selecciona valera, mostrar el dashboard
                    Application.Run(new Forms.FrmPrincipal());
                }
                else
                {
                    // Si cancela selección de valera, salir
                    Application.Exit();
                }
            }
            else
            {
                // Si el usuario cancela el login, salir
                Application.Exit();
            }
        }
    }
}
