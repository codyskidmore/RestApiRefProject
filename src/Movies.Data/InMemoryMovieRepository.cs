﻿using Movies.Contracts.Data.Interfaces;
using Movies.Contracts.Data.Models;

namespace Movies.Data;

public class InMemoryMovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = new();
    
    public  Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        _movies.Add(movie);
        return Task.FromResult(true);
    }

    public Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var movie = _movies.SingleOrDefault(x => x.Id == id);
        return Task.FromResult(movie);
    }

    public Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        var movie = _movies.SingleOrDefault(x => x.Slug == slug);
        return Task.FromResult(movie);
    }

    public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)
    {
        return Task.FromResult(_movies.AsEnumerable());
    }

    public Task<bool> UpdateAsync(Movie movie, CancellationToken token = default)
    {
        var movieIndex = _movies.FindIndex(x => x.Id == movie.Id);
        if (movieIndex == -1)
        {
            return Task.FromResult(false);
        }

        _movies[movieIndex] = movie;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        // NOTE! In real code you almost always soft-delete these with a flag
        // instead of actually removing the entity from the repository.
        var removeCount = _movies.RemoveAll(x => x.Id == id);
        return Task.FromResult(removeCount > 0);
    }

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        return Task.FromResult(_movies.Exists(x => x.Id == id));
    }
}