# Criterios verificables de evaluación

## Introducción

Este documento define criterios concretos para evaluar si el diseño y una futura implementación de LogiTrack mantienen trazabilidad con los requisitos, atienden los escenarios de calidad y respetan la decisión estructural registrada en el ADR-001. Los criterios permiten reunir evidencia y reconocer incumplimientos sin afirmar que la arquitectura ya ha sido probada.

Las métricas numéricas incluidas provienen de los escenarios de calidad aprobados. Son objetivos académicos propuestos y pendientes de validación con información real del negocio, no mediciones ni compromisos de la empresa.

## Metodología de evaluación

La evaluación se divide en tres niveles:

### Verificación documental durante el diseño

Comprueba la coherencia entre el alcance, los requisitos, los módulos, sus responsabilidades, las dependencias y las decisiones registradas. Se realiza mediante inspecciones, matrices de trazabilidad, diagramas y revisiones de contratos. Puede aplicarse durante la Actividad 2 porque no requiere una implementación.

Una revisión documental demuestra que una decisión está descrita de forma completa y coherente, pero no demuestra que el sistema se comporte correctamente en ejecución.

### Verificación mediante pruebas durante la implementación

Comprueba el comportamiento observable del sistema por medio de pruebas funcionales, de autorización, de contratos, de inyección de fallos, de carga y de rendimiento. Se aplicará cuando exista una implementación y se hayan definido los datos, reglas y condiciones necesarios para ejecutar cada prueba.

Los criterios asociados con EC-001 a EC-008 permanecerán pendientes hasta que sus pruebas se ejecuten y produzcan evidencia. No se puede asignarles el resultado «Cumple» a partir del análisis documental.

### Validación pendiente con información real del negocio

Confirma que las reglas, fuentes, actores, permisos, intercambios, volúmenes y objetivos propuestos representan las necesidades reales de la operación. Requiere información que el caso no proporciona. Esta validación puede ajustar los requisitos, las métricas, los límites modulares o la decisión estructural, pero cualquier cambio deberá quedar documentado.

El orden general será: validar la información necesaria, completar el diseño verificable, implementar y ejecutar las pruebas. Cuando una prueba pueda prepararse con datos sintéticos antes de la validación total, sus resultados seguirán siendo académicos y no se presentarán como evidencia de la operación real.

## Matriz de criterios verificables

