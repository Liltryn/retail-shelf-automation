using System.Collections.Generic;

namespace OxxoFlejesAuto.Modelos
{
    public class MuebleTienda
    {
        public string IdMueble { get; set; }
        public string NombreVisible { get; set; }
        public string TipoMueble { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Ancho { get; set; }
        public int Alto { get; set; }

        // UNIFICADO: Diccionario con llaves idénticas a los clics del mouse de Juan
        public Dictionary<string, CaraMueble> Caras { get; set; } = new Dictionary<string, CaraMueble>()
        {
          
        };
    }
}