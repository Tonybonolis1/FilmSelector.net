using MarineTraffic.Application.DTOs.Requests;
using MarineTraffic.Application.DTOs.Responses;
using MarineTraffic.Application.Interfaces;
using MarineTraffic.Domain.Common;
using MarineTraffic.Domain.Entities;
using MarineTraffic.Domain.Interfaces;

namespace MarineTraffic.Application.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;

    public FavoriteService(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task<Result<IEnumerable<FavoriteResponseDto>>> GetAllAsync(int userId)
    {
        var favorites = await _favoriteRepository.GetAllByUserIdAsync(userId);
        var favoriteDtos = favorites.Select(MapToDto).ToList();
        return Result<IEnumerable<FavoriteResponseDto>>.Success(favoriteDtos);
    }

    public async Task<Result<FavoriteResponseDto>> GetByIdAsync(int id, int userId)
    {
        var favorite = await _favoriteRepository.GetByIdAsync(id, userId);

        if (favorite == null)
        {
            return Result<FavoriteResponseDto>.Failure("Favorito no encontrado");
        }

        return Result<FavoriteResponseDto>.Success(MapToDto(favorite));
    }

    public async Task<Result<FavoriteResponseDto>> CreateAsync(CreateFavoriteRequestDto request, int userId)
    {
        // Validar que no exista el mismo favorito
        if (await _favoriteRepository.ExistsByImdbIdAsync(userId, request.ImdbId))
        {
            return Result<FavoriteResponseDto>.Failure("Esta película ya está en tus favoritos");
        }

        var favorite = new Favorite
        {
            UserId = userId,
            MovieTitle = request.MovieTitle,
            ImdbId = request.ImdbId,
            Year = request.Year,
            Type = request.Type,
            Poster = request.Poster,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        favorite = await _favoriteRepository.AddAsync(favorite);

        return Result<FavoriteResponseDto>.Success(MapToDto(favorite));
    }

    public async Task<Result<FavoriteResponseDto>> UpdateAsync(int id, UpdateFavoriteRequestDto request, int userId)
    {
        var favorite = await _favoriteRepository.GetByIdAsync(id, userId);

        if (favorite == null)
        {
            return Result<FavoriteResponseDto>.Failure("Favorito no encontrado");
        }

        favorite.MovieTitle = request.MovieTitle;
        favorite.Notes = request.Notes;
        favorite.UpdatedAt = DateTime.UtcNow;

        await _favoriteRepository.UpdateAsync(favorite);

        return Result<FavoriteResponseDto>.Success(MapToDto(favorite));
    }

    public async Task<Result<bool>> DeleteAsync(int id, int userId)
    {
        var favorite = await _favoriteRepository.GetByIdAsync(id, userId);

        if (favorite == null)
        {
            return Result<bool>.Failure("Favorito no encontrado");
        }

        await _favoriteRepository.DeleteAsync(favorite);

        return Result<bool>.Success(true);
    }

    private static FavoriteResponseDto MapToDto(Favorite favorite)
    {
        return new FavoriteResponseDto
        {
            Id = favorite.Id,
            MovieTitle = favorite.MovieTitle,
            ImdbId = favorite.ImdbId,
            Year = favorite.Year,
            Type = favorite.Type,
            Poster = favorite.Poster,
            Notes = favorite.Notes,
            CreatedAt = favorite.CreatedAt,
            UpdatedAt = favorite.UpdatedAt
        };
    }
}