| Identificador | Aspecto que se evalúa | Requisitos o escenarios relacionados | Condición de aceptación verificable | Método de verificación | Evidencia esperada | Etapa de evaluación |
| --- | --- | --- | --- | --- | --- | --- |
| CV-001 | Cobertura de requisitos funcionales | RF-001 a RF-011; ADR-001 | Los once requisitos aparecen una vez en una matriz de trazabilidad y cada uno se relaciona con al menos un módulo preliminar, una responsabilidad observable y su criterio de aceptación. No existen requisitos omitidos ni funcionalidades fuera del alcance presentadas como obligatorias. | Inspección del catálogo, el diseño modular y la matriz de trazabilidad; conteo de RF-001 a RF-011. | Matriz con los once requisitos, módulo responsable, comportamiento esperado y criterio de aceptación. | Diseño; se reconfirma durante la implementación. |
| CV-002 | Cobertura de atributos de calidad | AC-001 a AC-008; EC-001 a EC-008; ADR-001 | Los ocho atributos aparecen una vez en la trazabilidad; cada uno se relaciona con su escenario, un mecanismo o responsabilidad de diseño y un método futuro de verificación. No se registra un atributo como cumplido sin evidencia de prueba. | Inspección cruzada del catálogo, los escenarios, el diseño y el ADR. | Matriz con AC-001 a AC-008, escenario relacionado, decisión o mecanismo y método de evaluación. | Diseño; resultados en implementación. |
| CV-003 | Consistencia ante duplicados y contradicciones | AC-001; RF-001, RF-002, RF-003 y RF-011; EC-001 | **Objetivo académico propuesto y pendiente de validación:** en un conjunto sintético de 1.000 mensajes que incluya 100 duplicados exactos y 50 casos con datos contradictorios, el 100 % de los duplicados se asocia con el pedido correcto, el 100 % de los conflictos se identifica y existen 0 pedidos lógicos activos duplicados. | Prueba funcional con datos sintéticos e inspección de pedidos, fuentes y conflictos detectados. | Conjunto de entrada, resultados de la prueba y comparación entre pedidos esperados y registrados. | Implementación, después de validar las reglas de identificación. |
| CV-004 | Trazabilidad de los cambios del pedido | AC-002; RF-004, RF-005 y RF-006; EC-002 | **Objetivo académico propuesto y pendiente de validación:** al ejecutar 200 cambios conocidos sobre pedidos de prueba, el 100 % aparece en el historial correcto, en orden temporal y sin eventos faltantes. Cada cambio conserva la evidencia prevista de actor, fuente y momento. | Comparación automatizada entre la secuencia ejecutada y el historial consultado, complementada con inspección. | Secuencia de 200 cambios, historial obtenido y reporte de coincidencias, orden y eventos faltantes. | Implementación, después de validar eventos y datos del historial. |
| CV-005 | Vigencia de la información | AC-003; RF-002, RF-003, RF-005, RF-009 y RF-011; EC-003 | **Objetivo académico propuesto y pendiente de validación:** en 200 actualizaciones de prueba, al menos el 95 % es visible en la consulta central en un máximo de 30 segundos; el 100 % muestra su fuente y momento de actualización, y todo caso que supera los 30 segundos queda identificado como pendiente o desactualizado. | Prueba controlada que compare el momento de aceptación con la consulta y revise las marcas de fuente y vigencia. | Registro de tiempos por actualización, fuente, momento, estado de vigencia y cálculo del porcentaje alcanzado. | Implementación, después de validar fuentes y reglas de vigencia. |
| CV-006 | Validez de los intercambios externos | AC-004; RF-008, RF-009, RF-010 y RF-011; EC-004 | **Objetivo académico propuesto y pendiente de validación:** por cada intercambio externo confirmado se ejecutan al menos 20 casos válidos y 20 inválidos; el 100 % de los válidos se acepta y relaciona correctamente, el 100 % de los inválidos se detecta y se producen 0 alteraciones parciales de datos válidos. | Pruebas de contrato con datos válidos e inválidos e inspección del resultado y del estado final del pedido. | Contrato validado, casos ejecutados, resultados de aceptación o rechazo y comparación de la información antes y después. | Implementación, solo para intercambios confirmados por el negocio. |
| CV-007 | Control de acceso según el actor | AC-005; RF-002 a RF-007; EC-005 | **Objetivo académico propuesto y pendiente de validación:** se prueban todas las combinaciones de actor y acción definidas en la matriz de permisos; el 100 % de las acciones permitidas se completa, el 100 % de las acciones prohibidas se rechaza y existen 0 modificaciones producidas por intentos rechazados. | Pruebas sistemáticas de autorización e inspección de la información antes y después de cada intento. | Matriz de permisos aprobada, casos por combinación, resultados y comparación del estado de la información. | Validación del negocio para definir permisos; prueba durante la implementación. |
| CV-008 | Manejo de errores y fallas en integraciones externas | AC-007; RF-008, RF-009, RF-010 y RF-011; EC-006 | **Objetivo académico propuesto y pendiente de validación:** al ejecutar 100 intercambios con fallas simuladas, el 100 % queda identificado como pendiente o fallido, ocurren 0 pérdidas o corrupciones de información y, al restablecer el sistema externo, cada operación reintentable termina una sola vez o conserva un estado final de error explícito. | Pruebas de inyección de fallos con indisponibilidad, demora, interrupción y repetición; comparación antes, durante y después de la recuperación. | Registro de los 100 casos, estados finales, reintentos y comparación de integridad de la información. | Implementación, después de confirmar al menos un intercambio externo. |
| CV-009 | Escalabilidad de la unidad desplegable | AC-006; RF-001, RF-002 y RF-005; EC-007 | **Objetivo académico propuesto y pendiente de validación:** al pasar de 100 operaciones concurrentes con recursos R a 200 con recursos 2R, el throughput de operaciones exitosas por segundo alcanza al menos 1,8 veces el valor de la línea base; el percentil 95 del tiempo de respuesta no supera 1,2 veces el valor base; la tasa de errores técnicos permanece por debajo del 1 % y se producen 0 pérdidas o corrupciones de operaciones aceptadas. Se conservan las demás condiciones definidas en EC-007. | Prueba de carga repetible con la misma mezcla de operaciones, datos, duración medida después del calentamiento y comportamiento simulado de sistemas externos. | Configuración reproducible de R y 2R, resultados de throughput, percentil 95, errores, integridad y uso de recursos. | Implementación; los objetivos deben validarse con datos reales del negocio. |
| CV-010 | Rendimiento de operaciones principales | AC-008; RF-001, RF-002 y RF-005; EC-008 | **Objetivo académico propuesto y pendiente de validación:** sobre un conjunto de al menos 1.000 operaciones, el percentil 95 de las consultas es igual o menor a 2 segundos, el percentil 95 de los registros y actualizaciones es igual o menor a 3 segundos y la tasa de errores técnicos es menor al 1 %. | Prueba de rendimiento con 100 operaciones concurrentes y datos sintéticos, registrando tiempos individuales, percentiles, operaciones completadas y errores. | Configuración de prueba, registros de tiempos, cálculo de percentiles y reporte de errores. | Implementación; las condiciones deben contrastarse con la operación real. |
| CV-011 | Separación de responsabilidades entre módulos | ADR-001; RF-001 a RF-011 | Gestión de pedidos, Inventario, Asignaciones y entregas, Incidencias, Acceso e Integraciones externas tienen una responsabilidad principal documentada. Cada RF tiene un responsable y ninguna regla o información tiene propietarios contradictorios sin una decisión que lo justifique. | Inspección del diseño modular, responsabilidades y trazabilidad de requisitos. | Diagrama o catálogo de módulos con responsabilidades, información a cargo y RF relacionados; acta de revisión sin conflictos abiertos. | Diseño; se reconfirma durante la implementación. |
| CV-012 | Control de dependencias y ausencia de ciclos | ADR-001 | Todas las dependencias entre módulos aparecen en un diagrama o matriz, tienen dirección y justificación, y el grafo resultante contiene 0 dependencias cíclicas. Si se identifica un ciclo, deberá revisarse el diseño o registrarse una nueva decisión arquitectónica; mientras el ciclo exista, este criterio no podrá marcarse como «Cumple». | Revisión del grafo de dependencias; análisis de ciclos durante el diseño y, cuando exista código, revisión automatizada o inspección equivalente. | Diagrama o matriz de dependencias y reporte de análisis que demuestre 0 ciclos; si se detecta alguno, evidencia de la revisión del diseño o de la nueva decisión arquitectónica y un estado distinto de «Cumple». | Diseño e implementación. |
| CV-013 | Respeto de interfaces o contratos internos | ADR-001; RF-001 a RF-011 | Cada interacción entre módulos identifica proveedor, consumidor, datos de entrada, resultado y errores esperados. No se documentan ni implementan accesos entre módulos que eviten el contrato definido. | Inspección de contratos durante el diseño; pruebas de contrato y revisión de dependencias durante la implementación. | Catálogo de contratos internos, resultados de pruebas y reporte de accesos no permitidos. | Diseño e implementación. |
| CV-014 | Protección de la información propia de cada módulo | AC-001 y AC-005; ADR-001 | Para cada conjunto de información se identifica un módulo responsable. Los demás módulos solo solicitan consultas o cambios mediante el contrato correspondiente; se detectan 0 modificaciones directas que omitan las reglas del responsable. | Inspección de propiedad de información en el diseño; revisión de dependencias y pruebas de integración en la implementación. | Matriz de propiedad, reporte de accesos y pruebas que demuestren el rechazo o la ausencia de modificaciones directas. | Diseño e implementación. |
| CV-015 | Coherencia entre arquitectura y alcance | RF-001 a RF-011; ADR-001; alcance de la Actividad 1 | Todos los módulos y relaciones se justifican mediante requisitos dentro del alcance. Los sistemas externos permanecen fuera del límite de LogiTrack y no se asignan al sistema responsabilidades excluidas, como procesar pagos, reemplazar sistemas externos u optimizar automáticamente rutas. | Revisión cruzada del alcance, el diagrama de contexto, el catálogo, el diseño y el ADR. | Lista de correspondencias entre módulos, requisitos y alcance; registro de desviaciones y decisiones asociadas. | Diseño y cada vez que cambie el alcance. |
| CV-016 | Documentación de decisiones y riesgos arquitectónicos | ADR-001; R-001 a R-008; EC-001 a EC-008 | La decisión mantiene estado, contexto, alternativas, justificación, consecuencias, riesgos, mitigaciones y condiciones de revisión. Cada riesgo arquitectónico identificado tiene una medida propuesta, un estado y evidencia o acción pendiente; los cambios estructurales se registran antes de considerarse aceptados. | Inspección del ADR, del registro de riesgos y de su trazabilidad con requisitos y escenarios. | ADR vigente, registro de riesgos actualizado y relación verificable entre riesgo, medida, estado y evidencia. | Diseño y revisión continua en actividades posteriores. |
| CV-017 | Validación de supuestos del negocio | RF-001 a RF-011; AC-001 a AC-008; ADR-001 | Cada asunto pendiente sobre canales, fuentes, reglas, estados, asignaciones, incidencias, permisos, integraciones, volúmenes y objetivos queda marcado como validado, rechazado o ajustado, con su fuente de evidencia. Los requisitos y decisiones afectados se actualizan antes de cerrar el diseño detallado. | Revisión con la información real disponible del negocio y control de trazabilidad de los cambios. | Registro de validación de supuestos, fuente consultada, resultado y documentos afectados. | Validación del negocio; pendiente mientras no exista esa información. |

