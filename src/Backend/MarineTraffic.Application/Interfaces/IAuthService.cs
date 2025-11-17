using MarineTraffic.Application.DTOs.Requests;
using MarineTraffic.Application.DTOs.Responses;
using MarineTraffic.Domain.Common;

namespace MarineTraffic.Application.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request);
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto request);
}
