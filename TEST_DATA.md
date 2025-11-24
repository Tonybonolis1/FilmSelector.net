# Datos de Ejemplo para Pruebas

Este archivo contiene ejemplos de consultas y datos que puedes usar para probar la aplicación.

## 🔍 Ejemplos de Búsqueda

### Por Nombre de Embarcación
```
MAERSK
MSC
EVERGREEN
CMA CGM
HAPAG
```

### Por MMSI (9 dígitos)
```
219018314   - Ejemplo de buque de carga
636019825   - Ejemplo de buque portacontenedores
538006090   - Ejemplo de buque tanque
```

### Por IMO (7 dígitos)
```
9632179     - Ejemplo de buque moderno
9305465     - Ejemplo de buque antiguo
9839012     - Ejemplo de mega portacontenedores
```

---

## 🚢 Ejemplos de Embarcaciones Reales (para referencia)

### MAERSK ESSEX
- **MMSI:** 219018314
- **IMO:** 9632179
- **Tipo:** Container Ship
- **Bandera:** Denmark (DK)
- **Descripción:** Portacontenedores de la flota Maersk

### MSC GÜLSÜN
- **MMSI:** 372618000
- **IMO:** 9839012
- **Tipo:** Container Ship
- **Bandera:** Panama (PA)
- **Descripción:** Uno de los portacontenedores más grandes del mundo

### EVER GIVEN
- **MMSI:** 353136000
- **IMO:** 9811000
- **Tipo:** Container Ship
- **Bandera:** Panama (PA)
- **Descripción:** Famoso por el incidente del Canal de Suez en 2021

---

## 🏙️ Códigos UNLOCODE de Puertos Colombianos

### Santa Marta
- **UNLOCODE:** COSMR
- **Alternativa:** CO SMR
- **País:** Colombia
- **Coordenadas:** 11.2472°N, 74.2017°W

### Cartagena
- **UNLOCODE:** COCTG
- **Alternativa:** CO CTG
- **País:** Colombia

### Barranquilla
- **UNLOCODE:** COBAQ
- **Alternativa:** CO BAQ
- **País:** Colombia

### Buenaventura
- **UNLOCODE:** COBUN
- **Alternativa:** CO BUN
- **País:** Colombia

---

## 📊 Ejemplos de Respuestas de la API (Mock Data)

### Búsqueda de Embarcaciones - Respuesta Exitosa
```json
[
  {
    "id": "12345",
    "name": "MAERSK ESSEX",
    "mmsi": "219018314",
    "imo": "9632179",
    "shipType": "Container Ship",
    "flag": "DK"
  },
  {
    "id": "67890",
    "name": "MAERSK ELBA",
    "mmsi": "219742000",
    "imo": "9744029",
    "shipType": "Container Ship",
    "flag": "DK"
  }
]
```

### Información de Viaje - Destino Santa Marta
```json
{
  "vesselId": "12345",
  "vesselName": "MAERSK ESSEX",
  "destinationPort": "SANTA MARTA",
  "destinationCountry": "Colombia",
  "estimatedTimeOfArrival": "2024-12-25T14:30:00Z",
  "voyageStatus": "Under way using engine",
  "currentLatitude": 11.2472,
  "currentLongitude": -74.2017,
  "currentSpeed": 15.5,
  "departurePort": "KINGSTON",
  "departureTime": "2024-12-20T08:00:00Z",
  "isDestinationSantaMarta": true
}
```

### Información de Viaje - Otro Destino
```json
{
  "vesselId": "67890",
  "vesselName": "MSC MEDITERRANEAN",
  "destinationPort": "CARTAGENA",
  "destinationCountry": "Colombia",
  "estimatedTimeOfArrival": "2024-12-28T10:00:00Z",
  "voyageStatus": "Under way using engine",
  "currentLatitude": 10.4236,
  "currentLongitude": -75.5243,
  "currentSpeed": 18.2,
  "departurePort": "COLON",
  "departureTime": "2024-12-24T06:00:00Z",
  "isDestinationSantaMarta": false
}
```

