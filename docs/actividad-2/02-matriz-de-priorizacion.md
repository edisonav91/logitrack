# Matriz de priorización

## Introducción

La priorización permite distinguir los requisitos que responden directamente al problema principal de LogiTrack de aquellos que apoyan el alcance o dependen de información todavía no validada. También ayuda a reconocer qué requisitos pueden influir con mayor fuerza en una futura decisión estructural.

Esta matriz incluye los once requisitos funcionales y los ocho atributos de calidad del catálogo aprobado. Todas las valoraciones son propuestas académicas y deberán revisarse cuando se conozcan datos reales de la operación, reglas de negocio y necesidades de calidad.

## Metodología de priorización

Se utiliza una escala cualitativa de **alto**, **medio** y **bajo** en cuatro dimensiones:

- **Impacto en el negocio:** indica cuánto contribuye el requisito a resolver el problema central, ejecutar el proceso logístico o reducir los riesgos documentados. Es alto cuando responde de forma directa al problema o a un riesgo actual; medio cuando apoya una capacidad del alcance; y bajo cuando corresponde a una interacción complementaria todavía no confirmada.
- **Impacto arquitectónico:** expresa cuánto puede condicionar la organización del sistema. Es alto cuando afecta varias capacidades, datos compartidos, integraciones o cualidades transversales; medio cuando su efecto es más localizado; y bajo cuando puede resolverse sin influir de manera importante en la estructura general.
- **Complejidad estimada:** considera la cantidad de reglas, fuentes, actores, dependencias e incertidumbres involucradas. Una complejidad alta no significa que el requisito sea menos importante; indica que requiere más análisis y validación.
- **Prioridad final:** combina la relación con el problema principal, el valor para el negocio, el impacto arquitectónico, las dependencias y los riesgos. No se calcula mediante cifras que aparenten una precisión inexistente.

La prioridad final se asigna con estas reglas:

1. **Alta:** el requisito responde directamente al problema central o a un riesgo actual de alta atención, es necesario para el flujo principal o constituye una cualidad transversal que debe considerarse desde el inicio.
2. **Media:** el requisito forma parte del alcance y aporta valor, pero depende de reglas, datos o integraciones pendientes, o puede abordarse después del núcleo sin impedir su funcionamiento inicial.
3. **Baja:** el requisito describe una capacidad complementaria o una integración no confirmada que puede aplazarse sin impedir la gestión central de pedidos.

La complejidad no reduce automáticamente la prioridad. Un requisito puede tener prioridad alta y complejidad alta si es indispensable; en ese caso debe analizarse antes para reducir la incertidumbre.

## Matriz de priorización

