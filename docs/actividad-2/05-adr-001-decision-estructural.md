# ADR-001 — Decisión estructural inicial de LogiTrack

## Estado

**Aceptado para el diseño académico inicial, sujeto a revisión si cambian los supuestos.**

## Contexto y problema

LogiTrack busca apoyar la logística de última milla de una empresa de retail digital que recibe pedidos desde múltiples canales, administra inventarios distribuidos y coordina rutas y repartidores. El problema principal documentado es la ausencia de una vista centralizada del estado real de cada pedido, situación relacionada con duplicación de información, retrasos, errores operativos y poca trazabilidad.

El alcance funcional inicial comprende el registro y la consulta de pedidos, la validación de inventario, la asignación de rutas y repartidores, el seguimiento de entregas, la gestión de incidencias, el acceso según el actor y posibles intercambios con sistemas externos. Estos intercambios aún deben confirmarse y los sistemas de pagos, geolocalización, notificaciones y ERP o CRM permanecen fuera del límite de LogiTrack.

La decisión estructural debe permitir avanzar en el diseño académico sin convertir en hechos los supuestos que todavía no han sido validados. En particular, no se conocen los volúmenes reales de operación, las proyecciones de crecimiento, las reglas detalladas del negocio, los contratos con sistemas externos ni la capacidad operativa del equipo. Tampoco existe una implementación sobre la cual comprobar los escenarios de calidad.

## Factores que influyen en la decisión

La decisión considera los siguientes factores:

- La consulta centralizada del pedido, el registro multicanal, la validación de inventario y el seguimiento de la entrega son capacidades centrales de LogiTrack.
- RF-001 a RF-005 y RF-007 tienen prioridad alta. RF-006, RF-009 y RF-011 tienen prioridad media, mientras que RF-008 y RF-010 tienen prioridad baja en la matriz aprobada.
- AC-001 a AC-005 y AC-007 tienen prioridad alta porque la consistencia, la trazabilidad, la vigencia, la interoperabilidad, el control de acceso y la resiliencia influyen de manera transversal en la solución.
- AC-006 y AC-008 tienen prioridad media. La escalabilidad y el rendimiento son relevantes, pero no existen volúmenes o mediciones reales que demuestren la necesidad de distribuir capacidades desde el inicio.
- EC-001 a EC-006 son escenarios de prioridad alta. EC-007 y EC-008 son de prioridad media y sus umbrales son objetivos académicos propuestos, no resultados obtenidos.
- Los riesgos R-001, R-002, R-004 y R-008 exigen prestar atención a la identificación, integridad, trazabilidad y vigencia de la información del pedido.
- R-006 obliga a aislar y controlar los intercambios con sistemas externos, aunque esos intercambios todavía no están confirmados.
- R-007 requiere considerar el crecimiento futuro sin suponer cargas o necesidades de escalado que no han sido proporcionadas.
- La comparación aprobada concluye de manera preliminar que una estructura unificada reduce la complejidad inicial de coordinación, pero advierte sobre el acoplamiento, el despliegue conjunto y el radio de impacto de las fallas.

## Alternativas consideradas

### Alternativa A: monolito modular

Una sola aplicación desplegable, organizada internamente en módulos con responsabilidades y límites explícitos. La colaboración entre capacidades ocurre dentro de la misma unidad, sin eliminar la separación lógica entre ellas.

Esta alternativa ofrece una coordinación inicial más directa para construir la vista central del pedido y evita introducir desde el comienzo los problemas propios de la comunicación distribuida. A cambio, todos los módulos comparten la unidad de despliegue, el escalado es menos granular y una falla grave puede afectar una parte amplia de LogiTrack.

### Alternativa B: arquitectura distribuida por servicios

Un conjunto de servicios separados por capacidades o dominios, con comunicación entre ellos y posibilidad de despliegue independiente.

Esta alternativa podría ofrecer mayor independencia de evolución, despliegue y escalado. Sin embargo, introduce desde el inicio coordinación remota, fallos parciales y mayor dificultad para mantener una vista consistente, vigente y trazable del pedido. Esas dificultades tendrían que asumirse cuando todavía no se conocen con suficiente claridad los límites del negocio, la carga ni las necesidades de independencia.

## Decisión adoptada

LogiTrack iniciará su diseño académico con una **arquitectura de monolito modular**. El sistema se considerará una sola unidad de despliegue, dividida internamente por capacidades. Esta decisión se limita a la estructura inicial y no selecciona tecnologías, mecanismos de almacenamiento ni infraestructura.

