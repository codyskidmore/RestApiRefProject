using AutoMapper;
using Movies.Api.Abstraction.Responses;
using Movies.Contracts.Api.Requests;
using Movies.Contracts.Api.Responses;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Infrastructure;

public class MovieMapper : Profile
{
    public MovieMapper()
    {
        CreateMap<Movie, MovieResponse>().ReverseMap();
        CreateMap<CreateMovieRequest, Movie>().ReverseMap();
        CreateMap<UpdateMovieRequest, Movie>().ReverseMap();
        CreateMap<IEnumerable<MovieResponse>, MoviesResponse>()
            .ForMember(
                dest => dest.Movies, 
                opt => opt.MapFrom(src => src)
            );
    }
}