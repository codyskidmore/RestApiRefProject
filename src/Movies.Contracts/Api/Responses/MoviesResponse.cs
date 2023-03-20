using Movies.Contracts.Api.Responses;

namespace Movies.Api.Abstraction.Responses;

public class MoviesResponse
{
    public required IEnumerable<MovieResponse> Movies { get; init; } = Enumerable.Empty<MovieResponse>();
}