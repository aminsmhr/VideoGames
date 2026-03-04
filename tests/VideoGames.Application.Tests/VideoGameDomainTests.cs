using System;
using FluentAssertions;
using Xunit;
using VideoGames.Domain;

namespace VideoGames.Domain.Tests;

public class VideoGameDomainTests
{
    [Fact]
    public void Ctor_ShouldSetProperties_WhenValid()
    {
        var game = new VideoGame("Stardew Valley", "PC", 2016, 14.99m);

        game.Id.Should().NotBe(Guid.Empty);
        game.Title.Should().Be("Stardew Valley");
        game.Platform.Should().Be("PC");
        game.ReleaseYear.Should().Be(2016);
        game.Price.Should().Be(14.99m);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Ctor_ShouldThrow_WhenTitleInvalid(string? title)
    {
        Action act = () => new VideoGame(title!, "PC", 2010, 9.99m);
        act.Should().Throw<ArgumentException>().WithMessage("Title is required.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Ctor_ShouldThrow_WhenPlatformInvalid(string? platform)
    {
        Action act = () => new VideoGame("Game", platform!, 2010, 9.99m);
        act.Should().Throw<ArgumentException>().WithMessage("Platform is required.");
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenPriceNegative()
    {
        Action act = () => new VideoGame("Game", "PC", 2010, -1m);
        act.Should().Throw<ArgumentException>().WithMessage("Price cannot be negative.");
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenReleaseYearTooOld()
    {
        Action act = () => new VideoGame("Game", "PC", 1969, 1m);
        act.Should().Throw<ArgumentException>().WithMessage("Invalid ReleaseYear.");
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenReleaseYearTooFarInFuture()
    {
        var tooFar = DateTime.UtcNow.Year + 2;
        Action act = () => new VideoGame("Game", "PC", tooFar, 1m);
        act.Should().Throw<ArgumentException>().WithMessage("Invalid ReleaseYear.");
    }

    [Fact]
    public void Update_ShouldChangeValues_WhenValid()
    {
        var game = new VideoGame("Old", "PC", 2015, 4.99m);

        game.Update("New Title", "Switch", 2018, 19.99m);

        game.Title.Should().Be("New Title");
        game.Platform.Should().Be("Switch");
        game.ReleaseYear.Should().Be(2018);
        game.Price.Should().Be(19.99m);
    }

    [Fact]
    public void Update_ShouldValidateInputs_AndThrowOnInvalid()
    {
        var game = new VideoGame("Old", "PC", 2015, 4.99m);

        Action act = () => game.Update("", "Switch", 2018, 19.99m);
        act.Should().Throw<ArgumentException>().WithMessage("Title is required.");

        Action act2 = () => game.Update("Title", "", 2018, 19.99m);
        act2.Should().Throw<ArgumentException>().WithMessage("Platform is required.");

        Action act3 = () => game.Update("Title", "Platform", 1960, 19.99m);
        act3.Should().Throw<ArgumentException>().WithMessage("Invalid ReleaseYear.");

        Action act4 = () => game.Update("Title", "Platform", 2018, -9m);
        act4.Should().Throw<ArgumentException>().WithMessage("Price cannot be negative.");
    }
}
