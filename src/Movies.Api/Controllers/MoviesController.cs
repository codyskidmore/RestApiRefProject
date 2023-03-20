using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movies.Contracts.Api.Requests;
using Movies.Api.Abstraction.Responses;
using Movies.Api.Infrastructure;
using Movies.Contracts.Api.Responses;
using Movies.Contracts.Application.Interfaces;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService, IMapper mapper)
    {
        _movieService = movieService;
        _mapper = mapper;
    }

    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest movieRequest,
        CancellationToken token)
    {
        var movie = _mapper.Map<Movie>(movieRequest);
        await _movieService.CreateAsync(movie, token);
        var movieResponse = _mapper.Map<MovieResponse>(movie);
        return CreatedAtAction(nameof(Get), new { id = movie.Id }, movieResponse);
    }
    
    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id,
        CancellationToken token)
    {
        var movie = await _movieService.GetByIdAsync(id, token);
        if (movie is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<MovieResponse>(movie));
    }
    
    [HttpGet(ApiEndpoints.Movies.GetBySlug)]
    public async Task<IActionResult> GetBySlug([FromRoute] string slug,
        CancellationToken token)
    {
        var movie = await _movieService.GetBySlugAsync(slug, token);
        if (movie is null)
        {
            return NotFound();
        }
    
        return Ok(_mapper.Map<MovieResponse>(movie));
    }
    
    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> Get(CancellationToken token)
    {
        var movies = await _movieService.GetAllAsync(token);
        var movieResponses = _mapper.Map<IEnumerable<MovieResponse>>(movies);
        return Ok(_mapper.Map<MoviesResponse>(movieResponses));
    }    
    
    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Put([FromRoute] Guid id, 
        [FromBody] UpdateMovieRequest updateMovieRequest,
        CancellationToken token)
    {
        var movieWithUpdates = _mapper.Map<Movie>(updateMovieRequest);
        movieWithUpdates.Id = id;

        var updatedMovie = await _movieService.UpdateAsync(movieWithUpdates, token);
        if (updatedMovie is null)
        {
            return NotFound();
        }
        
        return Ok(_mapper.Map<MovieResponse>(movieWithUpdates));
    }
    
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id,
        CancellationToken token)
    {
        var notDeleted = !await _movieService.DeleteByIdAsync(id, token);
        if (notDeleted)
        {
            return NotFound();
        }

        return Ok();
    }}