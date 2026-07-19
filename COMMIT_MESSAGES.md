# Mensajes de Commits

A continuación se presentan cinco commits separados y estructurados para subir al repositorio, con los comandos de Git y los archivos a agregar.

## 1. Mejora de la inyección de dependencias en Infrastructure
- Refactoriza `Infrastructure.DependencyInjection` para registrar servicios y repositorios de manera más limpia.
- Agrega documentación interna sobre el orden de resolución de dependencias.
- Archivos sugeridos:
  - `src/Infrastructure/DependencyInjection/DependencyInjection.cs`
  - `src/Infrastructure/DependencyInjection/*.cs` (si hay archivos adicionales relacionados)

Comandos:
```bash
git add src/Infrastructure/DependencyInjection/DependencyInjection.cs
git commit -m "Mejora inyección de dependencias en Infrastructure"
```

## 2. Añadir endpoints CRUD para Platos en Api
- Implementa los controladores y rutas necesarias para crear, leer, actualizar y eliminar platos.
- Asegura que los DTOs estén bien mapeados y las respuestas devuelvan códigos HTTP adecuados.
- Archivos sugeridos:
  - `src/Api/Controllers/PlatosController.cs`
  - `src/Application/Platos/Commands/*.cs`
  - `src/Application/Platos/Queries/*.cs`
  - `src/Domain/Entities/Plato.cs`
  - `src/Domain/ValueObjects/*.cs`

Comandos:
```bash
git add src/Api/Controllers/PlatosController.cs src/Application/Platos/**/*.cs src/Domain/Entities/Plato.cs
git commit -m "Añade endpoints CRUD para Platos en Api"
```

## 3. Validaciones de dominio para Clientes y Pedidos
- Refuerza las reglas de negocio en `Domain` para validar datos obligatorios y estados de pedido.
- Agrega errores del dominio específicos y mensajes claros en caso de validación fallida.
- Archivos sugeridos:
  - `src/Domain/Entities/Cliente.cs`
  - `src/Domain/Entities/Pedido.cs`
  - `src/Domain/Errors/DomainErrors.cs`
  - `src/Domain/Common/Result.cs`

Comandos:
```bash
git add src/Domain/Entities/Cliente.cs src/Domain/Entities/Pedido.cs src/Domain/Errors/DomainErrors.cs src/Domain/Common/Result.cs
git commit -m "Agrega validaciones de dominio para Clientes y Pedidos"
```

## 4. Integración de comandos y queries en Application
- Organiza los handlers de comandos y consultas para Clientes, Pedidos y Platos.
- Añade pruebas unitarias básicas en `tests/` para verificar el flujo de comandos.
- Archivos sugeridos:
  - `src/Application/Clientes/Commands/*.cs`
  - `src/Application/Clientes/Queries/*.cs`
  - `src/Application/Pedidos/Commands/*.cs`
  - `src/Application/Pedidos/Queries/*.cs`
  - `tests/Tests/Application/*.cs`

Comandos:
```bash
git add src/Application/Clientes/**/*.cs src/Application/Pedidos/**/*.cs src/Application/Platos/**/*.cs tests/Tests/Application/*.cs
git commit -m "Integra comandos y consultas en Application con pruebas básicas"
```

## 5. Mejoras de configuración y arranque en AppHost
- Estabiliza la carga de `appsettings.json` y `appsettings.Development.json` en AppHost.
- Asegura que los perfiles de lanzamiento y la configuración de desarrollo funcionen correctamente.
- Archivos sugeridos:
  - `src/AppHost/Program.cs`
  - `src/AppHost/appsettings.json`
  - `src/AppHost/appsettings.Development.json`
  - `src/AppHost/Properties/launchSettings.json`

Comandos:
```bash
git add src/AppHost/Program.cs src/AppHost/appsettings.json src/AppHost/appsettings.Development.json src/AppHost/Properties/launchSettings.json
git commit -m "Mejora configuración y arranque en AppHost"
```
