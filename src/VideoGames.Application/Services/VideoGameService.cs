using VideoGames.Application.Contracts;
using VideoGames.Application.Ports;
using VideoGames.Domain;

namespace VideoGames.Application.Services;

public class VideoGameService
{
    private readonly IVideoGameRepository _repo;

    public VideoGameService(IVideoGameRepository repo) => _repo = repo;

    public async Task<List<VideoGameDto>> BrowseAsync(CancellationToken ct)
    {
        var games = await _repo.GetAllAsync(ct);
        return games
            .OrderBy(x => x.Title)
            .Select(ToDto)
            .ToList();
    }

    public async Task<VideoGameDto?> GetAsync(Guid id, CancellationToken ct)
    {
        var game = await _repo.GetByIdAsync(id, ct);
        return game is null ? null : ToDto(game);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateVideoGameRequest req, CancellationToken ct)
    {
        var game = await _repo.GetByIdAsync(id, ct);
        if (game is null) return false;

        game.Update(req.Title, req.Platform, req.ReleaseYear, req.Price);
        await _repo.UpdateAsync(game, ct);
        return true;
    }

    public async Task<Guid> CreateAsync(UpdateVideoGameRequest req, CancellationToken ct)
    {
        var game = new VideoGame(req.Title, req.Platform, req.ReleaseYear, req.Price);
        await _repo.AddAsync(game, ct);
        return game.Id;
    }

    private static VideoGameDto ToDto(VideoGame x)
        => new(x.Id, x.Title, x.Platform, x.ReleaseYear, x.Price);
}