using MarineTraffic.Application.Interfaces;
using MarineTraffic.Application.Services;
using MarineTraffic.Domain.Interfaces;
using MarineTraffic.Infrastructure.Clients;
using MarineTraffic.Infrastructure.Configuration;
using MarineTraffic.Infrastructure.Data;
using MarineTraffic.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace MarineTraffic.Infrastructure.Extensions;

/// <summary>
/// Extensiones para configurar los servicios de infraestructura
/// Implementa HttpClientFactory con Polly para resiliencia
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Agrega los servicios de infraestructura al contenedor de DI
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configurar Entity Framework Core con SQL Server
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("MarineTraffic.Infrastructure")));

        // Registrar repositorios
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();

        // Configurar opciones usando el patrón IOptions
        services.Configure<OmdbOptions>(
            configuration.GetSection(OmdbOptions.SectionName));

        // Obtener opciones para configurar Polly
        var omdbOptions = configuration
            .GetSection(OmdbOptions.SectionName)
            .Get<OmdbOptions>() ?? new OmdbOptions();

        // Registrar HttpClient con Polly para OmdbHttpClient
        services.AddHttpClient<IOmdbClient, OmdbHttpClient>(client =>
        {
            client.BaseAddress = new Uri(omdbOptions.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(omdbOptions.TimeoutSeconds);
        })
        .AddPolicyHandler(GetRetryPolicy(omdbOptions))
        .AddPolicyHandler(GetCircuitBreakerPolicy());

        return services;
    }

    /// <summary>
    /// Agrega los servicios de aplicación al contenedor de DI
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<MovieService>();

        return services;
    }

    /// <summary>
    /// Política de reintentos con backoff exponencial
    /// Implementa el patrón de resiliencia recomendado
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(OmdbOptions options)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError() // Maneja 5xx y 408
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests) // Maneja 429
            .WaitAndRetryAsync(
                retryCount: options.RetryCount,
                sleepDurationProvider: retryAttempt => 
                    TimeSpan.FromSeconds(Math.Pow(options.RetryBackoffSeconds, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    // Log de reintento (se puede inyectar ILogger aquí en versiones más avanzadas)
                    Console.WriteLine($"Reintento {retryAttempt} después de {timespan.TotalSeconds}s debido a: {outcome.Result?.StatusCode}");
                });
    }

    /// <summary>
    /// Política de circuit breaker
    /// Previene llamadas continuas a un servicio que está fallando
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5, // Abre el circuito después de 5 fallos
                durationOfBreak: TimeSpan.FromSeconds(30), // Mantiene el circuito abierto por 30 segundos
                onBreak: (outcome, duration) =>
                {
                    Console.WriteLine($"Circuit breaker abierto por {duration.TotalSeconds}s debido a: {outcome.Result?.StatusCode}");
                },
                onReset: () =>
                {
                    Console.WriteLine("Circuit breaker cerrado, reiniciando llamadas normales");
                });
    }
}
