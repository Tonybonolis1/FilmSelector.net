namespace FilmSelector.Application.DTOs.External;

/// <summary>
/// DTO para mapear respuesta de búsqueda de la API externa de FilmSelector
/// </summary>
public class FilmSelectorVesselDto
{
    public string? MMSI { get; set; }
    public string? IMO { get; set; }
    public string? SHIP_ID { get; set; }
    public string? SHIPNAME { get; set; }
    public string? TYPE_NAME { get; set; }
    public string? FLAG { get; set; }
    public double? LAT { get; set; }
    public double? LON { get; set; }
    public long? LAST_POS { get; set; }
}

