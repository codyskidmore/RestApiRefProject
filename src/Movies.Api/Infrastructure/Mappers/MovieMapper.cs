using AutoMapper;
using Movies.Api.Contracts.Requests;
using Movies.Api.Contracts.Responses;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Infrastructure.Mappers;

public class MovieMapper : Profile
{
    public MovieMapper()
    {
        CreateMap<Movie, MovieResponse>().ReverseMap();
        CreateMap<CreateMovieRequest, Movie>().ReverseMap();
        CreateMap<UpdateMovieRequest, Movie>().ReverseMap();
    }
}

public static class MovieResponseMapper
{
    private static IEnumerable<MovieResponse> ToMovieResponses(this MovieList movieList)
    {
        return movieList.Select<Movie, MovieResponse>(m =>
            new MovieResponse
            {
                Genres = m.Genres,
                Id = m.Id,
                Slug = m.Slug,
                Title = m.Title,
                YearOfRelease = m.YearOfRelease,
                AverageRating = m.AverageRating,
                UserRating = m.UserRating
            }
        );
    }

    public static MoviesResponse ToMoviesResponse(this MovieList movies, int totalMovieCount, PagedRequest request)
    {
        
        
        return new MoviesResponse
        {
            Items = movies.ToMovieResponses(),
            Page = request.Page,
            PageSize = request.PageSize,
            Total = totalMovieCount
        };
    }
}