using System.Drawing;
using System.Windows.Forms;

namespace SistemaVales.Utilities
{
    /// <summary>
    /// Gestor centralizado de temas y estilos de la aplicación
    /// </summary>
    public static class ThemeManager
    {
        // Colores principales
        public static class Colors
        {
            public static Color Primary = Color.FromArgb(33, 150, 243);      // Azul #2196F3
            public static Color PrimaryDark = Color.FromArgb(25, 118, 210);  // Azul oscuro
            public static Color PrimaryLight = Color.FromArgb(66, 165, 245); // Azul claro
            
            public static Color Success = Color.FromArgb(76, 175, 80);       // Verde #4CAF50
            public static Color SuccessDark = Color.FromArgb(56, 142, 60);   // Verde oscuro
            public static Color SuccessLight = Color.FromArgb(165, 214, 167); // Verde claro
            
            public static Color Danger = Color.FromArgb(244, 67, 54);        // Rojo #F44336
            public static Color DangerDark = Color.FromArgb(211, 47, 47);    // Rojo oscuro
            public static Color DangerLight = Color.FromArgb(239, 154, 154); // Rojo claro
            
            public static Color Warning = Color.FromArgb(255, 152, 0);       // Naranja
            public static Color Info = Color.FromArgb(0, 188, 212);          // Cian
            
            public static Color Background = Color.FromArgb(245, 245, 245);  // Gris claro #F5F5F5
            public static Color Surface = Color.White;
            public static Color Border = Color.FromArgb(224, 224, 224);      // Gris borde
            
            public static Color TextPrimary = Color.FromArgb(33, 33, 33);    // Texto oscuro
            public static Color TextSecondary = Color.FromArgb(117, 117, 117); // Texto gris
            public static Color TextLight = Color.White;
        }

        // Fuentes
        public static class Fonts
        {
            public static Font TitleLarge = new Font("Segoe UI", 24F, FontStyle.Bold);
            public static Font TitleMedium = new Font("Segoe UI", 18F, FontStyle.Bold);
            public static Font Title = new Font("Segoe UI", 14F, FontStyle.Bold);
            public static Font Subtitle = new Font("Segoe UI", 12F, FontStyle.Bold);
            public static Font Body = new Font("Segoe UI", 11F);
            public static Font BodySmall = new Font("Segoe UI", 10F);
            public static Font Button = new Font("Segoe UI", 11F, FontStyle.Bold);
        }

        /// <summary>
        /// Aplica estilo moderno a un botón
        /// </summary>
        public static void ApplyButtonStyle(Button button, Color backgroundColor, bool isPrimary = false)
        {
            button.BackColor = backgroundColor;
            button.ForeColor = Colors.TextLight;
            button.Font = Fonts.Button;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(10, 5, 10, 5);
            button.Height = 45;
            button.MinimumSize = new Size(100, 45);

            // Agregar efecto hover
            button.MouseEnter += (s, e) => button.BackColor = AdjustBrightness(backgroundColor, 0.9);
            button.MouseLeave += (s, e) => button.BackColor = backgroundColor;
        }

        /// <summary>
        /// Estilo para botón primario
        /// </summary>
        public static void ApplyPrimaryButtonStyle(Button button)
        {
            ApplyButtonStyle(button, Colors.Primary);
        }

        /// <summary>
        /// Estilo para botón peligroso (rojo)
        /// </summary>
        public static void ApplyDangerButtonStyle(Button button)
        {
            ApplyButtonStyle(button, Colors.Danger);
        }

        /// <summary>
        /// Estilo para botón de éxito (verde)
        /// </summary>
        public static void ApplySuccessButtonStyle(Button button)
        {
            ApplyButtonStyle(button, Colors.Success);
        }

        /// <summary>
        /// Estilo para botón de advertencia (naranja)
        /// </summary>
        public static void ApplyWarningButtonStyle(Button button)
        {
            ApplyButtonStyle(button, Colors.Warning);
        }

