using Movies.Api.Contracts.Requests;
using Movies.Api.Contracts.Responses;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Infrastructure.Mappers;

public static class GetAllMoviesOptionsMapper
{
    public static GetAllMoviesOptions AddUserId(this GetAllMoviesOptions options, Guid? userId)
    {
        options.UserId = userId;
        return options;
    }

    public static MoviesResponse AddPagingInfoToResponse(this MoviesResponse response, int totalMovieCount, GetAllMoviesRequest request)
    {
        response.Total = totalMovieCount;
        response.Page = request.Page!.Value;
        response.PageSize = request.PageSize!.Value;
        return response;
    }
    
    public static GetAllMoviesOptions MapToGetAllMoviesOptions(this GetAllMoviesRequest request)
    {
        return new GetAllMoviesOptions
        {
            Title = request.Title,
            // YearOfRelease = request.YearOfRelease,
            // SortField = request.SortField,
            // SortOrder = request.SortOrder,
            Page = request.Page!.Value,
            PageSize = request.PageSize!.Value
        };
    }   
    
    public static GetAllMoviesOptions WithUser(this GetAllMoviesOptions options,
        Guid? userId)
    {
        options.UserId = userId;
        return options;
    }
    
}


