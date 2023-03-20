using FluentValidation;
using Movies.Contracts.Application.Interfaces;
using Movies.Contracts.Data.Interfaces;
using Movies.Contracts.Data.Models;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    private readonly IValidator<Movie> _movieValidator;

    // Should use MovieDto -- task for another day
    public MovieService(IMovieRepository movieRepository,
        IValidator<Movie> movieValidator)
    {
        _movieRepository = movieRepository;
        _movieValidator = movieValidator;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        movie.Id = Guid.NewGuid();
        await _movieValidator.ValidateAndThrowAsync(movie, token);
        return await _movieRepository.CreateAsync(movie, token);
    }

    public Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _movieRepository.GetByIdAsync(id, token);
    }

    public Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        return _movieRepository.GetBySlugAsync(slug, token);
    }

    public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)
    {
        return _movieRepository.GetAllAsync(token);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken token = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, token);
        var noMovieFound = !await _movieRepository.ExistsByIdAsync(movie.Id, token);
        if (noMovieFound)
        {
            return null;
        }

        await _movieRepository.UpdateAsync(movie, token);
        return movie;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _movieRepository.DeleteByIdAsync(id, token);
    }

}