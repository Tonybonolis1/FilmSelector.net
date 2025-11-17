using MarineTraffic.Application.DTOs.Responses;
using MarineTraffic.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarineTraffic.Api.Controllers;

/// <summary>
/// Controlador para operaciones relacionadas con películas
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MoviesController : ControllerBase
{
    private readonly MovieService _movieService;
    private readonly ILogger<MoviesController> _logger;

    public MoviesController(
        MovieService movieService,
        ILogger<MoviesController> logger)
    {
        _movieService = movieService;
        _logger = logger;
    }

    /// <summary>
    /// Busca películas por título
    /// </summary>
    /// <param name="title">Título de la película a buscar</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Lista de películas que coinciden con el título</returns>
    /// <response code="200">Búsqueda exitosa</response>
    /// <response code="400">Parámetro de búsqueda inválido</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<MovieSearchResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchMovies(
        [FromQuery] string title,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Recibida solicitud de búsqueda de películas: {Title}", title);

        if (string.IsNullOrWhiteSpace(title))
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "El título de búsqueda es requerido",
                Code = "INVALID_TITLE"
            });
        }

        var (success, data, error) = await _movieService.SearchMoviesAsync(title, cancellationToken);

        if (!success)
        {
            _logger.LogWarning("Error en búsqueda de películas: {Error}", error);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
            {
                Message = error ?? "Error al buscar películas",
                Code = "SEARCH_ERROR"
            });
        }

        return Ok(data ?? new List<MovieSearchResponseDto>());
    }

    /// <summary>
    /// Obtiene los detalles completos de una película específica
    /// </summary>
    /// <param name="imdbId">ID de IMDb de la película (e.g., tt3896198)</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Detalles completos de la película</returns>
    /// <response code="200">Detalles obtenidos exitosamente</response>
    /// <response code="400">ID de película inválido</response>
    /// <response code="404">Película no encontrada</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpGet("{imdbId}")]
    [ProducesResponseType(typeof(MovieDetailsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMovieDetails(
        string imdbId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Recibida solicitud de detalles para película: {ImdbId}", imdbId);

        if (string.IsNullOrWhiteSpace(imdbId))
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "El ID de IMDb es requerido",
                Code = "INVALID_IMDB_ID"
            });
        }

        var (success, data, error) = await _movieService.GetMovieDetailsAsync(imdbId, cancellationToken);

        if (!success)
        {
            _logger.LogWarning("Error al obtener detalles de película: {Error}", error);
            
            if (error?.Contains("no encontr", StringComparison.OrdinalIgnoreCase) == true)
            {
                return NotFound(new ErrorResponseDto
                {
                    Message = error,
                    Code = "MOVIE_NOT_FOUND"
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
            {
                Message = error ?? "Error al obtener detalles de película",
                Code = "MOVIE_ERROR"
            });
        }

        return Ok(data);
    }
}
