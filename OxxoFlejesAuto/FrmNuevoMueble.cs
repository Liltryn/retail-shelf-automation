using System;
using System.Windows.Forms;
using OxxoFlejesAuto.Modelos;
using System.Collections.Generic;

namespace OxxoFlejesAuto
{
    public partial class FrmNuevoMueble : Form
    {
        public MuebleTienda? MuebleCreado { get; private set; }

        public FrmNuevoMueble()
        {
            InitializeComponent();
            cmbTipo.SelectedIndex = 0;
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Por favor, ingresa un nombre para el mueble.", "Aviso");
                return;
            }

            // 1. Creamos las caras y generamos sus 3 bandejas por defecto
            CaraMueble caraArriba = new CaraMueble("Cara Superior");
            caraArriba.GenerarBandejasPorDefecto();

            CaraMueble caraAbajo = new CaraMueble("Cara Inferior");
            caraAbajo.GenerarBandejasPorDefecto();

            CaraMueble caraIzquierda = new CaraMueble("Cara Izquierda");
            caraIzquierda.GenerarBandejasPorDefecto();

            CaraMueble caraDerecha = new CaraMueble("Cara Derecha");
            caraDerecha.GenerarBandejasPorDefecto();

            // 2. Armamos el mueble con las LLAVES EXACTAS que espera tu gráfico
            MuebleCreado = new MuebleTienda
            {
                IdMueble = Guid.NewGuid().ToString().Substring(0, 5),
                NombreVisible = txtNombre.Text.Trim(),
                TipoMueble = cmbTipo.SelectedItem?.ToString() ?? "Góndola",
                PosX = 100,
                PosY = 100,
                Ancho = 150,
                Alto = 150,
                Caras = new Dictionary<string, CaraMueble>
                {
                    { "Arriba", caraArriba },
                    { "Abajo", caraAbajo },
                    { "Izquierda", caraIzquierda },
                    { "Derecha", caraDerecha }
                }
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void TipoMuebleClick(object sender, EventArgs e) { }
        private void NombreMueble_Click(object sender, EventArgs e) { }
        private void FrmNuevoMueble_Load(object sender, EventArgs e) { }
    }
}