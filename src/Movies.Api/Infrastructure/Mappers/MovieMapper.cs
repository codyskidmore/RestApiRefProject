using Movies.Api.Contracts.Requests;
using Movies.Api.Contracts.Responses;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Infrastructure.Mappers;

public static class MovieMapper
{
    public static Movie MapToMovie(this CreateMovieRequest movieRequest)
    {
        return new Movie
        {
            Id = Guid.NewGuid(), // Assuming a new Guid for Id
            Title = movieRequest.Title,
            YearOfRelease = movieRequest.YearOfRelease,
            Genres = movieRequest.Genres.ToList(),
            UserRating = null, // Assuming null for UserRating
            AverageRating = null // Assuming null for AverageRating
        };
    }
    
    public static MovieResponse MapToMovieResponse(this Movie movie)
    {
        return new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres,
            UserRating = movie.UserRating,
            AverageRating = movie.AverageRating,
            Slug = movie.Slug
        };
    }
    
    public static IEnumerable<MovieResponse> MapToMovieResponses(this MovieList movies)
    {
        return movies.Select(movie => movie.MapToMovieResponse());
    }
    
    public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies,
        int page, int pageSize, int totalCount)
    {
        return new MoviesResponse
        {
            Items = movies.Select(MapToMovieResponse),
            Page = page,
            PageSize = pageSize,
            Total = totalCount
        };
    }
    
    public static Movie MapToMovie(this UpdateMovieRequest request, Guid id)
    {
        return new Movie
        {
            Id = id,
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList(),
            UserRating = null, // Assuming null for UserRating
            AverageRating = null // Assuming null for AverageRati
        };
    }
}