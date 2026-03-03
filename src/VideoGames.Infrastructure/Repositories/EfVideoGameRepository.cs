using Microsoft.EntityFrameworkCore;
using VideoGames.Application.Ports;
using VideoGames.Domain;
using VideoGames.Infrastructure.Persistence;

namespace VideoGames.Infrastructure.Repositories;

public class EfVideoGameRepository : IVideoGameRepository
{
    private readonly AppDbContext _db;

    public EfVideoGameRepository(AppDbContext db) => _db = db;

    public Task<List<VideoGame>> GetAllAsync(CancellationToken ct)
        => _db.VideoGames.AsNoTracking().ToListAsync(ct);

    public Task<VideoGame?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.VideoGames.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task UpdateAsync(VideoGame game, CancellationToken ct)
    {
        _db.VideoGames.Update(game);
        await _db.SaveChangesAsync(ct);
    }

    public async Task AddAsync(VideoGame game, CancellationToken ct)
    {
        _db.VideoGames.Add(game);
        await _db.SaveChangesAsync(ct);
    }
}