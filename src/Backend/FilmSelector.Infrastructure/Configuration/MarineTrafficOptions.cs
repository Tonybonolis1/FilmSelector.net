namespace FilmSelector.Infrastructure.Configuration;

/// <summary>
/// Opciones de configuración para OMDB API
/// Usa el patrón IOptions<T> de .NET
/// </summary>
public class OmdbOptions
{
    public const string SectionName = "Omdb";

    /// <summary>
    /// URL base de la API de OMDB
    /// </summary>
    public string BaseUrl { get; set; } = "http://www.omdbapi.com";

    /// <summary>
    /// API Key para autenticación
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Timeout en segundos para las peticiones HTTP
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Número de reintentos en caso de fallo
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Tiempo base de espera entre reintentos (en segundos)
    /// Se usa backoff exponencial
    /// </summary>
    public int RetryBackoffSeconds { get; set; } = 2;
}

