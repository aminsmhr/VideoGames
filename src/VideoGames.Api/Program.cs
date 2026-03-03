using VideoGames.Application.Contracts;
using VideoGames.Application.Services;
using VideoGames.Infrastructure;
using VideoGames.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<VideoGameService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAngular");

// Ensure DB created/migrated (demo-friendly)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.MapGet("/api/games", async (VideoGameService svc, CancellationToken ct) =>
{
    var list = await svc.BrowseAsync(ct);
    return Results.Ok(list);
});

app.MapGet("/api/games/{id:guid}", async (Guid id, VideoGameService svc, CancellationToken ct) =>
{
    var game = await svc.GetAsync(id, ct);
    return game is null ? Results.NotFound() : Results.Ok(game);
});

app.MapPut("/api/games/{id:guid}", async (Guid id, UpdateVideoGameRequest req, VideoGameService svc, CancellationToken ct) =>
{
    var ok = await svc.UpdateAsync(id, req, ct);
    return ok ? Results.NoContent() : Results.NotFound();
});

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();