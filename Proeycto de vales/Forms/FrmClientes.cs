using SistemaVales.Business;
using SistemaVales.Models;
using SistemaVales.Data;
using SistemaVales.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaVales.Forms
{
    public partial class FrmClientes : Form
    {
        private int clienteSeleccionado = 0;

        public FrmClientes()
        {
            InitializeComponent();
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            this.Text = "Gestión de Clientes";
            CargarClientes();
            
            // Configurar TextBox de búsqueda
            txtBuscar.PlaceholderText = "Buscar cliente por nombre o teléfono...";
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
        }

        private void CargarClientes()
        {
            // Cargar clientes de la valera seleccionada
            List<Cliente> clientes = ValesDataAccess.ObtenerClientesPorValera(SesionActual.ValeraSeleccionada);
            ActualizarDataGridView(clientes);
            LimpiarFormulario();
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            string busqueda = txtBuscar.Text.Trim();
            List<Cliente> clientes = ValesLogic.BuscarClientesLogic(busqueda);
            ActualizarDataGridView(clientes);
        }

        private void ActualizarDataGridView(List<Cliente> clientes)
        {
            dgvClientes.DataSource = null; // Limpiar
            dgvClientes.DataSource = clientes;
            dgvClientes.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                clienteSeleccionado = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells["Id"].Value);
                txtNombre.Text = dgvClientes.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                txtTelefono.Text = dgvClientes.Rows[e.RowIndex].Cells["Telefono"].Value.ToString();
                btnAgregar.Text = "Actualizar";
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (clienteSeleccionado == 0)
            {
                // Agregar nuevo
                if (ValesLogic.CrearCliente(txtNombre.Text, txtTelefono.Text))
                {
                    MessageBox.Show("Cliente agregado exitosamente");
                    CargarClientes();
                }
            }
            else
            {
                // Actualizar
                if (ValesLogic.ActualizarClienteLogic(clienteSeleccionado, txtNombre.Text, txtTelefono.Text))
                {
                    MessageBox.Show("Cliente actualizado exitosamente");
                    CargarClientes();
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (clienteSeleccionado == 0)
            {
                MessageBox.Show("Debe seleccionar un cliente");
                return;
            }

            if (MessageBox.Show("¿Está seguro de que desea eliminar este cliente?", 
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (ValesLogic.EliminarClienteLogic(clienteSeleccionado))
                {
                    MessageBox.Show("Cliente eliminado exitosamente");
                    CargarClientes();
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtTelefono.Clear();
            clienteSeleccionado = 0;
            btnAgregar.Text = "Agregar";
        }

        private void InitializeComponent()
        {
            this.BackColor = ThemeManager.Colors.Background;

            // Panel Encabezado
            Panel panelHeader = new Panel();
            panelHeader.BackColor = ThemeManager.Colors.Primary;
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 60;
            panelHeader.Padding = new Padding(15, 10, 15, 10);

            Label lblTitulo = new Label();
            lblTitulo.Text = "👥 GESTIÓN DE CLIENTES";
            lblTitulo.Font = ThemeManager.Fonts.Title;
            lblTitulo.ForeColor = ThemeManager.Colors.TextLight;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(15, 12);

            panelHeader.Controls.Add(lblTitulo);

            // Panel Búsqueda
            Panel panelSearch = new Panel();
            panelSearch.BackColor = ThemeManager.Colors.Background;
            panelSearch.Dock = DockStyle.Top;
            panelSearch.Height = 50;
            panelSearch.Padding = new Padding(15);

            Label lblSearch = new Label();
            lblSearch.Text = "🔍 Buscar:";
            lblSearch.Font = ThemeManager.Fonts.Subtitle;
            lblSearch.ForeColor = ThemeManager.Colors.TextPrimary;
            lblSearch.Location = new Point(15, 8);
            lblSearch.AutoSize = true;

            this.txtBuscar = new TextBox();
            this.txtBuscar.PlaceholderText = "Buscar cliente por nombre o teléfono...";
            this.txtBuscar.Location = new Point(100, 5);
            this.txtBuscar.Size = new Size(700, 35);
            ThemeManager.ApplyTextBoxStyle(this.txtBuscar);

            panelSearch.Controls.Add(lblSearch);
            panelSearch.Controls.Add(this.txtBuscar);

            // Panel Formulario
            Panel panelForm = new Panel();
            panelForm.BackColor = Color.FromArgb(250, 250, 250);
            panelForm.Dock = DockStyle.Top;
            panelForm.Height = 100;
            panelForm.Padding = new Padding(15);

            // Fila 1: Nombre
            Label label1 = new Label();
            label1.Text = "👤 Nombre";
            label1.Font = ThemeManager.Fonts.Subtitle;
            label1.ForeColor = ThemeManager.Colors.TextPrimary;
            label1.Location = new Point(15, 10);
            label1.AutoSize = true;

            this.txtNombre = new TextBox();
            this.txtNombre.Location = new Point(120, 8);
            this.txtNombre.Size = new Size(200, 30);
            ThemeManager.ApplyTextBoxStyle(this.txtNombre);

            // Fila 1: Teléfono
            Label label2 = new Label();
            label2.Text = "📱 Teléfono";
            label2.Font = ThemeManager.Fonts.Subtitle;
            label2.ForeColor = ThemeManager.Colors.TextPrimary;
            label2.Location = new Point(340, 10);
            label2.AutoSize = true;

            this.txtTelefono = new TextBox();
            this.txtTelefono.Location = new Point(450, 8);
            this.txtTelefono.Size = new Size(150, 30);
            ThemeManager.ApplyTextBoxStyle(this.txtTelefono);

            // Botones
            this.btnAgregar = new Button();
            this.btnAgregar.Text = "➕ Agregar";
            this.btnAgregar.Location = new Point(15, 50);
            this.btnAgregar.Size = new Size(120, 40);
            ThemeManager.ApplyPrimaryButtonStyle(this.btnAgregar);
            this.btnAgregar.Click += new EventHandler(this.btnAgregar_Click);

            this.btnEliminar = new Button();
            this.btnEliminar.Text = "🗑️ Eliminar";
            this.btnEliminar.Location = new Point(145, 50);
            this.btnEliminar.Size = new Size(120, 40);
            ThemeManager.ApplyDangerButtonStyle(this.btnEliminar);
            this.btnEliminar.Click += new EventHandler(this.btnEliminar_Click);

            this.btnLimpiar = new Button();
            this.btnLimpiar.Text = "🔄 Limpiar";
            this.btnLimpiar.Location = new Point(275, 50);
            this.btnLimpiar.Size = new Size(120, 40);
            this.btnLimpiar.BackColor = Color.FromArgb(158, 158, 158);
            ThemeManager.ApplyButtonStyle(this.btnLimpiar, Color.FromArgb(158, 158, 158));
            this.btnLimpiar.Click += new EventHandler(this.btnLimpiar_Click);

            panelForm.Controls.Add(label1);
            panelForm.Controls.Add(this.txtNombre);
            panelForm.Controls.Add(label2);
            panelForm.Controls.Add(this.txtTelefono);
            panelForm.Controls.Add(this.btnAgregar);
            panelForm.Controls.Add(this.btnEliminar);
            panelForm.Controls.Add(this.btnLimpiar);

            // Panel Tabla
            Panel panelTable = new Panel();
            panelTable.BackColor = ThemeManager.Colors.Background;
            panelTable.Dock = DockStyle.Fill;
            panelTable.Padding = new Padding(15);

            Label lblTable = new Label();
            lblTable.Text = "📋 CLIENTES REGISTRADOS";
            lblTable.Font = ThemeManager.Fonts.Subtitle;
            lblTable.ForeColor = ThemeManager.Colors.TextPrimary;
            lblTable.Location = new Point(15, 5);
            lblTable.AutoSize = true;

            this.dgvClientes = new DataGridView();
            this.dgvClientes.Location = new Point(15, 35);
            this.dgvClientes.Dock = DockStyle.Fill;
            this.dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            ThemeManager.ApplyDataGridViewStyle(this.dgvClientes);
            this.dgvClientes.CellClick += new DataGridViewCellEventHandler(this.dgvClientes_CellClick);

            panelTable.Controls.Add(lblTable);
            panelTable.Controls.Add(this.dgvClientes);

            // Agregar controles al formulario
            this.Controls.Add(panelTable);
            this.Controls.Add(panelForm);
            this.Controls.Add(panelSearch);
            this.Controls.Add(panelHeader);

            // Configuración del formulario
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 600);
            this.Name = "FrmClientes";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Gestión de Clientes";
            this.Load += new EventHandler(this.FrmClientes_Load);
        }

        private TextBox txtNombre;
        private TextBox txtTelefono;
        private Button btnAgregar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvClientes;
        private TextBox txtBuscar;
    }
}
