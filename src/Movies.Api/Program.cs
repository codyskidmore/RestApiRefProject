using Movies.Api.Health;
using Movies.Api.Infrastructure;
using Movies.Api.Infrastructure.Constants;
using Movies.Api.Mapping;
using Movies.Application.Infrastructure;
using Movies.Data.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddMovieApiAuthentication(config);

builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddMovieApiVersioning();

// SEE: https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-7.0
// Replace with Redis cache..
//builder.Services.AddResponseCaching();
var cacheConfig = config.GetSection(CacheConstants.MovieCachePolicySection).Get<CacheSettings>();
builder.Services.AddMovieApiCache(cacheConfig!);

builder.Services.AddControllers();

builder.Services.AddHealthChecks().AddCheck<DatabaseHealthCheck>(DatabaseHealthCheck.Name);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddMovieApiSwaggerOptions();

builder.Services.AddMovieApplicationServices();
builder.Services.AddMovieApiServices();
builder.Services.AddMovieDataServices(config["Database:ConnectionString"]!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseMovieApiSwaggerUI();
}

app.MapHealthChecks("_health");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// app.UseCors(); // MUST COME BEFORE ADDING CACHING!!
//app.UseResponseCaching();
// Only 200 responses, GET, & HEAD requests are cached.
app.UseOutputCache();

// Order of statement is significant!
app.UseMiddleware<ValidationMappingMiddleware>();

app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<PgsqlDbInitializer>();
await dbInitializer.InitializeAsync();

app.Run();