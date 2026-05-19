using System;
using System.Windows.Forms;

namespace SistemaVales.Forms
{
    public partial class FrmSeleccionValera : Form
    {
        private string valeraSeleccionada = "";

        public FrmSeleccionValera()
        {
            InitializeComponent();
        }

        private void FrmSeleccionValera_Load(object sender, EventArgs e)
        {
            this.Text = "Sistema de Vales - Seleccionar Valera";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false; // Desactiva botón cerrar
        }

        private void btnImpulsa_Click(object sender, EventArgs e)
        {
            valeraSeleccionada = "Impulsa";
            GuardarValeraYCerrar();
        }

        private void btnNexus_Click(object sender, EventArgs e)
        {
            valeraSeleccionada = "Nexus";
            GuardarValeraYCerrar();
        }

        private void btnSaleVale_Click(object sender, EventArgs e)
        {
            valeraSeleccionada = "Sale Vale";
            GuardarValeraYCerrar();
        }

        private void GuardarValeraYCerrar()
        {
            // Guardar en la sesión actual
            SesionActual.ValeraSeleccionada = valeraSeleccionada;
            
            // Cerrar con éxito
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.btnImpulsa = new System.Windows.Forms.Button();
            this.btnNexus = new System.Windows.Forms.Button();
            this.btnSaleVale = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();

            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.panel1.Controls.Add(this.lblTitulo);
            this.panel1.Controls.Add(this.lblSubtitulo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(600, 120);
            this.panel1.TabIndex = 0;

            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(560, 35);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Selecciona la Valera de Trabajo";

            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Arial", 11F);
            this.lblSubtitulo.ForeColor = System.Drawing.Color.White;
            this.lblSubtitulo.Location = new System.Drawing.Point(20, 60);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(560, 20);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Elige con cuál valera deseas trabajar en esta sesión";

            // 
            // btnImpulsa
            // 
            this.btnImpulsa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnImpulsa.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnImpulsa.ForeColor = System.Drawing.Color.White;
            this.btnImpulsa.Location = new System.Drawing.Point(50, 150);
            this.btnImpulsa.Name = "btnImpulsa";
            this.btnImpulsa.Size = new System.Drawing.Size(150, 120);
            this.btnImpulsa.TabIndex = 1;
            this.btnImpulsa.Text = "Impulsa";
            this.btnImpulsa.UseVisualStyleBackColor = false;
            this.btnImpulsa.Click += new System.EventHandler(this.btnImpulsa_Click);

            // 
            // btnNexus
            // 
            this.btnNexus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(39)))), ((int)(((byte)(176)))));
            this.btnNexus.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnNexus.ForeColor = System.Drawing.Color.White;
            this.btnNexus.Location = new System.Drawing.Point(225, 150);
            this.btnNexus.Name = "btnNexus";
            this.btnNexus.Size = new System.Drawing.Size(150, 120);
            this.btnNexus.TabIndex = 2;
            this.btnNexus.Text = "Nexus";
            this.btnNexus.UseVisualStyleBackColor = false;
            this.btnNexus.Click += new System.EventHandler(this.btnNexus_Click);

            // 
            // btnSaleVale
            // 
            this.btnSaleVale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnSaleVale.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnSaleVale.ForeColor = System.Drawing.Color.White;
            this.btnSaleVale.Location = new System.Drawing.Point(400, 150);
            this.btnSaleVale.Name = "btnSaleVale";
            this.btnSaleVale.Size = new System.Drawing.Size(150, 120);
            this.btnSaleVale.TabIndex = 3;
            this.btnSaleVale.Text = "Sale Vale";
            this.btnSaleVale.UseVisualStyleBackColor = false;
            this.btnSaleVale.Click += new System.EventHandler(this.btnSaleVale_Click);

            // 
            // FrmSeleccionValera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 320);
            this.Controls.Add(this.btnSaleVale);
            this.Controls.Add(this.btnNexus);
            this.Controls.Add(this.btnImpulsa);
            this.Controls.Add(this.panel1);
            this.Name = "FrmSeleccionValera";
            this.Text = "Sistema de Vales - Seleccionar Valera";
            this.Load += new System.EventHandler(this.FrmSeleccionValera_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Button btnImpulsa;
        private System.Windows.Forms.Button btnNexus;
        private System.Windows.Forms.Button btnSaleVale;
    }
}
