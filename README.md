# 🚢 FilmSelector - Sistema de Consulta de Embarcaciones

## 📋 Descripción del Proyecto

Este es un proyecto de prueba técnica que implementa un sistema completo para consultar información de embarcaciones utilizando la API de FilmSelector, con enfoque especial en identificar embarcaciones con destino al puerto de Santa Marta, Colombia.

**Stack Tecnológico:**
- **Backend:** ASP.NET Core 8.0 Web API (C#)
- **Frontend:** HTML + CSS + JavaScript puro (sin frameworks)
- **Arquitectura:** Clean Architecture con separación de capas
- **Testing:** xUnit con Moq
- **Resiliencia:** Polly (reintentos + circuit breaker)
- **Cliente HTTP:** HttpClientFactory

---

## 🏗️ Arquitectura del Proyecto

El proyecto sigue los principios de **Clean Architecture** (también conocida como Arquitectura Hexagonal u Onion Architecture), con separación clara de responsabilidades:

```
FilmSelector/
├── src/
│   ├── Backend/
│   │   ├── FilmSelector.Domain/          # Capa de Dominio
│   │   │   ├── Entities/                  # Entidades de negocio
│   │   │   │   ├── Vessel.cs
│   │   │   │   ├── VoyageInfo.cs
│   │   │   │   ├── Port.cs
│   │   │   │   └── PortArrival.cs
│   │   │   └── Common/
│   │   │       └── Result.cs              # Tipo genérico para resultados
│   │   │
│   │   ├── FilmSelector.Application/     # Capa de Aplicación
│   │   │   ├── Interfaces/                # Interfaces (inversión de dependencias)
│   │   │   │   └── IFilmSelectorClient.cs
│   │   │   ├── Services/                  # Servicios de aplicación
│   │   │   │   ├── VesselService.cs
│   │   │   │   └── PortService.cs
│   │   │   ├── DTOs/                      # Data Transfer Objects
│   │   │   │   ├── Responses/             # DTOs de respuesta
│   │   │   │   └── External/              # DTOs de API externa
│   │   │   └── Mappings/                  # Mapeo de entidades a DTOs
│   │   │
│   │   ├── FilmSelector.Infrastructure/  # Capa de Infraestructura
│   │   │   ├── Clients/                   # Implementación de clientes HTTP
│   │   │   │   └── FilmSelectorHttpClient.cs
│   │   │   ├── Configuration/             # Configuración (IOptions pattern)
│   │   │   │   └── FilmSelectorOptions.cs
│   │   │   └── Extensions/                # Extensiones de servicios
│   │   │       └── ServiceCollectionExtensions.cs
│   │   │
│   │   └── FilmSelector.Api/             # Capa de Presentación
│   │       ├── Controllers/               # Controladores Web API
│   │       │   ├── VesselsController.cs
│   │       │   └── PortsController.cs
│   │       ├── Middleware/                # Middleware personalizado
│   │       │   └── ExceptionHandlingMiddleware.cs
│   │       ├── Program.cs                 # Configuración de la aplicación
│   │       └── appsettings.json           # Configuración
│   │
│   └── Frontend/                          # Frontend estático
│       ├── index.html                     # Página principal
│       ├── css/
│       │   └── styles.css                 # Estilos personalizados
│       └── js/
│           └── app.js                     # Lógica de la aplicación
│
├── tests/
│   └── FilmSelector.Tests/              # Tests unitarios
│       ├── Services/                      # Tests de servicios
│       ├── Domain/                        # Tests de lógica de dominio
│       └── Mappings/                      # Tests de mapeos
│
└── FilmSelector.sln                     # Solución de Visual Studio
```

### 📐 Capas de la Arquitectura

#### 1️⃣ **Capa de Dominio (Domain)**
- **Propósito:** Contiene la lógica de negocio y las entidades del dominio
- **Características:**
  - No tiene dependencias de otras capas
  - Define las reglas de negocio (ej: `IsDestinationSantaMarta`)
  - Entidades ricas con comportamiento
  - Independiente de infraestructura y frameworks

#### 2️⃣ **Capa de Aplicación (Application)**
- **Propósito:** Orquesta la lógica de la aplicación y casos de uso
- **Características:**
  - Define interfaces para servicios externos (inversión de dependencias)
  - Implementa servicios de aplicación (`VesselService`, `PortService`)
  - DTOs para comunicación con capas externas
  - Mapeo entre entidades de dominio y DTOs
  - Depende solo de la capa de Dominio

#### 3️⃣ **Capa de Infraestructura (Infrastructure)**
- **Propósito:** Implementa detalles técnicos y acceso a recursos externos
- **Características:**
  - Implementa las interfaces definidas en Application
  - Cliente HTTP para FilmSelector API
  - Configuración con `IOptions<T>`
  - Políticas de resiliencia con Polly
  - HttpClientFactory para gestión eficiente de conexiones

#### 4️⃣ **Capa de Presentación (API)**
- **Propósito:** Punto de entrada de la aplicación (Web API)
- **Características:**
  - Controladores RESTful
  - Middleware de manejo de excepciones
  - Configuración de servicios y pipeline HTTP
  - Swagger/OpenAPI para documentación
  - CORS para permitir frontend

---

## 🔧 Configuración Inicial

### Requisitos Previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Editor de código (Visual Studio 2022, VS Code o Rider)
- API Key de FilmSelector (obtener en [FilmSelector.com](https://www.FilmSelector.com/en/ais-api-services))
- Navegador web moderno

### 1. Clonar o Descargar el Proyecto

```bash
cd d:\informacion\Nueva carpeta - copia\tony
```

### 2. Configurar la API Key de FilmSelector

Editar el archivo `src/Backend/FilmSelector.Api/appsettings.Development.json`:

```json
{
  "FilmSelector": {
    "BaseUrl": "https://services.FilmSelector.com/api",
    "ApiKey": "TU_API_KEY_AQUI",
    "TimeoutSeconds": 60,
    "RetryCount": 2,
    "RetryBackoffSeconds": 1
  }
}
```

**⚠️ IMPORTANTE:** Nunca subir la API Key al control de versiones. Usar variables de entorno en producción:

```bash
# En PowerShell
$env:FilmSelector__ApiKey = "TU_API_KEY_AQUI"
```

### 3. Restaurar Dependencias

```powershell
dotnet restore
```

### 4. Compilar el Proyecto

```powershell
dotnet build
```

---

## 🚀 Ejecutar la Aplicación

### Backend (Web API)

Desde la raíz del proyecto:

```powershell
cd src\Backend\FilmSelector.Api
dotnet run
```

La API estará disponible en:
- **HTTP:** http://localhost:5001
- **Swagger UI:** http://localhost:5001/swagger

### Frontend (HTML/CSS/JS)

El frontend se sirve automáticamente desde el backend gracias a `UseStaticFiles()` y `UseDefaultFiles()`.

**Opción 1:** Abrir directamente desde el backend
- Navegar a: http://localhost:5001

**Opción 2:** Servir con un servidor estático simple

```powershell
cd src\Frontend

# Usando Python (si está instalado)
python -m http.server 8080

# O usando Node.js http-server (si está instalado)
npx http-server -p 8080
```

Luego abrir: http://localhost:8080

**NOTA:** Si se sirve el frontend independientemente, actualizar la URL de la API en `src/Frontend/js/app.js`:

```javascript
const API_CONFIG = {
    baseUrl: 'http://localhost:5001/api',
    // ...
};
```

---

## 🧪 Ejecutar Tests

Desde la raíz del proyecto:

```powershell
# Ejecutar todos los tests
dotnet test

# Ejecutar tests con detalles
dotnet test --verbosity normal

# Ejecutar tests con cobertura
dotnet test --collect:"XPlat Code Coverage"
```

Los tests incluyen:
- ✅ Tests de servicios de aplicación
- ✅ Tests de lógica de dominio
- ✅ Tests de mapeos
- ✅ Mocking de dependencias externas

---

## 📡 Endpoints de la API

### 1. Buscar Embarcaciones

```http
GET /api/vessels/search?query={query}
```

**Parámetros:**
- `query` (string, requerido): Nombre, MMSI o IMO de la embarcación

**Respuesta exitosa (200):**
```json
[
  {
    "id": "123",
    "name": "MAERSK ESSEX",
    "mmsi": "219018314",
    "imo": "9632179",
    "shipType": "Container Ship",
    "flag": "DK"
  }
]
```

### 2. Obtener Información de Viaje

```http
GET /api/vessels/{id}/voyage
```

**Parámetros:**
- `id` (string, requerido): ID de la embarcación

**Respuesta exitosa (200):**
```json
{
  "vesselId": "123",
  "vesselName": "MAERSK ESSEX",
  "destinationPort": "SANTA MARTA",
  "destinationCountry": "Colombia",
  "estimatedTimeOfArrival": "2024-12-25T14:30:00Z",
  "voyageStatus": "Under way using engine",
  "currentLatitude": 11.2472,
  "currentLongitude": -74.2017,
  "currentSpeed": 15.5,
  "departurePort": "CARTAGENA",
  "departureTime": "2024-12-20T08:00:00Z",
  "isDestinationSantaMarta": true
}
```

### 3. Próximas Llegadas a Santa Marta

```http
GET /api/ports/santamarta/arrivals
```

**Respuesta exitosa (200):**
```json
[
  {
    "vesselId": "456",
    "vesselName": "MSC GÜLSÜN",
    "mmsi": "372618000",
    "imo": "9839012",
    "shipType": "Container Ship",
    "flag": "PA",
    "originPort": "KINGSTON",
    "estimatedTimeOfArrival": "2024-12-26T10:00:00Z",
    "distanceToPort": 234.5
  }
]
```

### 4. Health Check

```http
GET /health
```

**Respuesta:**
```json
{
  "status": "Healthy",
  "timestamp": "2024-11-16T12:00:00Z",
  "environment": "Development"
}
```

---

## 🎨 Funcionalidades del Frontend

### 1. Búsqueda de Embarcaciones
- Formulario con validación
- Búsqueda por nombre, MMSI o IMO
- Resultados en tabla con información clave
- Manejo de estados de carga
- Mensajes de error amigables

### 2. Visualización de Información de Viaje
- Card destacada si el destino es Santa Marta
- Información detallada:
  - Puerto de destino
  - ETA (fecha de llegada estimada)
  - Posición actual (latitud/longitud)
  - Velocidad
  - Puerto de salida
- Badge visual para Santa Marta

### 3. Llegadas a Puerto
- Lista de próximas llegadas a Santa Marta
- Información de origen y ETA
- Distancia al puerto
- Ordenamiento por fecha

### 4. Diseño Responsivo
- Layout adaptable a móviles, tablets y escritorio
- CSS puro sin frameworks
- Animaciones sutiles
- Tema moderno y limpio

---

## 🛡️ Patrones de Resiliencia Implementados

### HttpClientFactory
```csharp
services.AddHttpClient<IFilmSelectorClient, FilmSelectorHttpClient>(client =>
{
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
})
```

**Beneficios:**
- ✅ Gestión eficiente del pool de conexiones HTTP
- ✅ Previene agotamiento de sockets
- ✅ Configuración centralizada

### Polly - Política de Reintentos
```csharp
.AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: retryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
    ))
```

**Beneficios:**
- ✅ Manejo de errores transitorios (5xx, 408, timeouts)
- ✅ Backoff exponencial para evitar sobrecarga
- ✅ Reintentos automáticos configurables

### Polly - Circuit Breaker
```csharp
.AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(
        handledEventsAllowedBeforeBreaking: 5,
        durationOfBreak: TimeSpan.FromSeconds(30)
    ))
```

**Beneficios:**
- ✅ Previene llamadas a servicios que están fallando
- ✅ Permite recuperación del servicio
- ✅ Protege el sistema de cascadas de fallos

---

## 💡 Principios SOLID Aplicados

### S - Single Responsibility Principle
- ✅ Cada clase tiene una única responsabilidad
- ✅ `VesselService` solo maneja lógica de embarcaciones
- ✅ `FilmSelectorHttpClient` solo hace llamadas HTTP

### O - Open/Closed Principle
- ✅ Servicios abiertos a extensión mediante interfaces
- ✅ Cerrados a modificación (agregar nuevos servicios sin modificar existentes)

### L - Liskov Substitution Principle
- ✅ Implementaciones de `IFilmSelectorClient` son intercambiables
- ✅ Tests usan mocks sin cambiar comportamiento esperado

### I - Interface Segregation Principle
- ✅ Interfaces específicas para cada cliente
- ✅ No hay interfaces "gordas" con métodos innecesarios

### D - Dependency Inversion Principle
- ✅ Dependencias en abstracciones, no en implementaciones concretas
- ✅ `VesselService` depende de `IFilmSelectorClient`, no de la implementación HTTP
- ✅ Inyección de dependencias en toda la aplicación

---

## 📝 Notas para Entrevista

### 🎯 ¿Por qué Clean Architecture?

**Respuesta:**
"Elegí Clean Architecture porque separa claramente las responsabilidades y hace que el código sea:
1. **Testeable:** Puedo testear la lógica de negocio sin depender de la infraestructura
2. **Mantenible:** Los cambios en la infraestructura (ej: cambiar de API externa) no afectan la lógica de negocio
3. **Escalable:** Es fácil agregar nuevos casos de uso sin modificar código existente
4. **Profesional:** Es el estándar de la industria para aplicaciones empresariales complejas"

### 🎯 ¿Por qué JavaScript puro en el frontend?

**Respuesta:**
"Decidí usar JavaScript puro sin frameworks porque:
1. **Demuestra dominio de fundamentos:** Manejo del DOM, eventos, async/await, fetch API
2. **Performance:** Sin overhead de frameworks, carga instantánea
3. **Simplicidad:** Para este caso de uso, un framework sería over-engineering
4. **Portabilidad:** Funciona en cualquier navegador moderno sin transpilación
5. **Entendimiento:** Es más fácil ver la lógica pura sin abstracciones de framework

En producción con funcionalidades más complejas, consideraría React, Vue o Angular según las necesidades del equipo."

### 🎯 ¿Por qué HttpClientFactory + Polly?

**Respuesta:**
"HttpClientFactory con Polly es la combinación recomendada por Microsoft porque:
1. **HttpClientFactory:**
   - Previene el agotamiento de sockets (socket exhaustion)
   - Gestiona el ciclo de vida de HttpClient correctamente
   - Permite configuración centralizada

2. **Polly:**
   - Maneja errores transitorios automáticamente
   - Implementa reintentos con backoff exponencial
   - Circuit breaker protege el sistema de cascadas de fallos
   - Es el estándar de resiliencia en .NET

Esta combinación hace que la aplicación sea robusta ante fallos de red o indisponibilidad temporal de servicios externos."

### 🎯 ¿Cómo extenderías este proyecto?

**Respuesta:**
"Hay varias formas de extender este proyecto según las necesidades del negocio:

**Funcionalidad:**
1. Agregar más puertos (no solo Santa Marta)
2. Historial de rutas de embarcaciones
3. Notificaciones cuando un buque llegue a puerto
4. Filtros avanzados (por tipo de buque, bandera, tamaño)
5. Dashboard con métricas y estadísticas

**Técnico:**
1. **Cache:** Agregar Redis para cachear respuestas de la API externa
2. **Base de datos:** Persistir histórico de consultas y embarcaciones
3. **Autenticación:** JWT para usuarios del sistema
4. **Rate Limiting:** Proteger la API de abuso
5. **SignalR:** Actualizar posiciones en tiempo real
6. **Docker:** Contenedorización para deployment
7. **CI/CD:** Pipeline de Azure DevOps o GitHub Actions
8. **Observabilidad:** Application Insights, Serilog estructurado

**Arquitectura:**
1. Event Sourcing para histórico completo
2. CQRS si hay necesidad de optimizar lecturas/escrituras
3. Microservicios si se requiere escalar componentes independientemente
4. API Gateway para enrutamiento y seguridad centralizada"

### 🎯 ¿Cómo manejas los errores?

**Respuesta:**
"Implementé un manejo de errores en capas:

1. **Capa de Infraestructura:**
   - Try-catch específicos para HttpRequestException, TaskCanceledException
   - Retorno de Result<T> con estado de éxito/fallo

2. **Capa de Aplicación:**
   - Validación de entrada
   - Logging de operaciones
   - Transformación de errores técnicos a mensajes de negocio

3. **Capa de API:**
   - Middleware global de excepciones
   - Retorno de códigos HTTP apropiados
   - DTOs de error estructurados

4. **Frontend:**
   - Manejo de errores en cada fetch
   - Mensajes amigables al usuario
   - No exponer detalles técnicos

Además, Polly maneja automáticamente errores transitorios con reintentos."

### 🎯 ¿Qué mejoras de testing propondrías?

**Respuesta:**
"Actualmente tengo tests unitarios, pero agregaría:

1. **Tests de Integración:**
   - Probar controladores con un servidor de prueba
   - Probar la integración entre capas

2. **Tests End-to-End:**
   - Selenium o Playwright para el frontend
   - Flujos completos de usuario

3. **Tests de Contrato:**
   - Pact para validar contratos con la API externa
   - Evitar romper integraciones

4. **Tests de Performance:**
   - K6 o JMeter para carga
   - Benchmarks de endpoints críticos

5. **Mutation Testing:**
   - Stryker.NET para validar calidad de tests

6. **Cobertura:**
   - Objetivo de >80% de cobertura de código
   - Enfoque en lógica crítica de negocio"

---

## 🔐 Seguridad

### API Key Management
- ✅ API Key en configuración, no hardcodeada
- ✅ appsettings.Development.json excluido de git
- ✅ Variables de entorno en producción

### Frontend
- ✅ Sanitización de HTML (escape de caracteres)
- ✅ Validación de entrada del usuario
- ✅ CORS configurado para dominios específicos

### Backend
- ✅ Middleware de manejo de excepciones
- ✅ No exponer stack traces en producción
- ✅ Logging de operaciones sospechosas

---

## 📚 Recursos y Documentación

- [FilmSelector API Documentation](https://servicedocs.FilmSelector.com/)
- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Polly Documentation](https://github.com/App-vNext/Polly)
- [HttpClientFactory Best Practices](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)

---

## 📄 Licencia

Este proyecto es un ejemplo de prueba técnica para fines educativos y de demostración.

---

## 👨‍💻 Autor

Proyecto desarrollado como prueba técnica para entrevista.

**Contacto:** [Tu email o GitHub]

---

## ⭐ Agradecimientos

- FilmSelector por proveer la API de datos de embarcaciones
- Microsoft por el excelente ecosistema .NET
- Comunidad open-source por las bibliotecas utilizadas

---

**¡Gracias por revisar este proyecto! 🚀**
