using MarineTraffic.Application.DTOs.Responses;
using MarineTraffic.Domain.Entities;

namespace MarineTraffic.Application.Mappings;

/// <summary>
/// Métodos de extensión para mapear entidades de dominio a DTOs de respuesta
/// Implementa el patrón de mapeo manual (evitando dependencias como AutoMapper)
/// </summary>
public static class MovieMappings
{
    /// <summary>
    /// Convierte una entidad Movie a su DTO de respuesta para búsqueda
    /// </summary>
    public static MovieSearchResponseDto ToSearchResponseDto(this Movie movie)
    {
        return new MovieSearchResponseDto
        {
            ImdbId = movie.ImdbId,
            Title = movie.Title,
            Year = movie.Year,
            Type = movie.Type,
            Poster = movie.Poster
        };
    }

    /// <summary>
    /// Convierte una lista de Movies a una lista de DTOs de búsqueda
    /// </summary>
    public static List<MovieSearchResponseDto> ToSearchResponseDtoList(this List<Movie> movies)
    {
        return movies.Select(m => m.ToSearchResponseDto()).ToList();
    }

    /// <summary>
    /// Convierte una entidad MovieDetails a su DTO de respuesta completo
    /// </summary>
    public static MovieDetailsResponseDto ToResponseDto(this MovieDetails details)
    {
        return new MovieDetailsResponseDto
        {
            ImdbId = details.ImdbId,
            Title = details.Title,
            Year = details.Year,
            Rated = details.Rated,
            Released = details.Released,
            Runtime = details.Runtime,
            Genre = details.Genre,
            Director = details.Director,
            Writer = details.Writer,
            Actors = details.Actors,
            Plot = details.Plot,
            Language = details.Language,
            Country = details.Country,
            Awards = details.Awards,
            Poster = details.Poster,
            ImdbRating = details.ImdbRating,
            ImdbVotes = details.ImdbVotes,
            Type = details.Type,
            BoxOffice = details.BoxOffice,
            IsHighlyRated = details.IsHighlyRated
        };
    }
}
