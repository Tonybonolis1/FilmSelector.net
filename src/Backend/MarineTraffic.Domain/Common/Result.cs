namespace MarineTraffic.Domain.Common;

/// <summary>
/// Representa una respuesta genérica del sistema
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorCode { get; set; }
    public Dictionary<string, string>? ErrorDetails { get; set; }

    public static Result<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };

    public static Result<T> Failure(string errorMessage, string? errorCode = null, Dictionary<string, string>? errorDetails = null) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage,
        ErrorCode = errorCode,
        ErrorDetails = errorDetails
    };
}
