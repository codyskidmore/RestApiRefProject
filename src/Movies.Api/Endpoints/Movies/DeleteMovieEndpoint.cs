using AutoMapper;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Contracts.Requests;
using Movies.Api.Contracts.Responses;
using Movies.Api.Infrastructure;
using Movies.Api.Infrastructure.Constants;
using Movies.Contracts.Application.Interfaces;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Endpoints.Movies;

public static class DeleteMovieEndpoint
{
    public const string Name = "DeleteMovie";

    public static IEndpointRouteBuilder MapDeleteMovie(this IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.Movies.Delete, async (Guid id, IMovieService movieService, 
            IOutputCacheStore outputCacheStore, CancellationToken token) =>
        {
            var notDeleted = !await movieService.DeleteByIdAsync(id, token);
            if (notDeleted)
            {
                return Results.NotFound();
            }

            await outputCacheStore.EvictByTagAsync(CacheConstants.MovieCacheTagName, token);

            return TypedResults.Ok();

        })
        .WithName(Name)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(AuthConstants.AdminUserPolicyName);

        return app;
    }
}