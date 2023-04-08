using Movies.Contracts.Data.Models;
using OneOf;
using OneOf.Types;

namespace Movies.Contracts.Application.Interfaces;

public interface IMovieService
{
    // Should return MovieDto -- task for another day
    Task<OneOf<Success, Error, ValidationFailed>> CreateAsync(Movie movie, CancellationToken token = default);
    
    Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default);
    
    Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default);
    
    Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default);
    
    Task<OneOf<Movie, NotFound, ValidationFailed>> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
    Task<int> GetCountAsync(string? title, int? yearOfRelease, CancellationToken token = default);
}