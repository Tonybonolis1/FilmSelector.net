using System.Text.Json.Serialization;

namespace FilmSelector.Infrastructure.DTOs.External;

/// <summary>
/// DTO que representa la respuesta detallada de la API OMDB para una película específica
/// Mapea directamente con el JSON de respuesta de: http://www.omdbapi.com/?i=imdbId&apikey=xxx
/// </summary>
public class OmdbMovieDetailsDto
{
    /// <summary>
    /// ID de IMDb
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
    /// Clasificación (G, PG, PG-13, R, etc.)
    /// </summary>
    [JsonPropertyName("Rated")]
    public string Rated { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de lanzamiento
    /// </summary>
    [JsonPropertyName("Released")]
    public string Released { get; set; } = string.Empty;

    /// <summary>
    /// Duración (e.g., "148 min")
    /// </summary>
    [JsonPropertyName("Runtime")]
    public string Runtime { get; set; } = string.Empty;

    /// <summary>
    /// Géneros (separados por coma)
    /// </summary>
    [JsonPropertyName("Genre")]
    public string Genre { get; set; } = string.Empty;

    /// <summary>
    /// Director(es)
    /// </summary>
    [JsonPropertyName("Director")]
    public string Director { get; set; } = string.Empty;

    /// <summary>
    /// Guionista(s)
    /// </summary>
    [JsonPropertyName("Writer")]
    public string Writer { get; set; } = string.Empty;

    /// <summary>
    /// Actores principales
    /// </summary>
    [JsonPropertyName("Actors")]
    public string Actors { get; set; } = string.Empty;

    /// <summary>
    /// Sinopsis de la película
    /// </summary>
    [JsonPropertyName("Plot")]
    public string Plot { get; set; } = string.Empty;

    /// <summary>
    /// Idioma(s)
    /// </summary>
    [JsonPropertyName("Language")]
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// País(es) de producción
    /// </summary>
    [JsonPropertyName("Country")]
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Premios y nominaciones
    /// </summary>
    [JsonPropertyName("Awards")]
    public string Awards { get; set; } = string.Empty;

    /// <summary>
    /// URL del póster
    /// </summary>
    [JsonPropertyName("Poster")]
    public string Poster { get; set; } = string.Empty;

    /// <summary>
    /// Calificación de IMDb (e.g., "8.4")
    /// </summary>
    [JsonPropertyName("imdbRating")]
    public string ImdbRating { get; set; } = string.Empty;

    /// <summary>
    /// Número de votos en IMDb
    /// </summary>
    [JsonPropertyName("imdbVotes")]
    public string ImdbVotes { get; set; } = string.Empty;

    /// <summary>
    /// Tipo (movie, series, episode)
    /// </summary>
    [JsonPropertyName("Type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Recaudación en taquilla
    /// </summary>
    [JsonPropertyName("BoxOffice")]
    public string? BoxOffice { get; set; }

    /// <summary>
    /// Indica si la petición fue exitosa
    /// </summary>
    [JsonPropertyName("Response")]
    public string Response { get; set; } = string.Empty;

    /// <summary>
    /// Mensaje de error si Response = "False"
    /// </summary>
    [JsonPropertyName("Error")]
    public string? Error { get; set; }
}

