
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Contracts.Requests;
using Movies.Api.Sdk;
using Movies.Api.Sdk.Consumer;
using Refit;

//var moviesApi = RestService.For<IMoviesApi>("https://localhost:5001");

var services = new ServiceCollection();

services
    .AddHttpClient()
    ///////////////// Should convert to an extension method to so API user doesn't have to worry about the details
    .AddSingleton<AuthTokenProvider>()
    .AddRefitClient<IMoviesApi>(s => new RefitSettings
    {
        // Calls Identity.Api and gets a fresh token with each run.
        AuthorizationHeaderValueGetter = async () => await s.GetRequiredService<AuthTokenProvider>().GetTokenAsync()
    })
    //////////////////////////////////////
    .ConfigureHttpClient(x =>
        x.BaseAddress = new Uri("https://localhost:5001"));

var provider = services.BuildServiceProvider();

var moviesApi = provider.GetRequiredService<IMoviesApi>();

var movieBySlug = await moviesApi.GetMovieBySlugAsync("jumanji-1995");
var movieById = await moviesApi.GetMovieByIdAsync("5a6c5d21-22a1-4280-a7e1-1ccb43373fee");

var newMovie = await moviesApi.CreateMovieAsync(new CreateMovieRequest
{
    Title = "Spiderman 3",
    YearOfRelease = 2003,
    Genres = new []{ "Action"}
});

await moviesApi.UpdateMovieAsync(newMovie.Id, new UpdateMovieRequest()
{
    Title = "Spiderman 3",
    YearOfRelease = 2003,
    Genres = new []{ "Action", "Adventure"}
});

await moviesApi.DeleteMovieAsync(newMovie.Id);

var request = new GetAllMoviesRequest
{
    Title = null,
    Year = null,
    SortBy = null,
    Page = 1,
    PageSize = 3
};

var movies = await moviesApi.GetMoviesAsync(request);

foreach (var movieResponse in movies.Items)
{
    Console.WriteLine(JsonSerializer.Serialize(movieResponse));
}

