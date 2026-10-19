# Registro de decisiones iniciales

## Introducción

Este registro documenta las decisiones iniciales tomadas para delimitar LogiTrack durante la Actividad 1. El caso del profesor establece el contexto de una operación logística de última milla, los problemas actuales y los actores relacionados. A partir de esos elementos, el proyecto propone un alcance funcional que todavía debe validarse.

Las decisiones incluidas aquí no definen la arquitectura interna ni las tecnologías de la plataforma. Su finalidad es dejar claro qué se abordará, por qué se considera necesario y qué efectos puede tener esa delimitación.

## Decisiones iniciales

| Identificador | Decisión | Justificación | Consecuencias |
| --- | --- | --- | --- |
| DEC-001 | Enfocar LogiTrack en la logística de última milla. | El caso del profesor se centra en la gestión de pedidos hasta su entrega. La decisión del proyecto es mantener ese foco y no ampliar el alcance a toda la operación del retail. | Permite concentrar el análisis en pedidos, inventario relacionado, rutas, repartidores y entregas. Otros procesos comerciales quedan fuera de la delimitación inicial. |
| DEC-002 | Proponer una vista centralizada de la información de los pedidos. | El caso establece que actualmente no existe una vista centralizada del estado real de cada pedido y relaciona esta carencia con duplicación, retrasos, errores y poca trazabilidad. | Los actores podrían consultar una referencia común sobre el pedido. Se deberán validar las fuentes, los estados y los responsables de actualizar la información. Esta decisión es funcional y no implica una forma específica de almacenamiento o despliegue. |
| DEC-003 | Incluir la validación de inventario distribuido en el alcance inicial. | El caso indica que la empresa administra inventarios distribuidos. El proyecto propone considerar la información de inventario necesaria para gestionar cada pedido. | Será necesario precisar qué significa validar inventario, qué datos se consultan y cuál es su fuente. LogiTrack no sustituirá la gestión completa de almacenes. |
| DEC-004 | Incluir la coordinación de rutas y repartidores. | El caso establece que la empresa coordina entregas mediante diferentes rutas y repartidores. La propuesta incorpora su asignación y consulta dentro de la gestión del pedido. | Los pedidos podrán relacionarse con una ruta y un repartidor. Quedan pendientes los criterios, responsables y reglas de asignación; tampoco se asume la optimización automática de rutas. |
| DEC-005 | Contemplar el seguimiento de entregas y la gestión de incidencias. | El seguimiento responde al problema de poca trazabilidad indicado en el caso. La gestión de incidencias forma parte de la propuesta funcional inicial, aunque el caso no define sus tipos ni su tratamiento. | Se deberán definir y validar los estados de entrega, las formas de actualización y las incidencias que se manejarán. No se asumen tiempos de actualización ni reglas de atención. |
| DEC-006 | Mantener los sistemas externos fuera del límite de LogiTrack. | El sistema de pagos, el servicio de geolocalización, el sistema de notificaciones y el ERP o CRM interno cumplen funciones propias. La propuesta es que LogiTrack se relacione con ellos sin reemplazarlos. | Los posibles intercambios se representan como parte del contexto, pero deben validarse. LogiTrack no asumirá las responsabilidades internas de esos sistemas. |
| DEC-007 | Posponer las decisiones de arquitectura y tecnología. | La Actividad 1 está enfocada en el diagnóstico y el contexto arquitectónico. Todavía no hay información suficiente sobre requisitos detallados o necesidades de calidad. | En esta etapa no se define la organización interna del software, el almacenamiento, los frameworks ni la infraestructura. Estas decisiones se evaluarán en actividades posteriores con mayor información. |

## Aspectos pendientes de decidir

Antes de convertir estas decisiones de alcance en requisitos o decisiones técnicas, se deben resolver los siguientes aspectos:

- Los canales desde los que se reciben pedidos y la información que aporta cada uno.
- La fuente que se considerará vigente para consultar el estado real de un pedido.
- Los estados que forman parte del seguimiento y los actores autorizados para consultarlos o actualizarlos.
- Las reglas y fuentes de información para validar el inventario distribuido.
- Los criterios y responsables de asignar rutas y repartidores.
- Los tipos de incidencias que se gestionarán y la forma en que se atenderán.
- Los datos que se intercambiarán con cada sistema externo y el sentido de esos intercambios.
- Los permisos y el nivel de información disponible para cada actor humano.
- Los requisitos de calidad y las restricciones del proyecto que puedan orientar decisiones posteriores.
- La arquitectura, las tecnologías y la infraestructura, que se estudiarán en las siguientes actividades y no forman parte de este registro inicial.

Estas cuestiones permanecen abiertas porque el caso no ofrece el detalle necesario para resolverlas sin introducir supuestos adicionales.
