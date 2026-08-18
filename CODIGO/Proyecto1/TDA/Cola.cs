using System;

namespace Proyecto1.TDA
{
    public class Cola<T>
    {
        private Nodo<T> frente;
        private Nodo<T> final;
        private int longitud;

        public bool EstaVacia => longitud == 0;
        public int Longitud => longitud;

        public void Encolar(T dato)
        {
            Nodo<T> nuevo = new Nodo<T>(dato);
            if (EstaVacia)
            {
                frente = nuevo;
                final = nuevo;
            }
            else
            {
                final.Siguiente = nuevo;
                final = nuevo;
            }
            longitud++;
        }

        public T Desencolar()
        {
            if (EstaVacia)
                throw new InvalidOperationException("La cola esta vacia");

            T dato = frente.Dato;
            frente = frente.Siguiente;
            longitud--;
            if (EstaVacia) final = null;

            return dato;
        }
    }
}