using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Services;
using Movies.Contracts.Application.Interfaces;

namespace Movies.Application.Infrastructure;

public static class MovieApplicationConfiguration
{
    public static IServiceCollection AddMovieApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IMovieService, MovieService>();
        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>(ServiceLifetime.Singleton);
        return services;
    }
}