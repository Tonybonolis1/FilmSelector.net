namespace FilmSelector.Domain.Entities;

/// <summary>
/// Representa una película en el sistema
/// </summary>
public class Movie
{
    public string ImdbId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Year { get; set; }
    public string? Type { get; set; }
    public string? Poster { get; set; }
}

