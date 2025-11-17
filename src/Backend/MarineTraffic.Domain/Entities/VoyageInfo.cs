namespace MarineTraffic.Domain.Entities;

/// <summary>
/// Representa los detalles completos de una película
/// </summary>
public class MovieDetails
{
    public string ImdbId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Year { get; set; }
    public string? Rated { get; set; }
    public string? Released { get; set; }
    public string? Runtime { get; set; }
    public string? Genre { get; set; }
    public string? Director { get; set; }
    public string? Writer { get; set; }
    public string? Actors { get; set; }
    public string? Plot { get; set; }
    public string? Language { get; set; }
    public string? Country { get; set; }
    public string? Awards { get; set; }
    public string? Poster { get; set; }
    public string? ImdbRating { get; set; }
    public string? ImdbVotes { get; set; }
    public string? Type { get; set; }
    public string? BoxOffice { get; set; }
    
    /// <summary>
    /// Indica si la película tiene un rating alto (>= 7.5)
    /// </summary>
    public bool IsHighlyRated
    {
        get
        {
            if (double.TryParse(ImdbRating, out var rating))
            {
                return rating >= 7.5;
            }
            return false;
        }
    }
}
