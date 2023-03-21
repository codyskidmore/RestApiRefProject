using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Contracts.Requests;
using Movies.Api.Contracts.Responses;
using Movies.Api.Infrastructure.Constants;
using Movies.Contracts.Application.Interfaces;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Endpoints.Movies;

 public static class CreateMovieEndpoint
 {
     public const string Name = "CreateMovie";

     public static IEndpointRouteBuilder MapCreateMovie(this IEndpointRouteBuilder app)
     {
         app.MapPost(ApiEndpoints.Movies.Create, async ([FromBody] CreateMovieRequest movieRequest, IMovieService movieService,
                 IMapper mapper, IOutputCacheStore outputCacheStore, CancellationToken token) =>
             {
                 var movie = mapper.Map<Movie>(movieRequest);
                 await movieService.CreateAsync(movie, token);
                 var movieResponse = mapper.Map<MovieResponse>(movie);
                 await outputCacheStore.EvictByTagAsync(CacheConstants.MovieCacheTagName, token);
                 return TypedResults.CreatedAtRoute(movieResponse, GetMovieByIdEndpoint.Name, new { id = movie.Id });
             })
             .WithName(Name)
             .Produces<MovieResponse>(StatusCodes.Status201Created)
             .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest)
             .RequireAuthorization(AuthConstants.TrustedMemberPolicyName);

         return app;
     }
}
