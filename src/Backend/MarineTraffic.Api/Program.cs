using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MarineTraffic.Api.Middleware;
using MarineTraffic.Application.Interfaces;
using MarineTraffic.Application.Services;
using MarineTraffic.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "OMDB Movie API",
        Version = "v1",
        Description = "API para consultar información de películas usando OMDB (Open Movie Database)",
        Contact = new()
        {
            Name = "Proyecto de ejemplo",
            Email = "contact@example.com"
        }
    });

    // Incluir comentarios XML si existen
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configurar CORS para permitir el frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Agregar servicios de infraestructura y aplicación
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();

// Registrar servicios de autenticación y favoritos
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();

// Configurar JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "MySecretKeyForJWT2024MarineTraffic123456";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "MarineTrafficApi";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "MarineTrafficClient";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OMDB Movie API v1");
        c.RoutePrefix = "swagger";
    });
}

// Middleware de manejo de excepciones
app.UseExceptionHandling();

// Servir archivos estáticos del frontend
app.UseStaticFiles();
app.UseDefaultFiles();

// CORS debe ir antes de Authorization
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Endpoint de health check
app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
}));

app.Run();
