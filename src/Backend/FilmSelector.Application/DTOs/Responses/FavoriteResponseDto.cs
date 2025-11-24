namespace FilmSelector.Application.DTOs.Responses;

public class FavoriteResponseDto
{
    public int Id { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string ImdbId { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Poster { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

