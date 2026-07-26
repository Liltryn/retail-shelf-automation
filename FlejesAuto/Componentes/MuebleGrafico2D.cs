using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FlejesAuto.Modelos;

namespace FlejesAuto.Componentes
{
    public class MuebleGrafico2D : UserControl
    {
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public MuebleTienda DatosMueble { get; set; }
        private int grosorBorde = 8;

        private bool estaArrastrando = false;
        private bool estaRedimensionando = false;
        private Point posicionInicialMouse;
        private Size tamanoInicialMueble;
        private const int MARGEN_REDIMENSIONAR = 15;
        private Point posicionAntesDeMover;

        public static bool ModoEdicionActivo { get; set; } = false;

        public MuebleGrafico2D(MuebleTienda datos)
        {
            this.DoubleBuffered = true;
            this.DatosMueble = datos;
            this.Size = new Size(datos.Ancho, datos.Alto);
            this.Location = new Point(datos.PosX, datos.PosY);

            this.MouseDown += MuebleGrafico2D_MouseDown;
            this.MouseMove += MuebleGrafico2D_MouseMove;
            this.MouseUp += MuebleGrafico2D_MouseUp;
        }

        private void MuebleGrafico2D_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (ModoEdicionActivo)
                {
                    DialogResult respuesta = MessageBox.Show(
                        $"¿Estás seguro de eliminar el mueble '{DatosMueble.NombreVisible}'?",
                        "Confirmar Eliminación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (respuesta == DialogResult.Yes)
                    {
                        if (this.Parent != null)
                        {
                            FrmPrincipal formPrincipal = this.FindForm() as FrmPrincipal;
                            Control contenedor = this.Parent;
                            contenedor.Controls.Remove(this);

                            if (formPrincipal != null)
                            {
                                List<MuebleTienda> listaActualizada = new List<MuebleTienda>();
                                foreach (Control ctrl in contenedor.Controls)
                                {
                                    if (ctrl is MuebleGrafico2D muebleVivo)
                                    {
                                        listaActualizada.Add(muebleVivo.DatosMueble);
                                    }
                                }

                                formPrincipal.listaMueblesMemoria = listaActualizada;
                                FlejesAuto.Datos.GestorTienda.GuardarTienda(listaActualizada);
                            }

                            this.Dispose();
                        }
                        return;
                    }
                }
                return;
            }

            if (ModoEdicionActivo)
            {
                if (e.Button == MouseButtons.Left)
                {
                    posicionAntesDeMover = this.Location;
                    posicionInicialMouse = this.PointToScreen(e.Location);
                    tamanoInicialMueble = this.Size;

                    if (e.X >= this.Width - MARGEN_REDIMENSIONAR && e.Y >= this.Height - MARGEN_REDIMENSIONAR)
                    {
                        estaRedimensionando = true;
                        this.Cursor = Cursors.SizeNWSE;
                    }
                    else
                    {
                        estaArrastrando = true;
                        this.Cursor = Cursors.SizeAll;
                        posicionInicialMouse = e.Location;
                    }
                }
            }
            else
            {
                EvaluarClickEnCara(e.Location);
            }
        }

        private void MuebleGrafico2D_MouseMove(object sender, MouseEventArgs e)
        {
            if (ModoEdicionActivo)
            {
                if (!estaArrastrando && !estaRedimensionando)
                {
                    if (e.X >= this.Width - MARGEN_REDIMENSIONAR && e.Y >= this.Height - MARGEN_REDIMENSIONAR)
                        this.Cursor = Cursors.SizeNWSE;
                    else
                        this.Cursor = Cursors.Default;
                }

                if (estaRedimensionando)
                {
                    Point mouseActualPantalla = this.PointToScreen(e.Location);
                    int deltaX = mouseActualPantalla.X - posicionInicialMouse.X;
                    int deltaY = mouseActualPantalla.Y - posicionInicialMouse.Y;

                    int nuevoAncho = tamanoInicialMueble.Width + deltaX;
                    int nuevoAlto = tamanoInicialMueble.Height + deltaY;

                    if (nuevoAncho < 50) nuevoAncho = 50;
                    if (nuevoAlto < 50) nuevoAlto = 50;

                    if (this.Parent != null)
                    {
                        if (this.Left + nuevoAncho > this.Parent.Width) nuevoAncho = this.Parent.Width - this.Left;
                        if (this.Top + nuevoAlto > this.Parent.Height) nuevoAlto = this.Parent.Height - this.Top;
                    }

                    this.Width = nuevoAncho;
                    this.Height = nuevoAlto;
                    DatosMueble.Ancho = this.Width;
                    DatosMueble.Alto = this.Height;
                    this.Invalidate();
                }
                else if (estaArrastrando)
                {
                    // 🌟 CORREGIDO: Si el modo eliminar está activo, bloqueamos por completo el arrastre
                    if (FrmPrincipal.ModoEliminarActivo)
                    {
                        this.Cursor = Cursors.NoMove2D;
                        return;
                    }

                    int deltaX = e.X - posicionInicialMouse.X;
                    int deltaY = e.Y - posicionInicialMouse.Y;
                    int nuevoLeft = this.Left + deltaX;
                    int nuevoTop = this.Top + deltaY;

                    if (this.Parent != null)
                    {
                        if (nuevoLeft < 0) nuevoLeft = 0;
                        if (nuevoLeft + this.Width > this.Parent.Width) nuevoLeft = this.Parent.Width - this.Width;
                        if (nuevoTop < 0) nuevoTop = 0;
                        if (nuevoTop + this.Height > this.Parent.Height) nuevoTop = this.Parent.Height - this.Height;
                    }

                    this.Left = nuevoLeft;
                    this.Top = nuevoTop;
                    DatosMueble.PosX = this.Left;
                    DatosMueble.PosY = this.Top;
                }
            }
        }

