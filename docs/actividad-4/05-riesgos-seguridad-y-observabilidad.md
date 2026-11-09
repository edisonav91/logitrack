# Riesgos, seguridad y observabilidad

## Registro de riesgos

| ID | Riesgo | Mitigación implementada | Evidencia | Riesgo residual |
| --- | --- | --- | --- | --- |
| RO-01 | Dos operadores asignan simultáneamente. | Versión esperada y actualización condicional. | Pruebas de versión y repositorio. | Requiere que todos los escritores usen el puerto. |
| RO-02 | La disponibilidad externa falla. | Resultado `ServiceUnavailable` y respuesta 503 sin modificar la entrega. | Prueba de integración. | No existe reintento automático de consulta. |
| RO-03 | Se guarda la asignación y falla la publicación. | Outbox transaccional, intentos y estado pendiente. | Prueba de aplicación y esquema. | No hay worker continuo de recuperación. |
| RO-04 | Solicitud duplicada. | `requestId` persistido y resultado idempotente. | Prueba de aplicación. | Solicitudes concurrentes con el mismo id requieren una política más fuerte. |
| RS-01 | Actor no autorizado asigna. | Clave API y rol autorizado. | Pruebas 401 y `FORBIDDEN`. | No sustituye un proveedor de identidad real. |
| RS-02 | Clave expuesta en repositorio. | Configuración vacía y variable de entorno. | `appsettings.json` sin secreto. | Rotación y almacén de secretos son futuros. |
| RS-03 | Errores exponen información interna. | Respuestas normalizadas y correlación. | Prueba de JSON malformado. | Debe revisarse cada endpoint nuevo. |
| RS-04 | Logs contienen datos sensibles. | Solo IDs técnicos, códigos y duración; no se registran tokens ni direcciones. | Inspección del código. | Los identificadores pueden requerir enmascaramiento según política real. |
| RO-05 | SQLite limita concurrencia o escalado. | Alcance local y transacciones breves. | Decisión ADR-003. | Migrar a motor servidor si la carga lo exige. |

## Señales de observabilidad

### Logs estructurados

- inicio y resultado de la asignación;
- `requestId`, `deliveryId` y `correlationId`;
- código de rechazo;
- publicación exitosa o pendiente.

### Métricas definidas

- `assign_courier.success`;
- `assign_courier.rejected`, etiquetada por razón;
- `assign_courier.event_pending`.

### Trazas

`ActivitySource` utiliza el nombre `LogiTrack.Assignments` y crea la actividad `AssignCourier`. Un colector compatible puede suscribirse sin cambiar el caso de uso.

### Salud

`GET /health` confirma que el proceso HTTP está disponible. No representa todavía un readiness check completo de SQLite o integraciones.

## Privacidad

Se excluyen de logs:

- clave API;
- cuerpo completo de la solicitud;
- direcciones de entrega;
- información del cliente;
- detalles de excepciones en la respuesta pública.