| Identificador | Nombre | Tipo | Impacto en el negocio | Impacto arquitectónico | Complejidad estimada | Prioridad final | Justificación breve |
| --- | --- | --- | --- | --- | --- | --- | --- |
| RF-001 | Registro de pedidos de distintos canales | Funcional | Alto | Alto | Alta | Alta | Es la entrada del flujo principal y responde a la recepción multicanal y al riesgo de duplicación. La identificación de pedidos y los canales aún deben validarse. |
| RF-002 | Consulta centralizada del pedido | Funcional | Alto | Alto | Media | Alta | Atiende directamente la falta de una vista centralizada, que es el problema principal del caso. Condiciona la forma de reunir y presentar información vigente. |
| RF-003 | Validación de inventario distribuido | Funcional | Alto | Alto | Alta | Alta | Forma parte del alcance aprobado y enfrenta el riesgo de inconsistencias entre inventarios. Depende de fuentes y reglas todavía desconocidas. |
| RF-004 | Asignación de ruta y repartidor | Funcional | Alto | Medio | Media | Alta | Es necesaria para coordinar la entrega y se relaciona con el riesgo de asignaciones incorrectas. Su efecto estructural es más localizado que el de la información central del pedido. |
| RF-005 | Seguimiento del estado de la entrega | Funcional | Alto | Alto | Alta | Alta | Responde a la poca trazabilidad y apoya la identificación de retrasos. Requiere definir estados, transiciones, responsables e historial. |
| RF-006 | Gestión de incidencias | Funcional | Medio | Medio | Baja | Media | Está dentro del alcance, pero el caso no define tipos ni tratamiento. El registro y la consulta iniciales son acotados frente al flujo central. |
| RF-007 | Acceso según el actor | Funcional | Medio | Alto | Media | Alta | No resuelve por sí solo el problema central, pero afecta todas las consultas y actualizaciones de los actores y debe considerarse de forma transversal. |
| RF-008 | Recepción del estado del pago | Funcional | Bajo | Medio | Media | Baja | Es una interacción propuesta y no confirmada. LogiTrack no procesa pagos y esta capacidad puede aplazarse sin impedir el seguimiento logístico principal. |
| RF-009 | Consulta de datos de ubicación | Funcional | Medio | Medio | Media | Media | Puede apoyar el seguimiento de rutas y entregas, pero los datos, la frecuencia y su uso todavía deben validarse. |
| RF-010 | Solicitud de notificaciones | Funcional | Bajo | Medio | Media | Baja | Es una capacidad complementaria dependiente de eventos y destinatarios no definidos. No es necesaria para construir la vista central del pedido. |
| RF-011 | Intercambio de información con ERP o CRM | Funcional | Medio | Alto | Alta | Media | Puede aportar información relevante de pedidos, pero no se conoce cuál sistema existe, qué datos maneja ni el sentido del intercambio. |
| AC-001 | Consistencia e integridad de la información | Atributo de calidad | Alto | Alto | Alta | Alta | Es fundamental para evitar duplicados y contradicciones y para que la vista central represente de manera confiable cada pedido. |
| AC-002 | Trazabilidad | Atributo de calidad | Alto | Alto | Media | Alta | Atiende directamente uno de los problemas actuales y condiciona el registro del historial de estados y cambios relevantes. |
| AC-003 | Vigencia de la información | Atributo de calidad | Alto | Alto | Alta | Alta | La información centralizada debe permitir reconocer su fuente y actualidad; de lo contrario, podría no reflejar el estado real del pedido. |
| AC-004 | Interoperabilidad | Atributo de calidad | Medio | Alto | Alta | Alta | Las fuentes y sistemas externos están fuera del límite de LogiTrack. Aunque los intercambios deben validarse, esta cualidad influye de forma transversal en las alternativas estructurales. |
| AC-005 | Control de acceso | Atributo de calidad | Medio | Alto | Media | Alta | Los actores tienen necesidades diferentes y los permisos afectan todas las operaciones. Debe considerarse desde el inicio, aunque la matriz de permisos aún no exista. |
| AC-006 | Escalabilidad | Atributo de calidad | Medio | Alto | Alta | Media | Puede condicionar la estructura ante un crecimiento de la operación, pero no existen volúmenes actuales ni proyecciones que permitan tratarla como prioridad confirmada. |
| AC-007 | Resiliencia ante intercambios externos | Atributo de calidad | Medio | Alto | Alta | Alta | Los fallos externos no deberían causar pérdida o corrupción de información. Es transversal a las integraciones, aunque las respuestas concretas están pendientes. |
| AC-008 | Rendimiento | Atributo de calidad | Medio | Alto | Alta | Media | La información oportuna puede apoyar la coordinación, pero no se ha demostrado que el rendimiento del sistema cause los retrasos actuales ni existen objetivos medibles. |

## Requisitos más importantes para el problema principal

Los requisitos funcionales **RF-001**, **RF-002** y **RF-005** forman el núcleo de la respuesta al problema. El registro multicanal permite reunir los pedidos, la consulta centralizada ofrece la vista que hoy no existe y el seguimiento permite conocer cómo avanza cada entrega. **RF-003** y **RF-004** completan el flujo al relacionar el pedido con el inventario, la ruta y el repartidor.

