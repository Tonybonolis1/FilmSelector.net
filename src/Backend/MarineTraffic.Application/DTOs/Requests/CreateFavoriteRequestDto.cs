namespace MarineTraffic.Application.DTOs.Requests;

public class CreateFavoriteRequestDto
{
    public string MovieTitle { get; set; } = string.Empty;
    public string ImdbId { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Poster { get; set; }
    public string? Notes { get; set; }
}
