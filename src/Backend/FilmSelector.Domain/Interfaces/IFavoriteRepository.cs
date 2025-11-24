using FilmSelector.Domain.Entities;

namespace FilmSelector.Domain.Interfaces;

public interface IFavoriteRepository
{
    Task<IEnumerable<Favorite>> GetAllByUserIdAsync(int userId);
    Task<Favorite?> GetByIdAsync(int id, int userId);
    Task<Favorite> AddAsync(Favorite favorite);
    Task UpdateAsync(Favorite favorite);
    Task DeleteAsync(Favorite favorite);
    Task<bool> ExistsByImdbIdAsync(int userId, string imdbId);
}

