using System.Text.Json;
using FilmSelector.Application.Interfaces;
using FilmSelector.Domain.Common;
using FilmSelector.Domain.Entities;
using FilmSelector.Infrastructure.Configuration;
using FilmSelector.Infrastructure.DTOs.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FilmSelector.Infrastructure.Clients;

/// <summary>
/// Implementación del cliente HTTP para OMDB API (Open Movie Database)
/// Usa HttpClientFactory para gestión eficiente de conexiones
/// Implementa la interfaz IOmdbClient (inversión de dependencias - SOLID)
/// </summary>
public class OmdbHttpClient : IOmdbClient
{
    private readonly HttpClient _httpClient;
    private readonly OmdbOptions _options;
    private readonly ILogger<OmdbHttpClient> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public OmdbHttpClient(
        HttpClient httpClient,
        IOptions<OmdbOptions> options,
        ILogger<OmdbHttpClient> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
        
        // Configuración de JSON para deserialización
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Configurar el HttpClient con la base URL y timeout
        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
    }

    /// <summary>
    /// Busca películas por título en la API de OMDB
    /// Endpoint: http://www.omdbapi.com/?s={title}&apikey={key}
    /// </summary>
    public async Task<Result<List<Movie>>> SearchMoviesAsync(
        string title, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Buscando películas con título: {Title}", title);

            // Construir URL con parámetros
            var url = $"?s={Uri.EscapeDataString(title)}&apikey={_options.ApiKey}";

            _logger.LogDebug("URL de la petición: {Url}", _httpClient.BaseAddress + url);

            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError(
                    "Error en la respuesta de OMDB. Status: {StatusCode}, Content: {Content}", 
                    response.StatusCode, 
                    errorContent);

                return Result<List<Movie>>.Failure(
                    $"Error al consultar OMDB API: {response.StatusCode}",
                    "OMDB_HTTP_ERROR");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Respuesta recibida de OMDB");

            // Deserializar la respuesta
            var searchResponse = JsonSerializer.Deserialize<OmdbSearchResponseDto>(content, _jsonOptions);

            if (searchResponse == null)
            {
                _logger.LogError("No se pudo deserializar la respuesta de OMDB");
                return Result<List<Movie>>.Failure(
                    "Error al procesar la respuesta de OMDB", 
                    "DESERIALIZATION_ERROR");
            }

            // Verificar si hubo error en la API
            if (searchResponse.Response == "False")
            {
                _logger.LogWarning("OMDB devolvió error: {Error}", searchResponse.Error);
                
                // Si es "Movie not found", es un caso válido (búsqueda sin resultados)
                if (searchResponse.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return Result<List<Movie>>.Success(new List<Movie>());
                }
                
                return Result<List<Movie>>.Failure(
                    searchResponse.Error ?? "Error desconocido de OMDB", 
                    "OMDB_API_ERROR");
            }

            // Mapear los resultados a entidades de dominio
            var movies = searchResponse.Search.Select(dto => new Movie
            {
                ImdbId = dto.ImdbId,
                Title = dto.Title,
                Year = dto.Year,
                Type = dto.Type,
                Poster = dto.Poster
            }).ToList();

            _logger.LogInformation("Se encontraron {Count} películas para '{Title}'", movies.Count, title);

            return Result<List<Movie>>.Success(movies);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout al llamar a OMDB API");
            return Result<List<Movie>>.Failure(
                "La petición excedió el tiempo de espera", 
                "TIMEOUT");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error HTTP al llamar a OMDB API");
            return Result<List<Movie>>.Failure(
                "Error de conexión con OMDB API", 
                "HTTP_ERROR");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error al deserializar la respuesta de OMDB");
            return Result<List<Movie>>.Failure(
                "Error al procesar la respuesta de OMDB", 
                "JSON_ERROR");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al buscar películas");
            return Result<List<Movie>>.Failure(
                "Error inesperado al buscar películas", 
                "UNEXPECTED_ERROR");
        }
    }

    /// <summary>
    /// Obtiene los detalles completos de una película por su ID de IMDb
    /// Endpoint: http://www.omdbapi.com/?i={imdbId}&apikey={key}
    /// </summary>
    public async Task<Result<MovieDetails>> GetMovieDetailsAsync(
        string imdbId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Obteniendo detalles de película: {ImdbId}", imdbId);

            // Construir URL con parámetros
            var url = $"?i={Uri.EscapeDataString(imdbId)}&apikey={_options.ApiKey}";

            _logger.LogDebug("URL de la petición: {Url}", _httpClient.BaseAddress + url);

            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError(
                    "Error en la respuesta de OMDB. Status: {StatusCode}, Content: {Content}", 
                    response.StatusCode, 
                    errorContent);

                return Result<MovieDetails>.Failure(
                    $"Error al consultar OMDB API: {response.StatusCode}",
                    "OMDB_HTTP_ERROR");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Respuesta recibida de OMDB");

            // Deserializar la respuesta
            var detailsDto = JsonSerializer.Deserialize<OmdbMovieDetailsDto>(content, _jsonOptions);

            if (detailsDto == null)
            {
                _logger.LogError("No se pudo deserializar la respuesta de OMDB");
                return Result<MovieDetails>.Failure(
                    "Error al procesar la respuesta de OMDB", 
                    "DESERIALIZATION_ERROR");
            }

            // Verificar si hubo error en la API
            if (detailsDto.Response == "False")
            {
                _logger.LogWarning("OMDB devolvió error: {Error}", detailsDto.Error);
                return Result<MovieDetails>.Failure(
                    detailsDto.Error ?? "Error desconocido de OMDB", 
                    "OMDB_API_ERROR");
            }

            // Mapear a entidad de dominio
            var movieDetails = new MovieDetails
            {
                ImdbId = detailsDto.ImdbId,
                Title = detailsDto.Title,
                Year = detailsDto.Year,
                Rated = detailsDto.Rated,
                Released = detailsDto.Released,
                Runtime = detailsDto.Runtime,
                Genre = detailsDto.Genre,
                Director = detailsDto.Director,
                Writer = detailsDto.Writer,
                Actors = detailsDto.Actors,
                Plot = detailsDto.Plot,
                Language = detailsDto.Language,
                Country = detailsDto.Country,
                Awards = detailsDto.Awards,
                Poster = detailsDto.Poster,
                ImdbRating = detailsDto.ImdbRating,
                ImdbVotes = detailsDto.ImdbVotes,
                Type = detailsDto.Type,
                BoxOffice = detailsDto.BoxOffice
            };

            _logger.LogInformation(
                "Detalles obtenidos. Título: {Title}, Rating: {Rating}, IsHighlyRated: {IsHighlyRated}", 
                movieDetails.Title, 
                movieDetails.ImdbRating,
                movieDetails.IsHighlyRated);

            return Result<MovieDetails>.Success(movieDetails);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout al llamar a OMDB API");
            return Result<MovieDetails>.Failure(
                "La petición excedió el tiempo de espera", 
                "TIMEOUT");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error HTTP al llamar a OMDB API");
            return Result<MovieDetails>.Failure(
                "Error de conexión con OMDB API", 
                "HTTP_ERROR");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error al deserializar la respuesta de OMDB");
            return Result<MovieDetails>.Failure(
                "Error al procesar la respuesta de OMDB", 
                "JSON_ERROR");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al obtener detalles de película");
            return Result<MovieDetails>.Failure(
                "Error inesperado al obtener detalles de película", 
                "UNEXPECTED_ERROR");
        }
    }
}

