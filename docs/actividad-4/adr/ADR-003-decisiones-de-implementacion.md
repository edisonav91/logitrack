# ADR-003 — Decisiones de implementación del vertical slice

## Estado

Aceptado para la implementación académica de la Actividad 4.

## Contexto

ADR-001 seleccionó un monolito modular y ADR-002 definió `Delivery` como agregado del flujo. La Actividad 4 requiere una rebanada ejecutable con entrada, dominio, persistencia, integración, pruebas y diagnóstico, sin implementar toda la plataforma.

## Decisiones

1. Utilizar .NET 9 disponible en el entorno y proyectos separados por capa.
2. Exponer una entrada HTTP mínima con API key configurable y autorización por rol.
3. Persistir estado durable en SQLite mediante SQL explícito y migración versionada.
4. Aplicar concurrencia optimista por versión.
5. Registrar `CourierAssigned` en un outbox dentro de la transacción.
6. Representar la disponibilidad mediante un adaptador configurable hasta confirmar el proveedor.
7. Utilizar logs estructurados, `Meter`, `ActivitySource` y correlación.
8. Automatizar build, pruebas, reglas de dependencia y formato.

## Alternativas descartadas

- Entity Framework: agrega una abstracción innecesaria para tres tablas y una migración académica.
- Base de datos en memoria: no demostraría persistencia durable ni migraciones.
- Event Sourcing: complejidad mayor sin necesidad del flujo.
- Broker real: no existe contrato ni proveedor confirmado.
- Microservicio independiente: contradice ADR-001 sin nueva evidencia.
- JWT propio: firmar y administrar tokens dentro del proyecto no equivale a una identidad empresarial; se documenta el límite de la API key.

## Consecuencias

La solución puede ejecutarse con pocos requisitos y demuestra los límites diseñados. A cambio, SQLite, la identidad simplificada, el adaptador configurable y la publicación inmediata no son una arquitectura productiva completa. Esos límites se registran como riesgos residuales y puntos de evolución.

## Evidencia

- build con cero advertencias y cero errores;
- 21 pruebas aprobadas;
- formato verificado;
- demostración HTTP con 200 y 409;
- dependencias interiores comprobadas automáticamente.

## Condiciones de revisión

Revisar si se confirma un proveedor externo, se requiere despliegue con varias instancias, aumenta la concurrencia, se adopta un proveedor de identidad o se necesita una garantía independiente de publicación.

