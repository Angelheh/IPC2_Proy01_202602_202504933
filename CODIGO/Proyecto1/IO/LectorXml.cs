using System;
using System.Xml.Linq;
using Proyecto1.TDA;
using Proyecto1.Modelo;

namespace Proyecto1.IO
{
    public class LectorXml
    {
        public ListaSimple<Ciudad> Ciudades { get; set; }
        public ListaSimple<Robot> Robots { get; set; }

        public LectorXml()
        {
            Ciudades = new ListaSimple<Ciudad>();
            Robots = new ListaSimple<Robot>();
        }
        public void CargarArchivo(string rutaArchivo)
        {
            XDocument doc = XDocument.Load(rutaArchivo);

            XElement listaCiudades = doc.Root.Element("listaCiudades");
            foreach (XElement nodoCiudad in listaCiudades.Elements("ciudad"))
            {
                CargarCiudad(nodoCiudad);
            }

            XElement robotsElement = doc.Root.Element("robots");
            foreach (XElement nodoRobot in robotsElement.Elements("robot"))
            {
                CargarRobot(nodoRobot);
            }
        }

        private void CargarCiudad(XElement nodoCiudad)
        {
            XElement nombreElement = nodoCiudad.Element("nombre");
            string nombre = nombreElement.Value;
            int filas = int.Parse(nombreElement.Attribute("filas").Value);
            int columnas = int.Parse(nombreElement.Attribute("columnas").Value);

            Ciudad ciudad = new Ciudad(nombre, filas, columnas);

            for (int f = 0; f < filas; f++)
            {
                ListaSimple<Celda> filaLista = new ListaSimple<Celda>();
                for (int c = 0; c < columnas; c++)
                {
                    filaLista.Agregar(new Celda(f, c, TipoCelda.Camino));
                }
                ciudad.Malla.Agregar(filaLista);
            }

            foreach (XElement nodoFila in nodoCiudad.Elements("fila"))
            {
                int numeroFila = int.Parse(nodoFila.Attribute("numero").Value) - 1;
                string contenido = nodoFila.Value;

                ListaSimple<Celda> filaLista = ciudad.Malla.ObtenerEn(numeroFila);
                for (int col = 0; col < contenido.Length; col++)
                {
                    char caracter = contenido[col];
                    TipoCelda tipo = InterpretarCaracter(caracter);
                    Celda celda = filaLista.ObtenerEn(col);
                    celda.Tipo = tipo;
                }
            }

            foreach (XElement nodoMilitar in nodoCiudad.Elements("unidadMilitar"))
            {
                int fila = int.Parse(nodoMilitar.Attribute("fila").Value) - 1;
                int columna = int.Parse(nodoMilitar.Attribute("columna").Value) - 1;
                int capacidad = int.Parse(nodoMilitar.Value);

                Celda celda = ciudad.ObtenerCelda(fila, columna);
                celda.Tipo = TipoCelda.Militar;
                celda.CapacidadMilitar = capacidad;
            }

            Ciudades.Agregar(ciudad);
        }

        private TipoCelda InterpretarCaracter(char c)
        {
            switch (c)
            {
                case '*': return TipoCelda.Intransitable;
                case ' ': return TipoCelda.Camino;
                case 'E': return TipoCelda.Entrada;
                case 'C': return TipoCelda.Civil;
                case 'R': return TipoCelda.Recurso;
                default: throw new Exception("Caracter invalido en fila: " + c);
            }
        }

        private void CargarRobot(XElement nodoRobot)
        {
            XElement nombreElement = nodoRobot.Element("nombre");
            string nombre = nombreElement.Value;
            string tipo = nombreElement.Attribute("tipo").Value;

            Robot robot;
            if (tipo == "ChapinFighter")
            {
                int capacidad = int.Parse(nombreElement.Attribute("capacidad").Value);
                robot = new ChapinFighter(nombre, capacidad);
            }
            else
            {
                robot = new ChapinRescue(nombre);
            }

            Robots.Agregar(robot);
        }

    }
}
