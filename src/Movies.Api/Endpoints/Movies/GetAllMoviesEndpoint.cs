using Asp.Versioning;
using AutoMapper;
using Movies.Api.Contracts.Requests;
using Movies.Api.Contracts.Responses;
using Movies.Api.Infrastructure;
using Movies.Api.Infrastructure.Constants;
using Movies.Api.Infrastructure.Mappers;
using Movies.Contracts.Application.Interfaces;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Endpoints.Movies;

public static class GetAllMoviesEndpoint
{
    public const string Name = "GetAllMovies";

    public static IEndpointRouteBuilder MapGetAllMovie(this IEndpointRouteBuilder app)
    {
        // NOTE! Versioning causes REST verbs to match the wrong route. This started recently.
        // I am suspect it has to do with patch levels or Nuget package updates. Basically 
        // endpoint using the same base URL confuses the code to it can't use for different verbs.
        app.MapGet($"{ApiEndpoints.Movies.GetAll}/v1", async ([AsParameters] GetAllMoviesRequest request,
                IMovieService movieService,
                HttpContext context, IMapper mapper, CancellationToken token) =>
            {
                var options = mapper.Map<GetAllMoviesOptions>(request).AddUserId(context.GetUserId());
                var movies = await movieService.GetAllAsync(options, token);
                var totalMovieCount = await movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
                var movieResponses = mapper.Map<IEnumerable<MovieResponse>>(movies);
                var moviesResponse = mapper.Map<MoviesResponse>(movieResponses)
                    .AddPagingInfoToResponse(totalMovieCount, request);
                return TypedResults.Ok(moviesResponse);
            })
            .WithName($"{Name}V1")
            .Produces<MoviesResponse>(StatusCodes.Status200OK)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .HasDeprecatedApiVersion(1.0);

        app.MapGet($"{ApiEndpoints.Movies.GetAll}/v2", async ([AsParameters] GetAllMoviesRequest request, IMovieService movieService, 
                HttpContext context, IMapper mapper, CancellationToken token) =>
            {
                var options = mapper.Map<GetAllMoviesOptions>(request).AddUserId(context.GetUserId());
                var movies = await movieService.GetAllAsync(options, token);
                var totalMovieCount = await movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
                var movieResponses = mapper.Map<IEnumerable<MovieResponse>>(movies);
                var moviesResponse = mapper.Map<MoviesResponse>(movieResponses).AddPagingInfoToResponse(totalMovieCount, request);
                return TypedResults.Ok(moviesResponse);
            })
            .WithName($"{Name}V2")
            .Produces<MoviesResponse>(StatusCodes.Status200OK)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(2.0)
            .CacheOutput(CacheConstants.MovieCachePolicyName);

        return app;
    }
}