Entre los atributos de calidad, **AC-001**, **AC-002** y **AC-003** son los más cercanos al diagnóstico: la información debe ser consistente, trazable y vigente para que la centralización tenga valor. Una vista única con información duplicada, sin historial o desactualizada no resolvería el problema documentado.

**RF-007**, **AC-004**, **AC-005** y **AC-007** también tienen prioridad alta por su efecto transversal. No representan por sí solos el objetivo del negocio, pero condicionan el acceso de los actores, los intercambios externos y el manejo de fallos, por lo que deben considerarse al evaluar alternativas estructurales.

## Dependencias y riesgos que influyen en la priorización

| Dependencia o riesgo | Requisitos relacionados | Influencia en la prioridad |
| --- | --- | --- |
| Identificación del pedido y fuente vigente | RF-001, RF-002, AC-001 y AC-003 | Sin criterios comunes de identificación y vigencia, la centralización puede reproducir la duplicación y las contradicciones del estado actual. |
| Reglas y fuentes de inventario | RF-003, AC-001 y AC-003 | La validación depende de información distribuida cuyo origen, actualidad y reglas todavía no están definidos. |
| Criterios de asignación | RF-004, RF-005 y AC-001 | Las rutas, los repartidores y sus reasignaciones deben mantener una relación coherente con el pedido y la entrega. |
| Estados y transiciones del pedido | RF-002, RF-005, RF-006, AC-002 y AC-003 | El seguimiento y las incidencias requieren un lenguaje común de estados, responsables y cambios para conservar trazabilidad. |
| Permisos de los actores | RF-002 a RF-007 y AC-005 | Los niveles de consulta y actualización no están definidos y afectan la mayoría de las funciones humanas. |
| Acuerdos con sistemas externos | RF-008 a RF-011, AC-004 y AC-007 | La prioridad y complejidad de estas capacidades dependen de confirmar sistemas, datos, direcciones de intercambio y respuestas ante fallos. |
| Volumen y crecimiento de la operación | AC-006 y AC-008 | Sin mediciones actuales ni proyecciones no es posible establecer objetivos verificables de capacidad o rendimiento. |
| R-001: duplicación de información | RF-001, RF-002 y AC-001 | Refuerza la prioridad del registro controlado, la consulta central y la consistencia. |
| R-003 y R-004: retrasos y poca trazabilidad | RF-005, AC-002, AC-003 y AC-008 | Elevan la importancia del seguimiento y del historial, pero la relación entre rendimiento técnico y retrasos debe validarse. |
| R-006: dificultades con sistemas externos | RF-008 a RF-011, AC-004 y AC-007 | Obliga a considerar interoperabilidad y resiliencia antes de definir la forma de integración. |
| R-007: crecimiento sin gestión adecuada | AC-006 y AC-008 | Justifica estudiar capacidad y rendimiento, sin inventar cifras ni asumir un crecimiento que no ha sido confirmado. |
| R-008: información incompleta o desactualizada | RF-002, RF-005, AC-001, AC-003 y AC-007 | Hace prioritario definir fuentes, responsables y mecanismos de verificación de la vigencia. |

## Conclusión

La matriz establece una secuencia razonada sin convertir valoraciones cualitativas en datos exactos. Los requisitos de prioridad alta indican qué capacidades y cualidades deben tratarse primero para responder al problema central y reducir los riesgos más importantes. Los requisitos medios y bajos se mantienen en el catálogo, pero requieren validación adicional o pueden abordarse después del núcleo.

Los atributos de calidad priorizados servirán como base para redactar escenarios con métricas verificables. A su vez, el impacto arquitectónico y las dependencias de esta matriz permitirán comparar alternativas estructurales con criterios comunes, sin anticipar todavía una arquitectura ni seleccionar tecnologías.
