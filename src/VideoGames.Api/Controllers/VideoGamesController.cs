using Microsoft.AspNetCore.Mvc;
using VideoGames.Application.Contracts;
using VideoGames.Application.Services;

namespace VideoGames.Api.Controllers;

[ApiController]
[Route("api/games")]
public class VideoGamesController : ControllerBase
{
    private readonly VideoGameService _svc;

    public VideoGamesController(VideoGameService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> Browse(CancellationToken ct)
        => Ok(await _svc.BrowseAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var game = await _svc.GetAsync(id, ct);
        return game is null ? NotFound() : Ok(game);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVideoGameRequest req, CancellationToken ct)
    {
        var ok = await _svc.UpdateAsync(id, req, ct);
        return ok ? NoContent() : NotFound();
    }
}