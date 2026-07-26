#pragma warning disable SYSLIB0014 
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FlejesAuto.Datos
{
    public static class ConexionApex
    {
        private static readonly HttpClient client;
        // 🌟 URL oficial y única que responde en tu servidor
        private static readonly string BaseUrl = "https://apex.oracle.com/ords/liltryn/flejes_api/productos";

        static ConexionApex()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = true
            };

            client = new HttpClient(handler);
        }

        // ===================================================
        // 1. BUSCADOR DIRECTO (Para barra manual - Enter)
        // ===================================================
        public static async Task<FlejesAuto.Modelos.ProductoFleje?> BuscarProductoEnApexAsync(string criterioBusqueda)
        {
            try
            {
                string upcLimpio = Uri.EscapeDataString(criterioBusqueda.Trim());
                string urlCompleta = $"{BaseUrl}?upc={upcLimpio}";

                var request = new HttpRequestMessage(HttpMethod.Get, urlCompleta);
                request.Version = HttpVersion.Version20;

                request.Headers.Clear();
                request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36");
                request.Headers.TryAddWithoutValidation("Accept", "application/json, text/plain, */*");
                request.Headers.TryAddWithoutValidation("Accept-Language", "es-ES,es;q=0.9,en;q=0.8");
                request.Headers.TryAddWithoutValidation("Connection", "keep-alive");

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string jsonRecibido = await response.Content.ReadAsStringAsync();
                    var jsonDinamico = JObject.Parse(jsonRecibido);
                    var items = jsonDinamico["items"] as JArray;

                    if (items != null && items.Count > 0)
                    {
                        var item = items[0] as JObject;
                        if (item != null)
                        {
                            string prodId = (item["productid"] ?? item["product_id"] ?? item["ProductId"])?.ToString() ?? string.Empty;
                            string upc = (item["upc"] ?? item["Upc"] ?? item["UPC"])?.ToString() ?? string.Empty;
                            string nombreReal = (item["nombre"] ?? item["Nombre"] ?? item["NOMBRE"])?.ToString() ?? "Sin Nombre";

                            return new FlejesAuto.Modelos.ProductoFleje
                            {
                                MecanismoSeleccion = false,
                                Upc = upc,
                                Nombre = nombreReal,
                                ProductId = prodId
                            };
                        }
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    MessageBox.Show("Oracle APEX (ORDS) está bloqueando la conexión (Error 403).", "Bloqueo de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception)
            {
                // Silencioso
            }
            return null;
        }

        // ===================================================
        // 2. BUSCADOR PREDICTIVO (Para las sugerencias de la barra)
        // ===================================================
        public static async Task<List<FlejesAuto.Modelos.ProductoFleje>> BuscarListaProductosEnApexAsync(string criterioBusqueda)
        {
            var listaProductos = new List<FlejesAuto.Modelos.ProductoFleje>();
            try
            {
                string upcLimpio = Uri.EscapeDataString(criterioBusqueda.Trim());
                string urlCompleta = $"{BaseUrl}?upc={upcLimpio}";

                var request = new HttpRequestMessage(HttpMethod.Get, urlCompleta);
                request.Version = HttpVersion.Version20;

                request.Headers.Clear();
                request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36");
                request.Headers.TryAddWithoutValidation("Accept", "application/json, text/plain, */*");
                request.Headers.TryAddWithoutValidation("Accept-Language", "es-ES,es;q=0.9,en;q=0.8");
                request.Headers.TryAddWithoutValidation("Connection", "keep-alive");

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string jsonRecibido = await response.Content.ReadAsStringAsync();
                    var jsonDinamico = JObject.Parse(jsonRecibido);
                    var items = jsonDinamico["items"] as JArray;

                    if (items != null)
                    {
                        foreach (var row in items)
                        {
                            var item = row as JObject;
                            if (item != null)
                            {
                                string prodId = (item["productid"] ?? item["product_id"] ?? item["ProductId"])?.ToString() ?? string.Empty;
                                string upc = (item["upc"] ?? item["Upc"] ?? item["UPC"])?.ToString() ?? string.Empty;
                                string nombreReal = (item["nombre"] ?? item["Nombre"] ?? item["NOMBRE"])?.ToString() ?? "Sin Nombre";

                                listaProductos.Add(new FlejesAuto.Modelos.ProductoFleje
                                {
                                    MecanismoSeleccion = false,
                                    Upc = upc,
                                    Nombre = nombreReal,
                                    ProductId = prodId
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Silencioso
            }
            return listaProductos;
        }

        // ===================================================================
        // 3. 🌟 CONSULTA POR LOTE VIA GET (Para el planograma PDF)
        // ===================================================================
        public static async Task<List<FlejesAuto.Modelos.ProductoFleje>> BuscarListaProductosPorLoteEnApexAsync(List<string> listaUpcs)
        {
            var listaProductos = new List<FlejesAuto.Modelos.ProductoFleje>();
            try
            {
                if (listaUpcs == null || listaUpcs.Count == 0) return listaProductos;

                // 🌟 LA MAGIA: Limitamos a un máximo de 3 peticiones simultáneas en paralelo
                using (var semaforo = new System.Threading.SemaphoreSlim(3))
                {
                    var tareas = new List<Task<FlejesAuto.Modelos.ProductoFleje?>>();

                    foreach (string upc in listaUpcs)
                    {
                        if (!string.IsNullOrWhiteSpace(upc))
                        {
                            tareas.Add(Task.Run(async () =>
                            {
                                await semaforo.WaitAsync(); // Pide permiso al semáforo
                                try
                                {
                                    // ⏱️ Mini respiro de 30ms para no saturar los sockets de red de Oracle
                                    await Task.Delay(30);
                                    return await BuscarProductoEnApexAsync(upc.Trim());
                                }
                                finally
                                {
                                    semaforo.Release(); // Libera el espacio para el siguiente UPC
                                }
                            }));
                        }
                    }

                    // Esperamos que el grupo controlado termine de procesar todo
                    var resultados = await Task.WhenAll(tareas);

                    foreach (var producto in resultados)
                    {
                        if (producto != null)
                        {
                            listaProductos.Add(producto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la consulta dosificada del PDF: {ex.Message}", "Fallo de Sincronización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return listaProductos;
        }
    }
}