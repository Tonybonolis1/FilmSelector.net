namespace FilmSelector.Application.DTOs.Responses;

/// <summary>
/// DTO para respuesta de búsqueda de películas
/// </summary>
public class MovieSearchResponseDto
{
    public string ImdbId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Year { get; set; }
    public string? Type { get; set; }
    public string? Poster { get; set; }
}

