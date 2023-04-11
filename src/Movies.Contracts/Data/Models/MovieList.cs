namespace Movies.Contracts.Data.Models;

public class MovieList : List<Movie>
{
    // OneOf doesn't like IENumerable<T>. This gets around it.
}