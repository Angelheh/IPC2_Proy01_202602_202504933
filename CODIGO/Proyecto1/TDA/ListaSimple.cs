namespace Proyecto1.TDA
{
    public class ListaSimple<T>
    {
        private Nodo<T> primero;
        private Nodo<T> ultimo;
        private int longitud;

        public int Longitud => longitud;
        public bool EstaVacia => longitud == 0;

        public void Agregar(T dato)
        {
            Nodo<T> nuevo = new Nodo<T>(dato);
            if (EstaVacia)
            {
                primero = nuevo;
                ultimo = nuevo;
            }
            else
            {
                ultimo.Siguiente = nuevo;
                ultimo = nuevo;
            }
            longitud++;
        }

        public T ObtenerEn(int indice)
        {
            if (indice < 0 || indice >= longitud)
                throw new IndexOutOfRangeException("Indice fuera de rango en ListaSimple");

            Nodo<T> actual = primero;
            for (int i = 0; i < indice; i++)
                actual = actual.Siguiente;

            return actual.Dato;
        }

        public Nodo<T> ObtenerPrimero()
        {
            return primero;
        }

        public void MarcarEnIndice(int indice, T valor)
        {
            if (indice < 0 || indice >= longitud)
                throw new IndexOutOfRangeException("Indice fuera de rango en ListaSimple");

            Nodo<T> actual = primero;
            for (int i = 0; i < indice; i++)
                actual = actual.Siguiente;

            actual.Dato = valor;
        }

        public T BuscarPorCondicion(Func<T, bool> condicion)
        {
            Nodo<T> actual = primero;
            while (actual != null)
            {
                if (condicion(actual.Dato))
                    return actual.Dato;
                actual = actual.Siguiente;
            }
            return default(T);
        }

        public void EliminarPorCondicion(Func<T, bool> condicion)
        {
            Nodo<T> actual = primero;
            Nodo<T> anterior = null;

            while (actual != null)
            {
                if (condicion(actual.Dato))
                {
                    if (anterior == null)
                        primero = actual.Siguiente;
                    else
                        anterior.Siguiente = actual.Siguiente;

                    if (actual == ultimo)
                        ultimo = anterior;

                    longitud--;
                    return;
                }
                anterior = actual;
                actual = actual.Siguiente;
            }
        }

    }
}