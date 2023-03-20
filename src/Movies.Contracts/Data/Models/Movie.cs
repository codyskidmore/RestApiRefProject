using System.Text.RegularExpressions;

namespace Movies.Contracts.Data.Models;

public partial class Movie
{
    public required Guid Id { get; set; }
    public required string Title { get; init; }
    // Don't think this is appropriate but will deal with it later.
    public string Slug => MovieSlugCalculator.GetMovieSlug(Title, YearOfRelease);
    public required int YearOfRelease { get; init; }
    public required List<string> Genres { get; init; } = new();
    public required int? UserRating { get; set; }
    public required float? AverageRating { get; set; }
}

internal static partial class MovieSlugCalculator
{
    // Don't think this is appropriate but will deal with it later.
    internal static string GetMovieSlug(string title, int yearOfRelease)
    {
        // NOTE!: Add guard code!
        var sluggedTitle = SlugRegex().Replace(title, string.Empty)
            .ToLower()
            .Replace(" ", "-");
        return $"{sluggedTitle}-{yearOfRelease}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();
}