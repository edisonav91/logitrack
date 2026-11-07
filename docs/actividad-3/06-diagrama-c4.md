# Diagramas C4 actualizados

## Nivel 1 — Contexto

```mermaid
C4Context
    title LogiTrack - Contexto del flujo de asignación
    Person(operador, "Operador logístico", "Solicita la asignación")
    Person(repartidor, "Repartidor", "Recibe y ejecuta entregas")
    System(logitrack, "LogiTrack", "Gestiona pedidos, asignaciones y seguimiento")
    System_Ext(disponibilidad, "Fuente de disponibilidad", "Confirma disponibilidad si se valida esta integración")
    System_Ext(notificaciones, "Sistema de notificaciones", "Comunica asignaciones si se valida el evento")
    Rel(operador, logitrack, "Asigna repartidor")
    Rel(repartidor, logitrack, "Consulta asignación y actualiza entrega")
    Rel(logitrack, disponibilidad, "Consulta disponibilidad", "Contrato por definir")
    Rel(logitrack, notificaciones, "Solicita notificación", "Evento CourierAssigned")
```

## Nivel 2 — Contenedores

```mermaid
C4Container
    title LogiTrack - Contenedores lógicos
    Person(operador, "Operador logístico")
    System_Boundary(lt, "LogiTrack") {
      Container(app, "Aplicación modular", "Tecnología pendiente", "Dominio, casos de uso y adaptadores")
      ContainerDb(db, "Persistencia", "Tecnología pendiente", "Datos separados por propiedad modular")
    }
    System_Ext(disponibilidad, "Fuente de disponibilidad", "Sistema por confirmar")
    Rel(operador, app, "Ejecuta AssignCourierUseCase")
    Rel(app, db, "Usa puertos de repositorio")
    Rel(app, disponibilidad, "Usa ICourierAvailability")
```

El contenedor Aplicación modular es una sola unidad de despliegue, coherente con ADR-001; los módulos no son microservicios.

## Nivel 3 — Componentes del flujo

```mermaid
C4Component
    title Asignaciones y entregas - Componentes
    Container_Boundary(app, "Aplicación modular") {
      Component(entry, "Adaptador de entrada", "Infraestructura", "Traduce la solicitud")
      Component(usecase, "AssignCourierUseCase", "Aplicación", "Orquesta la intención")
      Component(domain, "Delivery", "Dominio", "Protege las invariantes")
      Component(repoPort, "IDeliveryRepository", "Puerto de salida", "Persistencia requerida")
      Component(availPort, "ICourierAvailability", "Puerto de salida", "Disponibilidad requerida")
      Component(repoAdapter, "Adaptador de repositorio", "Infraestructura", "Implementa persistencia")
      Component(availAdapter, "Adaptador de disponibilidad", "Infraestructura", "Traduce la fuente")
    }
    Rel(entry, usecase, "IAssignCourier")
    Rel(usecase, domain, "AssignCourier")
    Rel(usecase, repoPort, "Carga y guarda")
    Rel(usecase, availPort, "Consulta")
    Rel(repoAdapter, repoPort, "Implementa")
    Rel(availAdapter, availPort, "Implementa")
```

## Secuencia principal

```mermaid
sequenceDiagram
    actor O as Operador
    participant U as AssignCourierUseCase
    participant A as Autorización
    participant R as IDeliveryRepository
    participant D as Disponibilidad
    participant E as Delivery
    participant P as Publicador
    O->>U: Execute(requestId, deliveryId, courierId, version)
    U->>A: Authorize(actor, DELIVERY_ASSIGN_COURIER)
    A-->>U: permitido
    U->>R: Get(deliveryId)
    R-->>U: Delivery
    U->>D: IsAvailable(courierId, routeId, now)
    D-->>U: disponible
    U->>E: AssignCourier(courierId, actorId, now)
    E-->>U: CourierAssigned pendiente
    U->>R: Save(delivery, expectedVersion)
    U->>P: PublishPending()
    U-->>O: asignación y nueva versión
```

## Coherencia entre diagramas

El contexto conserva los sistemas externos fuera de LogiTrack; el nivel de contenedores conserva el monolito modular; el nivel de componentes muestra la inversión de dependencias; y la secuencia utiliza los mismos nombres y contratos definidos en el modelo y los casos de uso.

