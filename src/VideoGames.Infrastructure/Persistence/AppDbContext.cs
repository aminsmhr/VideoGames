using Microsoft.EntityFrameworkCore;
using VideoGames.Domain;

namespace VideoGames.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<VideoGame> VideoGames => Set<VideoGame>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var g = modelBuilder.Entity<VideoGame>();
        g.HasKey(x => x.Id);
        g.Property(x => x.Title).HasMaxLength(200).IsRequired();
        g.Property(x => x.Platform).HasMaxLength(100).IsRequired();
        g.Property(x => x.Price).HasPrecision(18, 2);

        // Seed demo data
        g.HasData(
            new { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Title = "Hollow Knight", Platform = "PC", ReleaseYear = 2017, Price = 14.99m },
            new { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Title = "The Witcher 3", Platform = "PS4", ReleaseYear = 2015, Price = 19.99m }
        );
    }
}