Se proponen los siguientes límites modulares preliminares:

| Módulo preliminar | Responsabilidad principal | Requisitos relacionados |
| --- | --- | --- |
| Gestión de pedidos | Registrar pedidos, conservar su origen y ofrecer la información central necesaria para su consulta. | RF-001 y RF-002. |
| Inventario | Aplicar las reglas acordadas para validar la información de inventario relacionada con un pedido. | RF-003. |
| Asignaciones y entregas | Relacionar pedidos con rutas y repartidores y gestionar el estado de la entrega. | RF-004 y RF-005. |
| Incidencias | Registrar y consultar incidencias vinculadas con pedidos o entregas. | RF-006. |
| Acceso | Aplicar las reglas de consulta y actualización autorizadas para cada actor. | RF-007. |
| Integraciones externas | Proteger el núcleo de LogiTrack de las particularidades de los intercambios confirmados con pagos, geolocalización, notificaciones y ERP o CRM. | RF-008 a RF-011. |

Estos límites son una guía inicial y no constituyen un diseño de dominio definitivo. Podrán cambiar cuando se validen las reglas, fuentes, responsables y flujos reales del negocio.

La organización modular seguirá estos principios:

- Cada módulo tendrá una responsabilidad principal y será responsable de las reglas y la información que se le asignen.
- La colaboración entre módulos se realizará mediante interfaces o contratos internos explícitos.
- Un módulo no deberá modificar directamente la información que pertenezca a otro ni omitir sus reglas.
- Las dependencias deberán ser conocidas, justificadas y evitar ciclos que dificulten la evolución independiente de los módulos.
- Las políticas transversales, como control de acceso, trazabilidad y tratamiento de errores, deberán aplicarse de forma coherente sin borrar los límites de responsabilidad.
- Los intercambios con sistemas externos se mantendrán detrás de límites definidos para evitar que sus formatos o fallas se propaguen sin control al resto de la aplicación.
- La separación modular deberá poder verificarse durante el diseño y la implementación mediante revisión de dependencias y pruebas apropiadas, que se concretarán en actividades posteriores.

## Justificación

La decisión se fundamenta en la comparación estructural aprobada y en la prioridad actual de los requisitos y escenarios, no en una preferencia por una tecnología.

El monolito modular permite abordar RF-001 a RF-005 como partes coordinadas del flujo principal sin introducir comunicaciones distribuidas antes de comprender completamente las reglas del negocio. También permite aplicar RF-007 mediante una política común, aunque cada módulo seguirá siendo responsable de comprobar las acciones que expone. Para RF-008 a RF-011, el módulo de integraciones podrá separar los sistemas externos del resto de LogiTrack; esto no confirma que todos esos intercambios deban implementarse.

La relación con los escenarios de calidad es la siguiente:

| Escenario | Relación con la decisión |
| --- | --- |
| **EC-001 — Consistencia ante pedidos duplicados o contradictorios** | Una ejecución unificada facilita coordinar las reglas de identificación y los cambios relacionados con un pedido. No garantiza la consistencia: deberán definirse las fuentes, reglas y responsabilidades, e impedirse que los módulos las eludan. |
| **EC-002 — Reconstrucción del historial de un pedido** | Se puede aplicar un criterio común para registrar cambios de estado, asignaciones e incidencias. Será necesario evitar que el historial quede disperso o acoplado de forma indebida a todos los módulos. |
| **EC-003 — Vigencia de una actualización del pedido** | La colaboración interna puede reducir demoras de propagación entre capacidades, pero los datos externos aún pueden llegar tarde. Deberán conservarse la fuente, el momento y la condición pendiente o desactualizada. |
| **EC-004 — Intercambio válido e inválido con un sistema externo** | Un límite de integraciones puede concentrar la validación de los intercambios y proteger los demás módulos. Los contratos y sistemas involucrados siguen pendientes de confirmación. |
| **EC-005 — Aplicación de permisos por tipo de actor** | Una política común puede favorecer controles coherentes. Esto no garantiza la seguridad: deberán validarse los permisos y comprobarse que ninguna ruta interna omita la autorización. |
| **EC-006 — Falla durante un intercambio externo** | Los intercambios podrán aislarse para conservar la información válida y hacer visibles los resultados pendientes o fallidos. Al compartir despliegue, un aislamiento deficiente puede afectar una parte amplia del sistema. |
| **EC-007 — Aumento controlado de la carga** | La aplicación podrá aumentar sus recursos como unidad y evaluarse bajo las condiciones R y 2R definidas en el escenario. El escalado será menos granular y solo una prueba futura permitirá saber si alcanza los objetivos propuestos. |
| **EC-008 — Tiempo de respuesta de operaciones principales** | La comunicación interna evita parte de la demora de las llamadas remotas. Sin embargo, la competencia por recursos compartidos puede afectar los tiempos y deberá comprobarse mediante pruebas futuras. |

