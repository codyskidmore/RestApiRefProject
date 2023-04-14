using AutoMapper;
using Movies.Api.Contracts.Requests;
using Movies.Api.Contracts.Responses;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Infrastructure.Mappers;

public static class MapperExtensions
{
    public static GetAllMoviesOptions AddUserId(this GetAllMoviesOptions options, Guid? userId)
    {
        options.UserId = userId;
        return options;
    }
}