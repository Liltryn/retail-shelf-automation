using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace FlejesAuto.Automatizacion
{
    public static class ControladorTeclado
    {
        // Importamos las funciones nativas del sistema operativo Windows
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string? lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Busca la ventana de la app de flejes, la trae al frente e inyecta el código UPC.
        /// </summary>
        public static bool EnviarCodigoAAppFlejes(string upc, int delayMilisegundos)
        {
            // 1. Buscamos la ventana por su título exacto en la barra superior de Windows.
            // NOTA : Reemplaza "Nombre De app" por el título real que ves en su ventana.
            IntPtr ventanaOxxo = FindWindow(null, "*FLEJES.txt: Bloc de notas");

            // Si no la encuentra por el nombre exacto, podemos intentar buscar una aproximación más adelante
            if (ventanaOxxo == IntPtr.Zero)
            {
                return false; // Indicamos que la ventana no estaba abierta
            }

            // 2. Obligamos a Windows a traer la app de flejes al frente y hacerle foco
            SetForegroundWindow(ventanaOxxo);
            Thread.Sleep(100); // Pequeña pausa de estabilización para que Windows reaccione

            // 3. Emulamos la escritura humana
            SendKeys.SendWait(upc);
            SendKeys.SendWait("{ENTER}");

            // 4. Pausa configurada por el delay para que el sistema del local procese el código
            Thread.Sleep(delayMilisegundos);

            return true;
        }
    }
}
