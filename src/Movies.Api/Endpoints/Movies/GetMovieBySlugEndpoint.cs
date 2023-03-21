using AutoMapper;
using Movies.Api.Contracts.Responses;
using Movies.Api.Infrastructure;
using Movies.Api.Infrastructure.Constants;
using Movies.Contracts.Application.Interfaces;

namespace Movies.Api.Endpoints.Movies;

public static class GetMovieBySlugEndpoint
{
    public const string Name = "GetMovieBySlug";

    public static IEndpointRouteBuilder MapGetBySlugMovie(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Movies.GetBySlug, async (string slug, IMovieService movieService, 
            HttpContext context, IMapper mapper, CancellationToken token) =>
        {
            var userId = context.GetUserId();
            var movie = await movieService.GetBySlugAsync(slug, userId, token);
            if (movie is null)
            {
                return Results.NotFound();
            }

            return TypedResults.Ok(mapper.Map<MovieResponse>(movie));        
        }).WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput("MovieCache");

        return app;
    }
}