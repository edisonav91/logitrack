# Resultados, riesgos residuales y próximos pasos

## Resultados

- vertical slice ejecutable desde HTTP hasta SQLite;
- invariantes concentradas en `Delivery`;
- puertos definidos en Aplicación y adaptadores en Infraestructura;
- migración SQL reproducible;
- evento guardado atómicamente mediante outbox;
- errores traducidos a códigos HTTP estables;
- 21 pruebas automatizadas aprobadas;
- fitness functions de dependencias;
- build sin advertencias y formato verificado;
- demostración local exitosa con persistencia y caso límite.

## Trade-offs aceptados

| Decisión | Beneficio | Costo |
| --- | --- | --- |
| SQLite | Ejecución simple y reproducible | Menor concurrencia y escalado. |
| API key y rol por cabecera | Demuestra el límite de acceso | No es identidad de producción. |
| Disponibilidad configurable | Prueba el puerto y sus fallos | No demuestra un proveedor real. |
| Outbox con publicación inmediata | No pierde el evento al guardar | Falta procesamiento continuo. |
| Monolito modular | Límites claros con operación sencilla | Despliegue y escalado conjuntos. |

## Riesgos residuales

1. La autenticación debe migrar a un proveedor de identidad y claims firmados.
2. El outbox necesita un worker y política de alertas para eventos agotados.
3. La disponibilidad requiere contrato real, timeout, reintento y circuit breaker cuando se confirme el proveedor.
4. SQLite debe reevaluarse con volúmenes y concurrencia reales.
5. La idempotencia concurrente debe reforzarse si varias instancias reciben el mismo `requestId` al mismo tiempo.
6. Faltan objetivos reales de latencia, disponibilidad y retención.
7. Las métricas y trazas necesitan un exportador y panel operativo.

## Próximos pasos priorizados

1. validar permisos, proveedor de disponibilidad y reglas de ruta;
2. integrar identidad real y almacén de secretos;
3. añadir worker de outbox, alertas y operación de replay;
4. incorporar timeout y resiliencia al adaptador real;
5. medir carga y decidir si SQLite continúa siendo adecuado;
6. añadir exportación OpenTelemetry;
7. implementar las siguientes transiciones de la entrega;
8. repetir escenarios de calidad EC-001 a EC-008 con datos acordados.

