using FluentValidation.Results;

namespace Movies.Contracts.Application.Interfaces;

public record ValidationFailed(IEnumerable<ValidationFailure> Errors)
{
    public ValidationFailed(ValidationFailure error) : this(new[] { error })
    {
        
    }
}