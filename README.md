# Documentación Autenticación y Autorización de Identidad

## Descripción

PLANTILLA reutilizable como sistema de autenticación y autorización de usuarios. Incluye funciones como creación de roles, registro de usuarios, inicio de sesión, autorización basada en roles y su respectiva configuración.

 ⚠️ Este repositorio solo contiene el backend. Para la parte frontend visitá: IdentityAuthTemplatefe

## Análisis de Requerimientos

### Requerimientos Funcionales

- Validar las credenciales del usuario y devolver un JWT si son correctas
- Crear y asignar roles a los usuarios
- Restringir el acceso a ciertos endpoints según el rol del usuario
- Hashear la contraseña antes de guardarla en la base de datos
- Validar el JWT enviado en el encabezado de la solicitud
- Permitir que solo un administrador acceda a rutas protegidas como `/admin`

### Requerimientos No Funcionales

- Toda la comunicación debe realizarse sobre HTTPS (TLS)
- Autenticación mediante JWT
- La respuestas de la API no deben superar un tiempo promedio de 200ms bajo carga normal
- Hacer buen uso de DTOs para no saturar las entidades
- Documentar la API utilizando Postman
- El sistema debe estar preparado para integrarse fácilmente en proyectos más grandes, permitiendo la extensión de funcionalidades sin romper la arquitectura base

### Requerimientos Técnicos

**Backend**

- [ASP.NET](http://asp.net/) Core Web API
- Entity Framework Core
- Identity Framework Core
- SQL Server
- Swagger / Postman

**Frontend**

- HTML5, SCSS, Typescript, Angular
- Bootstrap v5

**Despliegue**

- Azure
- Vercel

## Diseño del Sistema

### Arquitectura en Capas con Seguridad y Autenticación JWT
<img width="628" height="341" alt="imagen" src="https://github.com/user-attachments/assets/dbd29888-09c9-4f08-923c-e2a2294d5ef8" />

### Diagrama Entidad Relación
<img width="632" height="478" alt="imagen" src="https://github.com/user-attachments/assets/b269a90f-3152-4730-8230-a35ee79dfbf5" />

### Endpoints disponibles

| Método | Endpoint | Descripción |
| --- | --- | --- |
| POST | `/api/Account/register` | Registrarse |
| POST | `/api/Account/login` | Loguearse |

| Método | Endpoint | Descripción |
| --- | --- | --- |
| GET | `/api/Role` | Obtener todos los roles |
| POST | `/api/Role` | Crear un nuevo rol |
| PUT | `/api/Role?roleName=admin&newRoleName=SuperAdmin` | Actualizar un rol |
| DELETE | `/api/Role?roleName=admin` | Eliminar una rol |

| Método | Endpoint | Descripción |
| --- | --- | --- |
| GET | `/api/User` | Obtener todos los usuario |
| POST | `/api/User` | Crear un nuevo usuario |
| PUT | `/api/User/?userName=xxxx` | Actualizar un usuario |
| DELETE | `/api/User/?userName=xxxx` | Eliminar un usuario |

> Autorización y Autenticación requerida para endpoints especificos
> Todos los endpoints devuelven JSON. Para probarlos podés usar Postman o Swagger

### Casos de uso
<img width="634" height="378" alt="imagen" src="https://github.com/user-attachments/assets/7cf957dc-2376-4476-b595-c16e8e8ded1c" />

## Configuración y Ejecución

```
1. Clonar el repositorio
2. Configurar la cadena de conexión en appsettings.json
3. Ejecutar las migraciones (Si usas EF Core)
    ef database update
4. Correr la API
    dotnet watch run
5. Acceder a swagger en: <http://localhost:5000/swagger>
6. Conexión con el frontend: Este backend se comunica con el frontend realizado en Angular, alojado en otro repositorio. El CORS está habilitado para aceptar solicitudes del origen del frontend.
```

## Pendientes / Mejoras Futuras

- Integrar Unit Testing

## Contribuciones

Aunque es un proyecto simple, estoy documentando todo como parte de mi aprendizaje y crecimiento.

Si te interesa aportar ideas, código o feedback, ¡sos más que bienvenido/a!

Gracias por leer 😄
