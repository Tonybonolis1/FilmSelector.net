using System.Text.Json.Serialization;

namespace MarineTraffic.Infrastructure.DTOs.External;

/// <summary>
/// DTO que representa la respuesta de la API OMDB para búsqueda de películas
/// Mapea directamente con el JSON de respuesta de: http://www.omdbapi.com/?s=title&apikey=xxx
/// </summary>
public class OmdbSearchResponseDto
{
    /// <summary>
    /// Lista de películas encontradas
    /// </summary>
    [JsonPropertyName("Search")]
    public List<OmdbMovieDto> Search { get; set; } = new();

    /// <summary>
    /// Total de resultados disponibles
    /// </summary>
    [JsonPropertyName("totalResults")]
    public string TotalResults { get; set; } = string.Empty;

    /// <summary>
    /// Indica si la búsqueda fue exitosa
    /// </summary>
    [JsonPropertyName("Response")]
    public string Response { get; set; } = string.Empty;

    /// <summary>
    /// Mensaje de error si Response = "False"
    /// </summary>
    [JsonPropertyName("Error")]
    public string? Error { get; set; }
}

/// <summary>
/// DTO que representa una película individual en los resultados de búsqueda
/// </summary>
public class OmdbMovieDto
{
    /// <summary>
    /// ID de IMDb (e.g., "tt3896198")
    /// </summary>
    [JsonPropertyName("imdbID")]
    public string ImdbId { get; set; } = string.Empty;

    /// <summary>
    /// Título de la película
    /// </summary>
    [JsonPropertyName("Title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Año de lanzamiento
    /// </summary>
    [JsonPropertyName("Year")]
    public string Year { get; set; } = string.Empty;

    /// <summary>
    /// Tipo (movie, series, episode)
    /// </summary>
    [JsonPropertyName("Type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// URL del póster
    /// </summary>
    [JsonPropertyName("Poster")]
    public string Poster { get; set; } = string.Empty;
}
