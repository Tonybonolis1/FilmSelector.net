using System.Net;
using System.Text.Json;
using MarineTraffic.Application.DTOs.Responses;

namespace MarineTraffic.Api.Middleware;

/// <summary>
/// Middleware para manejo global de excepciones
/// Captura excepciones no controladas y las transforma en respuestas HTTP apropiadas
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no controlada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var errorResponse = new ErrorResponseDto
        {
            Message = "Ha ocurrido un error interno en el servidor",
            Code = "INTERNAL_ERROR",
            Details = new Dictionary<string, string>
            {
                { "type", exception.GetType().Name }
            }
        };

        // En desarrollo, incluir detalles de la excepción
        var isDevelopment = context.RequestServices
            .GetRequiredService<IWebHostEnvironment>()
            .IsDevelopment();

        if (isDevelopment)
        {
            errorResponse.Details["message"] = exception.Message;
            errorResponse.Details["stackTrace"] = exception.StackTrace ?? string.Empty;
        }

        context.Response.StatusCode = exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(errorResponse, options);
        await context.Response.WriteAsync(json);
    }
}

/// <summary>
/// Extensión para facilitar el registro del middleware
/// </summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