## Registro de resultados

Cada evaluación se registrará sin reemplazar la evidencia que la sustenta. Los estados permitidos son:

- **Cumple:** se ejecutó el método indicado y toda la condición de aceptación fue satisfecha con evidencia disponible.
- **Cumple parcialmente:** se ejecutó la evaluación, pero solo una parte de la condición fue satisfecha. Deben registrarse la diferencia y la acción pendiente.
- **No cumple:** la evidencia muestra que una o más condiciones obligatorias no fueron satisfechas.
- **Pendiente de evaluar:** todavía no existe el diseño, la implementación, la información del negocio o la evidencia necesaria para aplicar el método.

No se usará «Cumple» cuando solo exista una intención de diseño. En los criterios documentales, este estado solo podrá asignarse cuando la evidencia concreta indicada en la matriz esté disponible y haya sido revisada frente a toda la condición de aceptación. La existencia de este documento no demuestra por sí sola que el diseño cumple los criterios. Los resultados de pruebas académicas con datos sintéticos se identificarán como tales y no se presentarán como validación de la operación real.

Para cada ejecución se propone el siguiente registro:

| Campo | Contenido esperado |
| --- | --- |
| Criterio | Identificador CV evaluado. |
| Fecha y etapa | Momento de la revisión y etapa: diseño, implementación o validación del negocio. |
| Estado | Cumple, Cumple parcialmente, No cumple o Pendiente de evaluar. |
| Evidencia | Enlace o referencia al documento, reporte o resultado de prueba. |
| Hallazgo | Diferencia observada frente a la condición de aceptación. |
| Acción pendiente | Corrección, validación o nueva evaluación necesaria. |
| Responsable | Persona o función asignada cuando esta información sea definida. |

