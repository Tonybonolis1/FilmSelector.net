namespace FilmSelector.Application.DTOs.Responses;

/// <summary>
/// DTO para respuesta de llegadas a puerto
/// </summary>
public class PortArrivalResponseDto
{
    public string VesselId { get; set; } = string.Empty;
    public string VesselName { get; set; } = string.Empty;
    public string? Mmsi { get; set; }
    public string? Imo { get; set; }
    public string? ShipType { get; set; }
    public string? Flag { get; set; }
    public string? OriginPort { get; set; }
    public DateTime? EstimatedTimeOfArrival { get; set; }
    public double? DistanceToPort { get; set; }
}

