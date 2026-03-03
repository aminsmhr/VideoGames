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
}