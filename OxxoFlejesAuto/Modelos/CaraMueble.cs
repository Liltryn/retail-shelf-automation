using Newtonsoft.Json;
using System.Collections.Generic;

namespace OxxoFlejesAuto.Modelos
{
    public class CaraMueble
    {
        public string NombreCara { get; set; }
        public List<BandejaMueble> Bandejas { get; set; }

        // 🌟 1. CONSTRUCTOR LIMPIO: Ahora solo prepara la lista vacía
        public CaraMueble(string nombre)
        {
            NombreCara = nombre;
            Bandejas = new List<BandejaMueble>();
        }

        // 🌟 2. MÉTODO CONTROLADO: Solo crea las 3 bandejas si la lista está vacía
        public void GenerarBandejasPorDefecto()
        {
            // Si ya hay bandejas (porque se cargaron del disco duro), abortamos para no duplicar
            if (Bandejas != null && Bandejas.Count > 0) return;

            Bandejas = new List<BandejaMueble>();

            // Creamos las 3 bandejas por defecto
            for (int i = 1; i <= 3; i++)
            {
                Bandejas.Add(new BandejaMueble
                {
                    NombreBandeja = $"Bandeja {i}",
                    Productos = new List<ProductoFleje>()
                });
            }
        }
    }

    public class BandejaMueble
    {
        public int NumeroBandeja { get; set; }
        public string NombreBandeja { get; set; } = string.Empty;

        public List<ProductoFleje> Productos { get; set; } = new List<ProductoFleje>();
    }

    public class ProductoFleje
    {
        public bool MecanismoSeleccion { get; set; }

        [JsonProperty("Upc")]
        public string? Upc { get; set; }

        [JsonProperty("Nombre")]
        public string? Nombre { get; set; }

        [JsonProperty("ProductId")]
        public string? ProductId { get; set; }
    }
}