La arquitectura distribuida podría ofrecer ventajas si algunas capacidades necesitaran evolucionar, desplegarse o escalar de manera independiente. Por ahora no existe evidencia operativa que compense la complejidad adicional de mantener consistencia, trazabilidad, vigencia, seguridad y recuperación entre servicios. Por eso se acepta el monolito modular como punto de partida revisable.

## Beneficios esperados

- Menor complejidad inicial de desarrollo y operación frente a una solución distribuida.
- Coordinación más directa de la información necesaria para la vista central del pedido.
- Menos puntos de falla y comunicaciones remotas entre las capacidades internas.
- Aplicación más uniforme de criterios de trazabilidad, acceso y tratamiento de errores.
- Posibilidad de aprender y ajustar los límites del negocio antes de considerar separaciones físicas.
- Organización interna que puede facilitar el mantenimiento si se respetan los límites y contratos modulares.

Estos beneficios son resultados esperados de la estructura elegida. No prueban el cumplimiento de los requisitos ni de las métricas de calidad.

## Costos, limitaciones y consecuencias negativas aceptadas

- Todos los módulos forman parte de una misma unidad de despliegue; un cambio obliga a desplegar la aplicación completa.
- Una falla grave, un consumo excesivo de recursos o un despliegue defectuoso puede tener un radio de impacto mayor que en capacidades físicamente aisladas.
- El escalado se realiza principalmente sobre la unidad completa, incluso si la demanda se concentra en una sola capacidad.
- Los módulos comparten recursos de ejecución y podrían interferir entre sí en rendimiento o estabilidad.
- La independencia de evolución depende de conservar límites internos; compartir despliegue no evita el acoplamiento.
- Si en el futuro se requiere separar un módulo, la transición tendrá un costo de diseño, adaptación de contratos y migración.
- La resiliencia frente a sistemas externos exige mecanismos explícitos; una única unidad desplegable no la proporciona automáticamente.

Estas consecuencias se aceptan porque, con la información actual, introducir una distribución temprana generaría una complejidad mayor sin una necesidad operativa confirmada.

## Riesgos y medidas de mitigación

| Riesgo de la alternativa elegida | Posible efecto | Medida propuesta |
| --- | --- | --- |
| Pérdida progresiva de los límites modulares. | Acoplamiento alto y dificultad para modificar una capacidad sin afectar otras. | Documentar responsabilidades, revisar dependencias y exigir que la colaboración ocurra mediante contratos internos explícitos. |
| Dependencias cíclicas o acceso directo a información ajena. | Reglas duplicadas, cambios difíciles de controlar e inconsistencias. | Definir una dirección clara para las dependencias, evitar ciclos y asignar un responsable para cada regla e información compartida. |
| Mayor radio de impacto ante fallas o despliegues defectuosos. | Varias capacidades podrían quedar afectadas al mismo tiempo. | Aislar errores por módulo, controlar el uso de recursos, verificar cambios antes del despliegue y preparar mecanismos de recuperación en etapas posteriores. |
| Fallas externas que consumen recursos compartidos. | Demoras o interrupciones en capacidades que no dependen directamente del sistema externo. | Encapsular los intercambios, limitar su propagación, conservar estados pendientes o fallidos y comprobar la recuperación mediante EC-006. |
| Escalado poco granular. | Uso ineficiente de recursos o dificultad para responder a una capacidad con carga diferente. | Medir por capacidad, aplicar EC-007 y revisar la estructura si la evidencia muestra necesidades sostenidas de escalado independiente. |
| Interferencia de rendimiento entre módulos. | Los tiempos de consulta, registro o actualización podrían aumentar. | Observar el consumo por capacidad, aplicar EC-008 y revisar dependencias y responsabilidades antes de considerar una separación física. |
| Política transversal de acceso aplicada de forma incompleta. | Consultas o modificaciones no autorizadas. | Definir la matriz de permisos y verificar todas las combinaciones de actor y acción previstas en EC-005. |
| Supuestos incorrectos sobre reglas, fuentes o integraciones. | Los límites elegidos podrían no representar adecuadamente el negocio. | Mantener trazabilidad con los requisitos, validar los supuestos pendientes y ajustar los límites antes de tratarlos como definitivos. |

