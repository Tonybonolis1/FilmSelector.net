namespace FilmSelector.Application.DTOs.Requests;

public class UpdateFavoriteRequestDto
{
    public string MovieTitle { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

