namespace MarineTraffic.Application.DTOs.Responses;

/// <summary>
/// DTO genérico para respuestas de error
/// </summary>
public class ErrorResponseDto
{
    public string Message { get; set; } = string.Empty;
    public string? Code { get; set; }
    public Dictionary<string, string>? Details { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
