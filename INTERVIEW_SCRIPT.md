# Script de Presentación para Entrevista Técnica

## Introducción (1-2 minutos)

"Buenos días/tardes. Hoy voy a presentar un sistema completo de consulta de embarcaciones que desarrollé utilizando la API de MarineTraffic, con enfoque en identificar buques con destino al puerto de Santa Marta, Colombia.

El proyecto está construido con:
- Backend en .NET 8 Web API con Clean Architecture
- Frontend en HTML, CSS y JavaScript puro
- Tests unitarios con xUnit
- Patrones de resiliencia con Polly

Voy a mostrar primero la arquitectura, luego la demo en vivo, y finalmente los aspectos técnicos más destacados."

---

## Arquitectura (3-4 minutos)

### Mostrar Diagrama de Capas

"Implementé Clean Architecture con 4 capas bien definidas:

**1. Dominio (Core):**
- Entidades de negocio: Vessel, VoyageInfo, PortArrival
- Lógica de negocio pura, sin dependencias externas
- Por ejemplo, aquí está la lógica que determina si una embarcación va a Santa Marta

[Mostrar código: VoyageInfo.cs, propiedad IsDestinationSantaMarta]

**2. Aplicación:**
- Servicios de aplicación que orquestan los casos de uso
- Interfaces para inversión de dependencias (SOLID)
- DTOs para comunicación entre capas
- Mapeos entre entidades y DTOs

[Mostrar: IMarineTrafficClient.cs y VesselService.cs]

**3. Infraestructura:**
- Implementación del cliente HTTP para MarineTraffic
- HttpClientFactory para gestión eficiente de conexiones
- Polly para resiliencia: reintentos con backoff exponencial y circuit breaker
- Configuración con IOptions<T>

[Mostrar: MarineTrafficHttpClient.cs y ServiceCollectionExtensions.cs]

**4. Presentación (API):**
- Controladores RESTful
- Middleware de manejo de excepciones
- Swagger para documentación
- CORS configurado para el frontend

[Mostrar: VesselsController.cs]"

---

## Demo en Vivo (3-5 minutos)

### 1. Backend - Swagger
"Primero voy a mostrar la API en Swagger..."

