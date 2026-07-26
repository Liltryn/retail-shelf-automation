namespace OxxoFlejesAuto
{
    partial class FrmCaraProductos
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
            dgvProductos = new DataGridView();
            MecanismoSeleccion = new DataGridViewCheckBoxColumn();
            Upc = new DataGridViewTextBoxColumn();
            Nombre = new DataGridViewTextBoxColumn();
            ProductoId = new DataGridViewTextBoxColumn();
            btnEnviarSeleccionados = new Button();
            btnEnviarTodo = new Button();
            numDelay = new NumericUpDown();
            label1 = new Label();
            lstBandejas = new ListBox();
            txtBuscadorUpc = new TextBox();
            lstSugerencias = new ListBox();
            btnCargarPDF = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDelay).BeginInit();
            SuspendLayout();
            // 
            // dgvProductos
            // 
            dgvProductos.AllowUserToOrderColumns = true;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Columns.AddRange(new DataGridViewColumn[] { MecanismoSeleccion, Upc, Nombre, ProductoId });
            dgvProductos.Location = new Point(170, 64);
            dgvProductos.Name = "dgvProductos";
            dgvProductos.RowHeadersWidth = 51;
            dgvProductos.Size = new Size(514, 256);
            dgvProductos.TabIndex = 0;
            dgvProductos.CellContentClick += dgvProductos_CellContentClick;
            dgvProductos.MouseDown += dgvProductos_MouseDown;
            dgvProductos.MouseUp += dgvProductos_MouseUp;
            // 
            // MecanismoSeleccion
            // 
            MecanismoSeleccion.DataPropertyName = "MecanismoSeleccion";
            MecanismoSeleccion.HeaderText = "Seleccionar";
            MecanismoSeleccion.MinimumWidth = 6;
            MecanismoSeleccion.Name = "MecanismoSeleccion";
            MecanismoSeleccion.Width = 125;
            // 
            // Upc
            // 
            Upc.DataPropertyName = "Upc";
            Upc.HeaderText = "Código UPC / Barras";
            Upc.MinimumWidth = 6;
            Upc.Name = "Upc";
            Upc.Width = 125;
            // 
            // Nombre
            // 
            Nombre.DataPropertyName = "Nombre";
            Nombre.HeaderText = "Nombre Producto";
            Nombre.MinimumWidth = 6;
            Nombre.Name = "Nombre";
            Nombre.Width = 150;
            // 
            // ProductoId
            // 
            ProductoId.DataPropertyName = "ProductId";
            ProductoId.HeaderText = "ID Corto";
            ProductoId.MinimumWidth = 6;
            ProductoId.Name = "ProductoId";
            // 
            // btnEnviarSeleccionados
            // 
            btnEnviarSeleccionados.Location = new Point(14, 36);
            btnEnviarSeleccionados.Name = "btnEnviarSeleccionados";
            btnEnviarSeleccionados.Size = new Size(125, 23);
            btnEnviarSeleccionados.TabIndex = 1;
            btnEnviarSeleccionados.Text = "Enviar Seleccionados";
            btnEnviarSeleccionados.UseVisualStyleBackColor = true;
            btnEnviarSeleccionados.Click += btnEnviarSeleccionados_Click_1;
            // 
            // btnEnviarTodo
            // 
            btnEnviarTodo.Location = new Point(14, 64);
            btnEnviarTodo.Name = "btnEnviarTodo";
            btnEnviarTodo.Size = new Size(130, 23);
            btnEnviarTodo.TabIndex = 2;
            btnEnviarTodo.Text = "Enviar Cara Completa";
            btnEnviarTodo.UseVisualStyleBackColor = true;
            btnEnviarTodo.Click += btnEnviarTodo_Click_1;
            // 
            // numDelay
            // 
            numDelay.Location = new Point(45, 10);
            numDelay.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numDelay.Minimum = new decimal(new int[] { 1000, 0, 0, 0 });
            numDelay.Name = "numDelay";
            numDelay.Size = new Size(80, 23);
            numDelay.TabIndex = 3;
            numDelay.Value = new decimal(new int[] { 1250, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Enabled = false;
            label1.Location = new Point(73, 16);
            label1.Name = "label1";
            label1.Size = new Size(23, 15);
            label1.TabIndex = 4;
            label1.Text = "ms";
            // 
            // lstBandejas
            // 
            lstBandejas.FormattingEnabled = true;
            lstBandejas.Location = new Point(12, 121);
            lstBandejas.Margin = new Padding(3, 2, 3, 2);
            lstBandejas.Name = "lstBandejas";
            lstBandejas.Size = new Size(132, 229);
            lstBandejas.TabIndex = 5;
            // 
            // txtBuscadorUpc
            // 
            txtBuscadorUpc.Location = new Point(170, 39);
            txtBuscadorUpc.Margin = new Padding(3, 2, 3, 2);
            txtBuscadorUpc.Name = "txtBuscadorUpc";
            txtBuscadorUpc.Size = new Size(514, 23);
            txtBuscadorUpc.TabIndex = 6;
            txtBuscadorUpc.TextChanged += txtBuscadorUpc_TextChanged;
            txtBuscadorUpc.Enter += txtBuscadorUpc_Enter;
            txtBuscadorUpc.KeyDown += txtBuscadorUpc_KeyDown;
            txtBuscadorUpc.Leave += txtBuscadorUpc_Leave;
            // 
            // lstSugerencias
            // 
            lstSugerencias.FormattingEnabled = true;
            lstSugerencias.Location = new Point(170, 130);
            lstSugerencias.Name = "lstSugerencias";
            lstSugerencias.Size = new Size(514, 94);
            lstSugerencias.TabIndex = 7;
            lstSugerencias.Visible = false;
            lstSugerencias.SelectedIndexChanged += lstSugerencias_SelectedIndexChanged;
            // 
            // btnCargarPDF
            // 
            btnCargarPDF.Location = new Point(35, 93);
            btnCargarPDF.Name = "btnCargarPDF";
            btnCargarPDF.Size = new Size(75, 23);
            btnCargarPDF.TabIndex = 8;
            btnCargarPDF.Text = "Cargar PDF";
            btnCargarPDF.UseVisualStyleBackColor = true;
            btnCargarPDF.Click += btnCargarPDF_Click_1;
            // 
            // FrmCaraProductos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(836, 377);
            Controls.Add(btnCargarPDF);
            Controls.Add(lstSugerencias);
            Controls.Add(txtBuscadorUpc);
            Controls.Add(lstBandejas);
            Controls.Add(label1);
            Controls.Add(numDelay);
            Controls.Add(btnEnviarTodo);
            Controls.Add(btnEnviarSeleccionados);
            Controls.Add(dgvProductos);
            Name = "FrmCaraProductos";
            Text = "FrmCaraProductos";
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDelay).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvProductos;
        private Button btnEnviarSeleccionados;
        private Button btnEnviarTodo;
        private NumericUpDown numDelay;
        private Label label1;
        private ListBox lstBandejas;
        private TextBox txtBuscadorUpc;
        private ListBox lstSugerencias;
        private DataGridViewCheckBoxColumn MecanismoSeleccion;
        private DataGridViewTextBoxColumn Upc;
        private DataGridViewTextBoxColumn Nombre;
        private DataGridViewTextBoxColumn ProductoId;
        private Button btnCargarPDF;
    }
}