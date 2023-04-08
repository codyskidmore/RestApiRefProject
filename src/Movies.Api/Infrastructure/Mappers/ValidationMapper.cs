using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using FluentValidation.Results;
using Movies.Contracts.Application.Interfaces;
using Movies.Contracts.Responses;

namespace Movies.Api.Infrastructure.Mappers;

public static class ValidationMapper
{
    public static ValidationFailureResponse MapToResponse(this IEnumerable<ValidationFailure> failures)
    {
        return new ValidationFailureResponse
        {
            Errors = failures.Select(x => new ValidationResponse
            {
                PropertyName = x.PropertyName,
                Message = x.ErrorMessage
            })
        };
    }
}