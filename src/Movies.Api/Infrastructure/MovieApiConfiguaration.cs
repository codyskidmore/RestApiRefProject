using System.Reflection;

namespace Movies.Api.Infrastructure;

public static class MovieApiConfiguaration
{
    public static IServiceCollection AddMovieApiServices(this IServiceCollection services)
    {
        var moviesApiAssembly = Assembly.GetExecutingAssembly();
        services.AddAutoMapper(moviesApiAssembly);
        return services;
    }
}