namespace FilmSelector.Application.DTOs.External;

/// <summary>
/// DTO para mapear respuesta de información de viaje de la API externa de FilmSelector
/// </summary>
public class FilmSelectorVoyageDto
{
    public string? MMSI { get; set; }
    public string? SHIP_ID { get; set; }
    public string? SHIPNAME { get; set; }
    public string? DESTINATION { get; set; }
    public string? DESTINATION_UNLOCODE { get; set; }
    public string? PORT_COUNTRY { get; set; }
    public long? ETA { get; set; }
    public string? VOYAGE_STATUS { get; set; }
    public double? LAT { get; set; }
    public double? LON { get; set; }
    public double? SPEED { get; set; }
    public double? COURSE { get; set; }
    public string? DEPARTURE_PORT { get; set; }
    public long? DEPARTURE_TIME { get; set; }
}