En la Actividad 2 pueden evaluarse principalmente CV-001, CV-002, CV-011, CV-012, CV-013, CV-015 y CV-016 mediante documentación. Los criterios que dependen de código o ejecución permanecerán pendientes. CV-017 también permanecerá pendiente en los aspectos para los cuales no exista información real del negocio.

## Tabla de trazabilidad

| Criterio | Requisitos o atributos relacionados | Escenario relacionado | Relación con ADR-001 |
| --- | --- | --- | --- |
| CV-001 | RF-001 a RF-011 | EC-001 a EC-008, mediante los RF relacionados | Cobertura de los módulos preliminares y del alcance funcional. |
| CV-002 | AC-001 a AC-008 | EC-001 a EC-008 | Factores de decisión y obligación de verificar los escenarios en actividades posteriores. |
| CV-003 | RF-001, RF-002, RF-003 y RF-011; AC-001 | EC-001 | Reglas comunes e información bajo responsabilidades definidas. |
| CV-004 | RF-004, RF-005 y RF-006; AC-002 | EC-002 | Política transversal de trazabilidad sin eliminar los límites modulares. |
| CV-005 | RF-002, RF-003, RF-005, RF-009 y RF-011; AC-003 | EC-003 | Conservación de fuente, momento y condición de vigencia. |
| CV-006 | RF-008 a RF-011; AC-004 | EC-004 | Sistemas externos fuera del límite e intercambios encapsulados. |
| CV-007 | RF-002 a RF-007; AC-005 | EC-005 | Módulo y política de Acceso; autorización obligatoria en las operaciones expuestas. |
| CV-008 | RF-008 a RF-011; AC-007 | EC-006 | Aislamiento de integraciones y reconocimiento del mayor radio de impacto. |
| CV-009 | RF-001, RF-002 y RF-005; AC-006 | EC-007 | Escalado de la unidad desplegable y condición futura de revisión de la decisión. |
| CV-010 | RF-001, RF-002 y RF-005; AC-008 | EC-008 | Recursos compartidos y condición futura de revisión por interferencia. |
| CV-011 | RF-001 a RF-011 | Apoya EC-001 a EC-006 | Límites modulares preliminares y responsabilidad principal por módulo. |
| CV-012 | Atributos transversales | Apoya EC-001 a EC-008 | Dependencias conocidas, justificadas y sin ciclos. |
| CV-013 | RF-001 a RF-011 | Apoya EC-001 a EC-006 | Colaboración mediante interfaces o contratos internos explícitos. |
| CV-014 | AC-001 y AC-005 | EC-001 y EC-005 | Prohibición de modificar información ajena omitiendo al módulo responsable. |
| CV-015 | RF-001 a RF-011 | Contexto de EC-001 a EC-008 | Monolito modular limitado a las capacidades incluidas y sistemas externos fuera del límite. |
| CV-016 | R-001 a R-008; AC-001 a AC-008 | EC-001 a EC-008 | Conservación del contexto, justificación, riesgos, consecuencias y condiciones de revisión. |
| CV-017 | RF-001 a RF-011; AC-001 a AC-008 | Puede ajustar EC-001 a EC-008 | Decisión sujeta a revisión si cambian los supuestos o aparece nueva evidencia. |

