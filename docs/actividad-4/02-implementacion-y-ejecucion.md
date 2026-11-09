# Implementación y ejecución

## Estructura

```text
src/
  LogiTrack.Domain/          entidades, objetos de valor, evento e invariantes
  LogiTrack.Application/     caso de uso, DTO y puertos
  LogiTrack.Infrastructure/  SQLite, disponibilidad, outbox, reloj y telemetría
  LogiTrack.Api/             entrada HTTP, validación y traducción de errores
tests/
  LogiTrack.Domain.Tests/
  LogiTrack.Application.Tests/
  LogiTrack.IntegrationTests/
  LogiTrack.ArchitectureTests/
```

## Requisitos

- .NET SDK 9.0.200, fijado mediante `global.json`;
- PowerShell para el script integral de verificación;
- no se requiere instalar SQLite por separado.

## Preparación y ejecución

Desde la raíz:

```powershell
dotnet restore LogiTrack.sln
$env:Security__ApiKey = "una-clave-local-no-compartida"
dotnet run --project src/LogiTrack.Api/LogiTrack.Api.csproj --urls http://127.0.0.1:5055
```

En Development, la aplicación aplica la migración y crea una entrega pendiente de demostración:

```text
DeliveryId: 11111111-1111-1111-1111-111111111111
OrderId:    22222222-2222-2222-2222-222222222222
RouteId:    33333333-3333-3333-3333-333333333333
```

## Solicitud exitosa

```powershell
$headers = @{
  "X-Api-Key" = "una-clave-local-no-compartida"
  "X-Actor-Id" = "operador-demo"
  "X-Actor-Role" = "operator"
}

$body = @{
  requestId = [guid]::NewGuid()
  courierId = "44444444-4444-4444-4444-444444444444"
  expectedVersion = 1
} | ConvertTo-Json

Invoke-RestMethod `
  -Method Post `
  -Uri "http://127.0.0.1:5055/deliveries/11111111-1111-1111-1111-111111111111/assign-courier" `
  -Headers $headers `
  -ContentType "application/json" `
  -Body $body
```

## Respuestas relevantes

| Situación | HTTP | Código |
| --- | ---: | --- |
| Asignación aceptada | 200 | `COURIER_ASSIGNED` |
| Sin clave válida | 401 | `UNAUTHENTICATED` |
| Rol no autorizado | 403 | `FORBIDDEN` |
| Entrega inexistente | 404 | `DELIVERY_NOT_FOUND` |
| Ya tiene repartidor | 409 | `DELIVERY_ALREADY_ASSIGNED` |
| Versión obsoleta | 409 | `VERSION_CONFLICT` |
| Repartidor no disponible | 422 | `COURIER_NOT_AVAILABLE` |
| Disponibilidad caída | 503 | `AVAILABILITY_UNAVAILABLE` |
| JSON inválido | 400 | `INVALID_REQUEST` |

## Verificación completa

```powershell
powershell -ExecutionPolicy Bypass -File scripts/verify.ps1
```

El script restaura, compila, ejecuta cada suite en serie y verifica el formato.

