# Diagrama de contexto

## Introducción

Este diagrama presenta a LogiTrack como el sistema central y muestra los actores humanos y los sistemas externos que forman parte de su contexto. Su propósito es representar los límites y los principales intercambios de información, sin describir componentes internos ni establecer una arquitectura técnica.

## Diagrama

```mermaid
flowchart LR
    subgraph humanos["Actores humanos"]
        direction TB
        cliente(["Cliente"])
        operador(["Operador de comercio<br/>o tienda"])
        almacen(["Administrador de almacén"])
        repartidor(["Repartidor"])
        soporte(["Soporte al cliente"])
    end

    subgraph limite["Límite de LogiTrack"]
        LT["LogiTrack<br/>Gestión logística centralizada"]
    end

    subgraph externos["Sistemas externos"]
        direction TB
        pagos[["Sistema de pagos"]]
        geo[["Servicio de geolocalización"]]
        notificaciones[["Sistema de notificaciones"]]
        erp[["ERP o CRM interno"]]
    end

    cliente <-->|"Consulta y estado del pedido"| LT
    operador <-->|"Registro, consulta y estado de pedidos"| LT
    almacen <-->|"Información para validar inventario"| LT
    repartidor <-->|"Ruta, pedidos y estado de entrega"| LT
    soporte <-->|"Estado y trazabilidad del pedido"| LT

    pagos -->|"Estado del pago"| LT
    geo -->|"Datos de ubicación"| LT
    LT -->|"Solicitud de comunicación"| notificaciones
    erp <-->|"Información de pedidos"| LT

    classDef humano fill:#dcfce7,stroke:#15803d,color:#14532d,stroke-width:1.5px;
    classDef externo fill:#fef3c7,stroke:#b45309,color:#78350f,stroke-width:1.5px;
    classDef central fill:#dbeafe,stroke:#1d4ed8,color:#1e3a8a,stroke-width:3px;

    class cliente,operador,almacen,repartidor,soporte humano;
    class pagos,geo,notificaciones,erp externo;
    class LT central;
    style limite fill:#eff6ff,stroke:#2563eb,stroke-width:2px,stroke-dasharray:5 5;
```

## Relaciones representadas

Los actores humanos se relacionan con LogiTrack de acuerdo con su participación en la gestión del pedido. El cliente consulta su estado; el operador de comercio o tienda registra o consulta pedidos; el administrador de almacén interviene en la validación de inventario; el repartidor consulta sus asignaciones e informa el estado de la entrega; y soporte al cliente consulta el estado y la trazabilidad para atender solicitudes.

Los sistemas externos aparecen fuera del límite de LogiTrack. El sistema de pagos aporta el estado del pago, el servicio de geolocalización suministra datos de ubicación, el sistema de notificaciones recibe solicitudes de comunicación y el ERP o CRM interno intercambia información relacionada con los pedidos. En el diagrama, el verde identifica actores humanos, el amarillo identifica sistemas externos y el azul identifica LogiTrack.

## Interacciones pendientes de validación

Las relaciones del diagrama son propuestas iniciales construidas a partir del caso y de los documentos anteriores. No representan integraciones confirmadas. Aún deben validarse los datos exactos, el sentido de cada intercambio, los responsables de actualizar la información y los mecanismos de comunicación. El diagrama tampoco define la estructura interna de LogiTrack ni implica una elección tecnológica.
