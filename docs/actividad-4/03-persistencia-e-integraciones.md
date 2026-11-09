# Persistencia, migraciones e integraciones

## Persistencia elegida

Se utiliza SQLite con `Microsoft.Data.Sqlite`. Es suficiente para demostrar persistencia real y reproducible sin introducir un servidor adicional. El dominio y la aplicación desconocen SQLite; solo Infraestructura referencia el paquete.

## Migración reproducible

`Migrations/001_initial.sql` crea:

- `deliveries`: estado durable y versión;
- `outbox_events`: eventos registrados con intentos, error y publicación;
- `idempotency_records`: resultado estable por `requestId`;
- `schema_migrations`: historial de migraciones aplicadas.

`DatabaseMigrator` aplica cada versión una sola vez al iniciar. Los datos demo se insertan únicamente si `DemoData:Enabled` es verdadero.

## Atomicidad y concurrencia

El repositorio ejecuta dentro de una transacción:

1. actualización de `deliveries` condicionada por `expectedVersion`;
2. inserción de los eventos producidos por el agregado;
3. confirmación de ambos cambios.

Si la versión no coincide, la transacción se revierte y devuelve `VersionConflict`. Esto evita que dos operadores sobrescriban la misma entrega.

## Disponibilidad

`ConfiguredCourierAvailability` implementa `ICourierAvailability`. Permite:

- configurar repartidores no disponibles;
- simular indisponibilidad del adaptador;
- devolver un resultado normalizado sin introducir formatos externos en Aplicación.

No se conectó un proveedor real porque el caso no confirma cuál existe. El adaptador configurable permite validar el límite y su respuesta ante fallos.

## Publicación del resultado

`CourierAssigned` se registra en la tabla outbox en la misma transacción de la entrega. `SqliteOutboxPublisher` intenta publicarlo y:

- marca `published_at` cuando tiene éxito;
- incrementa `attempts` y conserva `last_error` cuando falla;
- no revierte una asignación ya confirmada por una falla posterior de publicación;
- informa `eventPublicationPending` al consumidor.

El adaptador actual publica mediante logs. Un broker real puede reemplazarlo sin modificar Dominio o Aplicación.

## Alternativas no aplicadas

| Alternativa | Justificación |
| --- | --- |
| Event Sourcing | El flujo solo necesita estado durable e historial de eventos de integración. |
| Flyway | La migración SQL ya tiene ejecutor versionado dentro de la solución. |
| Broker y DLQ | No existe proveedor confirmado ni necesidad de distribución. |
| API Gateway | Hay una sola aplicación modular y una entrada HTTP. |
| Microservicios | ADR-001 conserva el monolito modular. |
| Process manager | La asignación no coordina una transacción de larga duración con varios participantes. |

