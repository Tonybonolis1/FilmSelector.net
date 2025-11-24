namespace FilmSelector.Domain.Entities;

/// <summary>
/// Representa una llegada esperada a un puerto
/// </summary>
public class PortArrival
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
    public string DestinationPort { get; set; } = string.Empty;
}

