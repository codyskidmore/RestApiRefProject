using AutoMapper;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Contracts.Requests;
using Movies.Api.Contracts.Responses;
using Movies.Api.Infrastructure;
using Movies.Api.Infrastructure.Constants;
using Movies.Contracts.Application.Interfaces;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Endpoints.Movies;

public static class UpdateMovieEndpoint
{
    public const string Name = "UpdateMovie";

    public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Movies.Update, async (Guid id, UpdateMovieRequest updateMovieRequest, 
            IMovieService movieService, HttpContext context, IMapper mapper, IOutputCacheStore outputCacheStore, 
            CancellationToken token) =>
        {
            var userId = context.GetUserId();
            var movieWithUpdates = mapper.Map<Movie>(updateMovieRequest);
            movieWithUpdates.Id = id;

            var updatedMovie = await movieService.UpdateAsync(movieWithUpdates, userId, token);
            if (updatedMovie is null)
            {
                return Results.NotFound();
            }

            await outputCacheStore.EvictByTagAsync(CacheConstants.MovieCacheTagName, token);
        
            return TypedResults.Ok(mapper.Map<MovieResponse>(movieWithUpdates));        
        }).WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthConstants.TrustedMemberPolicyName);

        return app;
    }
}