[Abrir http://localhost:5001/swagger]

"Tenemos 3 endpoints principales:
1. Search vessels - para buscar embarcaciones
2. Get voyage info - para obtener información de viaje
3. Santa Marta arrivals - para ver próximas llegadas

Voy a ejecutar una búsqueda..."

[Ejecutar GET /api/vessels/search?query=MAERSK]

"Como pueden ver, obtenemos una lista de embarcaciones con su información básica."

### 2. Frontend
"Ahora el frontend, que está hecho con HTML, CSS y JavaScript puro, sin frameworks..."

[Abrir http://localhost:5001]

**Demostrar:**
1. Buscar una embarcación: "MAERSK"
2. Ver resultados en tabla
3. Click en "Ver Viaje" de uno
4. Mostrar información detallada
5. Resaltar el indicador de Santa Marta si aplica
6. Mostrar "Cargar Llegadas a Santa Marta"

"Decidí usar JavaScript puro para demostrar dominio de fundamentos web: manejo del DOM, eventos, async/await, fetch API. En producción, según la complejidad, consideraría React o Angular."

---

## Aspectos Técnicos Destacados (5-7 minutos)

### 1. Principios SOLID

"El código aplica los 5 principios SOLID:

**S - Single Responsibility:**
- Cada clase tiene una única responsabilidad
- VesselService solo maneja lógica de embarcaciones

**O - Open/Closed:**
- Puedo agregar nuevos servicios sin modificar los existentes

**L - Liskov Substitution:**
- Las implementaciones de interfaces son intercambiables

**I - Interface Segregation:**
- Interfaces específicas, no interfaces 'gordas'

**D - Dependency Inversion:**
- Dependemos de abstracciones, no de implementaciones
- VesselService depende de IMarineTrafficClient, no de la implementación HTTP

[Mostrar: inyección de dependencias en VesselService.cs]"

### 2. Resiliencia con Polly

"Para hacer la aplicación robusta ante fallos de red, implementé dos políticas de Polly:

**Política de Reintentos:**
- Maneja errores transitorios (5xx, 408, timeouts)
- Backoff exponencial: 2s, 4s, 8s
- Evita sobrecargar un servicio que ya está teniendo problemas

**Circuit Breaker:**
- Abre el circuito después de 5 fallos consecutivos
- Previene llamadas continuas a un servicio caído
- Se cierra automáticamente después de 30 segundos

[Mostrar código en ServiceCollectionExtensions.cs]

Esto combinado con HttpClientFactory nos da:
- Gestión eficiente del pool de conexiones
- Prevención de socket exhaustion
- Configuración centralizada"

### 3. Testing

"Implementé tests unitarios con xUnit y Moq:

[Mostrar: VesselServiceTests.cs]

- Tests de servicios con mocking de dependencias
- Tests de lógica de dominio
- Tests de mapeos
- Cobertura de casos exitosos y de error

Por ejemplo, este test verifica que cuando el cliente falla, el servicio maneja el error correctamente..."

[Mostrar un test específico]

### 4. Configuración Segura

"La API Key nunca está hardcodeada:

[Mostrar: appsettings.json y MarineTrafficOptions.cs]

- Uso del patrón IOptions<T> de .NET
- Configuración en appsettings.json para desarrollo
- Variables de entorno para producción
- appsettings.Development.json excluido de git"

---

## Posibles Extensiones (2-3 minutos)

"Este es un MVP, pero hay varias formas de extenderlo según necesidades del negocio:

**Funcionales:**
- Más puertos, no solo Santa Marta
- Historial de rutas de embarcaciones
- Notificaciones cuando un buque llegue
- Dashboard con métricas

**Técnicas:**
- Redis para cachear respuestas de la API externa
- Base de datos para persistir histórico
- JWT para autenticación de usuarios
- SignalR para updates en tiempo real
- Docker para containerización
- CI/CD con Azure DevOps o GitHub Actions

**Arquitectura:**
- Event Sourcing si necesitamos histórico completo
- CQRS para optimizar lecturas/escrituras
- Microservicios si hay necesidad de escalar componentes independientemente"

---

## Cierre (1 minuto)

"En resumen, este proyecto demuestra:

✅ Arquitectura limpia y escalable
✅ Principios SOLID aplicados
✅ Resiliencia ante fallos
✅ Testing de calidad
✅ Seguridad en manejo de secretos
✅ Frontend funcional sin frameworks
✅ Documentación completa

Todo el código está documentado y listo para producción. El README incluye instrucciones completas de setup, arquitectura y notas para extender el proyecto.

¿Tienen alguna pregunta sobre algún aspecto en particular?"

---

## Preguntas Frecuentes Anticipadas

### "¿Por qué Clean Architecture y no MVC tradicional?"

"Clean Architecture separa las preocupaciones en capas con dependencias unidireccionales hacia el núcleo. Esto hace que:
- La lógica de negocio sea independiente de frameworks y UI
- Sea más fácil de testear
- Podamos cambiar infraestructura sin afectar el negocio
- Sea más mantenible a largo plazo

MVC es excelente para aplicaciones simples, pero para sistemas empresariales donde la lógica de negocio es compleja y puede cambiar, Clean Architecture ofrece mejor separación y flexibilidad."

### "¿Cómo manejarías autenticación?"

"Implementaría JWT (JSON Web Tokens):
- Endpoint de login que devuelve un token
- Middleware de autenticación en el pipeline
- [Authorize] attribute en controladores
- Claims para roles y permisos
- Refresh tokens para sesiones largas
- Almacenamiento seguro en el frontend (httpOnly cookies o sessionStorage con precauciones)"

### "¿Qué harías diferente con más tiempo?"

"Con más tiempo agregaría:
1. Base de datos para persistencia
2. Caché distribuido (Redis)
3. Más tests (integración, E2E)
4. Logging estructurado (Serilog + Application Insights)
5. Health checks más completos
6. Rate limiting
7. API versioning
8. OpenAPI/Swagger mejorado con ejemplos
9. Docker y docker-compose
10. Pipeline de CI/CD"

### "¿Cómo escalarías esto?"

"Estrategia de escalamiento:

**Horizontal:**
- Containerizar con Docker
- Deploy en Kubernetes o Azure App Service
- Load balancer delante de múltiples instancias
- Redis para sesiones compartidas

**Vertical:**
- Optimizar queries y procesamiento
- Caché agresivo de datos de la API externa
- Connection pooling ya implementado con HttpClientFactory

**Arquitectura:**
- Si crece mucho, separar en microservicios
- Event-driven con mensajería (Azure Service Bus, RabbitMQ)
- CQRS para separar lecturas de escrituras
- Cache distribuido para reducir llamadas a API externa"

---

## Notas para el Presentador

- Tener el proyecto corriendo antes de la presentación
- Tener ejemplos de búsqueda preparados (MAERSK, 219018314, etc.)
- Practicar la navegación del código
- Tener el README abierto como referencia
- Preparar respuestas para preguntas sobre tecnologías específicas
- Mantener un ritmo que permita preguntas
- Mostrar entusiasmo por las decisiones técnicas tomadas
