using Movies.Api.Contracts.Responses;
using Movies.Contracts.Api.Responses;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Infrastructure.Mappers;

public static class RatingMapper
{
    public static IEnumerable<MovieRatingResponse> MapToResponse(this IEnumerable<MovieRating> ratings)
    {
        return ratings.Select(x => new MovieRatingResponse
        {
            MovieId = x.MovieId,
            Rating = x.Rating,
            Slug = x.Slug
        });
    }
    
    public static MoviesRatingResponse MapToMoviesRatingResponse(this IEnumerable<MovieRatingResponse> ratings)
    {
        return new MoviesRatingResponse
        {
            Ratings = ratings.Select(x => new MovieRatingResponse
            {
                MovieId = x.MovieId,
                Rating = x.Rating,
                Slug = x.Slug
            })
        };
    }
}