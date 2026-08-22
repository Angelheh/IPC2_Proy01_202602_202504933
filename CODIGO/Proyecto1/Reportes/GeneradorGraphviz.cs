using System;
using System.Collections.Generic;
using System.Text;
using Proyecto1.TDA;
using Proyecto1.Modelo;

namespace Proyecto1.Reportes
{
    public class GeneradorGraphviz
    {
        public string GenerarDot(Ciudad ciudad, ListaSimple<Celda> camino)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph G {");
            sb.AppendLine("  node [shape=plaintext];");
            sb.AppendLine("  mapa [label=<");
            sb.AppendLine("    <TABLE BORDER=\"0\" CELLBORDER=\"1\" CELLSPACING=\"0\">");

            for (int f = 0; f < ciudad.Filas; f++)
            {
                sb.Append("      <TR>");
                for (int c = 0; c < ciudad.Columnas; c++)
                {
                    Celda celda = ciudad.ObtenerCelda(f, c);
                    bool enCamino = camino != null && camino.BuscarPorCondicion(x => x == celda) != null;

                    string color = ObtenerColor(celda.Tipo);
                    if (enCamino && celda.Tipo == TipoCelda.Camino)
                        color = "orange";

                    string borde = enCamino ? " BORDER=\"3\"" : "";
                    sb.Append("<TD WIDTH=\"25\" HEIGHT=\"25\" BGCOLOR=\"" + color + "\"" + borde + "></TD>");
                }
                sb.AppendLine("</TR>");
            }

            sb.AppendLine("    </TABLE>");
            sb.AppendLine("  >];");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string ObtenerColor(TipoCelda tipo)
        {
            switch (tipo)
            {
                case TipoCelda.Intransitable: return "black";
                case TipoCelda.Entrada: return "green";
                case TipoCelda.Camino: return "white";
                case TipoCelda.Militar: return "red";
                case TipoCelda.Civil: return "dodgerblue";
                case TipoCelda.Recurso: return "gray";
                default: return "white";
            }
        }

        public void GuardarDot(string contenidoDot, string rutaArchivo)
        {
            System.IO.File.WriteAllText(rutaArchivo, contenidoDot);
        }
    }
}
