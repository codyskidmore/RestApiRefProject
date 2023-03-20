using Movies.Api.Infrastructure;
using Movies.Api.Mapping;
using Movies.Application.Infrastructure;
using Movies.Data.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMovieApplicationServices();
builder.Services.AddMovieApiServices();
builder.Services.AddMovieDataServices(config["Database:ConnectionString"]!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Order of statement is significant!
app.UseMiddleware<ValidationMappingMiddleware>();

app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<PgsqlDbInitializer>();
await dbInitializer.InitializeAsync();

app.Run();