# Escenarios de calidad

## Introducción

Los escenarios de calidad convierten atributos generales, como trazabilidad o interoperabilidad, en situaciones concretas que pueden comprobarse. Esto permite comparar alternativas estructurales con la misma evidencia y evita tomar decisiones basadas solamente en descripciones abstractas.

Los escenarios se presentan en el orden definido por la matriz de priorización: primero aparecen los atributos de prioridad alta y luego los de prioridad media. **Todos los valores numéricos de este documento son objetivos académicos propuestos y pendientes de validación.** No representan mediciones, capacidades actuales ni compromisos reales de la empresa.

## Estructura de los escenarios

Cada escenario utiliza los siguientes elementos:

- **Fuente del estímulo:** persona, sistema o prueba que origina la situación.
- **Estímulo o situación:** evento que LogiTrack debe atender.
- **Entorno o condiciones:** contexto en el que ocurre el evento.
- **Elemento afectado:** capacidad de LogiTrack sobre la que actúa el estímulo, sin definir componentes internos.
- **Respuesta esperada:** comportamiento observable que debería presentar el sistema.
- **Métrica o umbral:** objetivo concreto utilizado para evaluar la respuesta.
- **Método de verificación:** prueba, inspección o evidencia con la que se comprobará el resultado.
- **Requisitos y riesgos relacionados:** trazabilidad con el catálogo y el registro de riesgos.

Los umbrales permiten realizar una comparación académica reproducible. Deberán ajustarse cuando se conozcan los volúmenes, procesos, reglas y necesidades reales de la operación.

## EC-001 — Consistencia ante pedidos duplicados o contradictorios

| Elemento | Descripción |
| --- | --- |
| **Identificador** | EC-001 |
| **Prioridad** | Alta |
| **Atributo de calidad relacionado** | AC-001 — Consistencia e integridad de la información |
| **Fuente del estímulo** | Canales de pedidos o un sistema externo autorizado. |
| **Estímulo o situación** | Se reciben varias veces los datos de un mismo pedido o llegan datos contradictorios para un pedido identificado con las reglas acordadas. |
| **Entorno o condiciones** | Operación normal, incluyendo reintentos y recepción concurrente desde más de una fuente. Las reglas de identificación ya fueron validadas. |
| **Elemento del sistema afectado** | Registro y representación centralizada del pedido. |
| **Respuesta esperada** | LogiTrack relaciona los duplicados con un mismo pedido lógico, conserva la fuente de la información y señala los conflictos sin crear estados activos contradictorios. |
| **Métrica o umbral verificable** | **Objetivo académico propuesto y pendiente de validación:** en un conjunto sintético de 1.000 mensajes que incluya 100 duplicados exactos y 50 casos con datos contradictorios, el 100 % de los duplicados debe asociarse con el pedido correcto, el 100 % de los conflictos debe identificarse y deben existir 0 pedidos lógicos activos duplicados. |
| **Método de verificación** | Prueba funcional con datos sintéticos, seguida de la inspección de los pedidos registrados, sus fuentes y los conflictos detectados. |
| **Requisitos y riesgos relacionados** | RF-001, RF-002, RF-003 y RF-011; R-001, R-002 y R-008. |
| **Justificación del umbral** | Una vez acordadas las reglas de identificación, la asociación de duplicados y la detección de conflictos deben ser deterministas. La cantidad propuesta permite incluir variedad de casos sin presentarla como volumen real. |

## EC-002 — Reconstrucción del historial de un pedido

