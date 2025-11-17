# Guía de Deployment - MarineTraffic

## 🚀 Opciones de Deployment

### Opción 1: Azure App Service (Recomendado)

#### Requisitos
- Cuenta de Azure
- Azure CLI instalado

#### Pasos

1. **Crear App Service**
```bash
# Login en Azure
az login

# Crear grupo de recursos
az group create --name rg-marinetraffic --location eastus

# Crear App Service Plan
az appservice plan create --name plan-marinetraffic --resource-group rg-marinetraffic --sku B1 --is-linux

# Crear Web App
az webapp create --resource-group rg-marinetraffic --plan plan-marinetraffic --name app-marinetraffic --runtime "DOTNET|8.0"
```

2. **Configurar Variables de Entorno**
```bash
az webapp config appsettings set --resource-group rg-marinetraffic --name app-marinetraffic --settings \
  MarineTraffic__ApiKey="TU_API_KEY" \
  MarineTraffic__BaseUrl="https://services.marinetraffic.com/api" \
  MarineTraffic__TimeoutSeconds="30" \
  MarineTraffic__RetryCount="3" \
  MarineTraffic__RetryBackoffSeconds="2"
```

3. **Publicar desde Visual Studio**
- Click derecho en `MarineTraffic.Api` > Publicar
- Seleccionar Azure > Azure App Service (Linux)
- Seleccionar tu suscripción y la app creada
- Publicar

4. **O Publicar desde CLI**
```bash
cd src/Backend/MarineTraffic.Api
dotnet publish -c Release -o ./publish
cd publish
zip -r app.zip .
az webapp deployment source config-zip --resource-group rg-marinetraffic --name app-marinetraffic --src app.zip
```

---

### Opción 2: Docker + Docker Compose

#### 1. Crear Dockerfile

```dockerfile
# En: src/Backend/MarineTraffic.Api/Dockerfile

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Backend/MarineTraffic.Api/MarineTraffic.Api.csproj", "MarineTraffic.Api/"]
COPY ["src/Backend/MarineTraffic.Application/MarineTraffic.Application.csproj", "MarineTraffic.Application/"]
COPY ["src/Backend/MarineTraffic.Domain/MarineTraffic.Domain.csproj", "MarineTraffic.Domain/"]
COPY ["src/Backend/MarineTraffic.Infrastructure/MarineTraffic.Infrastructure.csproj", "MarineTraffic.Infrastructure/"]

RUN dotnet restore "MarineTraffic.Api/MarineTraffic.Api.csproj"
COPY src/Backend/ .
WORKDIR "/src/MarineTraffic.Api"
RUN dotnet build "MarineTraffic.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarineTraffic.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarineTraffic.Api.dll"]
```

#### 2. Crear docker-compose.yml

```yaml
# En la raíz del proyecto

version: '3.8'

services:
  marinetraffic-api:
    build:
      context: .
      dockerfile: src/Backend/MarineTraffic.Api/Dockerfile
    ports:
      - "5001:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - MarineTraffic__ApiKey=${MARINETRAFFIC_API_KEY}
      - MarineTraffic__BaseUrl=https://services.marinetraffic.com/api
      - MarineTraffic__TimeoutSeconds=30
      - MarineTraffic__RetryCount=3
      - MarineTraffic__RetryBackoffSeconds=2
    restart: unless-stopped
```

#### 3. Crear .env

```env
# .env (no subir a git)
MARINETRAFFIC_API_KEY=tu_api_key_aqui
```

#### 4. Ejecutar

```bash
# Build y run
docker-compose up --build

# En background
docker-compose up -d

# Ver logs
docker-compose logs -f

# Detener
docker-compose down
```

---

### Opción 3: IIS (Windows Server)

#### Requisitos
- Windows Server con IIS instalado
- .NET 8 Runtime instalado

#### Pasos

1. **Publicar la aplicación**
```powershell
cd src\Backend\MarineTraffic.Api
dotnet publish -c Release -o C:\inetpub\wwwroot\marinetraffic
```

2. **Configurar IIS**
- Abrir IIS Manager
- Crear nuevo Application Pool (.NET CLR: No Managed Code)
- Crear nuevo sitio web
- Apuntar al directorio de publicación
- Configurar bindings (puerto 80/443)

3. **Configurar Variables de Entorno**
- En IIS Manager > Sitio > Configuration Editor
- Section: system.webServer/aspNetCore
- Agregar variables de entorno en environmentVariables

4. **Instalar ASP.NET Core Hosting Bundle**
```
https://dotnet.microsoft.com/download/dotnet/8.0
```

---

### Opción 4: Linux VPS (Ubuntu)

#### 1. Preparar el servidor

