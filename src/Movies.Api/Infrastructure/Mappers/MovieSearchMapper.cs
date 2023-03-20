using AutoMapper;
using Movies.Api.Contracts.Requests;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Infrastructure.Mappers;

public class MovieSearchMapper : Profile
{
    public MovieSearchMapper()
    {
        CreateMap<GetAllMoviesRequest, GetAllMoviesOptions>()
            .ForMember(
                dest => dest.SortField, 
                opt => opt.MapFrom(src => src.SortBy!.Trim('+','-'))
            )
            .ForMember(
                dest => dest.YearOfRelease, 
                opt => opt.MapFrom(src => src.Year)
            )
            .ForMember(
                dest => dest.SortOrder, 
                opt => opt.MapFrom(src => src.SortBy!.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending)
            );
        CreateMap<GetAllMoviesRequest, GetAllMoviesOptions>()
            .ForMember(
                dest => dest.SortField, 
                opt => opt.MapFrom(src => src.SortBy!.Trim('+','-'))
            )
            .ForMember(
                dest => dest.YearOfRelease, 
                opt => opt.MapFrom(src => src.Year)
            )
            .ForMember(
                dest => dest.SortOrder, 
                opt => opt.MapFrom(src => src.SortBy!.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending)
            );
    }
}