# Análisis del problema

## Contextualización del negocio

El caso corresponde a una empresa de retail digital cuya operación de última milla comprende la recepción de pedidos desde múltiples canales, la administración de inventarios distribuidos y la coordinación de rutas y repartidores. Estas actividades forman parte de un mismo proceso logístico: llevar cada pedido desde su recepción hasta su entrega.

De acuerdo con el enunciado, la empresa no dispone de una vista centralizada del estado real de cada pedido. Como resultado, su operación presenta duplicación de información, retrasos, errores operativos y poca trazabilidad.

## Problema central

La empresa carece de una vista centralizada que permita conocer el estado real de cada pedido dentro de una operación que involucra varios canales de recepción, inventarios distribuidos, rutas y repartidores. Esta carencia dificulta mantener una referencia común sobre el avance de los pedidos y relacionar de manera coherente la información necesaria para su gestión.

Esta formulación describe el problema del negocio y no presupone una solución tecnológica ni una arquitectura específica.

## 1. ¿Qué ocurre hoy?

### Hechos establecidos por el caso

- La empresa recibe pedidos desde múltiples canales.
- Los inventarios se encuentran distribuidos.
- La operación coordina diferentes rutas y repartidores para realizar las entregas.
- No existe una vista centralizada del estado real de cada pedido.
- Se presentan duplicación de información, retrasos, errores operativos y poca trazabilidad.

### Posibles implicaciones

A partir de estos hechos, se puede inferir que la información necesaria para seguir un pedido debe relacionarse a través de varios componentes de la operación. Al no existir una vista centralizada, puede resultar difícil establecer cuál es la información vigente cuando se consulta el estado de un pedido. Esta es una implicación analítica del caso; el enunciado no describe cómo se almacena, intercambia o actualiza actualmente dicha información.

## 2. ¿Por qué es un problema?

### Hechos establecidos por el caso

El caso sitúa en una misma operación la recepción de pedidos desde múltiples canales, los inventarios distribuidos y la coordinación de rutas y repartidores. También establece que no existe una vista centralizada del estado real de cada pedido y que se presentan problemas operativos.

### Posibles implicaciones

La falta de una referencia común puede dificultar la coordinación entre las actividades de la operación y la verificación del avance de los pedidos. También puede generar incertidumbre sobre la vigencia o consistencia de la información disponible. El caso no permite determinar qué actividades concretas originan estas dificultades ni con qué frecuencia ocurren.

## 3. ¿Qué consecuencias tiene?

### Consecuencias establecidas por el caso

- **Duplicación de información:** existen datos repetidos dentro de la gestión de los pedidos.
- **Retrasos:** la operación presenta demoras en el proceso logístico.
- **Errores operativos:** se producen fallos durante la gestión de la operación.
- **Poca trazabilidad:** resulta insuficiente la capacidad de seguir el estado de los pedidos a lo largo del proceso.

### Posibles implicaciones

Estas consecuencias pueden limitar la capacidad de coordinar oportunamente pedidos, inventarios, rutas y repartidores. Asimismo, pueden dificultar la reconstrucción del recorrido de un pedido y la identificación del punto de la operación en el que se produjo un retraso o un error. No es posible cuantificar su impacto con la información suministrada.

## 4. ¿Qué variables importan?

Para analizar el problema sin asumir un diseño de solución, deben considerarse las siguientes variables conceptuales:

| Variable | Relevancia para el diagnóstico |
| --- | --- |
| Pedido | Es la unidad principal cuyo estado debe conocerse y seguirse durante la operación. |
| Canal de recepción | Permite reconocer que los pedidos ingresan desde múltiples orígenes. |
| Estado real del pedido | Expresa la situación vigente del pedido y constituye el centro del problema planteado. |
| Inventario distribuido | Representa la disponibilidad de inventario que debe considerarse dentro de la gestión del pedido. |
| Ruta | Relaciona el pedido con la coordinación de su entrega. |
| Repartidor | Identifica al participante encargado de realizar la entrega dentro del caso. |
| Información duplicada | Permite observar una de las dificultades actuales de la operación. |
| Retrasos | Permiten examinar las demoras presentes en el proceso logístico. |
| Errores operativos | Permiten reconocer fallos asociados con la gestión actual. |
| Trazabilidad | Permite evaluar la capacidad de seguir cada pedido a lo largo del proceso. |

Estas variables se derivan directamente de los elementos mencionados en el caso. El enunciado no proporciona valores, frecuencias, responsables detallados ni indicadores cuantitativos para ellas; por tanto, su medición y definición operativa deberán precisarse en etapas posteriores.

## Síntesis

El diagnóstico evidencia una brecha entre la complejidad de la operación logística y la capacidad actual de consultar de forma centralizada el estado real de los pedidos. Los hechos disponibles permiten identificar el problema y sus consecuencias generales, pero no permiten atribuir causas específicas a procesos, personas o herramientas. Cualquier análisis posterior deberá conservar esta distinción y validar las implicaciones planteadas antes de convertirlas en requisitos o decisiones.
