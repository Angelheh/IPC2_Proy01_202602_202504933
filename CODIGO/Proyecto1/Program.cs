using Proyecto1.Algoritmos;
using Proyecto1.IO;
using Proyecto1.Reportes;

LectorXml lector = new LectorXml();
lector.CargarArchivo("ConfigPrueba.xml");

Console.WriteLine("Ciudades cargadas: " + lector.Ciudades.Longitud);
Console.WriteLine("Robots cargados: " + lector.Robots.Longitud);

var ciudad = lector.Ciudades.ObtenerEn(0);
Console.WriteLine("Nombre: " + ciudad.Nombre + " (" + ciudad.Filas + "x" + ciudad.Columnas + ")");

var buscador = new BuscadorRutas();

var entrada = ciudad.ObtenerCelda(0, 1);
var civil = ciudad.ObtenerCelda(2, 2);
var recurso = ciudad.ObtenerCelda(3, 2);

var rutaRescate = buscador.BuscarRutaRescate(ciudad, entrada, civil);
Console.WriteLine("\nRuta de rescate:");
if (rutaRescate == null) Console.WriteLine("Mision Imposible");
else for (int i = 0; i < rutaRescate.Longitud; i++)
    Console.Write("(" + rutaRescate.ObtenerEn(i).Fila + "," + rutaRescate.ObtenerEn(i).Columna + ") ");

int capacidadFinal;
var rutaExtraccion = buscador.BuscarRutaExtraccion(ciudad, entrada, recurso, 50, out capacidadFinal);
Console.WriteLine("\nRuta de extraccion (capacidad final: " + capacidadFinal + "):");
if (rutaExtraccion == null) Console.WriteLine("Mision Imposible");
else for (int i = 0; i < rutaExtraccion.Longitud; i++)
    Console.Write("(" + rutaExtraccion.ObtenerEn(i).Fila + "," + rutaExtraccion.ObtenerEn(i).Columna + ") ");

var generador = new GeneradorGraphviz();
string dotRescate = generador.GenerarDot(ciudad, rutaRescate);
generador.GuardarDot(dotRescate, "rutaRescate.dot");
Console.WriteLine("\nArchivo .dot generado: rutaRescate.dot");