| Elemento | Descripción |
| --- | --- |
| **Identificador** | EC-002 |
| **Prioridad** | Alta |
| **Atributo de calidad relacionado** | AC-002 — Trazabilidad |
| **Fuente del estímulo** | Un actor autorizado o un sistema externo validado. |
| **Estímulo o situación** | Se registra un cambio de estado, una asignación o reasignación, o una incidencia relacionada con un pedido. |
| **Entorno o condiciones** | Operación normal con pedidos previamente registrados y reglas de actualización definidas. |
| **Elemento del sistema afectado** | Seguimiento e historial del pedido y de su entrega. |
| **Respuesta esperada** | LogiTrack conserva cada cambio con el pedido relacionado, el orden temporal, la fuente o actor y los datos necesarios para reconstruir lo ocurrido. |
| **Métrica o umbral verificable** | **Objetivo académico propuesto y pendiente de validación:** al ejecutar 200 cambios conocidos sobre un conjunto de pedidos de prueba, el 100 % debe aparecer en el historial correcto, en orden temporal y sin eventos faltantes. |
| **Método de verificación** | Comparación automatizada entre la secuencia de cambios ejecutada y el historial consultado, complementada con una inspección de la evidencia de actor, fuente y momento del cambio. |
| **Requisitos y riesgos relacionados** | RF-004, RF-005 y RF-006; R-004 y R-005. |
| **Justificación del umbral** | La trazabilidad exige que los cambios registrados puedan reconstruirse por completo. Los 200 eventos son una muestra académica suficiente para probar diferentes tipos y secuencias, no una estimación de uso real. |

## EC-003 — Vigencia de una actualización del pedido

| Elemento | Descripción |
| --- | --- |
| **Identificador** | EC-003 |
| **Prioridad** | Alta |
| **Atributo de calidad relacionado** | AC-003 — Vigencia de la información |
| **Fuente del estímulo** | Un canal, un repartidor o un sistema externo autorizado. |
| **Estímulo o situación** | LogiTrack acepta una actualización del estado o de la información relacionada con un pedido. |
| **Entorno o condiciones** | Operación normal, con el intercambio disponible y la fuente de cada dato definida. |
| **Elemento del sistema afectado** | Consulta centralizada del pedido. |
| **Respuesta esperada** | La consulta refleja la actualización e identifica su fuente y momento. Si el objetivo de actualización no se cumple, la información se muestra como pendiente o posiblemente desactualizada. |
| **Métrica o umbral verificable** | **Objetivo académico propuesto y pendiente de validación:** en 200 actualizaciones de prueba, al menos el 95 % debe ser visible en la consulta central en un máximo de 30 segundos; el 100 % debe mostrar su fuente y momento de actualización, y todo caso que supere los 30 segundos debe quedar identificado como pendiente o desactualizado. |
| **Método de verificación** | Prueba controlada que compare el momento de aceptación de cada actualización con el momento en que aparece en la consulta y revise las marcas de fuente y vigencia. |
| **Requisitos y riesgos relacionados** | RF-002, RF-003, RF-005, RF-009 y RF-011; R-003 y R-008. |
| **Justificación del umbral** | Treinta segundos ofrece un punto inicial para evaluar si la vista puede considerarse oportuna sin afirmar que el negocio exige tiempo real. El porcentaje permite observar casos excepcionales, que deben quedar visibles y no ocultarse. |

## EC-004 — Intercambio válido e inválido con un sistema externo

| Elemento | Descripción |
| --- | --- |
| **Identificador** | EC-004 |
| **Prioridad** | Alta |
| **Atributo de calidad relacionado** | AC-004 — Interoperabilidad |
| **Fuente del estímulo** | Sistema de pagos, servicio de geolocalización, sistema de notificaciones o ERP o CRM cuyo intercambio haya sido validado. |
| **Estímulo o situación** | El sistema externo envía o recibe información válida y también se presentan casos con datos incompletos, mal formados o no reconocidos. |
| **Entorno o condiciones** | Prueba controlada con el contenido y las reglas del intercambio previamente acordados. |
| **Elemento del sistema afectado** | Límite de intercambio entre LogiTrack y el sistema externo. |
| **Respuesta esperada** | LogiTrack acepta la información válida y la relaciona con el pedido correcto; los casos inválidos se rechazan o se marcan con un resultado verificable, sin alterar información válida existente. |
| **Métrica o umbral verificable** | **Objetivo académico propuesto y pendiente de validación:** por cada intercambio externo confirmado, ejecutar al menos 20 casos válidos y 20 inválidos; el 100 % de los válidos debe aceptarse y relacionarse correctamente, el 100 % de los inválidos debe detectarse y deben producirse 0 alteraciones parciales de datos válidos. |
| **Método de verificación** | Pruebas de contrato con conjuntos de datos válidos e inválidos e inspección del resultado del intercambio y del estado final del pedido. |
| **Requisitos y riesgos relacionados** | RF-008, RF-009, RF-010 y RF-011; R-006 y R-008. |
| **Justificación del umbral** | Una vez acordado un intercambio, aceptar o rechazar cada caso debe ser verificable y determinista. La cantidad de pruebas busca cubrir variaciones básicas sin asumir el tráfico real de los sistemas externos. |

