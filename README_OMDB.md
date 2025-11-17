# 🎬 OMDB Movie Search - Proyecto Completo

## 📋 Descripción General

Proyecto completo de ejemplo con **Backend ASP.NET Core Web API (.NET 8)** y **Frontend HTML+CSS+JS puro** que consume la **API de OMDB (Open Movie Database)** para buscar películas y mostrar información detallada.

### 🔑 Características Principales

- ✅ **Backend:** ASP.NET Core 8.0 Web API (C#) con Clean Architecture
- ✅ **Frontend:** HTML5 + CSS3 + JavaScript Vanilla (sin frameworks)
- ✅ **API Externa:** OMDB API (http://www.omdbapi.com)
- ✅ **API Key:** 8844ac86
- ✅ **Arquitectura Limpia:** 4 capas (Domain, Application, Infrastructure, API)
- ✅ **Patrones:** Repository, Result, IOptions, HttpClientFactory
- ✅ **Resiliencia:** Polly (Retry con backoff exponencial + Circuit Breaker)
- ✅ **Testing:** xUnit + Moq
- ✅ **Documentación:** Swagger/OpenAPI

---

## 🏗️ Arquitectura del Proyecto

```
tony/
├── src/
│   ├── Backend/
│   │   ├── MarineTraffic.Domain/          # Entidades de dominio (Movie, MovieDetails)
│   │   ├── MarineTraffic.Application/     # Servicios, DTOs, Interfaces
│   │   ├── MarineTraffic.Infrastructure/  # HttpClient, Polly, Configuración
│   │   └── MarineTraffic.API/            # Controllers, Program.cs, Middleware
│   └── Frontend/
│       ├── index.html                     # Interfaz de usuario
│       ├── css/styles.css                 # Estilos (tema oscuro estilo Netflix)
│       └── js/app.js                      # Lógica del cliente
└── tests/
    └── MarineTraffic.Tests/              # Tests unitarios (xUnit + Moq)
```

### Capas de Clean Architecture

#### 1️⃣ **Capa de Dominio (Domain)**
- **Propósito:** Contiene la lógica de negocio y las entidades del dominio
- **Archivos clave:**
  - `Movie.cs` - Entidad de película básica
  - `MovieDetails.cs` - Entidad con información completa (incluye propiedad `IsHighlyRated` para rating >= 7.5)

#### 2️⃣ **Capa de Aplicación (Application)**
- **Propósito:** Orquesta la lógica de la aplicación y casos de uso
- **Archivos clave:**
  - `IOmdbClient.cs` - Interface para el cliente OMDB
  - `MovieService.cs` - Servicio de aplicación
  - `MovieSearchResponseDto.cs` y `MovieDetailsResponseDto.cs` - DTOs de respuesta
  - `MovieMappings.cs` - Mapeo de entidades a DTOs

#### 3️⃣ **Capa de Infraestructura (Infrastructure)**
- **Propósito:** Implementa detalles técnicos y acceso a recursos externos
- **Archivos clave:**
  - `OmdbHttpClient.cs` - Cliente HTTP para OMDB API
  - `OmdbOptions.cs` - Configuración usando IOptions<T>
  - `OmdbSearchResponseDto.cs` y `OmdbMovieDetailsDto.cs` - DTOs externos para JSON de OMDB
  - `ServiceCollectionExtensions.cs` - Configuración de DI con Polly

#### 4️⃣ **Capa de Presentación (API)**
- **Propósito:** Punto de entrada de la aplicación (Web API)
- **Archivos clave:**
  - `MoviesController.cs` - Endpoints REST para búsqueda y detalles
  - `Program.cs` - Configuración de la aplicación
  - `ExceptionHandlingMiddleware.cs` - Manejo global de excepciones

---

## 🚀 Inicio Rápido

### Prerequisitos

- .NET 8.0 SDK
- Python 3.x (para servidor HTTP del frontend)
- Navegador web moderno

### 1. Ejecutar el Backend

```bash
cd "d:\informacion\Nueva carpeta - copia\tony\src\Backend\MarineTraffic.API"
dotnet run
```

El backend estará disponible en:
- **HTTP:** http://localhost:5001
- **Swagger UI:** http://localhost:5001/swagger

### 2. Ejecutar el Frontend

```bash
cd "d:\informacion\Nueva carpeta - copia\tony\src\Frontend"
python -m http.server 9000
```

Luego abrir: http://localhost:9000

---

## 📡 Endpoints de la API

### 1. Buscar Películas
**GET** `/api/movies/search?title={title}`

**Parámetros:**
- `title` (string, requerido): Título de la película

**Ejemplo:**
```
GET http://localhost:5001/api/movies/search?title=Guardians
```

**Respuesta exitosa (200):**
```json
[
  {
    "imdbId": "tt3896198",
    "title": "Guardians of the Galaxy Vol. 2",
    "year": "2017",
    "type": "movie",
    "poster": "https://m.media-amazon.com/images/..."
  }
]
```

### 2. Obtener Detalles de Película
**GET** `/api/movies/{imdbId}`

**Parámetros:**
- `imdbId` (string, requerido): ID de IMDb (ej: tt3896198)

**Ejemplo:**
```
GET http://localhost:5001/api/movies/tt3896198
```

**Respuesta exitosa (200):**
```json
{
  "imdbId": "tt3896198",
  "title": "Guardians of the Galaxy Vol. 2",
  "year": "2017",
  "rated": "PG-13",
  "released": "05 May 2017",
  "runtime": "136 min",
  "genre": "Action, Adventure, Comedy",
  "director": "James Gunn",
  "writer": "James Gunn, Dan Abnett, Andy Lanning",
  "actors": "Chris Pratt, Zoe Saldana, Dave Bautista",
  "plot": "The Guardians struggle to keep together as a team...",
  "language": "English",
  "country": "United States",
  "awards": "Nominated for 1 Oscar. 15 wins & 60 nominations total",
  "poster": "https://m.media-amazon.com/images/...",
  "imdbRating": "7.6",
  "imdbVotes": "742,050",
  "type": "movie",
  "boxOffice": "$389,813,101",
  "isHighlyRated": true
}
```

**Nota:** `isHighlyRated` es `true` cuando el rating de IMDb es >= 7.5

---

## 🎨 Funcionalidades del Frontend

### 1. Búsqueda de Películas
- Formulario con validación
- Indicador de carga
- Mensajes de éxito/error/advertencia
- Grid responsivo con posters

### 2. Visualización de Detalles
- Card destacada con póster grande
- Rating con indicador visual (verde si es >= 7.5)
- Información completa (director, actores, premios, recaudación, etc.)
- Diseño estilo Netflix (tema oscuro)

### 3. Diseño Responsivo
- Layout adaptable a móviles, tablets y escritorio
- Grid de películas adaptativo
- Tipografía y espaciado optimizados

---

## 🔧 Tecnologías Aplicadas

### Backend

#### Clean Architecture
- ✅ Separación en 4 capas independientes
- ✅ Principios SOLID aplicados
- ✅ Inversión de dependencias (DI)

#### HttpClientFactory
```csharp
services.AddHttpClient<IOmdbClient, OmdbHttpClient>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());
```
- ✅ Gestión eficiente del pool de conexiones HTTP
- ✅ Previene el agotamiento de sockets

#### Polly - Política de Reintentos
```csharp
.WaitAndRetryAsync(
    retryCount: 3,
    sleepDurationProvider: retryAttempt => 
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
)
```
- ✅ Manejo de errores transitorios (5xx, 408, timeouts)
- ✅ Backoff exponencial (2^n segundos entre reintentos)

#### Polly - Circuit Breaker
```csharp
.CircuitBreakerAsync(
    handledEventsAllowedBeforeBreaking: 5,
    durationOfBreak: TimeSpan.FromSeconds(30)
)
```
- ✅ Previene llamadas a servicios que están fallando
- ✅ Se abre tras 5 fallos y se cierra tras 30 segundos

#### Patrón Result<T>
```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? ErrorMessage { get; }
}
```
- ✅ Manejo explícito de éxitos y fallos
- ✅ Sin excepciones para control de flujo

#### IOptions<T> Pattern
```csharp
public class OmdbOptions
{
    public const string SectionName = "Omdb";
    public string BaseUrl { get; set; }
    public string ApiKey { get; set; }
}
```
- ✅ Configuración tipada y validada
- ✅ Inyección de dependencias para configuración

### Frontend

#### Vanilla JavaScript
- ✅ Sin dependencias externas (no React, Vue, Angular)
- ✅ Manipulación directa del DOM
- ✅ Fetch API para llamadas HTTP
- ✅ Async/Await para operaciones asíncronas

#### CSS Moderno
- ✅ Variables CSS (Custom Properties)
- ✅ Grid Layout para diseño adaptativo
- ✅ Flexbox para alineación
- ✅ Animaciones y transiciones

---

## 🧪 Testing

### Ejecutar Tests

```bash
cd tests/MarineTraffic.Tests
dotnet test
```

### Cobertura de Tests
- ✅ Tests de entidades de dominio (lógica de negocio)
- ✅ Tests de servicios de aplicación (casos de uso)
- ✅ Tests de mapeo (DTOs)
- ✅ Tests con Moq para dependencias

**Ejemplo de test:**
```csharp
[Fact]
public void IsHighlyRated_ShouldReturnTrue_WhenRatingAbove7_5()
{
    // Arrange
    var movie = new MovieDetails { ImdbRating = "8.4" };
    
    // Act
    var result = movie.IsHighlyRated;
    
    // Assert
    Assert.True(result);
}
```

---

## 📝 Ejemplos de Uso

### Búsqueda de Películas de Marvel
1. Abrir http://localhost:9000
2. Ingresar "Guardians" en el campo de búsqueda
3. Click en "Buscar"
4. Verás una grid con todas las películas que coincidan

### Ver Detalles de una Película
1. Hacer click en cualquier película del grid
2. Se mostrarán los detalles completos
3. Rating destacado en verde si es >= 7.5
4. Click en "Volver a Resultados" para regresar

---

## 🔍 Características Destacadas

### 🎯 Lógica de Negocio en el Dominio
- La propiedad `IsHighlyRated` está en la entidad `MovieDetails`
- Se calcula automáticamente según el rating de IMDb
- Ejemplo de responsabilidad del dominio

### 🛡️ Resiliencia
- Reintentos automáticos en fallos transitorios
- Circuit breaker para prevenir cascadas de fallos
- Timeouts configurables

### 📊 Swagger Documentation
- Documentación automática de endpoints
- Interfaz interactiva para probar la API
- Ejemplos de request/response

### 🎨 UI/UX
- Tema oscuro estilo Netflix
- Indicadores de carga
- Mensajes de feedback
- Diseño responsive

---

## ⚙️ Configuración

### appsettings.json
```json
{
  "Omdb": {
    "BaseUrl": "http://www.omdbapi.com",
    "ApiKey": "8844ac86",
    "TimeoutSeconds": 30,
    "RetryCount": 3,
    "RetryBackoffSeconds": 2
  }
}
```

---

## 🐛 Troubleshooting

### El backend no inicia
- Verificar que el puerto 5001 no esté ocupado
- Verificar instalación de .NET 8 SDK: `dotnet --version`

### El frontend no se conecta al backend
- Verificar que el backend esté corriendo
- Verificar URL de la API en `app.js` (debe ser http://localhost:5001/api)
- Verificar CORS en Program.cs

### La búsqueda no devuelve resultados
- Verificar que la API Key de OMDB sea válida (8844ac86)
- Verificar conexión a internet
- Verificar logs del backend en la consola

---

## 📚 Próximas Mejoras

- [ ] Paginación de resultados
- [ ] Filtros por año, tipo (movie/series/episode)
- [ ] Búsqueda avanzada
- [ ] Favoritos locales (LocalStorage)
- [ ] Cache de resultados
- [ ] Tests de integración
- [ ] Containerización con Docker

---

## 👨‍💻 Autor

Proyecto de ejemplo para demostración de Clean Architecture con .NET 8 y consumo de APIs externas.

---

## 📄 Licencia

Este proyecto es de código abierto para fines educativos.
