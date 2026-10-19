# Registro de riesgos

## Introducción

Identificar los riesgos permite reconocer situaciones que pueden afectar la operación logística o limitar los resultados de la propuesta de LogiTrack. Este análisis también ayuda a separar los problemas que el caso presenta como actuales de los riesgos que podrían aparecer al definir o desarrollar la solución.

Las valoraciones de probabilidad e impacto son estimaciones cualitativas iniciales con escala **baja**, **media** o **alta**. No representan mediciones, ya que el caso no proporciona datos sobre frecuencia, volumen o gravedad. En las situaciones que el caso señala como existentes, una probabilidad alta indica que el problema ya está presente, no una frecuencia calculada.

## Riesgos identificados

| Identificador | Riesgo | Posible causa | Consecuencia | Probabilidad estimada | Impacto estimado | Posible medida de mitigación |
| --- | --- | --- | --- | --- | --- | --- |
| R-001 | **Problema actual:** duplicación de información entre canales. | Recepción de pedidos desde múltiples canales sin una vista centralizada de su estado. | Dificultad para reconocer la información vigente y mayor posibilidad de errores en la gestión del pedido. | Alta | Alta | Definir criterios comunes para identificar los pedidos, establecer cuál es la fuente vigente y revisar los registros duplicados. |
| R-002 | **Riesgo potencial:** inconsistencias en la información de los inventarios distribuidos. | Diferencias o desactualización entre las fuentes de inventario que se consulten para validar un pedido. | Validaciones incorrectas y errores posteriores en la gestión del pedido. | Media | Alta | Acordar qué información se necesita para validar inventario, definir responsables y establecer revisiones de consistencia. |
| R-003 | **Problema actual:** retrasos en las entregas. | El caso no precisa una causa única; la falta de información centralizada y las dificultades de coordinación pueden contribuir. | Demoras en el proceso logístico y dificultad para conocer el avance real de la entrega. | Alta | Alta | Definir los puntos de seguimiento del pedido, asignar responsables y revisar las causas de los retrasos antes de establecer acciones específicas. |
| R-004 | **Problema actual:** pérdida o insuficiencia de trazabilidad de los pedidos. | Ausencia de una vista centralizada del estado real y falta de definición sobre cómo se actualiza cada estado. | Dificultad para reconstruir el recorrido del pedido o identificar dónde ocurrió un error o retraso. | Alta | Alta | Definir un conjunto común de estados, responsables de actualización y datos mínimos para seguir cada pedido. |
| R-005 | **Riesgo potencial:** errores en la coordinación de rutas y repartidores. | Información incompleta o desactualizada sobre las asignaciones, junto con criterios de coordinación todavía no definidos. | Asignaciones incorrectas y posibles demoras en las entregas. | Media | Alta | Aclarar los criterios y responsables de asignación, y verificar la información antes de confirmar una ruta o un repartidor. |
| R-006 | **Riesgo potencial de la propuesta:** dificultades para intercambiar información con sistemas externos. | Los datos, responsabilidades y flujos con pagos, geolocalización, notificaciones y ERP o CRM aún no han sido validados. | Información incompleta en LogiTrack o interrupciones en las actividades que dependan de esos intercambios. | Media | Alta | Validar con cada sistema qué información puede aportar o recibir, quién es responsable y cómo se manejarían los errores de intercambio. |
| R-007 | **Riesgo potencial:** crecimiento de la operación sin una gestión adecuada. | Un posible aumento de pedidos, canales o puntos de inventario sin revisar los procesos y necesidades de la operación. | Aumento de la duplicación, los retrasos y los errores, o una propuesta que no responda a las nuevas necesidades. | Media | Alta | Validar las expectativas de crecimiento, observar la evolución de la operación y revisar periódicamente el alcance y los requisitos. |
| R-008 | **Riesgo potencial:** información incompleta o desactualizada sobre los pedidos. | Fuentes de información y responsables de actualización todavía no definidos. | La vista centralizada podría no reflejar el estado real del pedido y conducir a decisiones operativas incorrectas. | Media | Alta | Definir los datos mínimos de cada pedido, su fuente, el responsable de actualizarlos y controles generales de integridad y vigencia. |

## Riesgos que requieren mayor atención

Los riesgos R-001, R-003 y R-004 requieren atención prioritaria porque corresponden a problemas que el caso ya identifica: duplicación de información, retrasos y poca trazabilidad. Su valoración inicial es alta tanto en probabilidad como en impacto, pero deberá confirmarse con información real de la operación.

Los riesgos R-002, R-005 y R-008 también merecen seguimiento temprano por su posible impacto en la confiabilidad del estado de los pedidos. El intercambio con sistemas externos y el posible crecimiento de la operación deben analizarse a medida que se conozcan mejor los flujos de información y las condiciones del negocio.

## Aspectos pendientes de validación

Para revisar y ajustar este registro se necesita validar:

- La frecuencia y el efecto real de la duplicación, los retrasos, los errores operativos y la poca trazabilidad.
- Los canales que reciben pedidos y la forma en que identifican y actualizan su información.
- Las fuentes de inventario, sus responsables y las diferencias que pueden existir entre ellas.
- Los estados utilizados para seguir los pedidos y el momento en que se actualiza cada uno.
- El proceso actual para coordinar rutas y repartidores.
- La información que puede intercambiarse con cada sistema externo y sus condiciones.
- El volumen actual de la operación y sus posibles cambios, sin asumir cifras que no han sido proporcionadas.
- Los datos mínimos que debe contener un pedido para considerarse completo y vigente.
- Los responsables de cada riesgo y los criterios con los que se aceptará o priorizará su tratamiento.

Hasta contar con esta información, las causas, valoraciones y medidas propuestas deben entenderse como una base inicial de análisis y no como conclusiones definitivas.
