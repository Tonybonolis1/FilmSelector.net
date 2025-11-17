using MarineTraffic.Domain.Common;
using MarineTraffic.Domain.Entities;

namespace MarineTraffic.Application.Interfaces;

/// <summary>
/// Interfaz para el cliente de OMDB API
/// Siguiendo principio de inversión de dependencias (SOLID)
/// </summary>
public interface IOmdbClient
{
    /// <summary>
    /// Busca películas por título
    /// </summary>
    Task<Result<List<Movie>>> SearchMoviesAsync(string title, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtiene los detalles completos de una película por su IMDb ID
    /// </summary>
    Task<Result<MovieDetails>> GetMovieDetailsAsync(string imdbId, CancellationToken cancellationToken = default);
}
