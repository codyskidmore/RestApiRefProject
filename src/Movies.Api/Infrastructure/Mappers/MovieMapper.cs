﻿using AutoMapper;
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
        CreateMap<IEnumerable<MovieResponse>, MoviesResponse>()
            .ForMember(
                dest => dest.Items, 
                opt => opt.MapFrom(src => src)
            );
        CreateMap<GetAllMoviesRequest, MoviesResponse>()
            .ForMember(
                dest => dest.Page,
                opt => opt.MapFrom(src => src.Page)
            )
            .ForMember(
                dest => dest.PageSize,
                opt => opt.MapFrom(src => src.PageSize)
            );
    }
}