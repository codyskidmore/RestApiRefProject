﻿using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Contracts.Requests;
using Movies.Api.Contracts.Responses;
using Movies.Api.Infrastructure;
using Movies.Api.Infrastructure.Constants;
using Movies.Api.Infrastructure.Mappers;
using Movies.Contracts.Application.Interfaces;
using Movies.Contracts.Data.Models;

namespace Movies.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
public class MoviesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMovieService _movieService;
    private readonly IOutputCacheStore _outputCacheStore;

    public MoviesController(IMovieService movieService, IMapper mapper, IOutputCacheStore outputCacheStore)
    {
        _movieService = movieService;
        _mapper = mapper;
        _outputCacheStore = outputCacheStore;
    }

    [HttpPost(ApiEndpoints.Movies.Create)]
    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    //[ServiceFilter(typeof(ApiKeyAuthFilter))]
    // Describes the response type to Swagger
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status201Created)]
    // Describes the response type to Swagger
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest movieRequest,
        CancellationToken token)
    {
        var movie = _mapper.Map<Movie>(movieRequest);
        await _movieService.CreateAsync(movie, token);
        var movieResponse = _mapper.Map<MovieResponse>(movie);
        await _outputCacheStore.EvictByTagAsync(CacheConstants.MovieCacheTagName, token);
        return CreatedAtAction(nameof(Get), new { id = movie.Id }, movieResponse);
    }
    
    [HttpGet(ApiEndpoints.Movies.GetById)]
    //[ApiVersion(1.0, Deprecated = true)] //NICE HUH?!
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ResponseCache(Duration = 30, VaryByHeader = "Accept, Accept-Encoding", Location = ResponseCacheLocation.Any)] 
    [OutputCache(PolicyName = CacheConstants.MovieCachePolicyName)]
    public async Task<IActionResult> GetV1([FromRoute] Guid id,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var movie = await _movieService.GetByIdAsync(id, userId, token);
        if (movie is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<MovieResponse>(movie));
    }
    
    // [HttpGet(ApiEndpoints.Movies.Get)]
    // [ApiVersion(2.0)] // NICE HUH?!
    // public async Task<IActionResult> GetV2([FromRoute] Guid id,
    //     CancellationToken token)
    // {
    //     var userId = HttpContext.GetUserId();
    //     var movie = await _movieService.GetByIdAsync(id, userId, token);
    //     if (movie is null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return Ok(_mapper.Map<MovieResponse>(movie));
    // }

    [HttpGet(ApiEndpoints.Movies.GetBySlug)]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [OutputCache(PolicyName = CacheConstants.MovieCachePolicyName)]
    public async Task<IActionResult> GetBySlug([FromRoute] string slug,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var movie = await _movieService.GetBySlugAsync(slug, userId, token);
        if (movie is null)
        {
            return NotFound();
        }
    
        return Ok(_mapper.Map<MovieResponse>(movie));
    }
    
    [HttpGet(ApiEndpoints.Movies.GetAll)]
    // Describes the response type to Swagger
    [ProducesResponseType(typeof(MoviesResponse), StatusCodes.Status200OK)]
    //[ResponseCache(Duration = 30, VaryByQueryKeys = new []{"title", "year", "sortBy", "page", "pageSize"}, VaryByHeader = "Accept, Accept-Encoding", Location = ResponseCacheLocation.Any)]
    [OutputCache(PolicyName = CacheConstants.MovieCachePolicyName)]
    public async Task<IActionResult> Get(
        [FromQuery] GetAllMoviesRequest request, 
        CancellationToken token)
    {
        var options = _mapper.Map<GetAllMoviesOptions>(request).AddUserId(HttpContext.GetUserId());
        var movies = await _movieService.GetAllAsync(options, token);
        var totalMovieCount = await _movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
        var movieResponses = _mapper.Map<IEnumerable<MovieResponse>>(movies);
        var moviesResponse = _mapper.Map<MoviesResponse>(movieResponses).AddTotalMovieCount(totalMovieCount, request);
        return Ok(moviesResponse);
    }    
    
    [HttpPut(ApiEndpoints.Movies.Update)]
    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put([FromRoute] Guid id, 
        [FromBody] UpdateMovieRequest updateMovieRequest,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var movieWithUpdates = _mapper.Map<Movie>(updateMovieRequest);
        movieWithUpdates.Id = id;

        var updatedMovie = await _movieService.UpdateAsync(movieWithUpdates, userId, token);
        if (updatedMovie is null)
        {
            return NotFound();
        }

        await _outputCacheStore.EvictByTagAsync(CacheConstants.MovieCacheTagName, token);
        
        return Ok(_mapper.Map<MovieResponse>(movieWithUpdates));
    }
    
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    [Authorize(AuthConstants.AdminUserPolicyName)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id,
        CancellationToken token)
    {
        var notDeleted = !await _movieService.DeleteByIdAsync(id, token);
        if (notDeleted)
        {
            return NotFound();
        }

        await _outputCacheStore.EvictByTagAsync(CacheConstants.MovieCacheTagName, token);

        return Ok();
    }}