using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OxxoFlejesAuto.Modelos;
using OxxoFlejesAuto.Componentes;
using OxxoFlejesAuto.Datos;

namespace OxxoFlejesAuto
{
    public partial class FrmPrincipal : Form
    {
        public static bool ModoEliminarActivo { get; set; } = false;

        // Lista global en memoria para llevar el registro de los muebles activos
        public List<MuebleTienda> listaMueblesMemoria = new List<MuebleTienda>();

        public FrmPrincipal()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 🌟 NUEVO: Cargar las medidas del local guardadas
            var medidas = GestorTienda.CargarMedidas();

            // Actualizamos los cuadritos de texto numéricos de tu interfaz
            numAnchoTienda.Value = medidas.AnchoLienzo;
            numAltoTienda.Value = medidas.AltoLienzo;

            // Configuramos los límites reales de la tienda basándonos en lo guardado
            ConfigurarLimitesTienda(medidas.AnchoLienzo, medidas.AltoLienzo);

            // --- CARGA REAL: Cargar los muebles guardados en el disco duro al arrancar ---
            listaMueblesMemoria = GestorTienda.CargarTienda();

            // Dibujamos de forma gráfica cada mueble recuperado en el panel gris
            foreach (var mueble in listaMueblesMemoria)
            {
                MuebleGrafico2D controlGrafico = new MuebleGrafico2D(mueble);
                pnlTienda.Controls.Add(controlGrafico);
                controlGrafico.BringToFront();
            }
        }

        private void ConfigurarLimitesTienda(int anchoTienda, int altoTienda)
        {
            pnlTienda.Dock = DockStyle.None;
            pnlTienda.Location = new Point(50, 80);
            pnlTienda.Size = new Size(anchoTienda, altoTienda);
            pnlTienda.BackColor = Color.FromArgb(240, 240, 240);
            pnlTienda.BorderStyle = BorderStyle.FixedSingle;
            pnlTienda.Visible = true;
        }

        private void btnAgregarMueble_Click(object sender, EventArgs e)
        {
            using (FrmNuevoMueble ventanaForm = new FrmNuevoMueble())
            {
                if (ventanaForm.ShowDialog() == DialogResult.OK)
                {
                    // 1. Agregamos el "?" porque sabemos que podría ser nulo
                    MuebleTienda? nuevoMuebleDatos = ventanaForm.MuebleCreado;

                    // 2. Validamos que realmente se haya creado el mueble
                    if (nuevoMuebleDatos != null)
                    {
                        // Lo añadimos al registro y guardamos en el JSON
                        listaMueblesMemoria.Add(nuevoMuebleDatos);
                        GestorTienda.GuardarTienda(listaMueblesMemoria);

                        // Lo renderizamos en pantalla
                        MuebleGrafico2D controlGrafico = new MuebleGrafico2D(nuevoMuebleDatos);
                        pnlTienda.Controls.Add(controlGrafico);
                        controlGrafico.BringToFront();
                    }
                }
            }
        }

        private void chkModoEdicion_CheckedChanged_1(object sender, EventArgs e)
        {
            bool edicionActiva = chkModoEdicion.Checked;

            MuebleGrafico2D.ModoEdicionActivo = edicionActiva;
            btnAgregarMueble.Visible = edicionActiva;

            // Cuando el usuario apaga el Modo Edición, aprovechamos de salvar todo
            if (!edicionActiva)
            {
                GestorTienda.GuardarTienda(listaMueblesMemoria);
                GestorTienda.GuardarMedidas((int)numAnchoTienda.Value, (int)numAltoTienda.Value);
            }

            pnlTienda.Refresh();
        }

        private void numAnchoTienda_ValueChanged(object sender, EventArgs e)
        {
            pnlTienda.Width = (int)numAnchoTienda.Value;
        }

        private void numAltoTienda_ValueChanged(object sender, EventArgs e)
        {
            pnlTienda.Height = (int)numAltoTienda.Value;
        }

        private void AnchoTienda_Click(object sender, EventArgs e) { }

        private void BtnGuadarCambios_Click(object sender, EventArgs e)
        {
            // Guarda los muebles
            GestorTienda.GuardarTienda(listaMueblesMemoria);

            // 🌟 NUEVO: Guarda también el tamaño del panel basándose en los numeritos de la pantalla
            GestorTienda.GuardarMedidas((int)numAnchoTienda.Value, (int)numAltoTienda.Value);

            MessageBox.Show("¡La tienda y las medidas del local se guardaron correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}