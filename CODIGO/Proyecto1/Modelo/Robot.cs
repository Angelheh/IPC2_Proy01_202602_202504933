namespace Proyecto1.Modelo
{
    public abstract class Robot
    {
        public string Nombre { get; set; }

        public Robot(string nombre)
        {
            Nombre = nombre;
        }
    }

    public class ChapinRescue : Robot
    {
        public ChapinRescue(string nombre) : base(nombre) { }
    }

    public class ChapinFighter : Robot
    {
        public int Capacidad { get; set; }

        public ChapinFighter(string nombre, int capacidad) : base(nombre)
        {
            Capacidad = capacidad;
        }
    }
}