## Condiciones para revisar la decisión

El ADR deberá revisarse si aparece evidencia de una o más de las siguientes condiciones:

- una capacidad necesita desplegarse con una frecuencia o calendario independiente y el despliegue conjunto produce impactos repetidos y documentados;
- las mediciones muestran cargas sostenidamente diferentes entre módulos y el escalado de toda la aplicación no permite satisfacer EC-007 de manera razonable;
- la competencia por recursos entre capacidades impide alcanzar EC-008, incluso después de revisar responsabilidades, dependencias y uso de recursos;
- las fallas de una capacidad o integración afectan repetidamente funciones no relacionadas y el aislamiento interno no resulta suficiente para EC-006;
- los límites del negocio se estabilizan y demuestran capacidades con autonomía real de datos, reglas, evolución y operación;
- las integraciones confirmadas presentan necesidades de disponibilidad, recuperación o evolución que requieren aislamiento independiente;
- el crecimiento real de pedidos, canales o fuentes de inventario supera la capacidad de la unidad desplegable bajo los objetivos validados;
- la organización dispone de equipos y prácticas capaces de asumir de forma sostenible la operación distribuida;
- cambian de forma relevante el alcance, los requisitos prioritarios, los escenarios de calidad o los supuestos que sostienen esta decisión.

Cumplir una de estas condiciones no obliga automáticamente a migrar. Primero deberá actualizarse la comparación, evaluar alternativas y registrar una nueva decisión.

## Consecuencias para las siguientes actividades

- El diseño deberá representar los módulos preliminares, sus responsabilidades y sus relaciones sin convertirlos todavía en límites definitivos del dominio.
- Los criterios verificables deberán incluir controles sobre dependencias, responsabilidades, acceso entre módulos y aislamiento de integraciones.
- La implementación futura deberá conservar trazabilidad entre módulos, requisitos funcionales y escenarios de calidad.
- Las pruebas futuras deberán evaluar EC-001 a EC-008 bajo las condiciones documentadas; este ADR no sustituye esa evidencia.
- Deberán validarse las fuentes de información, estados, reglas de inventario, criterios de asignación, incidencias, permisos e intercambios externos antes de cerrar el diseño detallado.
- Las decisiones sobre tecnologías, almacenamiento e infraestructura se tomarán por separado y deberán ser compatibles con esta estructura sin quedar justificadas únicamente por ella.
- Cualquier cambio significativo en los supuestos o en la evidencia deberá activar la revisión de este ADR.

## Referencias

- [Análisis del problema](../actividad-1/01-analisis-del-problema.md)
- [Actores](../actividad-1/02-actores.md)
- [Alcance y supuestos](../actividad-1/03-alcance-y-supuestos.md)
- [Diagrama de contexto](../actividad-1/04-diagrama-de-contexto.md)
- [Registro de decisiones iniciales](../actividad-1/05-registro-de-decisiones.md)
- [Registro de riesgos](../actividad-1/06-registro-de-riesgos.md)
- [Catálogo de requisitos](./01-catalogo-de-requisitos.md)
- [Matriz de priorización](./02-matriz-de-priorizacion.md)
- [Escenarios de calidad](./03-escenarios-de-calidad.md)
- [Comparación de alternativas estructurales](./04-comparacion-de-alternativas-estructurales.md)

## Documentos de referencia

- [Análisis del problema de la Actividad 1](../actividad-1/01-analisis-del-problema.md)
- [Alcance y supuestos de la Actividad 1](../actividad-1/03-alcance-y-supuestos.md)
- [Registro de decisiones de la Actividad 1](../actividad-1/05-registro-de-decisiones.md)
- [Registro de riesgos de la Actividad 1](../actividad-1/06-registro-de-riesgos.md)
- [Catálogo de requisitos de la Actividad 2](./01-catalogo-de-requisitos.md)
- [Matriz de priorización de la Actividad 2](./02-matriz-de-priorizacion.md)
- [Escenarios de calidad de la Actividad 2](./03-escenarios-de-calidad.md)
- [Comparación de alternativas estructurales de la Actividad 2](./04-comparacion-de-alternativas-estructurales.md)
