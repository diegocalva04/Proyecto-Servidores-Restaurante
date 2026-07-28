# Restaurante

Backend REST para gestionar platos, clientes y pedidos. La solución conserva DDD, Arquitectura Limpia y CQRS propio: `Domain` contiene agregados y reglas; `Application`, commands, queries y handlers; `Infrastructure`, EF Core, PostgreSQL, repositorios, unidad de trabajo y despacho de eventos; `Api`, controladores y HTTP; `AppHost`, la orquestación Aspire.

## Tecnologías y requisitos

- .NET SDK 10
- .NET Aspire 13.4.6 y Aspire CLI
- Docker Desktop o un runtime OCI compatible, en ejecución
- PostgreSQL se crea automáticamente como recurso de AppHost

## Restaurar, compilar y probar

Desde esta carpeta:

```powershell
dotnet restore Restaurante.slnx
dotnet build Restaurante.slnx --no-restore
dotnet test Restaurante.slnx --no-build
dotnet test tests/Tests/Tests.csproj
dotnet test tests/FunctionalTests/FunctionalTests.csproj
```

Las pruebas funcionales usan `Aspire.Hosting.Testing`, levantan AppHost, PostgreSQL y la API, y hacen peticiones HTTP reales sin puertos fijos. Requieren Docker activo.

## Iniciar el sistema

```powershell
aspire start --apphost src/AppHost/AppHost.csproj --non-interactive
```

Abra la URL del Dashboard indicada por Aspire. Allí aparecen `postgres`, `restaurantedb`, `pgadmin` y `api`. Espere a que `api` esté saludable y abra su endpoint; Swagger está en `/swagger` durante Development. La API aplica la migración de EF Core con `MigrateAsync` al iniciar.

Para ejecutar solo la API, use `dotnet run --project src/Api/Api.csproj --launch-profile http`; escucha en `http://localhost:5230` y necesita PostgreSQL accesible mediante la cadena `restaurantedb`.

## Api.http

Abra `src/Api/Api.http` en Visual Studio, Rider o VS Code con REST Client. Inicie la API, ajuste `baseUrl` si usa la URL dinámica mostrada por Aspire y ejecute las solicitudes en orden. El archivo captura `platoId`, `clienteId` y `pedidoId`, demuestra el flujo completo, errores y limpieza.

## Reglas principales

- El precio de un plato debe ser mayor que cero.
- Nombre, correo y teléfono del cliente son value objects validados e inmutables.
- Un pedido requiere cliente existente, al menos una línea, cantidades positivas y platos existentes/disponibles.
- El total es la suma de subtotales capturados al crear o actualizar líneas.
- Transiciones: `Pendiente → EnPreparacion/Cancelado`; `EnPreparacion → Entregado/Cancelado`.
- `Entregado` y `Cancelado` son estados finales e impiden modificaciones.
- `ClienteRegistrado` y `PedidoRegistrado` se despachan después de persistir; se limpian solo tras despacho satisfactorio.

## Proyectos

```text
src/Domain
src/Application
src/Infrastructure
src/Api
src/AppHost
src/ServiceDefaults
tests/Tests
tests/FunctionalTests
```
