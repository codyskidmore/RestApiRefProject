using Movies.Api.Endpoints.Movies;
using Movies.Api.Endpoints.Ratings;

namespace Movies.Api.Infrastructure;

public static class EndpointsExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapMovieEndpoints();
        app.MapRatingEndpoints();
        return app;
    }
    
    private static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetByIdMovie();
        app.MapGetBySlugMovie();
        app.MapGetAllMovie();
        app.MapCreateMovie();
        app.MapUpdateMovie();
        app.MapDeleteMovie();
        return app;
    }
    
    private static IEndpointRouteBuilder MapRatingEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRateMovie();
        app.MapDeleteRating();
        app.MapGetUserRatings();
        return app;
    }
}