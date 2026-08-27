using Proyecto1.TDA;

namespace Proyecto1.Modelo
{
    public class Ciudad
    {
        public string Nombre { get; set; }
        public int Filas { get; set; }
        public int Columnas { get; set; }
        public ListaSimple<ListaSimple<Celda>> Malla { get; set; }

        public Ciudad(string nombre, int filas, int columnas)
        {
            Nombre = nombre;
            Filas = filas;
            Columnas = columnas;
            Malla = new ListaSimple<ListaSimple<Celda>>();
        }

        public Celda ObtenerCelda(int fila, int columna)
        {
            ListaSimple<Celda> filaLista = Malla.ObtenerEn(fila);
            return filaLista.ObtenerEn(columna);
        }

        public ListaSimple<Celda> ObtenerCeldasPorTipo(TipoCelda tipo)
        {
            ListaSimple<Celda> resultado = new ListaSimple<Celda>();
            for (int f = 0; f < Filas; f++)
            {
                for (int c = 0; c < Columnas; c++)
                {
                    Celda celda = ObtenerCelda(f, c);
                    if (celda.Tipo == tipo)
                        resultado.Agregar(celda);
                }
            }
            return resultado;
        }

    }
}