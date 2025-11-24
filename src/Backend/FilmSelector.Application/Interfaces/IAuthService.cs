using FilmSelector.Application.DTOs.Requests;
using FilmSelector.Application.DTOs.Responses;
using FilmSelector.Domain.Common;

namespace FilmSelector.Application.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request);
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto request);
}

