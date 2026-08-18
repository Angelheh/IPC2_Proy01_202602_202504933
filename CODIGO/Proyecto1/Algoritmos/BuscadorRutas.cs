using System;
using Proyecto1.TDA;
using Proyecto1.Modelo;

namespace Proyecto1.Algoritmos
{
    public class BuscadorRutas
    {
        // Nodo interno del BFS: guarda la celda, la capacidad restante (solo aplica a Fighter)
        // y el padre, para reconstruir el camino al final.
        private class NodoRuta
        {
            public Celda Celda;
            public int CapacidadActual;
            public NodoRuta Padre;

            public NodoRuta(Celda celda, int capacidadActual, NodoRuta padre)
            {
                Celda = celda;
                CapacidadActual = capacidadActual;
                Padre = padre;
            }
        }

        private static readonly int[] DeltaFila = { -1, 1, 0, 0 };
        private static readonly int[] DeltaColumna = { 0, 0, -1, 1 };

        // ---------- MISION DE RESCATE ----------
        // No puede pasar por celdas Militares bajo ninguna circunstancia.
        public ListaSimple<Celda> BuscarRutaRescate(Ciudad ciudad, Celda entrada, Celda civilObjetivo)
        {
            ListaSimple<ListaSimple<bool>> visitado = CrearMatrizVisitados(ciudad);
            Cola<NodoRuta> cola = new Cola<NodoRuta>();

            NodoRuta inicio = new NodoRuta(entrada, 0, null);
            cola.Encolar(inicio);
            visitado.ObtenerEn(entrada.Fila).ObtenerEn(entrada.Columna); // no-op, solo referencia
            MarcarVisitado(visitado, entrada.Fila, entrada.Columna);

            while (!cola.EstaVacia)
            {
                NodoRuta actual = cola.Desencolar();

                if (actual.Celda == civilObjetivo)
                    return ReconstruirCamino(actual);

                for (int d = 0; d < 4; d++)
                {
                    int nf = actual.Celda.Fila + DeltaFila[d];
                    int nc = actual.Celda.Columna + DeltaColumna[d];

                    if (!EnRango(ciudad, nf, nc)) continue;
                    if (YaVisitado(visitado, nf, nc)) continue;

                    Celda vecina = ciudad.ObtenerCelda(nf, nc);

                    bool esTransitable = vecina.Tipo == TipoCelda.Camino
                                       || vecina.Tipo == TipoCelda.Entrada
                                       || vecina.Tipo == TipoCelda.Civil;

                    if (!esTransitable) continue; // Militar y Recurso quedan bloqueados para rescate

                    MarcarVisitado(visitado, nf, nc);
                    cola.Encolar(new NodoRuta(vecina, 0, actual));
                }
            }

            return null; // Mision Imposible
        }

        // ---------- MISION DE EXTRACCION ----------
        // Puede pasar por Militares si capacidadActual > capacidad de la unidad militar,
        // y en ese caso resta esa capacidad al robot.
        public ListaSimple<Celda> BuscarRutaExtraccion(Ciudad ciudad, Celda entrada, Celda recursoObjetivo,
                                                         int capacidadInicial, out int capacidadFinal)
        {
            ListaSimple<ListaSimple<bool>> visitado = CrearMatrizVisitados(ciudad);
            Cola<NodoRuta> cola = new Cola<NodoRuta>();

            NodoRuta inicio = new NodoRuta(entrada, capacidadInicial, null);
            cola.Encolar(inicio);
            MarcarVisitado(visitado, entrada.Fila, entrada.Columna);

            while (!cola.EstaVacia)
            {
                NodoRuta actual = cola.Desencolar();

                if (actual.Celda == recursoObjetivo)
                {
                    capacidadFinal = actual.CapacidadActual;
                    return ReconstruirCamino(actual);
                }

                for (int d = 0; d < 4; d++)
                {
                    int nf = actual.Celda.Fila + DeltaFila[d];
                    int nc = actual.Celda.Columna + DeltaColumna[d];

                    if (!EnRango(ciudad, nf, nc)) continue;
                    if (YaVisitado(visitado, nf, nc)) continue;

                    Celda vecina = ciudad.ObtenerCelda(nf, nc);
                    int capacidadTrasMover = actual.CapacidadActual;

                    if (vecina.Tipo == TipoCelda.Intransitable)
                        continue;

                    if (vecina.Tipo == TipoCelda.Recurso && vecina != recursoObjetivo)
                        continue; // un recurso que no es el objetivo no se puede pisar

                    if (vecina.Tipo == TipoCelda.Militar)
                    {
                        if (actual.CapacidadActual <= vecina.CapacidadMilitar)
                            continue; // no la puede vencer, camino bloqueado por aqui
                        capacidadTrasMover = actual.CapacidadActual - vecina.CapacidadMilitar;
                    }

                    MarcarVisitado(visitado, nf, nc);
                    cola.Encolar(new NodoRuta(vecina, capacidadTrasMover, actual));
                }
            }

            capacidadFinal = 0;
            return null; // Mision Imposible
        }

        // ---------- HELPERS ----------
        private ListaSimple<Celda> ReconstruirCamino(NodoRuta destino)
        {
            ListaSimple<Celda> invertido = new ListaSimple<Celda>();
            NodoRuta actual = destino;
            while (actual != null)
            {
                invertido.Agregar(actual.Celda);
                actual = actual.Padre;
            }

            ListaSimple<Celda> camino = new ListaSimple<Celda>();
            for (int i = invertido.Longitud - 1; i >= 0; i--)
                camino.Agregar(invertido.ObtenerEn(i));

            return camino;
        }

        private ListaSimple<ListaSimple<bool>> CrearMatrizVisitados(Ciudad ciudad)
        {
            ListaSimple<ListaSimple<bool>> matriz = new ListaSimple<ListaSimple<bool>>();
            for (int f = 0; f < ciudad.Filas; f++)
            {
                ListaSimple<bool> fila = new ListaSimple<bool>();
                for (int c = 0; c < ciudad.Columnas; c++)
                    fila.Agregar(false);
                matriz.Agregar(fila);
            }
            return matriz;
        }

        private void MarcarVisitado(ListaSimple<ListaSimple<bool>> matriz, int fila, int columna)
        {
            // ListaSimple no tiene "SetEn", asi que reconstruimos la fila con el valor actualizado.
            ListaSimple<bool> filaLista = matriz.ObtenerEn(fila);
            filaLista.MarcarEnIndice(columna, true);
        }

        private bool YaVisitado(ListaSimple<ListaSimple<bool>> matriz, int fila, int columna)
        {
            return matriz.ObtenerEn(fila).ObtenerEn(columna);
        }

        private bool EnRango(Ciudad ciudad, int fila, int columna)
        {
            return fila >= 0 && fila < ciudad.Filas && columna >= 0 && columna < ciudad.Columnas;
        }
    }
}