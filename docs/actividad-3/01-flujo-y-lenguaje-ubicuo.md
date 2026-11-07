# Flujo seleccionado y lenguaje ubicuo

## Flujo de negocio

El flujo seleccionado es **asignar un repartidor a una entrega existente**. Corresponde a RF-004, se relaciona con RF-005 y desarrolla el caso obligatorio `AssignCourierUseCase` indicado en la guía de la Actividad 3.

| Elemento | Definición |
| --- | --- |
| Actor principal | Operador de comercio o responsable logístico autorizado, por validar. |
| Actores colaboradores | Repartidor y fuente de disponibilidad de repartidores. |
| Propósito | Asociar un único repartidor activo con una entrega que todavía no lo tiene. |
| Inicio | Existe una entrega identificada, asociada con un pedido y una ruta, sin repartidor activo. |
| Resultado esperado | La entrega queda asignada al repartidor disponible y el cambio queda trazado. |

## Recorrido nominal

1. El actor solicita asignar un repartidor a una entrega.
2. La aplicación carga la entrega mediante `IDeliveryRepository`.
3. Comprueba que exista y que no tenga un repartidor activo.
4. Consulta la disponibilidad mediante `ICourierAvailability`, sin depender de una API concreta.
5. La entidad ejecuta `AssignCourier(courierId)` para proteger la invariante.
6. La aplicación guarda mediante el repositorio y registra o publica el resultado mediante `IDomainEventPublisher`.

La guía confirma la intención general, pero no define la fuente de disponibilidad, permisos, estados ni reglas reales de saturación. Esos elementos se presentan como propuestas académicas pendientes de validación.

## Alcance

Incluye la asignación inicial, las reglas necesarias para aceptarla o rechazarla y la trazabilidad del resultado. No incluye optimización automática de rutas, administración laboral del repartidor, persistencia concreta, API real, notificación real ni implementación completa del seguimiento.

## Lenguaje ubicuo

| Término | Significado en LogiTrack | Aclaración |
| --- | --- | --- |
| Pedido | Solicitud comercial que origina la necesidad logística. | Pertenece a Gestión de pedidos. |
| Entrega | Entidad logística identificable que lleva un pedido a su destino. | No es sinónimo de pedido ni de ruta. |
| Asignación | Asociación vigente entre una entrega y un repartidor. | No crea ni optimiza la ruta. |
| Repartidor activo | Repartidor actualmente responsable de una entrega. | Solo puede existir uno por entrega. |
| Repartidor disponible | Repartidor que la fuente autorizada considera apto para ser asignado en ese momento. | El criterio exacto está pendiente. |
| Ruta | Recorrido o agrupación operativa asociada con la entrega. | Su cálculo queda fuera del flujo. |
| Ruta saturada | Ruta que alcanzó el límite operativo definido por el negocio. | El límite y la fuente deben confirmarse. |
| Estado de entrega | Situación vigente de la entrega dentro de su ciclo de vida. | No es el estado de pago ni de inventario. |
| Invariante | Regla que debe conservarse antes y después de una operación. | No puede omitirse desde un adaptador. |
| Puerto | Interfaz definida por dominio o aplicación para expresar una necesidad. | No contiene detalles de proveedor. |
| Adaptador | Implementación que conecta un puerto con base de datos, API, cola o interfaz de entrada. | Depende del puerto, no al contrario. |
| Evento de dominio | Hecho inmutable que describe un cambio ya aceptado. | No es una orden ni concede autorización. |

## Términos ambiguos por confirmar

- responsable autorizado para asignar y reasignar;
- significado de “activo” y “disponible”;
- fuente y vigencia de la disponibilidad;
- criterio de saturación de una ruta;
- momento en que se crea la entrega y estado inicial real;
- posibilidad y reglas de desasignación o reasignación;
- datos mínimos y periodo de conservación del historial.

