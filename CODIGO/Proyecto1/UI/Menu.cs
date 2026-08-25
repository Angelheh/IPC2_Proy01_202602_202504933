using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Proyecto1.Algoritmos;
using Proyecto1.Modelo;
using Proyecto1.Reportes;
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
                        EjecutarMision();
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

        private void EjecutarMision()
        {
            Console.WriteLine("\n    Tipo de mision    ");
            Console.WriteLine("1. Rescate");
            Console.WriteLine("2. Extraccion de recursos");
            Console.Write("Seleccione: ");
            string tipo = Console.ReadLine();

            if (tipo == "1")
                EjecutarRescate();
            else if (tipo == "2")
                Console.WriteLine("Pendiente: extraccion");
            else
                Console.WriteLine("Opcion invalida.");
        }


        private void EjecutarRescate()
        {
            var ciudadesConCiviles = lector.Ciudades.FiltrarPorCondicion(
                c => c.ObtenerCeldasPorTipo(TipoCelda.Civil).Longitud > 0
            );

            if (ciudadesConCiviles.EstaVacia)
            {
                Console.WriteLine("No hay ciudades con unidades civiles disponibles ");
                return;
            }

            Console.WriteLine("\n    Ciudades disponibles para rescate    ");
            for (int i = 0; i < ciudadesConCiviles.Longitud; i++)
                Console.WriteLine((i + 1) + ". " + ciudadesConCiviles.ObtenerEn(i).Nombre);

            int indiceCiudad = LeerOpcionNumerica(ciudadesConCiviles.Longitud);
            if (indiceCiudad == -1) return;
            Ciudad ciudad = ciudadesConCiviles.ObtenerEn(indiceCiudad);

            var rescuers = lector.Robots.FiltrarPorCondicion(r => r is ChapinRescue);
            if (rescuers.EstaVacia)
            {
                Console.WriteLine("No hay robots disponibles.");
                return;
            }

            Robot robotElegido;
            if (rescuers.Longitud == 1)
            {
                robotElegido = rescuers.ObtenerEn(0);
            }
            else
            {
                Console.WriteLine("\n--- Robots ChapinRescue disponibles ---");
                for (int i = 0; i < rescuers.Longitud; i++)
                    Console.WriteLine((i + 1) + ". " + rescuers.ObtenerEn(i).Nombre);

                int indiceRobot = LeerOpcionNumerica(rescuers.Longitud);
                if (indiceRobot == -1) return;
                robotElegido = rescuers.ObtenerEn(indiceRobot);
            }

            var civiles = ciudad.ObtenerCeldasPorTipo(TipoCelda.Civil);
            Celda civilObjetivo;
            if (civiles.Longitud == 1)
            {
                civilObjetivo = civiles.ObtenerEn(0);
            }
            else
            {
                Console.WriteLine("\n     Unidades civiles en " + ciudad.Nombre);
                for (int i = 0; i < civiles.Longitud; i++)
                {
                    Celda c = civiles.ObtenerEn(i);
                    Console.WriteLine((i + 1) + ". Fila " + (c.Fila + 1) + ", Columna " + (c.Columna + 1));
                }

                int indiceCivil = LeerOpcionNumerica(civiles.Longitud);
                if (indiceCivil == -1) return;
                civilObjetivo = civiles.ObtenerEn(indiceCivil);
            }

            var entradas = ciudad.ObtenerCeldasPorTipo(TipoCelda.Entrada);
            if (entradas.EstaVacia)
            {
                Console.WriteLine("Esta ciudad no tiene puntos de entrada.");
                return;
            }
            Celda entrada = entradas.ObtenerEn(0);

            var buscador = new BuscadorRutas();
            var camino = buscador.BuscarRutaRescate(ciudad, entrada, civilObjetivo);

            if (camino == null)
            {
                Console.WriteLine("\nMision Imposible");
                return;
            }

            Console.WriteLine("\nTipo de mision: rescate");
            Console.WriteLine("Unidad civil rescatada: " + (civilObjetivo.Fila + 1) + "," + (civilObjetivo.Columna + 1));
            Console.WriteLine("Robot utilizado: " + robotElegido.Nombre);

            var generador = new GeneradorGraphviz();
            string dotContenido = generador.GenerarDot(ciudad, camino);

            string carpetaHistorial = "Historial";
            Directory.CreateDirectory(carpetaHistorial);
            string marcaTiempo = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string nombreBase = carpetaHistorial + "/reporte_rescate_" + ciudad.Nombre + "_" + marcaTiempo;

            generador.GuardarDot(dotContenido, nombreBase + ".dot");
            generador.GenerarImagen(nombreBase + ".dot", nombreBase + ".png");
            Console.WriteLine("Reporte generado: " + nombreBase + ".png");
        }

        private int LeerOpcionNumerica(int cantidadOpciones)
        {
            Console.Write("Seleccione: ");
            string entrada = Console.ReadLine();
            int opcion;

            if (!int.TryParse(entrada, out opcion) || opcion < 1 || opcion > cantidadOpciones)
            {
                Console.WriteLine("Opcion invalida.");
                return -1;
            }

            return opcion - 1;
        }

    }
}
