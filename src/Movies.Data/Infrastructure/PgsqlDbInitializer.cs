using System.Data;
using Dapper;
using Movies.Data.Database;

namespace Movies.Data.Infrastructure;

public class PgsqlDbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public PgsqlDbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        try
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync("""
            create table if not exists movies (
            id UUID primary key,
            slug TEXT not null, 
            title TEXT not null,
            yearofrelease integer not null);
        """);
        
            await connection.ExecuteAsync("""
            create unique index concurrently if not exists movies_slug_idx
            on movies
            using btree(slug);
        """);
        
            await connection.ExecuteAsync("""
            create table if not exists genres (
            movieId UUID references movies (Id),
            name TEXT not null);
        """);         }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
   
    }
}