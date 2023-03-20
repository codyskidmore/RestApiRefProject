using FluentValidation;
using Movies.Contracts.Application.Interfaces;
using Movies.Contracts.Data.Interfaces;
using Movies.Contracts.Data.Models;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IRatingRepository _ratingRepository;

    private readonly IValidator<Movie> _movieValidator;
    private readonly IValidator<GetAllMoviesOptions> _getAllMoviesOptionsValidator;

    // Should use MovieDto -- task for another day
    public MovieService(
        IMovieRepository movieRepository,
        IRatingRepository ratingRepository,
        IValidator<Movie> movieValidator,
        IValidator<GetAllMoviesOptions> getAllMoviesOptionsValidator)
    {
        _movieRepository = movieRepository;
        _ratingRepository = ratingRepository;
        _movieValidator = movieValidator;
        _getAllMoviesOptionsValidator = getAllMoviesOptionsValidator;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        movie.Id = Guid.NewGuid();
        await _movieValidator.ValidateAndThrowAsync(movie, token);
        return await _movieRepository.CreateAsync(movie, token);
    }

    public Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
    {
        return _movieRepository.GetByIdAsync(id, userId, token);
    }

    public Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
    {
        return _movieRepository.GetBySlugAsync(slug, userId, token);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default)
    {
        await _getAllMoviesOptionsValidator.ValidateAndThrowAsync(options, token);
        var movies = await _movieRepository.GetAllAsync(options, token);
        return movies;
    }

    public async Task<Movie?> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, token);
        var noMovieFound = !await _movieRepository.ExistsByIdAsync(movie.Id, token);
        if (noMovieFound)
        {
            return null;
        }

        await _movieRepository.UpdateAsync(movie, token);

        if (!userId.HasValue)
        {
            var rating = await _ratingRepository.GetAverageMovieRatingAsync(movie.Id, token);
            movie.AverageRating = rating;
            return movie;
        }
        
        var ratings = await _ratingRepository.GetAverageMovieRatingByUserAsync(movie.Id, userId.Value, token);
        movie.AverageRating = ratings.AverageRating;
        movie.UserRating = ratings.UserRating;
        
        return movie;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _movieRepository.DeleteByIdAsync(id, token);
    }
    public Task<int> GetCountAsync(string? title, int? yearOfRelease, CancellationToken token = default)
    {
        return _movieRepository.GetCountAsync(title, yearOfRelease, token);
    }
}