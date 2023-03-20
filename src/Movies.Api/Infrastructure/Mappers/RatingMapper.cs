using AutoMapper;
using Movies.Api.Contracts.Responses;
using Movies.Contracts.Api.Responses;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Infrastructure.Mappers;

public class RatingMapper : Profile
{
    public RatingMapper()
    {
        CreateMap<MovieRating, MovieRatingResponse>().ReverseMap();
        CreateMap<IEnumerable<MovieRatingResponse>, MoviesRatingResponse>()
            .ForMember(
                dest => dest.Ratings, 
                opt => opt.MapFrom(src => src)
            );
    }
}