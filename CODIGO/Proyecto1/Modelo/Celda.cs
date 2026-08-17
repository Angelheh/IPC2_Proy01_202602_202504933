namespace Proyecto1.Modelo
{
    public enum TipoCelda
    {
        Intransitable,
        Entrada,
        Camino,
        Civil,
        Recurso,
        Militar
    }

    public class Celda
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
        public TipoCelda Tipo { get; set; }
        public int CapacidadMilitar { get; set; } // solo aplica si Tipo == Militar

        public Celda(int fila, int columna, TipoCelda tipo)
        {
            Fila = fila;
            Columna = columna;
            Tipo = tipo;
            CapacidadMilitar = 0;
        }
    }
}