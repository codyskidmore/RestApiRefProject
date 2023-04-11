using FluentValidation;
using Movies.Contracts.Application.Interfaces;
using Movies.Contracts.Data.Interfaces;
using Movies.Contracts.Data.Models;
using OneOf;
using OneOf.Types;

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

    public async Task<OneOf<Success, Error, ValidationFailed>> CreateAsync(Movie movie, CancellationToken token = default)
    {
        movie.Id = Guid.NewGuid();

        var validationResult = await _movieValidator.ValidateAsync(movie, token);
        if (!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        var result = await _movieRepository.CreateAsync(movie, token);

        if (!result)
        {
            return new Error();
        }
        
        return new Success();
    }

    public async Task<OneOf<Movie,NotFound>> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
    {
        var movieResult = await _movieRepository.GetByIdAsync(id, userId, token);

        if (movieResult == null)
        {
            return new NotFound();
        }

        return movieResult;
    }

    public async Task<OneOf<Movie,NotFound>> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
    {
        var movieResult = await _movieRepository.GetBySlugAsync(slug, userId, token);

        if (movieResult == null)
        {
            return new NotFound();
        }

        return movieResult;
    }

    public async Task<OneOf<MovieList, ValidationFailed>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default)
    {
        var validationResult = await _getAllMoviesOptionsValidator.ValidateAsync(options, token);
        if (!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        var movies = await _movieRepository.GetAllAsync(options, token);
        return movies;
    }

    public async Task<OneOf<Movie, NotFound, ValidationFailed>> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default)
    {
        //await _movieValidator.ValidateAndThrowAsync(movie, token);
        var validationResult = await _movieValidator.ValidateAsync(movie, token);

        if (!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }
        
        var noMovieFound = !await _movieRepository.ExistsByIdAsync(movie.Id, token);
        if (noMovieFound)
        {
            return new NotFound();
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