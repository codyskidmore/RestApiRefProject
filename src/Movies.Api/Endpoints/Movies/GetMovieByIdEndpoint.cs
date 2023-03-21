using AutoMapper;
using Movies.Api.Contracts.Responses;
using Movies.Api.Infrastructure;
using Movies.Api.Infrastructure.Constants;
using Movies.Contracts.Application.Interfaces;

namespace Movies.Api.Endpoints.Movies;

public static class GetMovieByIdEndpoint
{
    public const string Name = "GetMovieById";

    public static IEndpointRouteBuilder MapGetByIdMovie(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Movies.GetById, async (Guid id, IMovieService movieService, 
            HttpContext context, IMapper mapper, CancellationToken token) =>
        {
            var userId = context.GetUserId();
            var movie = await movieService.GetByIdAsync(id, userId, token);
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