### Llegadas a Santa Marta
```json
[
  {
    "vesselId": "11111",
    "vesselName": "CARIBBEAN SUNRISE",
    "mmsi": "308123456",
    "imo": "9123456",
    "shipType": "Cargo",
    "flag": "BS",
    "originPort": "KINGSTON",
    "estimatedTimeOfArrival": "2024-12-26T08:00:00Z",
    "distanceToPort": 234.5
  },
  {
    "vesselId": "22222",
    "vesselName": "ATLANTIC CARRIER",
    "mmsi": "538234567",
    "imo": "9234567",
    "shipType": "Container Ship",
    "flag": "MH",
    "originPort": "MIAMI",
    "estimatedTimeOfArrival": "2024-12-26T14:30:00Z",
    "distanceToPort": 456.8
  }
]
```

---

## 🧪 Escenarios de Prueba

### Escenario 1: Búsqueda Exitosa
1. Ingresar: **MAERSK**
2. Resultado esperado: Lista de embarcaciones Maersk
3. Verificar: Tabla con al menos 1 resultado

### Escenario 2: Embarcación con Destino Santa Marta
1. Buscar una embarcación
2. Click en "Ver Viaje"
3. Verificar: Badge verde "SANTA MARTA" si aplica
4. Verificar: `isDestinationSantaMarta: true`

### Escenario 3: Embarcación con Otro Destino
1. Buscar una embarcación
2. Click en "Ver Viaje"
3. Verificar: No aparece badge de Santa Marta
4. Verificar: `isDestinationSantaMarta: false`

### Escenario 4: Llegadas a Puerto
1. Click en "Cargar Llegadas"
2. Resultado esperado: Tabla con próximas llegadas
3. Verificar: Columnas ETA, Origen, Distancia

### Escenario 5: Búsqueda sin Resultados
1. Ingresar: **XXXNONEXISTENTXXX**
2. Resultado esperado: Mensaje "No se encontraron embarcaciones"
3. Verificar: No se muestra tabla

### Escenario 6: Manejo de Errores
1. Detener el backend
2. Intentar una búsqueda
3. Resultado esperado: Mensaje de error amigable
4. Verificar: No se rompe la aplicación

---

## 🌐 URLs de Prueba

### Desarrollo Local
- **API Base:** http://localhost:5001/api
- **Frontend:** http://localhost:5001
- **Swagger:** http://localhost:5001/swagger
- **Health Check:** http://localhost:5001/health

### Endpoints Específicos
```
GET http://localhost:5001/api/vessels/search?query=MAERSK
GET http://localhost:5001/api/vessels/12345/voyage
GET http://localhost:5001/api/ports/santamarta/arrivals
```

---

## 📝 Notas Adicionales

### Limitaciones de la API de FilmSelector (versión gratuita)
- Límite de llamadas por mes
- Datos pueden no estar actualizados en tiempo real
- Algunos buques pueden no tener información completa

### Alternativas para Pruebas sin API Key
Si no tienes una API Key válida, puedes:
1. Modificar `FilmSelectorHttpClient.cs` para retornar datos mock
2. Usar los ejemplos JSON de arriba
3. Implementar un `FakeFilmSelectorClient` para testing

### Formato de Fechas
Las fechas en la API se manejan en formato ISO 8601:
```
2024-12-25T14:30:00Z
```

El frontend las formatea automáticamente a formato local colombiano.

---

## 🎯 Checklist de Pruebas Completas

- [ ] Búsqueda por nombre funciona
- [ ] Búsqueda por MMSI funciona
- [ ] Búsqueda por IMO funciona
- [ ] Ver viaje muestra información completa
- [ ] Identificación de Santa Marta funciona
- [ ] Llegadas a Santa Marta carga correctamente
- [ ] Manejo de errores muestra mensajes amigables
- [ ] Estados de carga se muestran correctamente
- [ ] Diseño es responsivo en móvil
- [ ] No hay errores en consola del navegador
- [ ] Tests unitarios pasan (dotnet test)
- [ ] Swagger documenta correctamente los endpoints

---

**¡Listo para demostrar! 🚀**
