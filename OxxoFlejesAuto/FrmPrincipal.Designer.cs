namespace OxxoFlejesAuto
{
    partial class FrmPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlTienda = new Panel();
            chkModoEdicion = new CheckBox();
            btnAgregarMueble = new Button();
            numAnchoTienda = new NumericUpDown();
            AnchoTienda = new Label();
            numAltoTienda = new NumericUpDown();
            AltoTienda = new Label();
            BtnGuadarCambios = new Button();
            ((System.ComponentModel.ISupportInitialize)numAnchoTienda).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numAltoTienda).BeginInit();
            SuspendLayout();
            // 
            // pnlTienda
            // 
            pnlTienda.BackColor = SystemColors.ControlLight;
            pnlTienda.BorderStyle = BorderStyle.FixedSingle;
            pnlTienda.Location = new Point(12, 37);
            pnlTienda.Name = "pnlTienda";
            pnlTienda.Size = new Size(1397, 692);
            pnlTienda.TabIndex = 0;
            pnlTienda.Visible = false;
            // 
            // chkModoEdicion
            // 
            chkModoEdicion.AutoSize = true;
            chkModoEdicion.FlatStyle = FlatStyle.System;
            chkModoEdicion.Location = new Point(12, 12);
            chkModoEdicion.Name = "chkModoEdicion";
            chkModoEdicion.Size = new Size(106, 20);
            chkModoEdicion.TabIndex = 0;
            chkModoEdicion.Text = "Modo Edición";
            chkModoEdicion.UseVisualStyleBackColor = true;
            chkModoEdicion.CheckedChanged += chkModoEdicion_CheckedChanged_1;
            // 
            // btnAgregarMueble
            // 
            btnAgregarMueble.Location = new Point(124, 12);
            btnAgregarMueble.Name = "btnAgregarMueble";
            btnAgregarMueble.Size = new Size(75, 23);
            btnAgregarMueble.TabIndex = 0;
            btnAgregarMueble.Text = "➕ Añadir Mueble";
            btnAgregarMueble.UseVisualStyleBackColor = true;
            btnAgregarMueble.Visible = false;
            btnAgregarMueble.Click += btnAgregarMueble_Click;
            // 
            // numAnchoTienda
            // 
            numAnchoTienda.Location = new Point(214, 12);
            numAnchoTienda.Maximum = new decimal(new int[] { 6000, 0, 0, 0 });
            numAnchoTienda.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numAnchoTienda.Name = "numAnchoTienda";
            numAnchoTienda.Size = new Size(120, 23);
            numAnchoTienda.TabIndex = 0;
            numAnchoTienda.Value = new decimal(new int[] { 600, 0, 0, 0 });
            numAnchoTienda.ValueChanged += numAnchoTienda_ValueChanged;
            // 
            // AnchoTienda
            // 
            AnchoTienda.AutoSize = true;
            AnchoTienda.Location = new Point(340, 16);
            AnchoTienda.Name = "AnchoTienda";
            AnchoTienda.Size = new Size(81, 15);
            AnchoTienda.TabIndex = 1;
            AnchoTienda.Text = "Ancho Tienda";
            AnchoTienda.Click += AnchoTienda_Click;
            // 
            // numAltoTienda
            // 
            numAltoTienda.Location = new Point(436, 14);
            numAltoTienda.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numAltoTienda.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numAltoTienda.Name = "numAltoTienda";
            numAltoTienda.Size = new Size(120, 23);
            numAltoTienda.TabIndex = 0;
            numAltoTienda.Value = new decimal(new int[] { 600, 0, 0, 0 });
            numAltoTienda.ValueChanged += numAltoTienda_ValueChanged;
            // 
            // AltoTienda
            // 
            AltoTienda.AutoSize = true;
            AltoTienda.Location = new Point(562, 17);
            AltoTienda.Name = "AltoTienda";
            AltoTienda.Size = new Size(68, 15);
            AltoTienda.TabIndex = 2;
            AltoTienda.Text = "Alto Tienda";
            // 
            // BtnGuadarCambios
            // 
            BtnGuadarCambios.Location = new Point(636, 13);
            BtnGuadarCambios.Name = "BtnGuadarCambios";
            BtnGuadarCambios.Size = new Size(75, 23);
            BtnGuadarCambios.TabIndex = 3;
            BtnGuadarCambios.Text = "Guardar Tienda";
            BtnGuadarCambios.UseVisualStyleBackColor = true;
            BtnGuadarCambios.Click += BtnGuadarCambios_Click;
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1421, 741);
            Controls.Add(BtnGuadarCambios);
            Controls.Add(AltoTienda);
            Controls.Add(numAltoTienda);
            Controls.Add(AnchoTienda);
            Controls.Add(numAnchoTienda);
            Controls.Add(btnAgregarMueble);
            Controls.Add(chkModoEdicion);
            Controls.Add(pnlTienda);
            Name = "FrmPrincipal";
            Text = "   ";
            ((System.ComponentModel.ISupportInitialize)numAnchoTienda).EndInit();
            ((System.ComponentModel.ISupportInitialize)numAltoTienda).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlTienda;
        private CheckBox chkModoEdicion;
        private Button btnAgregarMueble;
        private NumericUpDown numAnchoTienda;
        private Label AnchoTienda;
        private NumericUpDown numAltoTienda;
        private Label AltoTienda;
        private Button BtnGuadarCambios;
    }
}
