namespace FilmSelector.Domain.Entities;

public class Favorite
{
    public int Id { get; set; }
    public int UserId { get; set; }
    
    // Datos de la película
    public string MovieTitle { get; set; } = string.Empty;
    public string ImdbId { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // movie, series, episode
    public string? Poster { get; set; }
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Relación con usuario
    public User User { get; set; } = null!;
}