        /// <summary>
        /// Aplica estilo moderno a un TextBox
        /// </summary>
        public static void ApplyTextBoxStyle(TextBox textBox)
        {
            textBox.Font = Fonts.Body;
            textBox.ForeColor = Colors.TextPrimary;
            textBox.BackColor = Colors.Surface;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Padding = new Padding(5);
            textBox.Height = 35;
        }

        /// <summary>
        /// Aplica estilo moderno a un DataGridView
        /// </summary>
        public static void ApplyDataGridViewStyle(DataGridView dgv)
        {
            // Colores
            dgv.BackgroundColor = Colors.Background;
            dgv.GridColor = Colors.Border;
            dgv.DefaultCellStyle.BackColor = Colors.Surface;
            dgv.DefaultCellStyle.ForeColor = Colors.TextPrimary;
            dgv.DefaultCellStyle.Font = Fonts.Body;
            dgv.DefaultCellStyle.Padding = new Padding(5);

            // Encabezados
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Colors.Primary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Colors.TextLight;
            dgv.ColumnHeadersDefaultCellStyle.Font = Fonts.Subtitle;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 40;

            // Bordes
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgv.BorderStyle = BorderStyle.Fixed3D;

            // Alternancia de colores
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Colors.Background;
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = Colors.TextPrimary;

            // Selección
            dgv.DefaultCellStyle.SelectionBackColor = Colors.PrimaryLight;
            dgv.DefaultCellStyle.SelectionForeColor = Colors.TextLight;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Colors.PrimaryDark;

            // Altura de filas
            dgv.RowTemplate.Height = 30;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
        }

        /// <summary>
        /// Crea un Panel tipo Card (tarjeta)
        /// </summary>
        public static Panel CreateCard(string title, string value, Color cardColor, int x, int y)
        {
            Panel card = new Panel();
            card.BackColor = Colors.Surface;
            card.BorderStyle = BorderStyle.None;
            card.Location = new Point(x, y);
            card.Size = new Size(250, 120);
            card.Padding = new Padding(15);

            // Shadow/Borde superior
            card.Paint += (s, e) =>
            {
                // Borde superior de color
                e.Graphics.FillRectangle(new SolidBrush(cardColor), 0, 0, card.Width, 5);
                // Borde
                e.Graphics.DrawRectangle(new Pen(Colors.Border, 1), 0, 0, card.Width - 1, card.Height - 1);
            };

            // Label de título
            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = Fonts.BodySmall;
            lblTitle.ForeColor = Colors.TextSecondary;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(220, 25);
            lblTitle.Location = new Point(15, 15);

            // Label de valor
            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = Fonts.TitleMedium;
            lblValue.ForeColor = cardColor;
            lblValue.AutoSize = false;
            lblValue.Size = new Size(220, 50);
            lblValue.Location = new Point(15, 45);
            lblValue.TextAlign = ContentAlignment.TopLeft;

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            return card;
        }

        /// <summary>
        /// Ajusta el brillo de un color
        /// </summary>
        private static Color AdjustBrightness(Color color, double brightness)
        {
            return Color.FromArgb(
                (int)(color.R * brightness),
                (int)(color.G * brightness),
                (int)(color.B * brightness)
            );
        }

        /// <summary>
        /// Aplica estilo moderno a un Panel (encabezado)
        /// </summary>
        public static void ApplyHeaderStyle(Panel panel)
        {
            panel.BackColor = Colors.Primary;
            panel.ForeColor = Colors.TextLight;
        }

        /// <summary>
        /// Aplica estilo a un Label
        /// </summary>
        public static void ApplyLabelStyle(Label label, bool isBold = false, bool isLight = false)
        {
            label.Font = isBold ? Fonts.Subtitle : Fonts.Body;
            label.ForeColor = isLight ? Colors.TextLight : Colors.TextPrimary;
            label.AutoSize = true;
        }
    }
}