## EC-005 — Aplicación de permisos por tipo de actor

| Elemento | Descripción |
| --- | --- |
| **Identificador** | EC-005 |
| **Prioridad** | Alta |
| **Atributo de calidad relacionado** | AC-005 — Control de acceso |
| **Fuente del estímulo** | Cliente, operador de comercio o tienda, administrador de almacén, repartidor o soporte al cliente. |
| **Estímulo o situación** | Un actor intenta consultar o actualizar información permitida y también intenta ejecutar una acción no autorizada para su función. |
| **Entorno o condiciones** | Matriz de actores y permisos previamente validada, con usuarios de prueba para cada tipo de actor. |
| **Elemento del sistema afectado** | Acceso a consultas y actualizaciones de pedidos, inventario, asignaciones, entregas e incidencias. |
| **Respuesta esperada** | LogiTrack permite las acciones autorizadas, rechaza las no autorizadas y evita que un intento rechazado modifique la información. |
| **Métrica o umbral verificable** | **Objetivo académico propuesto y pendiente de validación:** probar todas las combinaciones de actor y acción definidas en la matriz de permisos; el 100 % de las acciones permitidas debe completarse, el 100 % de las acciones prohibidas debe rechazarse y debe haber 0 modificaciones producidas por intentos rechazados. |
| **Método de verificación** | Pruebas sistemáticas de autorización para cada combinación de actor y acción, con inspección del resultado y de la información antes y después de cada intento. |
| **Requisitos y riesgos relacionados** | RF-002 a RF-007; supuesto de permisos de la Actividad 1. No existe un riesgo específico registrado; de forma indirecta se relaciona con R-008. |
| **Justificación del umbral** | Las reglas de acceso, una vez aprobadas, no admiten excepciones silenciosas. Por eso se propone cubrir toda la matriz y exigir resultados exactos, sin asumir todavía cuántos roles o permisos existirán. |

## EC-006 — Falla durante un intercambio externo

| Elemento | Descripción |
| --- | --- |
| **Identificador** | EC-006 |
| **Prioridad** | Alta |
| **Atributo de calidad relacionado** | AC-007 — Resiliencia ante intercambios externos |
| **Fuente del estímulo** | Un sistema externo o una prueba que simula su falla. |
| **Estímulo o situación** | El sistema externo no está disponible, demora su respuesta, interrumpe el intercambio o devuelve una respuesta repetida. |
| **Entorno o condiciones** | LogiTrack ya conserva información válida del pedido y ocurre una falla durante un intercambio confirmado. |
| **Elemento del sistema afectado** | Intercambio externo y estado relacionado del pedido. |
| **Respuesta esperada** | LogiTrack identifica el fallo, conserva la información existente, deja un resultado pendiente o fallido verificable y permite reintentar cuando corresponda sin producir efectos duplicados. |
| **Métrica o umbral verificable** | **Objetivo académico propuesto y pendiente de validación:** ejecutar 100 intercambios con fallas simuladas; el 100 % debe quedar identificado como pendiente o fallido, deben ocurrir 0 pérdidas o corrupciones de información y, al restablecer el sistema externo, cada operación reintentable debe terminar una sola vez o conservar un estado final de error explícito. |
| **Método de verificación** | Pruebas de inyección de fallos mediante simulaciones de indisponibilidad, demora, interrupción y repetición; comparación de la información antes, durante y después de la recuperación. |
| **Requisitos y riesgos relacionados** | RF-008, RF-009, RF-010 y RF-011; R-006 y R-008. |
| **Justificación del umbral** | La integridad ante una falla debe poder comprobarse de forma exacta. Los 100 casos permiten repetir diferentes condiciones de error sin presentarlos como frecuencia real de fallos. |

