using SistemaVales.Business;
using SistemaVales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SistemaVales.Forms
{
    public partial class FrmPagos : Form
    {
        private int pagoSeleccionado = 0;
        private int valeSeleccionado = 0;
        private List<Vale> vales;

        public FrmPagos()
        {
            InitializeComponent();
        }

        private void FrmPagos_Load(object sender, EventArgs e)
        {
            this.Text = "Gestión de Pagos";
            CargarVales();
            CargarPagos();
        }

        private void CargarVales()
        {
            vales = ValesLogic.ObtenerAllVales();
            var valesConDeuda = vales.Where(v => v.DeudaActual > 0).ToList();
            cmbVales.DataSource = valesConDeuda;
            cmbVales.DisplayMember = "Id";
            cmbVales.ValueMember = "Id";
            cmbVales.SelectedIndex = -1;
        }

        private void CargarPagos()
        {
            if (valeSeleccionado > 0)
            {
                List<Pago> pagos = ValesLogic.ObtenerPagosPorValeLogic(valeSeleccionado);
                dgvPagos.DataSource = pagos;
                dgvPagos.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            LimpiarFormulario();
        }

        private void cmbVales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVales.SelectedIndex >= 0)
            {
                Vale valeSelec = (Vale)cmbVales.SelectedItem;
                valeSeleccionado = valeSelec.Id;
                
                if (valeSelec != null)
                {
                    lblDeudaActual.Text = $"Deuda Actual: ${valeSelec.DeudaActual:N2}";
                    lblEstado.Text = $"Estado: {valeSelec.Estado}";
                }
                
                CargarPagos();
            }
        }

        private void dgvPagos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                pagoSeleccionado = Convert.ToInt32(dgvPagos.Rows[e.RowIndex].Cells["Id"].Value);
                decimal montoPagado = Convert.ToDecimal(dgvPagos.Rows[e.RowIndex].Cells["MontoPagado"].Value);
                DateTime fechaPago = Convert.ToDateTime(dgvPagos.Rows[e.RowIndex].Cells["FechaPago"].Value);

                txtMontoPago.Text = montoPagado.ToString();
                dtpFechaPago.Value = fechaPago;
                btnAgregar.Text = "Actualizar";
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (valeSeleccionado == 0)
            {
                MessageBox.Show("Debe seleccionar un vale");
                return;
            }

            if (!decimal.TryParse(txtMontoPago.Text, out decimal montoPago))
            {
                MessageBox.Show("Monto inválido");
                return;
            }

            if (pagoSeleccionado == 0)
            {
                // Agregar nuevo pago
                if (ValesLogic.AgregarPagoLogic(valeSeleccionado, montoPago, dtpFechaPago.Value))
                {
                    MessageBox.Show("Pago registrado exitosamente");
                    CargarVales();
                    CargarPagos();
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (pagoSeleccionado == 0)
            {
                MessageBox.Show("Debe seleccionar un pago");
                return;
            }

            if (MessageBox.Show("¿Está seguro de que desea eliminar este pago?", 
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (ValesLogic.EliminarPagoLogic(pagoSeleccionado))
                {
                    MessageBox.Show("Pago eliminado exitosamente");
                    CargarVales();
                    CargarPagos();
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtMontoPago.Clear();
            dtpFechaPago.Value = DateTime.Now;
            pagoSeleccionado = 0;
            btnAgregar.Text = "Agregar";
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbVales = new System.Windows.Forms.ComboBox();
            this.txtMontoPago = new System.Windows.Forms.TextBox();
            this.dtpFechaPago = new System.Windows.Forms.DateTimePicker();
            this.lblDeudaActual = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.dgvPagos = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagos)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();

            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 10F);
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Vale:";

            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 10F);
            this.label2.Location = new System.Drawing.Point(10, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Monto Pago:";

            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 10F);
            this.label3.Location = new System.Drawing.Point(250, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "Fecha Pago:";

            // 
            // cmbVales
            // 
            this.cmbVales.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbVales.Location = new System.Drawing.Point(70, 15);
            this.cmbVales.Name = "cmbVales";
            this.cmbVales.Size = new System.Drawing.Size(170, 27);
            this.cmbVales.TabIndex = 3;
            this.cmbVales.SelectedIndexChanged += new System.EventHandler(this.cmbVales_SelectedIndexChanged);

            // 
            // txtMontoPago
            // 
            this.txtMontoPago.Font = new System.Drawing.Font("Arial", 10F);
            this.txtMontoPago.Location = new System.Drawing.Point(125, 50);
            this.txtMontoPago.Name = "txtMontoPago";
            this.txtMontoPago.Size = new System.Drawing.Size(115, 27);
            this.txtMontoPago.TabIndex = 4;

            // 
            // dtpFechaPago
            // 
            this.dtpFechaPago.Font = new System.Drawing.Font("Arial", 10F);
            this.dtpFechaPago.Location = new System.Drawing.Point(365, 50);
            this.dtpFechaPago.Name = "dtpFechaPago";
            this.dtpFechaPago.Size = new System.Drawing.Size(140, 27);
            this.dtpFechaPago.TabIndex = 5;

            // 
            // lblDeudaActual
            // 
            this.lblDeudaActual.AutoSize = true;
            this.lblDeudaActual.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblDeudaActual.ForeColor = System.Drawing.Color.DarkRed;
            this.lblDeudaActual.Location = new System.Drawing.Point(250, 15);
            this.lblDeudaActual.Name = "lblDeudaActual";
            this.lblDeudaActual.Size = new System.Drawing.Size(140, 19);
            this.lblDeudaActual.TabIndex = 6;
            this.lblDeudaActual.Text = "Deuda Actual: $0.00";

            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblEstado.Location = new System.Drawing.Point(420, 15);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(85, 19);
            this.lblEstado.TabIndex = 7;
            this.lblEstado.Text = "Estado: -";

            // 
            // btnAgregar
            // 
            this.btnAgregar.BackColor = System.Drawing.Color.Orange;
            this.btnAgregar.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnAgregar.ForeColor = System.Drawing.Color.White;
            this.btnAgregar.Location = new System.Drawing.Point(520, 38);
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
            this.btnEliminar.Location = new System.Drawing.Point(615, 38);
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
            this.btnLimpiar.Location = new System.Drawing.Point(710, 38);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(90, 35);
            this.btnLimpiar.TabIndex = 10;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);

            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbVales);
            this.panel1.Controls.Add(this.lblDeudaActual);
            this.panel1.Controls.Add(this.lblEstado);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtMontoPago);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.dtpFechaPago);
            this.panel1.Controls.Add(this.btnAgregar);
            this.panel1.Controls.Add(this.btnEliminar);
            this.panel1.Controls.Add(this.btnLimpiar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(810, 90);
            this.panel1.TabIndex = 11;

            // 
            // dgvPagos
            // 
            this.dgvPagos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPagos.BackgroundColor = System.Drawing.Color.White;
            this.dgvPagos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPagos.Location = new System.Drawing.Point(0, 90);
            this.dgvPagos.Name = "dgvPagos";
            this.dgvPagos.ReadOnly = true;
            this.dgvPagos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPagos.Size = new System.Drawing.Size(810, 310);
            this.dgvPagos.TabIndex = 12;
            this.dgvPagos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPagos_CellClick);

            // 
            // FrmPagos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 400);
            this.Controls.Add(this.dgvPagos);
            this.Controls.Add(this.panel1);
            this.Name = "FrmPagos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FrmPagos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagos)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbVales;
        private System.Windows.Forms.TextBox txtMontoPago;
        private System.Windows.Forms.DateTimePicker dtpFechaPago;
        private System.Windows.Forms.Label lblDeudaActual;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.DataGridView dgvPagos;
        private System.Windows.Forms.Panel panel1;
    }
}