        private void MuebleGrafico2D_MouseUp(object sender, MouseEventArgs e)
        {
            // 🌟 NUEVO: Si se estaba moviendo o redimensionando, guardamos los cambios físicos de inmediato al soltar
            if ((estaArrastrando || estaRedimensionando) && this.Parent != null)
            {
                FrmPrincipal formPrincipal = this.FindForm() as FrmPrincipal;
                if (formPrincipal != null)
                {
                    FlejesAuto.Datos.GestorTienda.GuardarTienda(formPrincipal.listaMueblesMemoria);
                }
            }

            estaArrastrando = false;
            estaRedimensionando = false;
            this.Cursor = Cursors.Default;
        }

        private void EvaluarClickEnCara(Point p)
        {
            if (p.Y <= grosorBorde) DispararEventoCara("Arriba");
            else if (p.Y >= this.Height - grosorBorde) DispararEventoCara("Abajo");
            else if (p.X <= grosorBorde) DispararEventoCara("Izquierda");
            else if (p.X >= this.Width - grosorBorde) DispararEventoCara("Derecha");
        }

        private void DispararEventoCara(string lado)
        {
            if (DatosMueble.Caras.ContainsKey(lado))
            {
                CaraMueble caraSeleccionada = DatosMueble.Caras[lado];
                FrmCaraProductos pantallaFormulario = new FrmCaraProductos(caraSeleccionada);
                pantallaFormulario.ShowDialog();
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Name = "MuebleGrafico2D";
            Size = new Size(1075, 515);
            Load += MuebleGrafico2D_Load;
            ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // 🌟 CORREGIDO: Uso estricto de 'using' para destruir los Pens de color y liberar memoria GDI
            using (Pen penMorado = new Pen(Color.Purple, grosorBorde))
            using (Pen penAmarillo = new Pen(Color.Yellow, grosorBorde))
            using (Pen penAzul = new Pen(Color.Blue, grosorBorde))
            using (Pen penRojo = new Pen(Color.Maroon, grosorBorde))
            {
                g.DrawLine(penMorado, 0, grosorBorde / 2, this.Width, grosorBorde / 2);
                g.DrawLine(penAmarillo, 0, this.Height - (grosorBorde / 2), this.Width, this.Height - (grosorBorde / 2));
                g.DrawLine(penAzul, grosorBorde / 2, 0, grosorBorde / 2, this.Height);
                g.DrawLine(penRojo, this.Width - (grosorBorde / 2), 0, this.Width - (grosorBorde / 2), this.Height);
            }

            if (ModoEdicionActivo)
            {
                using (Pen penEdicion = new Pen(Color.Orange, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                {
                    g.DrawRectangle(penEdicion, 2, 2, this.Width - 4, this.Height - 4);
                }
            }

            string texto = $"{DatosMueble.NombreVisible}\n({DatosMueble.TipoMueble})";

            // 🌟 CORREGIDO: Uso de 'using' para la fuente tipográfica, previniendo fugas de recursos
            using (Font fuente = new Font("Arial", 9, FontStyle.Regular))
            {
                Size sizeTexto = TextRenderer.MeasureText(texto, fuente);
                int posXTexto = (this.Width - sizeTexto.Width) / 2;
                int posYTexto = (this.Height - sizeTexto.Height) / 2;

                g.DrawString(texto, fuente, Brushes.Black, posXTexto, posYTexto);
            }
        }

        private void MuebleGrafico2D_Load(object sender, EventArgs e) { }
    }
}