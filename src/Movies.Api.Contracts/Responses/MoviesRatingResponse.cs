using Movies.Api.Contracts.Responses;

namespace Movies.Contracts.Api.Responses;

public class MoviesRatingResponse
{
    public required IEnumerable<MovieRatingResponse> Ratings { get; init; } = Enumerable.Empty<MovieRatingResponse>();
}    