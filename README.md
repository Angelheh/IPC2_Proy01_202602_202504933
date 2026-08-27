IPC2 - Proyecto 1: Chapin Warriors
Angel - Carnet 202504933 - Ingenieria en Ciencias y Sistemas, USAC

Sistema de control para misiones de rescate y extraccion de recursos en ciudades en conflicto, desarrollado para el curso de Introduccion a la Programacion y Computacion 2.

Descripcion

El sistema recibe archivos de configuracion en formato XML que describen una ciudad como una malla bidimensional de celdas (caminos, puntos de entrada, unidades militares, unidades civiles y recursos), junto con los robots disponibles (ChapinRescue y ChapinFighter). A partir de esta informacion, permite ejecutar dos tipos de mision:

Rescate: un robot ChapinRescue evade unidades militares para rescatar una unidad civil.
Extraccion de recursos: un robot ChapinFighter puede enfrentar unidades militares (si su capacidad de combate lo permite) para extraer un recurso.

El resultado de cada mision se visualiza graficamente mediante Graphviz, generando un mapa con la ruta recorrida resaltada.

Tecnologias
C# (.NET)
Graphviz (generacion de reportes graficos)
Tipos de Dato Abstracto propios (sin colecciones nativas de C#)
Estructura del repositorio
'''
CODIGO/           Codigo fuente del proyecto (C#)
Documentacion/    Ensayo y diagramas del proyecto
'''
Como ejecutar
Abrir CODIGO/Proyecto1/Proyecto1.slnx en Visual Studio.
Compilar y ejecutar (F5).
Usar el menu para cargar un archivo XML de configuracion y ejecutar misiones.
