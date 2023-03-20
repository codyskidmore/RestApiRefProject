using Microsoft.Extensions.DependencyInjection;
using Movies.Contracts.Data.Interfaces;
using Movies.Data.Database;

namespace Movies.Data.Infrastructure;

public static class MovieDataConfiguration
{
    public static IServiceCollection AddMovieDataServices(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlDbConnectionFactory(connectionString));
        services.AddSingleton<PgsqlDbInitializer>();
        services.AddSingleton<IMovieRepository, DapperMovieRepository>();
        return services;
    }
}