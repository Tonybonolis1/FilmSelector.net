namespace MarineTraffic.Domain.Entities;

/// <summary>
/// Representa información sobre un puerto
/// </summary>
public class Port
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Unlocode { get; set; }
    public string? Country { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
