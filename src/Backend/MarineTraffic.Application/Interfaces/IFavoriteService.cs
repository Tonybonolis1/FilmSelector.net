using MarineTraffic.Application.DTOs.Requests;
using MarineTraffic.Application.DTOs.Responses;
using MarineTraffic.Domain.Common;

namespace MarineTraffic.Application.Interfaces;

public interface IFavoriteService
{
    Task<Result<IEnumerable<FavoriteResponseDto>>> GetAllAsync(int userId);
    Task<Result<FavoriteResponseDto>> GetByIdAsync(int id, int userId);
    Task<Result<FavoriteResponseDto>> CreateAsync(CreateFavoriteRequestDto request, int userId);
    Task<Result<FavoriteResponseDto>> UpdateAsync(int id, UpdateFavoriteRequestDto request, int userId);
    Task<Result<bool>> DeleteAsync(int id, int userId);
}
