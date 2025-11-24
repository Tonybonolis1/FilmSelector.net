using FilmSelector.Application.DTOs.Responses;
using FilmSelector.Application.Interfaces;
using FilmSelector.Application.Mappings;
using Microsoft.Extensions.Logging;

namespace FilmSelector.Application.Services;

/// <summary>
/// Servicio de aplicación para gestionar operaciones de películas
/// Implementa la lógica de negocio y orquesta las llamadas al cliente externo
/// </summary>
public class MovieService
{
    private readonly IOmdbClient _omdbClient;
    private readonly ILogger<MovieService> _logger;

    public MovieService(
        IOmdbClient omdbClient,
        ILogger<MovieService> logger)
    {
        _omdbClient = omdbClient;
        _logger = logger;
    }

    /// <summary>
    /// Busca películas por título
    /// </summary>
    public async Task<(bool Success, List<MovieSearchResponseDto>? Data, string? Error)> SearchMoviesAsync(
        string title, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            _logger.LogWarning("Búsqueda de películas con título vacío");
            return (false, null, "El título de búsqueda no puede estar vacío");
        }

        _logger.LogInformation("Buscando películas con título: {Title}", title);

        var result = await _omdbClient.SearchMoviesAsync(title, cancellationToken);

        if (!result.IsSuccess)
        {
            _logger.LogError("Error al buscar películas: {Error}", result.ErrorMessage);
            return (false, null, result.ErrorMessage);
        }

        if (result.Data == null || result.Data.Count == 0)
        {
            _logger.LogInformation("No se encontraron películas para: {Title}", title);
            return (true, new List<MovieSearchResponseDto>(), null);
        }

        var dtos = result.Data.ToSearchResponseDtoList();
        _logger.LogInformation("Se encontraron {Count} películas", dtos.Count);

        return (true, dtos, null);
    }

    /// <summary>
    /// Obtiene los detalles completos de una película
    /// </summary>
    public async Task<(bool Success, MovieDetailsResponseDto? Data, string? Error)> GetMovieDetailsAsync(
        string imdbId, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(imdbId))
        {
            _logger.LogWarning("Solicitud de detalles de película con ID vacío");
            return (false, null, "El ID de la película no puede estar vacío");
        }

        _logger.LogInformation("Obteniendo detalles de película: {ImdbId}", imdbId);

        var result = await _omdbClient.GetMovieDetailsAsync(imdbId, cancellationToken);

        if (!result.IsSuccess)
        {
            _logger.LogError("Error al obtener detalles de película: {Error}", result.ErrorMessage);
            return (false, null, result.ErrorMessage);
        }

        if (result.Data == null)
        {
            _logger.LogWarning("No se encontraron detalles para película: {ImdbId}", imdbId);
            return (false, null, "No se encontraron detalles para esta película");
        }

        var dto = result.Data.ToResponseDto();
        
        _logger.LogInformation(
            "Detalles obtenidos. Título: {Title}, Rating: {Rating}, Altamente valorada: {IsHighlyRated}", 
            dto.Title, 
            dto.ImdbRating,
            dto.IsHighlyRated);

        return (true, dto, null);
    }
}

