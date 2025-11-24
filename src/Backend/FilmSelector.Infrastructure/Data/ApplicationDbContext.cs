using Microsoft.EntityFrameworkCore;
using FilmSelector.Domain.Entities;

namespace FilmSelector.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Favorite> Favorites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            // Índices únicos
            entity.HasIndex(e => e.Username)
                .IsUnique();
            
            entity.HasIndex(e => e.Email)
                .IsUnique();

            // Relación con Favoritos
            entity.HasMany(e => e.Favorites)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de Favorite
        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.MovieTitle)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.ImdbId)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Year)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Poster)
                .HasMaxLength(500);

            entity.Property(e => e.Notes)
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            // Índices
            entity.HasIndex(e => new { e.UserId, e.ImdbId })
                .IsUnique();
        });
    }
}

