using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using VideoGames.Infrastructure.Persistence;
using VideoGames.Infrastructure.Repositories;
using VideoGames.Application.Ports;
using VideoGames.Domain;

namespace VideoGames.Application.Tests;

public class EfVideoGameRepositoryTests
{
    private static AppDbContext CreateContext(SqliteConnection conn)
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(conn)
            .Options;

        return new AppDbContext(opts);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSeededData()
    {
        using var conn = new SqliteConnection("DataSource=:memory:");
        conn.Open();

        using var db = CreateContext(conn);
        db.Database.EnsureCreated();

        var repo = new EfVideoGameRepository(db);

        var all = await repo.GetAllAsync(CancellationToken.None);

        all.Should().NotBeNullOrEmpty();
        all.Select(x => x.Title).Should().Contain(new[] { "Hollow Knight", "The Witcher 3" });
    }

    [Fact]
    public async Task AddAsync_And_GetByIdAsync_Works()
    {
        using var conn = new SqliteConnection("DataSource=:memory:");
        conn.Open();

        using var db = CreateContext(conn);
        db.Database.EnsureCreated();

        var repo = new EfVideoGameRepository(db);

        var game = new VideoGame("Test Game", "PC", 2021, 5.50m);
        await repo.AddAsync(game, CancellationToken.None);

        var loaded = await repo.GetByIdAsync(game.Id, CancellationToken.None);

        loaded.Should().NotBeNull();
        loaded!.Title.Should().Be("Test Game");
    }

    [Fact]
    public async Task UpdateAsync_PersistsChanges()
    {
        using var conn = new SqliteConnection("DataSource=:memory:");
        conn.Open();

        using var db = CreateContext(conn);
        db.Database.EnsureCreated();

        var repo = new EfVideoGameRepository(db);

        var game = new VideoGame("Update Game", "PC", 2020, 9.99m);
        await repo.AddAsync(game, CancellationToken.None);

        var loaded = await repo.GetByIdAsync(game.Id, CancellationToken.None);
        loaded.Should().NotBeNull();

        loaded!.Update("Updated", "Switch", 2022, 29.99m);
        await repo.UpdateAsync(loaded, CancellationToken.None);

        var reloaded = await repo.GetByIdAsync(game.Id, CancellationToken.None);
        reloaded.Should().NotBeNull();
        reloaded!.Title.Should().Be("Updated");
        reloaded.Platform.Should().Be("Switch");
        reloaded.Price.Should().Be(29.99m);
    }
}
