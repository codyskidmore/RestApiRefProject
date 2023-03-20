
using Movies.Contracts.Data.Models;

namespace Movies.Contracts.Data.Interfaces;

public interface IRatingRepository
{
    Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken token = default);

    Task<float?> GetAverageMovieRatingAsync(Guid movieId, CancellationToken token = default);
    
    Task<(float? AverageRating, int? UserRating)> GetAverageMovieRatingByUserAsync(Guid movieId, Guid userId, CancellationToken token = default);

    Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default);

    Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default);
}



