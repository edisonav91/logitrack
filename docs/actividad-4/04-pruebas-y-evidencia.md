# Pruebas y evidencia de ejecución

## Estrategia

| Suite | Propósito | Casos aprobados |
| --- | --- | ---: |
| Dominio | Invariantes de `Delivery` y evento | 5 |
| Aplicación | Orquestación, errores, idempotencia y publicación pendiente | 7 |
| Integración | HTTP, seguridad, SQLite, idempotencia y fallos de disponibilidad | 7 |
| Arquitectura | Dirección de referencias entre capas | 2 |
| **Total** | | **21** |

## Casos principales cubiertos

- asignación válida y producción de `CourierAssigned`;
- segundo repartidor rechazado;
- estado e identificador inválidos;
- repartidor no disponible;
- adaptador de disponibilidad caído;
- actor sin autorización;
- solicitud repetida sin duplicar efectos;
- versión obsoleta;
- fallo del publicador con evento pendiente;
- recorrido HTTP–Aplicación–Dominio–SQLite;
- consulta de la entrega persistida;
- solicitud sin clave API;
- JSON malformado sin exposición de detalles internos;
- Dominio sin referencias a capas exteriores;
- Aplicación sin referencias a Infraestructura o API.

## Evidencia ejecutada el 8 de noviembre de 2026

Compilación:

```text
Compilación correcta.
0 Advertencia(s)
0 Errores
```

Resultados observados por suite:

```text
LogiTrack.Domain.Tests:       5 aprobadas
LogiTrack.Application.Tests:  7 aprobadas
LogiTrack.IntegrationTests:   7 aprobadas
LogiTrack.ArchitectureTests:  2 aprobadas
```

La comprobación `dotnet format --verify-no-changes` terminó con código cero y sin diferencias.

## Evidencia de demostración HTTP

Se inició la API sobre `http://127.0.0.1:5055` con una base recreada por la migración.

```text
HEALTH={"status":"healthy"}
BEFORE_STATUS=0;BEFORE_VERSION=1;BEFORE_COURIER=
ASSIGN_CODE=COURIER_ASSIGNED;ASSIGN_VERSION=2;EVENT_PENDING=False
AFTER_STATUS=1;AFTER_VERSION=2;AFTER_COURIER=44444444-4444-4444-4444-444444444444
SECOND_STATUS=409
SECOND_BODY={"code":"DELIVERY_ALREADY_ASSIGNED",...}
```

Los logs relacionaron la solicitud con el identificador de correlación y mostraron la publicación de `CourierAssigned`. No se presentan estos resultados como evidencia de producción; corresponden a la ejecución local reproducible.

## Automatización

- `scripts/verify.ps1` ejecuta la verificación completa en serie para limitar el consumo de memoria.
- `.github/workflows/ci.yml` repite restore, build, pruebas y formato en cada push o pull request a `main`.
- `TreatWarningsAsErrors` impide aceptar una compilación con advertencias.

