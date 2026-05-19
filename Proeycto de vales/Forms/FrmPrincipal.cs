using SistemaVales.Business;
using SistemaVales.Data;
using SistemaVales.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaVales.Forms
{
    public partial class FrmPrincipal : Form
    {
        private Panel cardPrestado, cardRecuperado, cardPendiente;
        private Label lblValuePrestado, lblValueRecuperado, lblValuePendiente;

        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            this.Text = "Sistema de Vales - Dashboard";
            this.BackColor = ThemeManager.Colors.Background;
            this.WindowState = FormWindowState.Maximized;
            
            // Mostrar información de sesión actual
            lblUsuarioActual.Text = $"👤 {SesionActual.NombreCompleto} | {SesionActual.Rol} | Valera: {SesionActual.ValeraSeleccionada}";

            // Verificar conexión
            if (!ConexionBD.VerificarConexion())
            {
                MessageBox.Show("No se pudo conectar a la base de datos.\nVerifica la cadena de conexión en ConexionBD.cs", 
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            CargarDashboard();
        }

        private void CargarDashboard()
        {
            // Cargar totales
            decimal totalPrestado = ValesLogic.ObtenerTotalPrestamoLogic();
            decimal totalRecuperado = ValesLogic.ObtenerTotalRecuperadoLogic();
            decimal totalPendiente = ValesLogic.ObtenerTotalPendienteLogic();

            // Actualizar cards
            lblValuePrestado.Text = $"${totalPrestado:N0}";
            lblValueRecuperado.Text = $"${totalRecuperado:N0}";
            lblValuePendiente.Text = $"${totalPendiente:N0}";

            // Cargar DataGridView
            CargarDataGridView();
        }

        private void CargarDataGridView()
        {
            DataTable dt = ValesLogic.ObtenerDashboardLogic();
            dgvDatos.DataSource = dt;

            // Ajustar ancho de columnas
            dgvDatos.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            // Cambiar colores según estado
            foreach (DataGridViewRow row in dgvDatos.Rows)
            {
                string estado = row.Cells["Estado"].Value?.ToString() ?? "";
                if (estado == "Pagado")
                    row.DefaultCellStyle.BackColor = ThemeManager.Colors.SuccessLight;
                else if (estado == "Atrasado")
                    row.DefaultCellStyle.BackColor = ThemeManager.Colors.DangerLight;
                else
                    row.DefaultCellStyle.BackColor = Color.FromArgb(227, 242, 253);
            }
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            FrmClientes frmClientes = new FrmClientes();
            frmClientes.ShowDialog();
            CargarDashboard();
        }

        private void btnVales_Click(object sender, EventArgs e)
        {
            FrmVales frmVales = new FrmVales();
            frmVales.ShowDialog();
            CargarDashboard();
        }

        private void btnPagos_Click(object sender, EventArgs e)
        {
            FrmPagos frmPagos = new FrmPagos();
            frmPagos.ShowDialog();
            CargarDashboard();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDashboard();
            MessageBox.Show("Dashboard actualizado", "Actualización", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de que desea cerrar sesión?", "Confirmar", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SesionActual.LimpiarSesion();
                this.Close();
            }
        }

        private void btnCambiarValera_Click(object sender, EventArgs e)
        {
            FrmSeleccionValera valeraForm = new FrmSeleccionValera();
            if (valeraForm.ShowDialog() == DialogResult.OK)
            {
                // Actualizar el label con la nueva valera
                lblUsuarioActual.Text = $"👤 {SesionActual.NombreCompleto} | {SesionActual.Rol} | Valera: {SesionActual.ValeraSeleccionada}";
                // Recargar dashboard con datos de la nueva valera
                CargarDashboard();
            }
        }

        private void InitializeComponent()
        {
            // Panel Superior (Encabezado)
            Panel panelHeader = new Panel();
            panelHeader.BackColor = ThemeManager.Colors.Primary;
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 70;
            panelHeader.Padding = new Padding(20, 10, 20, 10);

            // Crear encabezado
            Label lblTitulo = new Label();
            lblTitulo.Text = "📊 DASHBOARD - SISTEMA DE VALES";
            lblTitulo.Font = ThemeManager.Fonts.Title;
            lblTitulo.ForeColor = ThemeManager.Colors.TextLight;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 12);

            // Info Usuario
            this.lblUsuarioActual = new Label();
            this.lblUsuarioActual.Text = "Usuario";
            this.lblUsuarioActual.Font = ThemeManager.Fonts.BodySmall;
            this.lblUsuarioActual.ForeColor = ThemeManager.Colors.TextLight;
            this.lblUsuarioActual.AutoSize = true;
            this.lblUsuarioActual.Location = new Point(20, 42);

            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(this.lblUsuarioActual);

            // Panel de Botones
            Panel panelButtons = new Panel();
            panelButtons.BackColor = Color.FromArgb(250, 250, 250);
            panelButtons.Dock = DockStyle.Top;
            panelButtons.Height = 65;
            panelButtons.Padding = new Padding(15);

            this.btnClientes = new Button();
            this.btnClientes.Text = "👥 Clientes";
            this.btnClientes.Location = new Point(15, 10);
            this.btnClientes.Size = new Size(130, 45);
            ThemeManager.ApplyPrimaryButtonStyle(this.btnClientes);
            this.btnClientes.Click += new System.EventHandler(this.btnClientes_Click);

            this.btnVales = new Button();
            this.btnVales.Text = "💰 Vales";
            this.btnVales.Location = new Point(160, 10);
            this.btnVales.Size = new Size(130, 45);
            ThemeManager.ApplySuccessButtonStyle(this.btnVales);
            this.btnVales.Click += new System.EventHandler(this.btnVales_Click);

            this.btnPagos = new Button();
            this.btnPagos.Text = "💳 Pagos";
            this.btnPagos.Location = new Point(305, 10);
            this.btnPagos.Size = new Size(130, 45);
            ThemeManager.ApplyWarningButtonStyle(this.btnPagos);
            this.btnPagos.Click += new System.EventHandler(this.btnPagos_Click);

            this.btnActualizar = new Button();
            this.btnActualizar.Text = "🔄 Actualizar";
            this.btnActualizar.Location = new Point(450, 10);
            this.btnActualizar.Size = new Size(130, 45);
            this.btnActualizar.BackColor = Color.FromArgb(158, 158, 158);
            ThemeManager.ApplyButtonStyle(this.btnActualizar, Color.FromArgb(158, 158, 158));
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);

            this.btnCambiarValera = new Button();
            this.btnCambiarValera.Text = "🔀 Cambiar Valera";
            this.btnCambiarValera.Location = new Point(595, 10);
            this.btnCambiarValera.Size = new Size(150, 45);
            ThemeManager.ApplyWarningButtonStyle(this.btnCambiarValera);
            this.btnCambiarValera.Click += new System.EventHandler(this.btnCambiarValera_Click);

            this.btnCerrarSesion = new Button();
            this.btnCerrarSesion.Text = "🚪 Cerrar Sesión";
            this.btnCerrarSesion.Location = new Point(760, 10);
            this.btnCerrarSesion.Size = new Size(150, 45);
            ThemeManager.ApplyDangerButtonStyle(this.btnCerrarSesion);
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);

            panelButtons.Controls.Add(this.btnClientes);
            panelButtons.Controls.Add(this.btnVales);
            panelButtons.Controls.Add(this.btnPagos);
            panelButtons.Controls.Add(this.btnActualizar);
            panelButtons.Controls.Add(this.btnCambiarValera);
            panelButtons.Controls.Add(this.btnCerrarSesion);

            // Panel de Cards
            Panel panelCards = new Panel();
            panelCards.BackColor = ThemeManager.Colors.Background;
            panelCards.Dock = DockStyle.Top;
            panelCards.Height = 160;
            panelCards.Padding = new Padding(15);

            Label lblCards = new Label();
            lblCards.Text = "📈 RESUMEN FINANCIERO";
            lblCards.Font = ThemeManager.Fonts.Subtitle;
            lblCards.ForeColor = ThemeManager.Colors.TextPrimary;
            lblCards.Location = new Point(15, 5);
            lblCards.AutoSize = true;

            // Card: Total Prestado
            cardPrestado = ThemeManager.CreateCard("Total Prestado", "$0.00", ThemeManager.Colors.Primary, 15, 35);
            lblValuePrestado = cardPrestado.Controls[1] as Label;

            // Card: Total Recuperado
            cardRecuperado = ThemeManager.CreateCard("Total Recuperado", "$0.00", ThemeManager.Colors.Success, 280, 35);
            lblValueRecuperado = cardRecuperado.Controls[1] as Label;

            // Card: Total Pendiente
            cardPendiente = ThemeManager.CreateCard("Total Pendiente", "$0.00", ThemeManager.Colors.Danger, 545, 35);
            lblValuePendiente = cardPendiente.Controls[1] as Label;

            panelCards.Controls.Add(lblCards);
            panelCards.Controls.Add(cardPrestado);
            panelCards.Controls.Add(cardRecuperado);
            panelCards.Controls.Add(cardPendiente);

            // Panel de Tabla
            Panel panelTable = new Panel();
            panelTable.BackColor = ThemeManager.Colors.Background;
            panelTable.Dock = DockStyle.Fill;
            panelTable.Padding = new Padding(15);

            Label lblTable = new Label();
            lblTable.Text = "📋 DETALLES DE VALES";
            lblTable.Font = ThemeManager.Fonts.Subtitle;
            lblTable.ForeColor = ThemeManager.Colors.TextPrimary;
            lblTable.Location = new Point(15, 5);
            lblTable.AutoSize = true;
            lblTable.Size = new Size(250, 25);

            this.dgvDatos = new DataGridView();
            this.dgvDatos.Location = new Point(15, 35);
            this.dgvDatos.Dock = DockStyle.Fill;
            this.dgvDatos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDatos.ReadOnly = true;
            ThemeManager.ApplyDataGridViewStyle(this.dgvDatos);

            panelTable.Controls.Add(lblTable);
            panelTable.Controls.Add(this.dgvDatos);

            // Agregar controles al formulario
            this.Controls.Add(panelTable);
            this.Controls.Add(panelCards);
            this.Controls.Add(panelButtons);
            this.Controls.Add(panelHeader);

            // Configuración del formulario
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Name = "FrmPrincipal";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sistema de Vales";
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
        }

        private Label lblUsuarioActual;
        private Button btnClientes;
        private Button btnVales;
        private Button btnPagos;
        private Button btnActualizar;
        private Button btnCambiarValera;
        private Button btnCerrarSesion;
        private DataGridView dgvDatos;
    }
}
