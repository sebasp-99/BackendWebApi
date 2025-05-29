# Estructura de aplicación web To Do API

## FUNCIONAMIENTO GENERAL DEL FLUJO

- El cliente (frontend) hace una petición a la API, por ejemplo: `POST /api/todo`.
- El `TodoController` recibe la solicitud.
- El Controller usa un `TodoService` que contiene la lógica de negocio.
- El Service usa el `DbContext` definido en `Data/` para acceder a la base de datos.
- Los datos se mapean entre DTOs y Models.
- Se responde con los datos necesarios al cliente.

---

## EXPLICACIÓN DEL FUNCIONAMIENTO

### `Program.cs`
- Es el punto de entrada de la aplicación.
- Aquí se configura el host, middlewares (como CORS, autenticación, logging), servicios (como EF Core, AutoMapper), etc.

### `Controllers/`
- Contienen clases que heredan de `ControllerBase`.
- Cada método se asocia con una ruta HTTP (`GET`, `POST`, `PUT`, `DELETE`) y responde a las solicitudes del cliente.
- Delegan la lógica al servicio correspondiente.

### `Models/`
- Contienen las entidades o clases que representan las tablas de la base de datos.
- Se usan en el `DbContext` para definir el modelo relacional de la base de datos.

### `DTOs/`
- Los DTOs (Data Transfer Objects) sirven para exponer solo lo necesario al cliente.
- Separan el modelo de dominio del modelo de respuesta/entrada.

### `Services/`
- Contienen la lógica de negocio, por ejemplo: validaciones, cálculos, acceso a datos a través de EF Core.
- Se inyectan en los controladores mediante Inyección de Dependencias (DI).

### `Data/`
- Usualmente contiene el `ApplicationDbContext` o `TodoDbContext`, que extiende de `DbContext`.
- Define las `DbSet<T>` que representan las tablas.
- Aquí se configuran relaciones, claves, restricciones con `OnModelCreating`.

### `Migrations/`
- Se genera automáticamente con `dotnet ef migrations add`.
- Contiene clases que describen cómo aplicar o revertir cambios a la base de datos.

### `Enums/`
- En esta carpeta defines tipos de datos constantes, como:


## ÁRBOL DE CARPETAS

```csharp
enum Priority { Low, Medium, High }

TodoApi/
│
├── Connected Services/           # Servicios externos conectados (opcional, poco usado en APIs modernas)
├── Dependencies/                 # Paquetes NuGet y dependencias del proyecto
├── Properties/                   # Configuración del proyecto (.launchSettings.json)
│
├── Controllers/                  # Controladores de la API (manejan las peticiones HTTP)
├── Data/                         # Contexto de base de datos y configuraciones de EF Core
├── DTOs/                         # Objetos de transferencia de datos (DTOs)
├── Enums/                        # Enumeraciones utilizadas por el modelo o lógica de negocio
├── Migrations/                   # Archivos generados por Entity Framework para migraciones
├── Models/                       # Clases que representan las entidades de dominio (entidades de base de datos)
├── Services/                     # Lógica de negocio o acceso a datos encapsulado
│
├── appsettings.json              # Archivo de configuración (cadena de conexión, claves, etc.)
├── Program.cs                    # Punto de entrada de la aplicación (host web)
├── TodoApi.http                  # Archivo para probar endpoints HTTP (VS Code / VS)
└── WeatherForecast.cs            # Archivo de ejemplo, se puede eliminar si no se usa

```

## Diagrama ER

![image](https://github.com/user-attachments/assets/39d4e449-8bce-4dc5-9206-93b90187ede2)


## ENDPOINTS PRINCIPALES

Todos los endpoints (excepto register y login) requieren autenticación con JWT.

Autenticación

### POST /api/account/register

Registra un nuevo usuario.

{
  "username": "usuario1",
  "email": "usuario@email.com",
  "password": "Password123!"
}

### POST /api/account/login

Inicia sesión y devuelve un token JWT.

{
  "username": "usuario1",
  "password": "Password123!"
}

Respuesta:

{ "token": "eyJhbGciOi..." }

Gestión de Tareas

### GET /api/tareas

Obtiene las tareas del usuario autenticado.

### POST /api/tareas

Crea una nueva tarea.

{
  "titulo": "Estudiar para el examen",
  "descripcion": "Matemáticas y Física",
  "estado": "Pendiente"
}

### PUT /api/tareas/{id}

Actualiza una tarea por ID.

### DELETE /api/tareas/{id}

Elimina una tarea por ID.

💂 Gestión de Categorías

### GET /api/categorias

Lista todas las categorías.

### POST /api/categorias

Crea una nueva categoría.

{
  "nombre": "Trabajo"
}

### Autenticación JWT

Para acceder a los endpoints protegidos:

Inicia sesión con /api/account/login.

Copia el token recibido.

En Swagger, haz clic en "Authorize" y pega:

Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...

O en Postman, usa el header:

Authorization: Bearer TU_TOKEN

 Ejecución del Proyecto

### Clona el repositorio:

git clone https://github.com/tu-usuario/TodoApp.git

Configura la cadena de conexión en appsettings.json.

### Ejecuta las migraciones:

dotnet ef database update

### Inicia el servidor:

dotnet run
