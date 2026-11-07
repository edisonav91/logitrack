# Trazabilidad, pendientes y checklist

## Trazabilidad con actividades anteriores

| Elemento | Fuente | Evidencia de la Actividad 3 |
| --- | --- | --- |
| Asignar ruta y repartidor | RF-004, DEC-004 y R-005 | CU-01, modelo y EA-01 a EA-09. |
| Seguimiento | RF-005 y AC-002 | Estados, `CourierAssigned` y CU-03. |
| Control de acceso | RF-007 y AC-005 | `IAuthorizationService` y EA-06. |
| Integridad | AC-001 y EC-001 | INV-01 a INV-10. |
| Trazabilidad | AC-002 y EC-002 | Evento, actor, instante y versión. |
| Límites modulares | ADR-001 y CV-011 a CV-014 | Context map, puertos, adaptadores y ADR-002. |

## Evaluación documental

| Criterio de Actividad 2 | Estado actual | Evidencia o pendiente |
| --- | --- | --- |
| CV-004 — Trazabilidad | Cumple parcialmente | El evento y los campos están diseñados; faltan implementación y prueba de 200 cambios. |
| CV-007 — Acceso | Pendiente de evaluar | Existe el puerto, pero falta aprobar y probar la matriz de permisos. |
| CV-011 — Responsabilidades | Cumple para este flujo | Cada contexto tiene propiedad documentada. |
| CV-012 — Ausencia de ciclos | Cumple en el diseño | Las dependencias apuntan hacia adentro; falta verificación sobre código. |
| CV-013 — Contratos internos | Cumple en el diseño | Proveedor, consumidor, datos y errores están definidos. |
| CV-014 — Protección de información | Cumple en el diseño | Se prohíbe escribir datos de otros módulos; falta comprobar la implementación. |
| CV-015 — Coherencia con alcance | Cumple | No se añade optimización de rutas ni procesamiento de pagos. |
| CV-017 — Validación de supuestos | Pendiente | Continúan abiertas VP-01 a VP-07. |

## Validaciones pendientes

| ID | Pregunta | Impacto |
| --- | --- | --- |
| VP-01 | ¿Quién puede asignar y con qué permiso? | Actor, autorización y EA-06. |
| VP-02 | ¿Cómo se determina disponibilidad y cuánto dura? | Puerto, adaptador e INV-05. |
| VP-03 | ¿Existe capacidad máxima de ruta y cómo se calcula? | `IRouteCapacity`, INV-06 y EA-05. |
| VP-04 | ¿Cuáles son los estados y transiciones reales? | Modelo y seguimiento. |
| VP-05 | ¿Cómo se desasigna o reasigna? | Nuevo caso de uso e invariantes. |
| VP-06 | ¿Qué debe contener y cuánto se conserva el historial? | Evento y AC-002. |
| VP-07 | ¿Qué sistema recibe `CourierAssigned`? | Adaptador y contrato externo. |

## Checklist de la guía

| Requisito | Estado | Evidencia |
| --- | --- | --- |
| Flujo, actores, propósito y resultado | Completo en diseño | Documento 01. |
| Lenguaje ubicuo y ambigüedades | Completo en diseño | Documento 01. |
| Entidades, objetos de valor y estados | Completo en diseño | Documento 02. |
| Invariantes y escenarios BDD | Completo en diseño | Documentos 02 y 03. |
| Contextos y contratos intercambiados | Completo en diseño | Documento 04. |
| Casos de uso, puertos y adaptadores | Completo en diseño | Documentos 04 y 05. |
| C4 actualizado | Completo en diseño | Documento 06. |
| `Architecture.md` actualizado | Completo en diseño | Archivo raíz. |
| ADR con alternativas y trade-offs | Completo en diseño | ADR-002. |
| README actualizado | Completo en diseño | Sección Actividad 3. |
| Implementación, persistencia e integración real | No exigida | Corresponde a Actividad 4. |
| Video | Pendiente | Agregar enlace confirmado. |