## EC-007 — Aumento controlado de la carga

| Elemento | Descripción |
| --- | --- |
| **Identificador** | EC-007 |
| **Prioridad** | Media |
| **Atributo de calidad relacionado** | AC-006 — Escalabilidad |
| **Fuente del estímulo** | Prueba de carga con operaciones sintéticas de registro, consulta y actualización de pedidos. |
| **Estímulo o situación** | La carga concurrente se duplica con respecto a una línea base controlada. |
| **Entorno o condiciones** | Dos ejecuciones en un entorno reproducible. La línea base utiliza 100 operaciones concurrentes y un presupuesto abstracto de recursos **R**, entendido como la capacidad de procesamiento y memoria disponible para LogiTrack. La segunda ejecución utiliza 200 operaciones concurrentes y un presupuesto **2R** con recursos de las mismas características. Se mantienen iguales la mezcla de operaciones, los datos, la duración medida después del calentamiento y el comportamiento simulado de los sistemas externos. No se cambia ninguna otra condición entre las ejecuciones. |
| **Elemento del sistema afectado** | Capacidades generales de registro, consulta y seguimiento de pedidos. |
| **Respuesta esperada** | Al duplicar de manera proporcional la carga y los recursos disponibles, LogiTrack aumenta la cantidad de operaciones exitosas atendidas por segundo, mantiene tiempos de respuesta cercanos a la línea base y no pierde ni corrompe información aceptada. |
| **Métrica o umbral verificable** | **Objetivo académico propuesto y pendiente de validación:** al pasar de 100 operaciones concurrentes con recursos R a 200 con recursos 2R, el throughput de operaciones exitosas por segundo debe alcanzar al menos 1,8 veces el valor de la línea base; el percentil 95 del tiempo de respuesta no debe superar 1,2 veces el valor base; la tasa de errores técnicos debe permanecer por debajo del 1 % y deben producirse 0 pérdidas o corrupciones de operaciones aceptadas. |
| **Método de verificación** | Prueba de carga repetible que compare, bajo las condiciones de recursos definidas, las operaciones exitosas por segundo, el percentil 95 del tiempo de respuesta, la tasa de errores, la integridad de las operaciones aceptadas y el uso de recursos en ambas ejecuciones. |
| **Requisitos y riesgos relacionados** | RF-001, RF-002 y RF-005; R-007. |
| **Justificación del umbral** | La comparación proporcional evita exigir más throughput con recursos fijos. El objetivo de 1,8 veces el throughput acepta una pérdida de eficiencia por coordinación, mientras que el límite de 1,2 veces para el percentil 95 evita que el crecimiento se obtenga a costa de una degradación elevada. Estos factores permiten contrastar alternativas sin presentarlos como capacidad real del negocio. |

## EC-008 — Tiempo de respuesta de operaciones principales

