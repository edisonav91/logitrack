# ADR-002 — Modelo y límites para asignar un repartidor

## Estado

Propuesto para la Actividad 3 y pendiente de validar con el negocio.

## Contexto

ADR-001 seleccionó un monolito modular. La Actividad 3 exige modelar `AssignCourierUseCase` y proteger la lógica de negocio de bases de datos, API y frameworks. Debe evitarse que el caso de uso o un adaptador asigne directamente un repartidor y omita las reglas.

## Factores de decisión

- RF-004 exige asociar una ruta y un repartidor con una entrega.
- RF-005 y AC-002 exigen seguimiento y trazabilidad.
- RF-007 y AC-005 exigen control de acceso.
- ADR-001 exige contratos explícitos, propiedad de información y dependencias sin ciclos.
- La fuente real de disponibilidad, las reglas de capacidad y los permisos no están confirmados.

## Decisión

- `Delivery` será la raíz del agregado de Asignaciones y entregas.
- La entrega existirá antes de la asignación en `PENDIENTE_ASIGNACION`.
- `Delivery.AssignCourier` protegerá que no haya otro repartidor activo y que el estado permita asignar.
- Disponibilidad, capacidad, autorización, persistencia, reloj, idempotencia y publicación se expresarán como puertos.
- Los casos de uso orquestarán; los adaptadores implementarán detalles y dependerán de los puertos.
- La escritura del agregado y sus eventos deberá ser atómica; la publicación podrá reintentarse sin duplicar efectos.

## Alternativas descartadas

| Alternativa | Motivo |
| --- | --- |
| Asignar modificando un campo desde el controlador | Permite omitir invariantes y acopla la entrada con el dominio. |
| Consultar una API concreta desde `Delivery` | Hace al dominio dependiente de infraestructura e impide pruebas aisladas. |
| Colocar todas las reglas en el caso de uso | Deja la entidad anémica y permite asignaciones inválidas desde otros flujos. |
| Compartir tablas entre módulos | Rompe la propiedad de información y los contratos de ADR-001. |
| Crear microservicios para el flujo | No existe evidencia que justifique la complejidad distribuida. |

## Consecuencias y trade-offs

El diseño hace comprobables las invariantes, permite sustituir adaptadores y mantiene la dirección de dependencias. A cambio, introduce interfaces, DTO, control de versiones, idempotencia y publicación fiable que deberán implementarse y probarse en la Actividad 4. Las reglas que dependen de disponibilidad pueden fallar aunque el agregado sea válido; por ello se necesita una política explícita de error y vigencia.

## Riesgos y mitigaciones

| Riesgo | Mitigación de diseño |
| --- | --- |
| Una decisión de disponibilidad queda obsoleta antes de guardar. | Conservar `checkedAt`, acordar vigencia y validar de nuevo si el negocio lo exige. |
| Dos operadores asignan al mismo tiempo. | Versión esperada y concurrencia optimista. |
| El cambio se guarda pero falla la publicación. | Persistir el evento con el agregado y reintentar de forma idempotente. |
| Un adaptador omite el dominio. | Pruebas de arquitectura y repositorios encapsulados. |
| La regla de ruta saturada no pertenece a este contexto. | Mantenerla detrás de un puerto y revisar su propietario al validar el negocio. |

## Condiciones de revisión

Revisar cuando se validen la fuente de disponibilidad, la capacidad de ruta, la reasignación, los permisos o si la operación real demuestra que Entrega no es el límite de consistencia adecuado.

