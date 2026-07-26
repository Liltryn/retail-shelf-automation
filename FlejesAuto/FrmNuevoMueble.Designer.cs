using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlejesAuto
{
    partial class FrmNuevoMueble
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtNombre = new TextBox();
            cmbTipo = new ComboBox();
            btnGuardar = new Button();
            NombreMueble = new Label();
            TipoMueble = new Label();
            SuspendLayout();
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(115, 49);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(100, 23);
            txtNombre.TabIndex = 0;
            // 
            // cmbTipo
            // 
            cmbTipo.FormattingEnabled = true;
            cmbTipo.Items.AddRange(new object[] { "Gondola", "Trascaja", "Refrijeradores", "FrontalCaja" });
            cmbTipo.Location = new Point(95, 102);
            cmbTipo.Name = "cmbTipo";
            cmbTipo.Size = new Size(121, 23);
            cmbTipo.TabIndex = 1;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(140, 165);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(75, 23);
            btnGuardar.TabIndex = 2;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click_1;
            // 
            // NombreMueble
            // 
            NombreMueble.AutoSize = true;
            NombreMueble.Location = new Point(12, 52);
            NombreMueble.Name = "NombreMueble";
            NombreMueble.Size = new Size(97, 15);
            NombreMueble.TabIndex = 3;
            NombreMueble.Text = "Nombre Mueble:";
            NombreMueble.Click += NombreMueble_Click;
            // 
            // TipoMueble
            // 
            TipoMueble.AutoSize = true;
            TipoMueble.Location = new Point(12, 102);
            TipoMueble.Name = "TipoMueble";
            TipoMueble.Size = new Size(77, 15);
            TipoMueble.TabIndex = 4;
            TipoMueble.Text = "Tipo Mueble:";
            TipoMueble.Click += TipoMuebleClick;
            // 
            // FrmNuevoMueble
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(249, 207);
            Controls.Add(TipoMueble);
            Controls.Add(NombreMueble);
            Controls.Add(btnGuardar);
            Controls.Add(cmbTipo);
            Controls.Add(txtNombre);
            Name = "FrmNuevoMueble";
            Text = "FrmNuevoMueble";
            Load += FrmNuevoMueble_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtNombre;
        private ComboBox cmbTipo;
        private Button btnGuardar;
        private Label NombreMueble;
        private Label TipoMueble;
    }
}