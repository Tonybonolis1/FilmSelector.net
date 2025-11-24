using Microsoft.EntityFrameworkCore;
using FilmSelector.Domain.Entities;
using FilmSelector.Domain.Interfaces;
using FilmSelector.Infrastructure.Data;

namespace FilmSelector.Infrastructure.Repositories;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly ApplicationDbContext _context;

    public FavoriteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Favorite>> GetAllByUserIdAsync(int userId)
    {
        return await _context.Favorites
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<Favorite?> GetByIdAsync(int id, int userId)
    {
        return await _context.Favorites
            .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);
    }

    public async Task<Favorite> AddAsync(Favorite favorite)
    {
        _context.Favorites.Add(favorite);
        await _context.SaveChangesAsync();
        return favorite;
    }

    public async Task UpdateAsync(Favorite favorite)
    {
        _context.Favorites.Update(favorite);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Favorite favorite)
    {
        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByImdbIdAsync(int userId, string imdbId)
    {
        return await _context.Favorites
            .AnyAsync(f => f.UserId == userId && f.ImdbId == imdbId);
    }
}

