using VideoGames.Domain;

namespace VideoGames.Application.Ports;

public interface IVideoGameRepository
{
    Task<List<VideoGame>> GetAllAsync(CancellationToken ct);
    Task<VideoGame?> GetByIdAsync(Guid id, CancellationToken ct);
    Task UpdateAsync(VideoGame game, CancellationToken ct);
    Task AddAsync(VideoGame game, CancellationToken ct);
}