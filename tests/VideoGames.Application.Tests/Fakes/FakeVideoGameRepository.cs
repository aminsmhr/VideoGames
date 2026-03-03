using VideoGames.Application.Ports;
using VideoGames.Domain;

namespace VideoGames.Application.Tests.Fakes;

public class FakeVideoGameRepository : IVideoGameRepository
{
    private readonly List<VideoGame> _store = new();

    public Task<List<VideoGame>> GetAllAsync(CancellationToken ct) => Task.FromResult(_store.ToList());
    public Task<VideoGame?> GetByIdAsync(Guid id, CancellationToken ct) => Task.FromResult(_store.FirstOrDefault(x => x.Id == id));
    public Task UpdateAsync(VideoGame game, CancellationToken ct) => Task.CompletedTask;
    public Task AddAsync(VideoGame game, CancellationToken ct) { _store.Add(game); return Task.CompletedTask; }
}