# Users API

API REST desarrollada con .NET 8 utilizando Minimal API, Entity Framework Core, SQLite y CQRS.

## Repositorio

https://github.com/richar3990/UsersApi

## Requisitos

* .NET 8 SDK
* SQLite
* Git

## Tecnologías utilizadas

* .NET 8
* ASP.NET Core Minimal API
* Entity Framework Core
* SQLite
* FluentValidation
* CQRS
* Swagger / OpenAPI

## Cómo ejecutar el proyecto

Clonar el repositorio:

```bash
git clone https://github.com/richar3990/UsersApi.git
```

Acceder al proyecto:

```bash
cd UsersApi
```

Restaurar las dependencias:

```bash
dotnet restore
```

Compilar:

```bash
dotnet build
```

Ejecutar:

```bash
dotnet run
```

La API estará disponible en la URL indicada por la consola.

Swagger está disponible en:

```text
/swagger
```

## Base de datos SQLite

La aplicación utiliza SQLite mediante Entity Framework Core.

La cadena de conexión se encuentra en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=users-api.db"
  }
}
```

### Crear una migración

Si se realizan cambios en las entidades:

```bash
dotnet ef migrations add NombreDeLaMigracion
```

### Aplicar las migraciones

```bash
dotnet ef database update
```

Esto crea o actualiza la base de datos `users-api.db`.

La aplicación también ejecuta las migraciones al iniciar y carga las monedas iniciales si la tabla de monedas está vacía.

## API Key

La API utiliza autenticación mediante API Key.

La API Key de prueba configurada es:

```text
mini-users-api-key-2026
```

Header requerido:

```http
X-API-KEY: mini-users-api-key-2026
```

Ejemplo con `curl`:

```bash
curl -H "X-API-KEY: mini-users-api-key-2026" https://localhost:<port>/users
```

También puede configurarse desde Swagger utilizando el botón **Authorize**.

## Funcionalidades implementadas

### Users

* Crear usuario
* Obtener usuarios
* Obtener usuario por ID
* Actualizar usuario
* Eliminar usuario
* Carga masiva de usuarios mediante `POST /users/bulk`
* Procesamiento paralelo mediante `Task.WhenAll`

### Addresses

* Crear dirección
* Obtener direcciones
* Obtener dirección por ID
* Obtener direcciones por usuario
* Actualizar dirección
* Eliminar dirección

### Currencies

* Crear moneda
* Obtener monedas
* Obtener moneda por ID
* Actualizar moneda
* Eliminar moneda
* Conversión entre monedas

La conversión utiliza:

```text
montoBase = amount * from.RateToBase

convertedAmount = montoBase / to.RateToBase
```

La obtención de las monedas origen y destino se realiza en paralelo utilizando `Task.WhenAll` y diferentes instancias de `AppDbContext` mediante `IDbContextFactory`.

### Validaciones

Se utiliza FluentValidation para validar:

* Usuarios
* Direcciones
* Monedas
* Conversión de monedas

### Seguridad

* API Key mediante el header `X-API-KEY`.

## Carga masiva de usuarios

Endpoint:

```http
POST /users/bulk
```

Ejemplo:

```json
{
  "users": [
    {
      "name": "Ana Gómez",
      "email": "ana.gomez@test.com"
    },
    {
      "name": "Luis Duarte",
      "email": "luis.duarte@test.com"
    },
    {
      "name": "Marta Silva",
      "email": "marta.silva@test.com"
    },
    {
      "name": "Carlos Ríos",
      "email": "carlos.rios@test.com"
    }
  ]
}
```

También se encuentra disponible el archivo:

```text
sample-data/bulk_users_test.json
```

## Procesamiento paralelo

La carga masiva procesa los usuarios de forma concurrente utilizando `Task.WhenAll`.

Cada operación obtiene su propio contexto mediante:

```csharp
IDbContextFactory<AppDbContext>
```

Esto evita compartir una misma instancia de `DbContext` entre operaciones concurrentes.

## Estado de la implementación

### Implementado

* .NET 8
* Minimal API
* SQLite
* Entity Framework Core
* CQRS
* CRUD de usuarios
* CRUD de direcciones
* CRUD de monedas
* Conversión de monedas
* FluentValidation
* API Key
* Swagger
* Procesamiento paralelo con `Task.WhenAll`
* `IDbContextFactory`
* Carga masiva de usuarios
* Migraciones EF Core

### No implementado

No quedan funcionalidades requeridas pendientes de implementación.
