using SistemaVales.Business;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SistemaVales.Forms
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            EstiloModerno();
        }

        // 🔥 Bordes redondeados
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private void EstiloModerno()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            this.BackColor = Color.FromArgb(245, 245, 245);

            // Panel izquierdo
            panel1.BackColor = Color.FromArgb(33, 150, 243);

            // TextBox Usuario
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Font = new Font("Segoe UI", 11F);

            // TextBox Password
            txtContraseña.BorderStyle = BorderStyle.FixedSingle;
            txtContraseña.Font = new Font("Segoe UI", 11F);

            // Botón Login
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.BackColor = Color.FromArgb(33, 150, 243);
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            btnLogin.MouseEnter += (s, e) =>
                btnLogin.BackColor = Color.FromArgb(25, 118, 210);

            btnLogin.MouseLeave += (s, e) =>
                btnLogin.BackColor = Color.FromArgb(33, 150, 243);

            // Botón salir
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.BackColor = Color.FromArgb(244, 67, 54);
            btnSalir.ForeColor = Color.White;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contraseña = txtContraseña.Text;

            if (usuario == "" || contraseña == "")
            {
                MessageBox.Show("Ingresa usuario y contraseña");
                return;
            }

            var user = ValesLogic.AutenticarUsuario(usuario, contraseña);

            if (user != null)
            {
                SesionActual.UsuarioId = user.Id;
                SesionActual.NombreUsuario = usuario;
                SesionActual.NombreCompleto = user.NombreCompleto;
                SesionActual.Rol = user.Rol;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos");
                txtContraseña.Clear();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // 🔥 Diseño del formulario
        private Panel panel1;
        private TextBox txtUsuario;
        private TextBox txtContraseña;
        private Button btnLogin;
        private Button btnSalir;
        private Label lblTitulo;

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.txtUsuario = new TextBox();
            this.txtContraseña = new TextBox();
            this.btnLogin = new Button();
            this.btnSalir = new Button();
            this.lblTitulo = new Label();

            // Panel izquierdo
            panel1.Dock = DockStyle.Left;
            panel1.Width = 200;

            // Título
            lblTitulo.Text = "Sistema de Vales";
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(230, 20);
            lblTitulo.AutoSize = true;

            // Usuario
            txtUsuario.Location = new Point(230, 80);
            txtUsuario.Width = 220;
            txtUsuario.PlaceholderText = "Usuario";

            // Contraseña
            txtContraseña.Location = new Point(230, 130);
            txtContraseña.Width = 220;
            txtContraseña.PasswordChar = '*';
            txtContraseña.PlaceholderText = "Contraseña";

            // Botón login
            btnLogin.Text = "Iniciar Sesión";
            btnLogin.Location = new Point(230, 190);
            btnLogin.Size = new Size(220, 40);
            btnLogin.Click += btnLogin_Click;

            // Botón salir
            btnSalir.Text = "Salir";
            btnSalir.Location = new Point(230, 240);
            btnSalir.Size = new Size(220, 35);
            btnSalir.Click += btnSalir_Click;

            // Form
            this.ClientSize = new Size(500, 320);
            this.Controls.Add(panel1);
            this.Controls.Add(lblTitulo);
            this.Controls.Add(txtUsuario);
            this.Controls.Add(txtContraseña);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnSalir);
        }
    }
}