## Información pendiente de validar

Antes de tratar los criterios como compromisos del negocio se necesita confirmar:

- los canales reales, los datos mínimos y la forma de identificar un pedido;
- la fuente considerada vigente para cada dato y los responsables de actualizarla;
- las fuentes y reglas de validación del inventario distribuido;
- los estados y transiciones de pedidos y entregas;
- los criterios y responsables de asignar rutas y repartidores;
- los tipos, datos y tratamiento de las incidencias;
- la matriz de actores, acciones y permisos;
- los sistemas externos que realmente intercambiarán información, sus contratos y su comportamiento ante fallos;
- los volúmenes actuales, variaciones de carga y expectativas de crecimiento;
- las necesidades reales de rendimiento, recuperación, conservación de información y escalado;
- la capacidad del equipo para diseñar, implementar y operar la solución.

Mientras estos puntos no se validen, los criterios que dependen de ellos deben permanecer como «Pendiente de evaluar» o identificarse expresamente como pruebas académicas con datos sintéticos.

## Condiciones que pueden exigir revisar la arquitectura

Los resultados de los criterios servirán como evidencia para revisar el ADR-001 cuando ocurra alguna de estas condiciones:

- CV-009 muestra que la unidad desplegable no puede satisfacer EC-007 de manera razonable después de revisar el diseño y el uso de recursos;
- CV-010 evidencia interferencia sostenida entre módulos que impide alcanzar EC-008;
- CV-008 muestra que las fallas de una integración afectan repetidamente capacidades no relacionadas y el aislamiento interno es insuficiente;
- CV-011 a CV-014 evidencian que los límites modulares no representan responsabilidades estables o no pueden mantenerse sin acoplamiento indebido;
- una capacidad requiere despliegue o evolución independiente y el despliegue conjunto causa impactos repetidos y documentados;
- las integraciones confirmadas requieren aislamiento independiente por sus necesidades validadas de disponibilidad, recuperación o evolución;
- los volúmenes y el crecimiento reales superan la capacidad comprobada de la unidad desplegable;
- cambian de forma importante el alcance, las prioridades, los escenarios o los supuestos;
- existe capacidad organizacional suficiente para asumir de manera sostenible una estructura distribuida.

Un resultado negativo no implica automáticamente migrar a servicios distribuidos. Primero deben analizarse su causa, las medidas posibles dentro del monolito modular y los trade-offs de cualquier alternativa. Si la decisión cambia, deberá actualizarse la comparación y registrarse un nuevo ADR.

## Conclusión

Estos criterios convierten los requisitos, escenarios y principios del ADR-001 en condiciones que pueden revisarse mediante documentos, pruebas o evidencia del negocio. En las siguientes actividades permitirán registrar avances, detectar diferencias y decidir si el monolito modular continúa siendo adecuado. Hasta que existan una implementación y datos reales, los resultados correspondientes permanecerán pendientes y no se considerarán cumplimiento demostrado.

## Documentos de referencia

- [Alcance y supuestos de la Actividad 1](../actividad-1/03-alcance-y-supuestos.md)
- [Registro de riesgos de la Actividad 1](../actividad-1/06-registro-de-riesgos.md)
- [Catálogo de requisitos](./01-catalogo-de-requisitos.md)
- [Matriz de priorización](./02-matriz-de-priorizacion.md)
- [Escenarios de calidad](./03-escenarios-de-calidad.md)
- [Comparación de alternativas estructurales](./04-comparacion-de-alternativas-estructurales.md)
- [ADR-001 — Decisión estructural inicial](./05-adr-001-decision-estructural.md)
