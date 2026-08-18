using Proyecto1.IO;

LectorXml lector = new LectorXml();
lector.CargarArchivo("ConfigPrueba.xml");

Console.WriteLine("Ciudades cargadas: " + lector.Ciudades.Longitud);
Console.WriteLine("Robots cargados: " + lector.Robots.Longitud);

var ciudad = lector.Ciudades.ObtenerEn(0);
Console.WriteLine("Nombre: " + ciudad.Nombre + " (" + ciudad.Filas + "x" + ciudad.Columnas + ")");

for (int f = 0; f < ciudad.Filas; f++)
{
    for (int c = 0; c < ciudad.Columnas; c++)
    {
        Console.Write(ciudad.ObtenerCelda(f, c).Tipo.ToString().Substring(0, 1));
    }
    Console.WriteLine();
}