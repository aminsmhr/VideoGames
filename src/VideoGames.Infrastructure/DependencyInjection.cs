using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoGames.Application.Ports;
using VideoGames.Infrastructure.Persistence;
using VideoGames.Infrastructure.Repositories;

namespace VideoGames.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("SqlServer")));

        services.AddScoped<IVideoGameRepository, EfVideoGameRepository>();
        return services;
    }
}