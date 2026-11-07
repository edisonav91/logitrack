# Casos de uso y contratos de aplicación

## CU-01 — AssignCourierUseCase

| Campo | Contenido |
| --- | --- |
| Intención | Asignar un repartidor disponible a una entrega existente sin repartidor activo. |
| Actor principal | Operador logístico autorizado, por validar. |
| Entrada | `requestId`, `deliveryId`, `courierId`, `expectedVersion` y actor autenticado. |
| Salida | `deliveryId`, `courierId`, `status`, `version` y `assignedAt`. |
| Precondiciones | Actor autorizado; entrega existente; fuentes de disponibilidad operables. |
| Poscondición | Una única asignación activa y un evento pendiente de publicación. |

### Flujo principal

1. `IAuthorizationService.Authorize(actor, DELIVERY_ASSIGN_COURIER)`.
2. Consultar el resultado previo asociado con `requestId`.
3. `IDeliveryRepository.Get(deliveryId)`.
4. Verificar existencia y versión de la entrega.
5. `ICourierAvailability.IsAvailable(courierId, routeId, instant)`.
6. `IRouteCapacity.HasCapacity(routeId)` si la regla queda confirmada.
7. `delivery.AssignCourier(courierId, actorId, instant)`.
8. `IDeliveryRepository.Save(delivery, expectedVersion)` junto con los eventos.
9. `IDomainEventPublisher.PublishPending()` sin acoplar el dominio al mecanismo.
10. Devolver el resultado actualizado.

### Flujos alternos

- actor no autenticado o sin permiso;
- entrega inexistente;
- entrega ya asignada o en estado incompatible;
- repartidor no disponible;
- ruta inexistente o saturada;
- versión concurrente;
- repetición contradictoria del mismo `requestId`;
- indisponibilidad temporal de persistencia o publicación.

## Casos relacionados

| ID | Caso | Alcance en esta actividad |
| --- | --- | --- |
| CU-02 | `GetDeliveryUseCase` | Consultar estado y asignación según permisos. |
| CU-03 | `ChangeDeliveryStatusUseCase` | Describir el seguimiento; no implementarlo completo. |
| CU-04 | `RegisterIncidentUseCase` | Relacionar una incidencia sin cambiar implícitamente la entrega. |

## Contratos de entrada y salida

| Puerto | Operación y resultado | Errores esperados |
| --- | --- | --- |
| `IAssignCourier` (entrada) | `Execute(command) -> AssignCourierResult` | Errores de aplicación normalizados. |
| `IDeliveryRepository` (salida) | `Get(id)`, `Save(delivery, expectedVersion)` | `DELIVERY_NOT_FOUND`, `VERSION_CONFLICT`, `STORAGE_UNAVAILABLE`. |
| `ICourierAvailability` (salida) | `IsAvailable(courierId, routeId, at) -> decision + checkedAt` | `AVAILABILITY_UNAVAILABLE`, respuesta inválida. |
| `IRouteCapacity` (salida) | `HasCapacity(routeId) -> decision + checkedAt` | `ROUTE_NOT_FOUND`, `CAPACITY_UNAVAILABLE`. |
| `IAuthorizationService` (salida) | `Authorize(actor, permission, resource) -> decision` | `UNAUTHENTICATED`, `FORBIDDEN`. |
| `IDomainEventPublisher` (salida) | `PublishPending() -> accepted/pending` | `PUBLISHING_UNAVAILABLE`; nunca borra el evento pendiente. |
| `IIdempotencyStore` (salida) | Obtener o registrar el resultado de `requestId`. | `REQUEST_CONFLICT`, `STORAGE_UNAVAILABLE`. |
| `IClock` (salida) | `Now() -> instant` | Evita depender del reloj del sistema dentro de la entidad. |

## Códigos funcionales

| Código | Significado |
| --- | --- |
| `DELIVERY_NOT_FOUND` | No existe una entrega visible con ese identificador. |
| `DELIVERY_ALREADY_ASSIGNED` | La entrega ya tiene repartidor activo. |
| `INVALID_DELIVERY_STATE` | El estado no admite la asignación. |
| `COURIER_NOT_AVAILABLE` | La fuente vigente rechazó la disponibilidad. |
| `ROUTE_SATURATED` | La ruta no admite otra asignación. |
| `FORBIDDEN` | El actor carece del permiso. |
| `VERSION_CONFLICT` | La entrega cambió desde la lectura del solicitante. |
| `REQUEST_CONFLICT` | El mismo `requestId` llegó con datos diferentes. |

Los contratos no exponen SQL, HTTP, nombres de tablas ni formatos de proveedores. Sus campos obligatorios no cambian de significado sin una nueva versión.

