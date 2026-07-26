using Microsoft.VisualBasic;
using OxxoFlejesAuto.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace OxxoFlejesAuto
{
    public partial class FrmCaraProductos : Form
    {
        private CaraMueble _caraLocal;
        private BandejaMueble? _bandejaActual;

        private ContextMenuStrip menuGrilla;
        private ContextMenuStrip menuBandejas;

        public FrmCaraProductos(CaraMueble cara)
        {
            InitializeComponent();
            this._caraLocal = cara;
            this.Text = cara.NombreCara;

            // Inicialización de Menús Contextuales
            menuGrilla = new ContextMenuStrip();
            menuGrilla.Items.Add("Eliminar Producto", null, EliminarProducto_Click);
            dgvProductos.ContextMenuStrip = menuGrilla;

            menuBandejas = new ContextMenuStrip();
            menuBandejas.Items.Add("Agregar Bandeja", null, AgregarBandeja_Click);
            menuBandejas.Items.Add("Cambiar Nombre", null, CambiarNombreBandeja_Click);
            menuBandejas.Items.Add("Eliminar Bandeja", null, EliminarBandeja_Click);
            lstBandejas.ContextMenuStrip = menuBandejas;

            lstBandejas.MouseDown += LstBandejas_MouseDown;

            if (this.lstBandejas != null)
            {
                this.lstBandejas.SelectedIndexChanged += LstBandejas_SelectedIndexChanged;
            }

            ConfigurarPantalla();
            CargarListadoBandejas();
        }

        private void ConfigurarPantalla()
        {
            dgvProductos.AutoGenerateColumns = false;

            var colNombre = dgvProductos.Columns["Nombre"];
            if (colNombre != null)
            {
                colNombre.HeaderText = "Nombre Producto";
                colNombre.DataPropertyName = "Nombre";
            }
            else
            {
                var colDesc = dgvProductos.Columns["Descripcion"];
                if (colDesc != null)
                {
                    colDesc.HeaderText = "Nombre Producto";
                    colDesc.DataPropertyName = "Nombre";
                }
            }

            var colProdId = dgvProductos.Columns["ProductoId"];
            if (colProdId != null)
            {
                colProdId.HeaderText = "ID Corto";
                colProdId.DataPropertyName = "ProductId";
                colProdId.Visible = true;
            }

            var colSeleccion = dgvProductos.Columns["MecanismoSeleccion"];
            if (colSeleccion != null)
                colSeleccion.DataPropertyName = "MecanismoSeleccion";

            var colUpc = dgvProductos.Columns["Upc"];
            if (colUpc != null)
                colUpc.DataPropertyName = "Upc";
        }

        private void CargarListadoBandejas()
        {
            if (lstBandejas == null) return;

            lstBandejas.Items.Clear();
            foreach (var bandeja in _caraLocal.Bandejas)
            {
                lstBandejas.Items.Add(bandeja.NombreBandeja);
            }

            if (lstBandejas.Items.Count > 0)
            {
                lstBandejas.SelectedIndex = 0;
            }
        }

        private void LstBandejas_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int indice = lstBandejas.SelectedIndex;
            if (indice >= 0 && indice < _caraLocal.Bandejas.Count)
            {
                _bandejaActual = _caraLocal.Bandejas[indice];
                RefrescarGrillaProductos();
            }
        }

        private void RefrescarGrillaProductos()
        {
            dgvProductos.DataSource = null;
            if (_bandejaActual != null && _bandejaActual.Productos != null)
            {
                dgvProductos.DataSource = _bandejaActual.Productos.ToArray();
            }
        }

        private void btnEnviarSeleccionados_Click_1(object sender, EventArgs e)
        {
            int milisegundosDelay = (int)numDelay.Value;
            bool ventanaEncontrada = true;

            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                var celdaSeleccion = fila.Cells["MecanismoSeleccion"];
                var celdaUpc = fila.Cells["Upc"];

                if (celdaSeleccion != null && celdaSeleccion.Value != null)
                {
                    bool estaMarcado = Convert.ToBoolean(celdaSeleccion.Value);

                    if (estaMarcado && celdaUpc != null && celdaUpc.Value != null)
                    {
                        string codigoBarra = celdaUpc.Value.ToString() ?? string.Empty;
                        ventanaEncontrada = OxxoFlejesAuto.Automatizacion.ControladorTeclado.EnviarCodigoAAppFlejes(codigoBarra, milisegundosDelay);

                        if (!ventanaEncontrada)
                        {
                            MessageBox.Show("No se encontró la ventana 'FLEJES' abierta. Por favor, abre la aplicación de Oxxo antes de enviar.", "App No Detectada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
        }

        private void btnEnviarTodo_Click_1(object sender, EventArgs e)
        {
            if (_bandejaActual == null) return;

            int milisegundosDelay = (int)numDelay.Value;

            foreach (var producto in _bandejaActual.Productos)
            {
                if (producto.Upc == null) continue;

                bool ventanaEncontrada = OxxoFlejesAuto.Automatizacion.ControladorTeclado.EnviarCodigoAAppFlejes(producto.Upc, milisegundosDelay);

                if (!ventanaEncontrada)
                {
                    MessageBox.Show("No se encontró la ventana 'FLEJES' abierta. Por favor, abre la aplicación de Oxxo antes de enviar.", "App No Detectada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void txtBuscadorUpc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                string codigoEscaneado = txtBuscadorUpc.Text
                    .Replace("\r", "")
                    .Replace("\n", "")
                    .Trim();

                if (string.IsNullOrEmpty(codigoEscaneado)) return;

                if (_bandejaActual == null)
                {
                    MessageBox.Show("Atención: Debes hacer clic sobre una Bandeja en la lista izquierda antes de buscar.", "Bandeja No Seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (lstSugerencias != null) lstSugerencias.Visible = false;

                ProcesarBusquedaDirecta(codigoEscaneado);
            }
        }

        private async void ProcesarBusquedaDirecta(string codigoEscaneado)
        {
            try
            {
                if (_bandejaActual == null) return;

                this.Cursor = Cursors.WaitCursor;
                var productoEncontrado = await OxxoFlejesAuto.Datos.ConexionApex.BuscarProductoEnApexAsync(codigoEscaneado);
                this.Cursor = Cursors.Default;

                if (productoEncontrado != null)
                {
                    if (string.IsNullOrEmpty(productoEncontrado.Upc)) productoEncontrado.Upc = codigoEscaneado;
                    productoEncontrado.MecanismoSeleccion = false;

                    if (_bandejaActual.Productos == null)
                    {
                        _bandejaActual.Productos = new List<OxxoFlejesAuto.Modelos.ProductoFleje>();
                    }

                    if (!_bandejaActual.Productos.Any(p => p.Upc == productoEncontrado.Upc))
                    {
                        _bandejaActual.Productos.Add(productoEncontrado);
                        RefrescarGrillaProductos();
                    }

                    txtBuscadorUpc.TextChanged -= txtBuscadorUpc_TextChanged;
                    txtBuscadorUpc.Clear();
                    txtBuscadorUpc.TextChanged += txtBuscadorUpc_TextChanged;

                    txtBuscadorUpc.Focus();
                }
                else
                {
                    MessageBox.Show($"El buscador directo no obtuvo resultados para: {codigoEscaneado}.", "Búsqueda Fallida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBuscadorUpc.SelectAll();
                    txtBuscadorUpc.Focus();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Error en ProcesarBusquedaDirecta: {ex.Message}");
            }
        }

        private async void txtBuscadorUpc_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                string texto = txtBuscadorUpc.Text.Trim();

                if (texto.Length < 3)
                {
                    if (lstSugerencias != null) lstSugerencias.Visible = false;
                    return;
                }

                var sugerencias = await OxxoFlejesAuto.Datos.ConexionApex.BuscarListaProductosEnApexAsync(texto);

                if (sugerencias != null && sugerencias.Count > 0 && lstSugerencias != null)
                {
                    lstSugerencias.SelectedIndexChanged -= lstSugerencias_SelectedIndexChanged;

                    lstSugerencias.DataSource = null;
                    lstSugerencias.DisplayMember = "Nombre";
                    lstSugerencias.DataSource = sugerencias.ToArray();

                    lstSugerencias.SelectedIndex = -1;
                    lstSugerencias.SelectedIndexChanged += lstSugerencias_SelectedIndexChanged;

                    lstSugerencias.Width = txtBuscadorUpc.Width;
                    lstSugerencias.Height = 120;
                    lstSugerencias.Location = new System.Drawing.Point(txtBuscadorUpc.Left, txtBuscadorUpc.Bottom + 2);

                    lstSugerencias.Visible = true;
                    lstSugerencias.BringToFront();
                    lstSugerencias.Invalidate();
                    lstSugerencias.Update();
                }
                else
                {
                    if (lstSugerencias != null) lstSugerencias.Visible = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en buscador predictivo: {ex.Message}");
            }
        }

        private void lstSugerencias_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (sender is ListBox lista && lista.SelectedIndex != -1 && lista.SelectedItem != null && _bandejaActual != null)
            {
                var productoSeleccionado = lista.SelectedItem as OxxoFlejesAuto.Modelos.ProductoFleje;

                if (productoSeleccionado != null)
                {
                    if (_bandejaActual.Productos == null)
                    {
                        _bandejaActual.Productos = new List<OxxoFlejesAuto.Modelos.ProductoFleje>();
                    }

                    if (!_bandejaActual.Productos.Any(p => p.Upc == productoSeleccionado.Upc))
                    {
                        productoSeleccionado.MecanismoSeleccion = false;
                        _bandejaActual.Productos.Add(productoSeleccionado);
                        RefrescarGrillaProductos();
                    }

                    txtBuscadorUpc.TextChanged -= txtBuscadorUpc_TextChanged;
                    txtBuscadorUpc.Clear();
                    lista.Visible = false;
                    txtBuscadorUpc.TextChanged += txtBuscadorUpc_TextChanged;

                    txtBuscadorUpc.Focus();
                }
            }
        }

        private void LstBandejas_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = lstBandejas.IndexFromPoint(e.Location);
                if (index >= 0 && index < lstBandejas.Items.Count)
                {
                    lstBandejas.SelectedIndex = index;
                }
            }
        }

        private void CambiarNombreBandeja_Click(object? sender, EventArgs e)
        {
            int indice = lstBandejas.SelectedIndex;
            if (indice >= 0 && indice < _caraLocal.Bandejas.Count)
            {
                // 🌟 CORREGIDO: Eliminado el carácter '延' que corrompía la compilación
                var bandeja = _caraLocal.Bandejas[indice];

                string nuevoNombre = Interaction.InputBox(
                    "Introduce el nuevo nombre para esta bandeja:",
                    "Renombrar Bandeja",
                    bandeja.NombreBandeja);

                if (!string.IsNullOrWhiteSpace(nuevoNombre))
                {
                    bandeja.NombreBandeja = nuevoNombre.Trim();
                    CargarListadoBandejas();
                    lstBandejas.SelectedIndex = indice;
                }
            }
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void EliminarProducto_Click(object? sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow != null && _bandejaActual != null)
            {
                var fila = dgvProductos.CurrentRow;
                var celdaUpc = fila.Cells["Upc"];

                if (celdaUpc != null && celdaUpc.Value != null)
                {
                    string upcAEliminar = celdaUpc.Value.ToString() ?? string.Empty;

                    var producto = _bandejaActual.Productos.FirstOrDefault(p => p.Upc == upcAEliminar);
                    if (producto != null)
                    {
                        _bandejaActual.Productos.Remove(producto);
                        RefrescarGrillaProductos();
                    }
                }
            }
        }

        private void AgregarBandeja_Click(object? sender, EventArgs e)
        {
            string nombreNuevaBandeja = Interaction.InputBox(
                "Introduce el nombre para la nueva bandeja:",
                "Agregar Bandeja",
                $"Bandeja {_caraLocal.Bandejas.Count + 1}");

            if (!string.IsNullOrWhiteSpace(nombreNuevaBandeja))
            {
                var nuevaBandeja = new BandejaMueble
                {
                    NombreBandeja = nombreNuevaBandeja.Trim(),
                    Productos = new List<OxxoFlejesAuto.Modelos.ProductoFleje>()
                };

                _caraLocal.Bandejas.Add(nuevaBandeja);
                CargarListadoBandejas();

                lstBandejas.SelectedIndex = lstBandejas.Items.Count - 1;
            }
        }

        private void EliminarBandeja_Click(object? sender, EventArgs e)
        {
            int indice = lstBandejas.SelectedIndex;
            if (indice >= 0 && indice < _caraLocal.Bandejas.Count)
            {
                var bandeja = _caraLocal.Bandejas[indice];

                var resultado = MessageBox.Show(
                    $"¿Estás seguro de que deseas eliminar la bandeja '{bandeja.NombreBandeja}' junto con todos sus productos?",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    _caraLocal.Bandejas.RemoveAt(indice);
                    CargarListadoBandejas();

                    if (lstBandejas.Items.Count > 0)
                    {
                        lstBandejas.SelectedIndex = Math.Min(indice, lstBandejas.Items.Count - 1);
                    }
                    else
                    {
                        _bandejaActual = null;
                        dgvProductos.DataSource = null;
                    }
                }
            }
        }

        // ===================================================================
        // 🌟 MÉTODO 1: CARGA, SINCRONIZACIÓN Y REPARTO POR BANDEJA (BLINDADO)
        // ===================================================================
        private async void btnCargarPDF_Click_1(object? sender, EventArgs e)
        {
            if (_caraLocal == null) return;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos PDF (*.pdf)|*.pdf";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DialogResult respuesta = MessageBox.Show(
                            "¿Deseas REEMPLAZAR por completo los productos actuales con este nuevo planograma?\n\n" +
                            "• SÍ: Se borrarán los productos de las bandejas para aplicar el orden exacto del nuevo PDF.\n" +
                            "• NO: Se AGREGARÁN los nuevos productos al final de cada bandeja, respetando los que ya tienes en pantalla.",
                            "Estrategia de Sincronización",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question);

                        if (respuesta == DialogResult.Cancel) return;
                        bool modoReemplazar = (respuesta == DialogResult.Yes);

                        this.Cursor = Cursors.WaitCursor;

                        List<ProductoPlanograma> productosDelPdf = AnalizarEstructuraPdf(ofd.FileName);
                        List<ProductoPlanograma> encontradosEnReporte = new List<ProductoPlanograma>();
                        List<ProductoPlanograma> faltantesEnReporte = new List<ProductoPlanograma>();

                        bool tieneFormatoSecuencial = productosDelPdf.Any(p => p.EsSecuencial);
                        int segmentoAFiltrar = 1;
                        int bandejasPorCara = _caraLocal.Bandejas.Count;

                        if (tieneFormatoSecuencial)
                        {
                            this.Cursor = Cursors.Default;
                            string input = Interaction.InputBox(
                                $"Este PDF posee un formato de bandejas secuenciales corridas (1 a 15).\n\n" +
                                $"Tu Cara/Mueble actual en pantalla contiene {bandejasPorCara} bandejas.\n\n" +
                                $"¿Qué número de sección (Cara/Segmento) de este PDF deseas cargar en esta vista?\n" +
                                $"(Ej: 1 para las bandejas 1-5, 2 para las bandejas 6-10, 3 para las bandejas 11-15):",
                                "Configuración de Segmento PDF",
                                "1");

                            if (!int.TryParse(input, out segmentoAFiltrar) || segmentoAFiltrar <= 0)
                            {
                                MessageBox.Show("Carga cancelada o número de segmento inválido.", "Proceso Interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            this.Cursor = Cursors.WaitCursor;
                        }

                        // 2. RESPALDO LOCAL SANEADO
                        var productosExistentesDic = _caraLocal.Bandejas
                            .Where(b => b.Productos != null)
                            .SelectMany(b => b.Productos)
                            .Where(p => p.Nombre != "⚠️ FALTANTE EN BD")
                            .GroupBy(p => p.Upc)
                            .ToDictionary(g => g.Key ?? string.Empty, g => g.First());

                        // 3. Identificar qué UPCs del PDF no conocemos legítimamente
                        var upcsParaConsultar = productosDelPdf
                            .Select(p => p.Upc)
                            .Where(upc => !string.IsNullOrEmpty(upc) && !productosExistentesDic.ContainsKey(upc))
                            .Distinct()
                            .ToList();

                        // 4. 🌟 LLAMADA CORREGIDA: Apunta al método centralizado en Datos.ConexionApex
                        var productosDesdeApexMasivo = await OxxoFlejesAuto.Datos.ConexionApex.BuscarListaProductosPorLoteEnApexAsync(upcsParaConsultar);
                        var dictProductosApex = productosDesdeApexMasivo
                            .GroupBy(p => p.Upc)
                            .ToDictionary(g => g.Key ?? string.Empty, g => g.First());

                        if (modoReemplazar)
                        {
                            foreach (var bandeja in _caraLocal.Bandejas)
                            {
                                bandeja.Productos = new List<OxxoFlejesAuto.Modelos.ProductoFleje>();
                            }
                        }

                        // 6. RECONSTRUCCIÓN SECUENCIAL SOBRE EL LIENZO
                        foreach (var prodPdf in productosDelPdf)
                        {
                            int indiceBandejaTarget = -1;

                            if (prodPdf.EsSecuencial)
                            {
                                int rangoDesde = (segmentoAFiltrar - 1) * bandejasPorCara + 1;
                                int rangoHasta = segmentoAFiltrar * bandejasPorCara;

                                if (prodPdf.NumeroBandeja < rangoDesde || prodPdf.NumeroBandeja > rangoHasta) continue;
                                indiceBandejaTarget = prodPdf.NumeroBandeja - rangoDesde;
                            }
                            else
                            {
                                indiceBandejaTarget = prodPdf.NumeroBandeja - 1;
                            }

                            if (indiceBandejaTarget < 0 || indiceBandejaTarget >= _caraLocal.Bandejas.Count) continue;

                            var bandejaDestino = _caraLocal.Bandejas[indiceBandejaTarget];
                            if (bandejaDestino.Productos == null)
                                bandejaDestino.Productos = new List<OxxoFlejesAuto.Modelos.ProductoFleje>();

                            if (bandejaDestino.Productos.Any(p => p.Upc == prodPdf.Upc)) continue;

                            // CASO A: Usa caché local
                            if (productosExistentesDic.TryGetValue(prodPdf.Upc, out var productoLocal))
                            {
                                bandejaDestino.Productos.Add(productoLocal);
                                encontradosEnReporte.Add(new ProductoPlanograma
                                {
                                    NumeroBandeja = indiceBandejaTarget + 1,
                                    Upc = productoLocal.Upc ?? prodPdf.Upc,
                                    ProductId = productoLocal.ProductId ?? prodPdf.ProductId,
                                    NombreReferencia = productoLocal.Nombre ?? "Sin Nombre"
                                });
                            }
                            // CASO B: Usa respuesta limpia de Oracle APEX
                            else if (dictProductosApex.TryGetValue(prodPdf.Upc, out var productoApex))
                            {
                                productoApex.MecanismoSeleccion = false;
                                bandejaDestino.Productos.Add(productoApex);
                                encontradosEnReporte.Add(new ProductoPlanograma
                                {
                                    NumeroBandeja = indiceBandejaTarget + 1,
                                    Upc = productoApex.Upc ?? prodPdf.Upc,
                                    ProductId = productoApex.ProductId ?? prodPdf.ProductId,
                                    NombreReferencia = productoApex.Nombre ?? "Sin Nombre"
                                });
                            }
                            // CASO C: No existe de verdad en el catálogo central
                            else
                            {
                                bandejaDestino.Productos.Add(new OxxoFlejesAuto.Modelos.ProductoFleje
                                {
                                    Upc = prodPdf.Upc,
                                    Nombre = "⚠️ FALTANTE EN BD",
                                    ProductId = prodPdf.ProductId
                                });
                                faltantesEnReporte.Add(prodPdf);
                            }
                        }

                        RefrescarGrillaProductos();
                        this.Cursor = Cursors.Default;
                        MostrarReporteCompleto(encontradosEnReporte, faltantesEnReporte);
                    }
                    catch (Exception ex)
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show($"Error durante la sincronización: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private List<ProductoPlanograma> AnalizarEstructuraPdf(string rutaArchivo)
        {
            var resultados = new List<ProductoPlanograma>();
            using (PdfDocument pdf = PdfDocument.Open(rutaArchivo))
            {
                foreach (Page pagina in pdf.GetPages())
                {
                    string textoPagina = UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor.ContentOrderTextExtractor.GetText(pagina);
                    if (!textoPagina.Contains("Product_ID") && !textoPagina.Contains("UPC")) continue;

                    var matches = Regex.Matches(textoPagina, @"Bandeja\s+(?:(?<seg>\d+)\s*/\s*)?(?<num>\d+)(?<contenido>[\s\S]*?)(?=Bandeja\s+\d+|$)", RegexOptions.IgnoreCase);

                    foreach (Match match in matches)
                    {
                        int numBandeja = int.Parse(match.Groups["num"].Value);
                        int segmento = match.Groups["seg"].Success ? int.Parse(match.Groups["seg"].Value) : 1;
                        bool esSecuencialCorrido = !match.Groups["seg"].Success;

                        string contenidoBandeja = match.Groups["contenido"].Value;

                        MatchCollection filas = Regex.Matches(contenidoBandeja, @"\b(?<id>\d+)\s+(?<upc>\d{12,14})\b");
                        foreach (Match fila in filas)
                        {
                            resultados.Add(new ProductoPlanograma
                            {
                                NumeroBandeja = numBandeja,
                                SegmentoPdf = segmento,
                                EsSecuencial = esSecuencialCorrido,
                                ProductId = fila.Groups["id"].Value,
                                Upc = fila.Groups["upc"].Value,
                                NombreReferencia = "Extraído de tabla"
                            });
                        }
                    }
                }
            }
            return resultados;
        }

        private void MostrarReporteCompleto(List<ProductoPlanograma> encontrados, List<ProductoPlanograma> faltantes)
        {
            Form frmReporte = new Form { Text = "Reporte de Auditoría OXXO", Size = new System.Drawing.Size(900, 550), StartPosition = FormStartPosition.CenterParent, ShowIcon = false };
            TextBox txtReporte = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill, Font = new System.Drawing.Font("Consolas", 10), ReadOnly = true, BackColor = System.Drawing.Color.White };

            string contenido = "REPORTE DE LECTURA DE PLANOGRAMA PDF\r\n" + new string('=', 96) + "\r\n\r\n";

            contenido += $"✅ PRODUCTOS ENCONTRADOS ({encontrados.Count}):\r\n";
            foreach (var p in encontrados)
            {
                contenido += $"[Bandeja {p.NumeroBandeja}] | ID: {p.ProductId,-8} | UPC: {p.Upc,-14} | {p.NombreReferencia}\r\n";
            }

            contenido += $"\r\n\r\n❌ PRODUCTOS FALTANTES ({faltantes.Count}):\r\n";

            foreach (var p in faltantes)
            {
                contenido += $"[Bandeja {p.NumeroBandeja}] | ID: {p.ProductId,-8} | UPC: {p.Upc,-14} | ⚠️ NO ENCONTRADO EN BD (Requiere ingreso en Oracle)\r\n";
            }

            txtReporte.Text = contenido;
            frmReporte.Controls.Add(txtReporte);
            frmReporte.ShowDialog();
        }

        public class ProductoPlanograma
        {
            public int NumeroBandeja { get; set; }
            public string Upc { get; set; } = string.Empty;
            public string ProductId { get; set; } = string.Empty;
            public string NombreReferencia { get; set; } = string.Empty;
            public int SegmentoPdf { get; set; } = 1;
            public bool EsSecuencial { get; set; } = false;
        }

        private void txtBuscadorUpc_Enter(object sender, EventArgs e)
        {
            if (lstSugerencias != null && lstSugerencias.Items.Count > 0 && !string.IsNullOrWhiteSpace(txtBuscadorUpc.Text))
            {
                lstSugerencias.Visible = true;
                lstSugerencias.BringToFront();
            }
        }

        private void txtBuscadorUpc_Leave(object sender, EventArgs e)
        {
            Task.Delay(150).ContinueWith(t =>
            {
                if (this.IsHandleCreated)
                {
                    this.Invoke(new Action(() => {
                        if (lstSugerencias != null) lstSugerencias.Visible = false;
                    }));
                }
            });
        }

        private int filaArrastrada = -1;

        private void dgvProductos_MouseDown(object sender, MouseEventArgs e)
        {
            var hit = dgvProductos.HitTest(e.X, e.Y);
            if (hit.RowIndex >= 0)
            {
                filaArrastrada = hit.RowIndex;
                dgvProductos.ClearSelection();
                dgvProductos.Rows[filaArrastrada].Selected = true;
            }
        }

        private void dgvProductos_MouseMove(object sender, MouseEventArgs e)
        {
            if (filaArrastrada >= 0 && (e.Button & MouseButtons.Left) != MouseButtons.None)
            {
                dgvProductos.Cursor = Cursors.SizeNS;
            }
            else
            {
                dgvProductos.Cursor = Cursors.Default;
            }
        }

        private void dgvProductos_MouseUp(object sender, MouseEventArgs e)
        {
            dgvProductos.Cursor = Cursors.Default;
            var hit = dgvProductos.HitTest(e.X, e.Y);

            if (hit.RowIndex >= 0 && filaArrastrada >= 0 && hit.RowIndex != filaArrastrada && _bandejaActual != null)
            {
                var producto = _bandejaActual.Productos[filaArrastrada];
                _bandejaActual.Productos.RemoveAt(filaArrastrada);
                _bandejaActual.Productos.Insert(hit.RowIndex, producto);

                RefrescarGrillaProductos();

                dgvProductos.ClearSelection();
                dgvProductos.Rows[hit.RowIndex].Selected = true;
            }

            filaArrastrada = -1;
        }
    }
}