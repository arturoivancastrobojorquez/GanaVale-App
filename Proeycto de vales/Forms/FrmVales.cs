using SistemaVales.Business;
using SistemaVales.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SistemaVales.Forms
{
    public partial class FrmVales : Form
    {
        private int valeSeleccionado = 0;
        private List<Cliente> clientes;

        public FrmVales()
        {
            InitializeComponent();
        }

        private void FrmVales_Load(object sender, EventArgs e)
        {
            this.Text = "Gestión de Vales";
            CargarClientes();
            CargarVales();
        }

        private void CargarClientes()
        {
            clientes = ValesLogic.ObtenerAllClientes();
            cmbClientes.DataSource = clientes;
            cmbClientes.DisplayMember = "Nombre";
            cmbClientes.ValueMember = "Id";
            cmbClientes.SelectedIndex = -1;
        }

        private void CargarVales()
        {
            List<Vale> vales = ValesLogic.ObtenerAllVales();
            dgvVales.DataSource = vales;
            dgvVales.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            LimpiarFormulario();
        }

        private void dgvVales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                valeSeleccionado = Convert.ToInt32(dgvVales.Rows[e.RowIndex].Cells["Id"].Value);
                int clienteId = Convert.ToInt32(dgvVales.Rows[e.RowIndex].Cells["ClienteId"].Value);
                decimal monto = Convert.ToDecimal(dgvVales.Rows[e.RowIndex].Cells["Monto"].Value);
                DateTime fechaPrestamo = Convert.ToDateTime(dgvVales.Rows[e.RowIndex].Cells["FechaPrestamo"].Value);
                DateTime fechaLimite = Convert.ToDateTime(dgvVales.Rows[e.RowIndex].Cells["FechaLimite"].Value);

                cmbClientes.SelectedValue = clienteId;
                txtMonto.Text = monto.ToString();
                dtpFechaPrestamo.Value = fechaPrestamo;
                dtpFechaLimite.Value = fechaLimite;
                btnAgregar.Text = "Actualizar";
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cmbClientes.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un cliente");
                return;
            }

            int clienteId = Convert.ToInt32(cmbClientes.SelectedValue);
            if (!decimal.TryParse(txtMonto.Text, out decimal monto))
            {
                MessageBox.Show("Monto inválido");
                return;
            }

            if (valeSeleccionado == 0)
            {
                // Agregar nuevo
                if (ValesLogic.CrearVale(clienteId, monto, dtpFechaPrestamo.Value, dtpFechaLimite.Value))
                {
                    MessageBox.Show("Vale agregado exitosamente");
                    CargarVales();
                }
            }
            else
            {
                // Actualizar
                if (ValesLogic.ActualizarValeLogic(valeSeleccionado, clienteId, monto, dtpFechaPrestamo.Value, dtpFechaLimite.Value))
                {
                    MessageBox.Show("Vale actualizado exitosamente");
                    CargarVales();
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (valeSeleccionado == 0)
            {
                MessageBox.Show("Debe seleccionar un vale");
                return;
            }

            if (MessageBox.Show("¿Está seguro de que desea eliminar este vale?", 
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (ValesLogic.EliminarValeLogic(valeSeleccionado))
                {
                    MessageBox.Show("Vale eliminado exitosamente");
                    CargarVales();
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            cmbClientes.SelectedIndex = -1;
            txtMonto.Clear();
            dtpFechaPrestamo.Value = DateTime.Now;
            dtpFechaLimite.Value = DateTime.Now.AddDays(30);
            valeSeleccionado = 0;
            btnAgregar.Text = "Agregar";
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbClientes = new System.Windows.Forms.ComboBox();
            this.txtMonto = new System.Windows.Forms.TextBox();
            this.dtpFechaPrestamo = new System.Windows.Forms.DateTimePicker();
            this.dtpFechaLimite = new System.Windows.Forms.DateTimePicker();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.dgvVales = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVales)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();

            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 10F);
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cliente:";

            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 10F);
            this.label2.Location = new System.Drawing.Point(10, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Monto:";

            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 10F);
            this.label3.Location = new System.Drawing.Point(280, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "Fecha Préstamo:";

            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 10F);
            this.label4.Location = new System.Drawing.Point(280, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 19);
            this.label4.TabIndex = 3;
            this.label4.Text = "Fecha Límite:";

            // 
            // cmbClientes
            // 
            this.cmbClientes.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbClientes.Location = new System.Drawing.Point(90, 15);
            this.cmbClientes.Name = "cmbClientes";
            this.cmbClientes.Size = new System.Drawing.Size(180, 27);
            this.cmbClientes.TabIndex = 4;

            // 
            // txtMonto
            // 
            this.txtMonto.Font = new System.Drawing.Font("Arial", 10F);
            this.txtMonto.Location = new System.Drawing.Point(90, 50);
            this.txtMonto.Name = "txtMonto";
            this.txtMonto.Size = new System.Drawing.Size(180, 27);
            this.txtMonto.TabIndex = 5;

            // 
            // dtpFechaPrestamo
            // 
            this.dtpFechaPrestamo.Font = new System.Drawing.Font("Arial", 10F);
            this.dtpFechaPrestamo.Location = new System.Drawing.Point(430, 15);
            this.dtpFechaPrestamo.Name = "dtpFechaPrestamo";
            this.dtpFechaPrestamo.Size = new System.Drawing.Size(150, 27);
            this.dtpFechaPrestamo.TabIndex = 6;

            // 
            // dtpFechaLimite
            // 
            this.dtpFechaLimite.Font = new System.Drawing.Font("Arial", 10F);
            this.dtpFechaLimite.Location = new System.Drawing.Point(430, 50);
            this.dtpFechaLimite.Name = "dtpFechaLimite";
            this.dtpFechaLimite.Size = new System.Drawing.Size(150, 27);
            this.dtpFechaLimite.TabIndex = 7;

            // 
            // btnAgregar
            // 
            this.btnAgregar.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnAgregar.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnAgregar.ForeColor = System.Drawing.Color.White;
            this.btnAgregar.Location = new System.Drawing.Point(590, 15);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(90, 35);
            this.btnAgregar.TabIndex = 8;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);

            // 
            // btnEliminar
            // 
            this.btnEliminar.BackColor = System.Drawing.Color.Crimson;
            this.btnEliminar.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.Location = new System.Drawing.Point(685, 15);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(90, 35);
            this.btnEliminar.TabIndex = 9;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.Gray;
            this.btnLimpiar.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnLimpiar.ForeColor = System.Drawing.Color.White;
            this.btnLimpiar.Location = new System.Drawing.Point(590, 50);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(185, 35);
            this.btnLimpiar.TabIndex = 10;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);

            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbClientes);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtMonto);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.dtpFechaPrestamo);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.dtpFechaLimite);
            this.panel1.Controls.Add(this.btnAgregar);
            this.panel1.Controls.Add(this.btnEliminar);
            this.panel1.Controls.Add(this.btnLimpiar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(785, 95);
            this.panel1.TabIndex = 11;

            // 
            // dgvVales
            // 
            this.dgvVales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvVales.BackgroundColor = System.Drawing.Color.White;
            this.dgvVales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVales.Location = new System.Drawing.Point(0, 95);
            this.dgvVales.Name = "dgvVales";
            this.dgvVales.ReadOnly = true;
            this.dgvVales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVales.Size = new System.Drawing.Size(785, 305);
            this.dgvVales.TabIndex = 12;
            this.dgvVales.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVales_CellClick);

            // 
            // FrmVales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 400);
            this.Controls.Add(this.dgvVales);
            this.Controls.Add(this.panel1);
            this.Name = "FrmVales";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FrmVales_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVales)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbClientes;
        private System.Windows.Forms.TextBox txtMonto;
        private System.Windows.Forms.DateTimePicker dtpFechaPrestamo;
        private System.Windows.Forms.DateTimePicker dtpFechaLimite;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.DataGridView dgvVales;
        private System.Windows.Forms.Panel panel1;
    }
}
