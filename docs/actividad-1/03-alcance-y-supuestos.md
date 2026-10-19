# Alcance y supuestos

Este documento delimita de manera inicial la propuesta de LogiTrack. Se basa en el caso planteado por el profesor y en el diagnóstico realizado hasta el momento. Los elementos presentados como alcance, exclusiones o supuestos son una propuesta del proyecto y deberán validarse; no corresponden a decisiones técnicas definitivas.

## 1. Objetivo y alcance general

El caso establece que la empresa recibe pedidos desde múltiples canales, administra inventarios distribuidos y coordina rutas y repartidores, pero no cuenta con una vista centralizada del estado real de cada pedido. Esta situación genera duplicación de información, retrasos, errores operativos y poca trazabilidad.

LogiTrack se propone como una plataforma centralizada para apoyar la gestión de los pedidos durante la operación logística de última milla. Su alcance general comprende relacionar la información necesaria para registrar pedidos, validar inventario, coordinar entregas y consultar su seguimiento. También contempla la gestión de incidencias asociadas con los pedidos o las entregas.

En la Actividad 1 este alcance se define solo desde la perspectiva del problema y su contexto. No incluye la implementación de la plataforma ni la elección de una arquitectura o de tecnologías.

## 2. Dentro del alcance

La siguiente delimitación funcional es una propuesta inicial basada en el caso:

- **Gestión de pedidos de distintos canales:** registrar y consultar en un punto central los pedidos recibidos desde los diferentes canales. La forma de conexión con esos canales aún no está definida.
- **Validación de inventario distribuido:** relacionar cada pedido con la información de inventario necesaria para validar su disponibilidad. Las reglas de validación no se especifican en el caso.
- **Asignación de rutas y repartidores:** asociar los pedidos con las rutas y los repartidores encargados de su entrega. Los criterios de asignación quedan pendientes de definición.
- **Seguimiento de entregas:** mantener una vista del estado del pedido durante la operación de entrega para mejorar su trazabilidad.
- **Gestión de incidencias:** registrar y consultar incidencias relacionadas con un pedido o una entrega. El caso no detalla sus tipos ni su tratamiento.
- **Consulta centralizada:** permitir que los actores relacionados con la operación consulten la información que les corresponda. Los permisos y el detalle visible para cada actor deberán validarse.

## 3. Fuera del alcance

Para mantener el proyecto centrado en el problema logístico, se proponen inicialmente las siguientes exclusiones:

- Procesar pagos o sustituir al sistema de pagos.
- Reemplazar el ERP, el CRM, el servicio de geolocalización o el sistema de notificaciones.
- Administrar procesos comerciales como catálogos, precios o promociones, ya que no forman parte del caso descrito.
- Gestionar operaciones completas de almacén distintas de la información necesaria para validar el inventario de un pedido.
- Definir algoritmos de optimización de rutas, pues el caso solo establece que se coordinan diferentes rutas y repartidores.
- Establecer reglas de negocio, indicadores o niveles de servicio que no hayan sido proporcionados.
- Diseñar o implementar integraciones, componentes de software o infraestructura durante esta actividad.

Estas exclusiones son una delimitación propuesta para la etapa actual y pueden revisarse si se obtiene nueva información del negocio.

## 4. Supuestos

Debido a que el caso no desarrolla todos los detalles de la operación, se trabaja con los siguientes supuestos iniciales:

- Los distintos canales pueden proporcionar la información necesaria para identificar y gestionar sus pedidos, aunque no se conoce el mecanismo de intercambio.
- Existe información de los inventarios distribuidos que puede consultarse para apoyar su validación.
- Un pedido puede relacionarse con una ruta y un repartidor para coordinar su entrega.
- Los estados de los pedidos pueden organizarse de manera común, aunque el caso no define cuáles son ni quién los actualiza.
- Las incidencias pueden vincularse con un pedido o una entrega, pero su clasificación y manejo aún no están establecidos.
- Los sistemas externos identificados podrían intercambiar información con LogiTrack. Esto no significa que sus integraciones ya estén confirmadas.
- Los actores humanos identificados requieren distintos niveles de acceso a la información, pendientes de definición.

## 5. Restricciones y aspectos pendientes de validación

La principal restricción es que la información disponible es conceptual. No se conocen cifras de operación, procesos internos detallados, herramientas actuales, reglas de negocio ni requisitos de calidad. Además, la Actividad 1 se limita al diagnóstico y al contexto arquitectónico, por lo que todavía no corresponde seleccionar tecnologías ni definir una arquitectura.

Antes de convertir este alcance en requisitos detallados, se deben validar las siguientes preguntas:

- ¿Cuáles son los canales que reciben pedidos y qué información aporta cada uno?
- ¿Cuál es la fuente de información que debe considerarse vigente para cada pedido?
- ¿Qué significa validar inventario dentro de la operación y qué reglas se aplican?
- ¿Cómo se asignan actualmente las rutas y los repartidores y quién es responsable de hacerlo?
- ¿Qué estados forman parte del seguimiento de un pedido y quién puede actualizarlos?
- ¿Qué tipos de incidencias deben gestionarse y cómo se atienden?
- ¿Qué información debe intercambiarse con cada sistema externo?
- ¿Qué información puede consultar o actualizar cada actor humano?

Estas preguntas no amplían el alcance por sí mismas. Su propósito es identificar la información que falta y evitar que los supuestos se conviertan en requisitos sin validación.