```bash
# Actualizar sistema
sudo apt update && sudo apt upgrade -y

# Instalar .NET 8 Runtime
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0 --runtime aspnetcore

# Instalar Nginx
sudo apt install nginx -y
```

#### 2. Publicar y transferir

```bash
# Local
dotnet publish -c Release -o ./publish

# Transferir con SCP
scp -r ./publish user@servidor:/var/www/marinetraffic
```

#### 3. Crear servicio systemd

```bash
# /etc/systemd/system/marinetraffic.service

[Unit]
Description=MarineTraffic API
After=network.target

[Service]
WorkingDirectory=/var/www/marinetraffic
ExecStart=/home/user/.dotnet/dotnet /var/www/marinetraffic/MarineTraffic.Api.dll
Restart=always
RestartSec=10
SyslogIdentifier=marinetraffic
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=MarineTraffic__ApiKey=TU_API_KEY

[Install]
WantedBy=multi-user.target
```

#### 4. Configurar Nginx

```nginx
# /etc/nginx/sites-available/marinetraffic

server {
    listen 80;
    server_name tu-dominio.com;
    
    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

#### 5. Activar y ejecutar

```bash
# Activar servicio
sudo systemctl enable marinetraffic
sudo systemctl start marinetraffic

# Activar Nginx
sudo ln -s /etc/nginx/sites-available/marinetraffic /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl restart nginx

# Ver logs
sudo journalctl -u marinetraffic -f
```

---

## 🔒 Seguridad en Producción

### 1. HTTPS/SSL

#### Azure App Service
```bash
# Configurar dominio personalizado
az webapp config hostname add --resource-group rg-marinetraffic --webapp-name app-marinetraffic --hostname tu-dominio.com

# Habilitar HTTPS
az webapp update --resource-group rg-marinetraffic --name app-marinetraffic --https-only true
```

#### Let's Encrypt (Linux)
```bash
sudo apt install certbot python3-certbot-nginx
sudo certbot --nginx -d tu-dominio.com
```

### 2. API Key Management

**Nunca exponer la API Key:**
- ✅ Variables de entorno
- ✅ Azure Key Vault
- ✅ AWS Secrets Manager
- ❌ Nunca en código fuente
- ❌ Nunca en archivos de configuración versionados

### 3. Rate Limiting

Agregar en `Program.cs`:
```csharp
using Microsoft.AspNetCore.RateLimiting;

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromMinutes(1);
        options.PermitLimit = 60;
    });
});

app.UseRateLimiter();
```

### 4. CORS en Producción

Actualizar `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins("https://tu-dominio.com")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

---

## 📊 Monitoreo

### Application Insights (Azure)

```bash
# Agregar paquete
dotnet add package Microsoft.ApplicationInsights.AspNetCore

# En Program.cs
builder.Services.AddApplicationInsightsTelemetry();
```

### Logging con Serilog

```bash
# Agregar paquetes
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File

# En Program.cs
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
```

---

## 🔄 CI/CD

### GitHub Actions

Crear `.github/workflows/deploy.yml`:

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --configuration Release --no-restore
    
    - name: Test
      run: dotnet test --no-restore --verbosity normal
    
    - name: Publish
      run: dotnet publish src/Backend/MarineTraffic.Api/MarineTraffic.Api.csproj -c Release -o ./publish
    
    - name: Deploy to Azure
      uses: azure/webapps-deploy@v2
      with:
        app-name: app-marinetraffic
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ./publish
```

---

## ✅ Checklist Pre-Deployment

- [ ] Tests pasan (dotnet test)
- [ ] Configuración de producción lista
- [ ] API Key configurada en variables de entorno
- [ ] HTTPS habilitado
- [ ] CORS configurado para dominio de producción
- [ ] Logging configurado
- [ ] Monitoreo configurado (opcional)
- [ ] Rate limiting habilitado (opcional)
- [ ] Documentación actualizada
- [ ] Health check funcionando
- [ ] Frontend apunta a la URL correcta de producción

---

## 🆘 Rollback

### Azure App Service
```bash
# Ver deployment slots
az webapp deployment slot list --resource-group rg-marinetraffic --name app-marinetraffic

# Swap slots
az webapp deployment slot swap --resource-group rg-marinetraffic --name app-marinetraffic --slot staging --target-slot production
```

### Docker
```bash
# Volver a imagen anterior
docker-compose down
docker-compose up -d --build <commit-anterior>
```

---

## 📞 Soporte

Para problemas de deployment:
1. Verificar logs
2. Revisar configuración de variables de entorno
3. Validar que el health check funcione
4. Probar endpoints con Postman/curl

---

**¡Listo para producción! 🎉**
