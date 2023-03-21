using AutoMapper;
using Movies.Api.Contracts.Responses;
using Movies.Api.Infrastructure;
using Movies.Api.Infrastructure.Constants;
using Movies.Contracts.Api.Responses;
using Movies.Contracts.Application.Interfaces;

namespace Movies.Api.Endpoints.Ratings;

public static class GetUserRatingsEndpoint
{
    public const string Name = "GetUserRatings";
    
    public static IEndpointRouteBuilder MapGetUserRatings(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Ratings.GetUserRatings,
                async (HttpContext context, IRatingService ratingService,
                    IMapper mapper, CancellationToken token) =>
                {
                    var userId = context.GetUserId();
                    var ratings = await ratingService.GetRatingsForUserAsync(userId!.Value, token);
                    var ratingResponses = mapper.Map<IEnumerable<MovieRatingResponse>>(ratings);
                    var ratingsResponse = mapper.Map<MoviesRatingResponse>(ratingResponses);
                    return TypedResults.Ok(ratingsResponse);
                })
            .WithName(Name)
            .Produces<MovieRatingResponse>(StatusCodes.Status200OK)
            .RequireAuthorization();
        
        return app;
    }
}
