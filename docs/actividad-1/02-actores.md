# Actores del sistema

## Introducción

Los actores son las personas y los sistemas externos que participan en la operación logística o intercambian información con LogiTrack. Identificarlos permite entender quiénes intervienen en la gestión de un pedido y qué relación podrían tener con la plataforma.

El caso proporciona una lista de actores, pero no detalla sus procesos ni la forma exacta en que se comunicarían con LogiTrack. Por esta razón, todas las interacciones incluidas en la tabla son propuestas iniciales: no describen integraciones ya definidas y deberán validarse posteriormente.

## Actores y su relación con LogiTrack

| Actor | Tipo | Función dentro del negocio | Posible interacción con LogiTrack |
| --- | --- | --- | --- |
| Cliente | Humano | Realiza un pedido y espera recibirlo. | Consultar el estado de su pedido y recibir información relacionada con la entrega. |
| Operador de comercio o tienda | Humano | Participa en la gestión de los pedidos recibidos por el comercio. | Registrar o consultar pedidos y revisar su estado dentro de la operación logística. |
| Administrador de almacén | Humano | Se relaciona con la administración del inventario distribuido. | Consultar la información necesaria para validar el inventario asociado con un pedido. |
| Repartidor | Humano | Realiza las entregas que le sean asignadas dentro de una ruta. | Consultar la ruta y los pedidos asignados, además de informar cambios en el estado de una entrega. |
| Soporte al cliente | Humano | Atiende las consultas de los clientes relacionadas con sus pedidos. | Consultar el estado y la trazabilidad disponible de un pedido para orientar al cliente. |
| Sistema de pagos | Sistema externo | Gestiona la información relacionada con el pago de los pedidos. | Comunicar a LogiTrack el estado del pago que resulte necesario para gestionar un pedido. El caso no especifica datos ni reglas de pago. |
| Servicio de geolocalización | Sistema externo | Proporciona información de ubicación aplicable al contexto de las rutas y entregas. | Suministrar datos de ubicación para apoyar la consulta o el seguimiento de una entrega. |
| Sistema de notificaciones | Sistema externo | Se encarga del envío de comunicaciones a sus destinatarios. | Recibir solicitudes de LogiTrack para comunicar cambios relevantes en el estado de un pedido. El caso no define canales ni tipos de notificación. |
| ERP o CRM interno | Sistema externo | Mantiene información empresarial o de clientes relacionada con la operación. | Intercambiar con LogiTrack información necesaria para identificar o consultar pedidos. El caso no precisa qué datos administra ni cuál de estos sistemas utiliza la empresa. |

## Actores humanos y sistemas externos

### Actores humanos

Los actores humanos toman parte en la operación o requieren información sobre ella. En este grupo se encuentran el cliente, el operador de comercio o tienda, el administrador de almacén, el repartidor y el personal de soporte al cliente. Sus necesidades no son iguales: algunos participan directamente en la gestión del pedido, mientras que otros necesitan conocer su estado para recibirlo o atender consultas.

### Sistemas externos

Los sistemas externos pertenecen al entorno de LogiTrack y podrían intercambiar información con la plataforma, pero no forman parte de ella. Este grupo incluye el sistema de pagos, el servicio de geolocalización, el sistema de notificaciones y el ERP o CRM interno. Su inclusión no implica que ya exista una integración definida; los datos, los flujos y los mecanismos de intercambio quedan pendientes de validación.

## Relación de los actores durante la gestión de un pedido

De manera general, la gestión comienza con un pedido realizado por el cliente y recibido por el comercio. El operador de comercio o tienda participa en su gestión, mientras que el administrador de almacén se relaciona con el inventario distribuido. Para la entrega se coordinan una ruta y un repartidor. Si el cliente necesita información, el personal de soporte puede intervenir en la consulta del estado del pedido.

Como planteamiento inicial, LogiTrack serviría como punto central para relacionar la información del pedido que requieren estos actores. Los sistemas de pagos, geolocalización, notificaciones y ERP o CRM podrían aportar o recibir información cuando sea necesaria para la operación. Esta relación es conceptual: el caso no establece el orden detallado de las actividades, las reglas de intercambio ni las integraciones que finalmente se implementarían.
