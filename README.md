# Sistema de Gestión de Restaurante — Backend

Backend para un **Sistema de Gestión de Restaurante** construido con **.NET 10**, **Clean Architecture**, **DDD**, **CQRS ligero** (sin MediatR) y **.NET Aspire** para orquestación local.

## Arquitectura

```
HTTP / API  →  Application  →  Domain  ←  Infrastructure  →  PostgreSQL
                    ↑
              AppHost (Aspire)
```

| Capa | Proyecto | Responsabilidad |
|------|----------|-----------------|
| **Dominio** | `src/Domain` | Entidades, value objects, reglas de negocio, eventos, `Result<T>`, interfaces de repositorio |
| **Aplicación** | `src/Application` | Casos de uso (Commands/Queries/Handlers), DTOs, validación de entrada |
| **Infraestructura** | `src/Infrastructure` | EF Core, PostgreSQL, repositorios, migraciones, `DbContext` |
| **API** | `src/Api` | Controllers delgados, Swagger, configuración HTTP, DI |
| **ServiceDefaults** | `src/ServiceDefaults` | Health checks, telemetría, resiliencia y service discovery (Aspire) |
| **AppHost** | `src/AppHost` | Orquestación: PostgreSQL, pgAdmin y API |
| **Tests** | `tests/Tests` | Pruebas unitarias de Dominio y Aplicación |

## Estructura de la solución

```
Restaurante/
├── Restaurante.slnx
├── README.md
├── src/
│   ├── Domain/
│   │   ├── Common/
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   ├── Enums/
│   │   ├── Events/
│   │   ├── Repositories/
│   │   ├── Services/
│   │   └── Errors/
│   ├── Application/
│   │   ├── Common/
│   │   ├── DependencyInjection/
│   │   ├── Platos/
│   │   ├── Clientes/
│   │   └── Pedidos/
│   ├── Infrastructure/
│   │   ├── DependencyInjection/
│   │   ├── Persistence/
│   │   └── Services/
│   ├── Api/
│   │   ├── Controllers/
│   │   ├── DependencyInjection/
│   │   └── Extensions/
│   ├── ServiceDefaults/
│   └── AppHost/
└── tests/
    └── Tests/
        ├── Domain/
        └── Application/
```

## Entidades del dominio

| Entidad | Descripción |
|---------|-------------|
| **Plato** | Catálogo: nombre, descripción, precio, categoría, disponibilidad |
| **Cliente** | Datos de contacto validados mediante value objects |
| **Pedido** | Agregado raíz con reglas de negocio (estado, líneas, total) |

## Casos de uso principales

1. **Lectura** — `GET /api/platos/disponibles` → Obtener platos disponibles (DTOs)
2. **Escritura** — `POST /api/clientes` → Registrar cliente vía fábrica de dominio
3. **Decisión** — `POST /api/pedidos` → Registrar pedido con reglas de negocio y `Result<T>`

Además se implementará **CRUD completo** para Platos, Clientes y Pedidos.

## Reglas de negocio (Dominio)

- No registrar un pedido sin platos
- No pedir platos no disponibles
- Total calculado automáticamente
- No modificar un pedido en estado **Entregado**
- El cliente debe existir antes de registrar un pedido

## Dependencias entre proyectos

```
Domain          → (ninguna)
Application     → Domain
Infrastructure  → Domain
Api             → Application, Infrastructure, ServiceDefaults
AppHost         → Api
Tests           → Domain, Application
```

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (para PostgreSQL y pgAdmin vía Aspire)
- [Aspire CLI](https://aspire.dev/) (opcional, para `aspire run`)

## Ejecución

### Con Aspire (recomendado)

```bash
cd src/AppHost
dotnet run
```

Esto levanta:

- **API** — servicio REST
- **PostgreSQL** — base de datos
- **pgAdmin** — administración de BD
- **Dashboard Aspire** — telemetría y health checks

### Solo API (desarrollo aislado)

```bash
cd src/Api
dotnet run
```

> La conexión a PostgreSQL se configurará en la Etapa 5 (Infraestructura).

## Swagger

En entorno de desarrollo, la documentación OpenAPI está disponible en:

- `/swagger`

## Tests

```bash
dotnet test
```

Stack de pruebas: **xUnit**, **FluentAssertions**, **NSubstitute**.

## Estado del proyecto

| Etapa | Estado | Contenido |
|-------|--------|-----------|
| 1 | ✅ Completada | Diseño de arquitectura |
| 2 | ✅ Completada | Solución, proyectos, referencias, Aspire base |
| 3 | ✅ Completada | Dominio (entidades, VOs, reglas, eventos) |
| 4 | ✅ Completada | Aplicación (CQRS, DTOs, handlers) |
| 5 | ✅ Completada | Infraestructura (EF Core, repos, migraciones) |
| 6 | ✅ Completada | API (controllers, endpoints) |
| 7 | ✅ Completada | Tests unitarios |
| 8 | ✅ Completada | Revisión final |

## Principios aplicados

- **SOLID** — responsabilidades separadas por capa
- **DDD** — agregados, value objects y eventos de dominio
- **Clean Architecture** — dependencias hacia el dominio
- **CQRS ligero** — commands y queries sin MediatR
- **Repository Pattern** — interfaces en Dominio, implementación en Infraestructura

## Licencia

Proyecto educativo / de referencia.
