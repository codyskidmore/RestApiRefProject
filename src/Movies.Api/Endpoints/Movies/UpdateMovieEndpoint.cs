using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Contracts.Requests;
using Movies.Api.Contracts.Responses;
using Movies.Api.Infrastructure;
using Movies.Api.Infrastructure.Constants;
using Movies.Api.Infrastructure.Mappers;
using Movies.Contracts.Application.Interfaces;

namespace Movies.Api.Endpoints.Movies;

public static class UpdateMovieEndpoint
{
    public const string Name = "UpdateMovie";

    public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Movies.Update, async (Guid id, UpdateMovieRequest updateMovieRequest, 
            IMovieService movieService, HttpContext context, IOutputCacheStore outputCacheStore, 
            CancellationToken token) =>
        {
            var userId = context.GetUserId();
            var movieWithUpdates = updateMovieRequest.MapToMovie(id);
            movieWithUpdates.Id = id;

            var updatedMovie = await movieService.UpdateAsync(movieWithUpdates, userId, token);
            if (updatedMovie is null)
            {
                return Results.NotFound();
            }

            await outputCacheStore.EvictByTagAsync(CacheConstants.MovieCacheTagName, token);
        
            return TypedResults.Ok(movieWithUpdates.MapToMovieResponse());        
        }).WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthConstants.TrustedMemberPolicyName);

        return app;
    }
}