| Elemento | Descripción |
| --- | --- |
| **Identificador** | EC-008 |
| **Prioridad** | Media |
| **Atributo de calidad relacionado** | AC-008 — Rendimiento |
| **Fuente del estímulo** | Actores autorizados representados mediante una prueba de rendimiento. |
| **Estímulo o situación** | Se realizan operaciones de consulta, registro y actualización de pedidos. |
| **Entorno o condiciones** | Entorno de prueba controlado con 100 operaciones concurrentes y datos sintéticos. No se considera una representación del volumen real de la empresa. |
| **Elemento del sistema afectado** | Consulta centralizada, registro y actualización del pedido. |
| **Respuesta esperada** | LogiTrack completa las operaciones válidas y entrega un resultado dentro del tiempo objetivo sin errores técnicos. |
| **Métrica o umbral verificable** | **Objetivo académico propuesto y pendiente de validación:** sobre un conjunto de al menos 1.000 operaciones, el percentil 95 de las consultas debe ser igual o menor a 2 segundos, el percentil 95 de los registros y actualizaciones debe ser igual o menor a 3 segundos y la tasa de errores técnicos debe ser menor al 1 %. |
| **Método de verificación** | Prueba de rendimiento que registre tiempos individuales, percentiles, operaciones completadas y errores, utilizando la misma carga y datos al evaluar cada alternativa. |
| **Requisitos y riesgos relacionados** | RF-001, RF-002 y RF-005; R-003. La relación entre rendimiento técnico y retrasos operativos sigue pendiente de validación. |
| **Justificación del umbral** | Los tiempos propuestos permiten comparar si las operaciones interactivas responden con agilidad, pero no constituyen un nivel de servicio de la empresa. El tamaño de la prueba busca obtener percentiles comparables, no representar tráfico real. |

## Tabla de trazabilidad

| Escenario | Atributo de calidad | Prioridad | Requisitos funcionales relacionados | Riesgos o supuestos relacionados | Evidencia principal |
| --- | --- | --- | --- | --- | --- |
| EC-001 | AC-001 — Consistencia e integridad de la información | Alta | RF-001, RF-002, RF-003 y RF-011 | R-001, R-002 y R-008 | Resultados de pruebas con duplicados y conflictos e inspección de pedidos registrados. |
| EC-002 | AC-002 — Trazabilidad | Alta | RF-004, RF-005 y RF-006 | R-004 y R-005 | Comparación entre cambios ejecutados e historial recuperado. |
| EC-003 | AC-003 — Vigencia de la información | Alta | RF-002, RF-003, RF-005, RF-009 y RF-011 | R-003 y R-008 | Tiempos de propagación y presencia de fuente y momento de actualización. |
| EC-004 | AC-004 — Interoperabilidad | Alta | RF-008, RF-009, RF-010 y RF-011 | R-006 y R-008 | Resultados de pruebas de contrato válidas e inválidas. |
| EC-005 | AC-005 — Control de acceso | Alta | RF-002 a RF-007 | Supuesto de permisos; relación indirecta con R-008. | Resultados para cada combinación de actor y acción. |
| EC-006 | AC-007 — Resiliencia ante intercambios externos | Alta | RF-008, RF-009, RF-010 y RF-011 | R-006 y R-008 | Estado de los intercambios y comparación de datos durante fallos y recuperación. |
| EC-007 | AC-006 — Escalabilidad | Media | RF-001, RF-002 y RF-005 | R-007 | Comparación de carga, recursos, throughput, percentil 95, errores e integridad. |
| EC-008 | AC-008 — Rendimiento | Media | RF-001, RF-002 y RF-005 | R-003; relación causal pendiente. | Tiempos, percentiles, operaciones completadas y errores. |

## Uso de los escenarios en la comparación estructural

Durante la Actividad 2 las alternativas estructurales se compararán mediante análisis documental y argumentos técnicos. Para cada escenario se estudiará cómo una alternativa favorece o dificulta la respuesta esperada, qué mecanismos requeriría y qué complejidad, riesgos y trade-offs introduciría. No se presentarán resultados de pruebas porque todavía no existe una implementación sobre la cual ejecutarlas.

Los escenarios de prioridad alta tendrán mayor influencia en este análisis porque responden al problema central o a cualidades transversales. Los escenarios de escalabilidad y rendimiento, aunque tienen prioridad media, servirán para discutir cómo podría comportarse cada alternativa sin afirmar que los objetivos ya fueron alcanzados.

Los métodos de verificación y las métricas quedan definidos como criterios para etapas futuras. Cuando exista una implementación, deberán ejecutarse con los mismos datos y condiciones sobre las alternativas o soluciones que se evalúen, y entonces podrá registrarse si cada umbral **se cumple**, **se cumple parcialmente** o **no se cumple**. Hasta ese momento, los umbrales orientan la comparación y no constituyen evidencia de resultados reales.
