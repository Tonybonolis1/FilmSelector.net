# Guía Rápida de Inicio - MarineTraffic

## 🚀 Inicio Rápido (5 minutos)

### 1. Configurar API Key
```powershell
# Editar este archivo:
src\Backend\MarineTraffic.Api\appsettings.Development.json

# Cambiar:
"ApiKey": "YOUR_DEVELOPMENT_API_KEY_HERE"
```

### 2. Ejecutar Backend
```powershell
cd src\Backend\MarineTraffic.Api
dotnet run
```

### 3. Abrir Frontend
Abrir en el navegador: http://localhost:5001

### 4. Probar
1. En la búsqueda, escribir: **MAERSK** (o cualquier MMSI/IMO)
2. Click en "Buscar"
3. Click en "Ver Viaje" en algún resultado
4. Verificar si el destino es Santa Marta 🎯

---

## 📋 Comandos Útiles

### Compilar
```powershell
dotnet build
```

### Ejecutar Tests
```powershell
dotnet test
```

### Ver Swagger
http://localhost:5001/swagger

### Limpiar
```powershell
dotnet clean
```

---

## 🐛 Troubleshooting

### Error: "API Key is invalid"
- Verificar que la API Key en appsettings.Development.json sea correcta
- Obtener una nueva en: https://www.marinetraffic.com/en/ais-api-services

### Error: "Port 5001 already in use"
- Cambiar el puerto en `src\Backend\MarineTraffic.Api\Properties\launchSettings.json`
- Actualizar también en `src\Frontend\js\app.js`

### Frontend no carga
- Verificar que el backend esté corriendo
- Abrir consola del navegador (F12) para ver errores
- Verificar la URL de la API en `app.js`

### Tests fallan
```powershell
# Restaurar paquetes
dotnet restore

# Limpiar y compilar
dotnet clean
dotnet build
```

---

## 📞 Contacto

Si tienes preguntas sobre el proyecto, revisar el README.md completo.
