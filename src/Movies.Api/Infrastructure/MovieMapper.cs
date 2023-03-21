using AutoMapper;
using Movies.Api.Contracts.Requests;
using Movies.Api.Contracts.Responses;
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
                dest => dest.Items, 
                opt => opt.MapFrom(src => src)
            );
    }
}