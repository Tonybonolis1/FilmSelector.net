# Sistema de Login y Favoritos - Marine Traffic

## 📋 Descripción

Sistema completo de autenticación y gestión de favoritos para la aplicación Marine Traffic. Permite a los usuarios registrarse, iniciar sesión y gestionar una lista de buques favoritos con operaciones CRUD completas.

## ✨ Características Implementadas

### Backend

#### 1. **Autenticación JWT**
- ✅ Registro de usuarios
- ✅ Login con credenciales
- ✅ Generación de tokens JWT
- ✅ Validación de tokens en endpoints protegidos
- ✅ Hash seguro de contraseñas (SHA256)

#### 2. **API de Favoritos (CRUD Completo)**
- ✅ **GET** `/api/favorites` - Listar todos los favoritos del usuario
- ✅ **GET** `/api/favorites/{id}` - Obtener un favorito específico
- ✅ **POST** `/api/favorites` - Crear un nuevo favorito
- ✅ **PUT** `/api/favorites/{id}` - Actualizar un favorito
- ✅ **DELETE** `/api/favorites/{id}` - Eliminar un favorito

#### 3. **Endpoints de Autenticación**
- ✅ **POST** `/api/auth/login` - Iniciar sesión
- ✅ **POST** `/api/auth/register` - Registrar nuevo usuario

### Frontend

#### 1. **Página de Login/Registro** (`login.html`)
- ✅ Formulario de inicio de sesión
- ✅ Formulario de registro
- ✅ Cambio dinámico entre pestañas
- ✅ Validación de formularios
- ✅ Mensajes de error/éxito
- ✅ Redirección automática tras login exitoso

#### 2. **Página de Favoritos** (`favorites.html`)
- ✅ Visualización en tarjetas de favoritos
- ✅ Formulario modal para agregar favoritos
- ✅ Edición de favoritos existentes
- ✅ Eliminación con confirmación
- ✅ Estado vacío cuando no hay favoritos
- ✅ Protección de ruta (requiere autenticación)

#### 3. **Integración en App Principal** (`index.html`)
- ✅ Botones de login/registro en header
- ✅ Mostrar nombre de usuario cuando está logueado
- ✅ Acceso rápido a favoritos
- ✅ Opción de cerrar sesión

## 🏗️ Arquitectura

### Backend (.NET 8)

```
Backend/
├── MarineTraffic.Domain/
│   └── Entities/
│       ├── User.cs          # Entidad de usuario
│       └── Favorite.cs      # Entidad de favorito
│
├── MarineTraffic.Application/
│   ├── DTOs/
│   │   ├── Requests/
│   │   │   ├── LoginRequestDto.cs
│   │   │   ├── RegisterRequestDto.cs
│   │   │   ├── CreateFavoriteRequestDto.cs
│   │   │   └── UpdateFavoriteRequestDto.cs
│   │   └── Responses/
│   │       ├── AuthResponseDto.cs
│   │       └── FavoriteResponseDto.cs
│   ├── Interfaces/
│   │   ├── IAuthService.cs
│   │   └── IFavoriteService.cs
│   └── Services/
│       ├── AuthService.cs   # Lógica de autenticación
│       └── FavoriteService.cs # Lógica de favoritos
│
└── MarineTraffic.Api/
    └── Controllers/
        ├── AuthController.cs      # Endpoints de auth
        └── FavoritesController.cs # Endpoints de favoritos (protegidos)
```

### Frontend (Vanilla JS)

```
Frontend/
├── login.html           # Página de login/registro
├── favorites.html       # Gestión de favoritos
├── index.html          # Página principal (actualizada)
└── js/
    ├── auth.js         # Lógica de autenticación
    ├── favorites.js    # Lógica de gestión de favoritos
    └── app.js          # Lógica principal (actualizada)
```

## 🚀 Cómo Usar

### 1. Ejecutar el Backend

```bash
cd Backend/MarineTraffic.Api
dotnet run
```

El API estará disponible en: `http://localhost:5001`

### 2. Abrir el Frontend

Abre `Frontend/index.html` en tu navegador o usa un servidor web local.

### 3. Flujo de Uso

1. **Registrarse**:
   - Ir a "Iniciar Sesión" en el header
   - Cambiar a la pestaña "Registrarse"
   - Completar formulario y registrarse

2. **Iniciar Sesión**:
   - Ingresar usuario y contraseña
   - El sistema guardará el token JWT en localStorage

3. **Gestionar Favoritos**:
   - Click en "⭐ Mis Favoritos"
   - Agregar nuevos buques con el botón "➕ Agregar Favorito"
   - Editar o eliminar favoritos existentes

## 🔐 Seguridad

- **Tokens JWT**: Expiración de 24 horas
- **Contraseñas**: Hash SHA256 (nota: en producción usar bcrypt)
- **Endpoints protegidos**: Requieren token Bearer válido
- **Validación**: En frontend y backend

## 📝 Modelos de Datos

### Usuario
```csharp
{
    "id": 1,
    "username": "usuario123",
    "email": "usuario@email.com",
    "passwordHash": "hash...",
    "createdAt": "2025-11-16T...",
    "lastLoginAt": "2025-11-16T..."
}
```

### Favorito
```csharp
{
    "id": 1,
    "userId": 1,
    "vesselName": "MSC GÜLSÜN",
    "imo": "9811000",
    "mmsi": "372595000",
    "vesselType": "Container Ship",
    "flag": "Panama",
    "notes": "Portacontenedores más grande del mundo",
    "createdAt": "2025-11-16T...",
    "updatedAt": "2025-11-16T..."
}
```

## 🔧 Tecnologías Utilizadas

### Backend
- .NET 8
- ASP.NET Core Web API
- JWT Bearer Authentication
- Clean Architecture (Domain, Application, Infrastructure, API)

### Frontend
- HTML5
- CSS3
- Vanilla JavaScript
- LocalStorage para persistencia de tokens

## 📦 Paquetes NuGet Agregados

```xml
<!-- MarineTraffic.Api -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />

<!-- MarineTraffic.Application -->
<PackageReference Include="Microsoft.Extensions.Configuration.Abstractions" Version="8.0.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.0.0" />
```

## ⚠️ Nota Importante

Este sistema usa almacenamiento **en memoria** para demostración. Los datos se pierden al reiniciar el servidor. Para producción:

1. Implementar base de datos (SQL Server, PostgreSQL, etc.)
2. Usar Entity Framework Core
3. Implementar bcrypt para hash de contraseñas
4. Agregar refresh tokens
5. Implementar rate limiting
6. Agregar validaciones más robustas

## 🎯 Próximas Mejoras Sugeridas

- [ ] Persistencia en base de datos
- [ ] Paginación en lista de favoritos
- [ ] Búsqueda y filtros en favoritos
- [ ] Exportar favoritos a CSV/JSON
- [ ] Compartir favoritos entre usuarios
- [ ] Notificaciones push
- [ ] Recuperación de contraseña
- [ ] Verificación de email
- [ ] OAuth2 (Google, GitHub, etc.)

---

**Desarrollado con ❤️ para Marine Traffic**
