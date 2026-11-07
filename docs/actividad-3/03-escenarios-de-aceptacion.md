# Escenarios de aceptación

Los escenarios están en formato Dado-Cuando-Entonces y describen comportamiento esperado, no pruebas ya ejecutadas.

## EA-01 — Asignación válida

**Dado** una entrega existente en `PENDIENTE_ASIGNACION`, sin repartidor activo, asociada con una ruta no saturada, y un repartidor disponible  
**Cuando** un actor autorizado ejecuta `AssignCourierUseCase`  
**Entonces** la entrega queda `ASIGNADA` al repartidor, aumenta una vez su versión y registra un evento `CourierAssigned`.

## EA-02 — Entrega inexistente

**Dado** un identificador que no corresponde a una entrega  
**Cuando** se intenta asignar un repartidor  
**Entonces** se devuelve `DELIVERY_NOT_FOUND` y no se modifica información.

## EA-03 — Ya existe repartidor activo

**Dado** una entrega con repartidor activo  
**Cuando** se intenta una nueva asignación inicial  
**Entonces** la entidad rechaza con `DELIVERY_ALREADY_ASSIGNED` y conserva estado, versión y asignación.

## EA-04 — Repartidor no disponible

**Dado** una entrega pendiente y un repartidor que la fuente considera no disponible  
**Cuando** se solicita la asignación  
**Entonces** el caso de uso devuelve `COURIER_NOT_AVAILABLE` y no invoca `AssignCourier`.

## EA-05 — Ruta saturada

**Dado** una entrega pendiente cuya ruta alcanzó su capacidad  
**Cuando** se solicita asignar un repartidor  
**Entonces** se devuelve `ROUTE_SATURATED` y la entrega no cambia.

## EA-06 — Actor no autorizado

**Dado** un actor autenticado sin permiso `DELIVERY_ASSIGN_COURIER`  
**Cuando** solicita una asignación  
**Entonces** se devuelve `FORBIDDEN`, no se revela la entrega y no se realizan cambios.

## EA-07 — Solicitud repetida

**Dado** una solicitud aceptada con un `requestId` conocido  
**Cuando** se repite con el mismo contenido  
**Entonces** se devuelve el resultado original sin crear otra asignación ni otro evento.

## EA-08 — Conflicto concurrente

**Dado** dos solicitudes basadas en la misma versión de una entrega  
**Cuando** ambas intentan guardar repartidores distintos  
**Entonces** solo la primera confirmada tiene efecto y la otra devuelve `VERSION_CONFLICT`.

## EA-09 — Fallo del publicador

**Dado** una asignación válida y una falla temporal del adaptador de publicación  
**Cuando** se confirma la operación  
**Entonces** no se pierde la asignación ni el evento pendiente, y el resultado no se duplica al reintentar la publicación.

## Relación con invariantes

| Escenarios | Invariantes verificadas |
| --- | --- |
| EA-01 | INV-01 a INV-07 |
| EA-02 y EA-03 | INV-02 e INV-03 |
| EA-04 y EA-05 | INV-05 e INV-06 |
| EA-06 | Control de acceso de RF-007. |
| EA-07 a EA-09 | INV-07 a INV-10. |

