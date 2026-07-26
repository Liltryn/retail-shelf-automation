using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using OxxoFlejesAuto.Modelos;

namespace OxxoFlejesAuto.Datos
{
    public static class GestorTienda
    {
        // Archivo para los muebles
        private static string RutaArchivoMuebles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tienda_guardada.json");
        // 🌟 NUEVO: Archivo exclusivo para el tamaño del lienzo
        private static string RutaArchivoMedidas = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "medidas_tienda.json");

        // =======================================================
        // GESTIÓN DE MUEBLES (Lo que ya tenías intacto)
        // =======================================================
        public static void GuardarTienda(List<MuebleTienda> listaMuebles)
        {
            try
            {
                string jsonTexto = JsonConvert.SerializeObject(listaMuebles, Formatting.Indented);
                File.WriteAllText(RutaArchivoMuebles, jsonTexto);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar los muebles: {ex.Message}", "Error");
            }
        }

        public static List<MuebleTienda> CargarTienda()
        {
            try
            {
                if (!File.Exists(RutaArchivoMuebles)) return new List<MuebleTienda>();
                string jsonTexto = File.ReadAllText(RutaArchivoMuebles);
                return JsonConvert.DeserializeObject<List<MuebleTienda>>(jsonTexto) ?? new List<MuebleTienda>();
            }
            catch
            {
                return new List<MuebleTienda>();
            }
        }

        // =======================================================
        // 🌟 GESTIÓN DEL TAMAÑO DEL LOCAL (Lo nuevo)
        // =======================================================
        public static void GuardarMedidas(int ancho, int alto)
        {
            try
            {
                var configuracion = new ConfigTienda { AnchoLienzo = ancho, AltoLienzo = alto };
                string jsonTexto = JsonConvert.SerializeObject(configuracion, Formatting.Indented);
                File.WriteAllText(RutaArchivoMedidas, jsonTexto);
            }
            catch { }
        }

        public static ConfigTienda CargarMedidas()
        {
            try
            {
                if (!File.Exists(RutaArchivoMedidas)) return new ConfigTienda(); // Devuelve 600x600 por defecto
                string jsonTexto = File.ReadAllText(RutaArchivoMedidas);
                return JsonConvert.DeserializeObject<ConfigTienda>(jsonTexto) ?? new ConfigTienda();
            }
            catch
            {
                return new ConfigTienda();
            }
        }
    }

    // 🌟 CLASE DE APOYO PARA GUARDAR LAS MEDIDAS
    public class ConfigTienda
    {
        public int AnchoLienzo { get; set; } = 600;
        public int AltoLienzo { get; set; } = 600;
    }
}