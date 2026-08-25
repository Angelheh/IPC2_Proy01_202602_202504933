using System;
using System.Collections.Generic;
using System.Text;
using Proyecto1.IO;

namespace Proyecto1.UI
{
    public class Menu
    {

        private LectorXml lector;

        public Menu()
        {
            lector = new LectorXml();
        }

        public void Iniciar()
        {
            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n===== SISTEMA DE CONTROL - CHAPIN WARRIORS =====");
                Console.WriteLine("1. Cargar archivo de configuracion");
                Console.WriteLine("2. Ver ciudades y robots cargados");
                Console.WriteLine("3. Ejecutar mision");
                Console.WriteLine("4. Salir");
                Console.Write("Seleccione una opcion: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        CargarConfiguracion();
                        break;
                    case "2":
                        VerCargados();
                        break;
                    case "3":
                        Console.WriteLine("(Pendiente: ejecutar mision)");
                        break;
                    case "4":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opcion invalida.");
                        break;
                }
            }
        }

        private void CargarConfiguracion()
        {
            Console.Write("Ingrese el nombre del archivo XML (debe estar en la carpeta del ejecutable): ");
            string nombreArchivo = Console.ReadLine();

            try
            {
                lector.CargarArchivo(nombreArchivo);
                Console.WriteLine("Archivo cargado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar el archivo: " + ex.Message);
            }
        }

        private void VerCargados()
        {
            Console.WriteLine("\nCiudades cargadas: " + lector.Ciudades.Longitud);
            for (int i = 0; i < lector.Ciudades.Longitud; i++)
            {
                var c = lector.Ciudades.ObtenerEn(i);
                Console.WriteLine("  - " + c.Nombre + " (" + c.Filas + "x" + c.Columnas + ")");
            }

            Console.WriteLine("Robots cargados: " + lector.Robots.Longitud);
            for (int i = 0; i < lector.Robots.Longitud; i++)
            {
                var r = lector.Robots.ObtenerEn(i);
                Console.WriteLine("  - " + r.Nombre + " (" + r.GetType().Name + ")");
            }
        }

    }
}
