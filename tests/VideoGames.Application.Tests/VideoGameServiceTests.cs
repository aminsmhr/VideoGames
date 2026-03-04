using FluentAssertions;
using VideoGames.Application.Contracts;
using VideoGames.Application.Services;
using VideoGames.Application.Tests.Fakes;

namespace VideoGames.Application.Tests;

public class VideoGameServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldReturnId_AndBeBrowsable()
    {
        var repo = new FakeVideoGameRepository();
        var svc = new VideoGameService(repo);

        var id = await svc.CreateAsync(new UpdateVideoGameRequest("Celeste", "PC", 2018, 9.99m), CancellationToken.None);

        id.Should().NotBe(Guid.Empty);

        var all = await svc.BrowseAsync(CancellationToken.None);
        all.Should().ContainSingle(x => x.Id == id && x.Title == "Celeste");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateValues_WhenGameExists()
    {
        var repo = new FakeVideoGameRepository();
        var svc = new VideoGameService(repo);

        var id = await svc.CreateAsync(new UpdateVideoGameRequest("Hollow Knight", "Switch", 2017, 14.99m), CancellationToken.None);

        var updateReq = new UpdateVideoGameRequest("Hollow Knight: Silksong", "Switch", DateTime.UtcNow.Year + 1, 29.99m);
        var updated = await svc.UpdateAsync(id, updateReq, CancellationToken.None);

        updated.Should().BeTrue();

        var dto = await svc.GetAsync(id, CancellationToken.None);
        dto.Should().NotBeNull();
        dto!.Title.Should().Be("Hollow Knight: Silksong");
        dto.ReleaseYear.Should().Be(updateReq.ReleaseYear);
        dto.Price.Should().Be(updateReq.Price);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenInvalidInput()
    {
        var repo = new FakeVideoGameRepository();
        var svc = new VideoGameService(repo);

        Func<Task> act = async () => await svc.CreateAsync(new UpdateVideoGameRequest("", "PC", 2010, 19.99m), CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenGameNotFound()
    {
        var repo = new FakeVideoGameRepository();
        var svc = new VideoGameService(repo);

        var updated = await svc.UpdateAsync(Guid.NewGuid(), new UpdateVideoGameRequest("X", "Y", 2000, 1m), CancellationToken.None);

        updated.Should().BeFalse();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenNotFound()
    {
        var repo = new FakeVideoGameRepository();
        var svc = new VideoGameService(repo);

        var dto = await svc.GetAsync(Guid.NewGuid(), CancellationToken.None);

        dto.Should().BeNull();
    }

    [Fact]
    public async Task BrowseAsync_ShouldReturnOrderedByTitle()
    {
        var repo = new FakeVideoGameRepository();
        var svc = new VideoGameService(repo);

        await svc.CreateAsync(new UpdateVideoGameRequest("Zelda", "Switch", 2017, 59.99m), CancellationToken.None);
        await svc.CreateAsync(new UpdateVideoGameRequest("Abzu", "PC", 2016, 14.99m), CancellationToken.None);

        var all = await svc.BrowseAsync(CancellationToken.None);

        all.Select(x => x.Title).Should().ContainInOrder(new[] { "Abzu", "Zelda" });
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_ForNegativePrice()
    {
        var repo = new FakeVideoGameRepository();
        var svc = new VideoGameService(repo);

        Func<Task> act = async () => await svc.CreateAsync(new UpdateVideoGameRequest("Game", "PC", 2020, -5m), CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>();
    }
}