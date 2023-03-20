namespace Movies.Api.Infrastructure.Constants;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Movies
    {
        private const string Base = $"{ApiBase}/movies";

        public const string Create = Base;
        public const string GetById = $"{Base}/guid/{{id:guid}}"; // constrained to must be GUID.
        public const string GetBySlug = $"{Base}/slug/{{slug}}"; // constrained to must be string.
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}"; // constrained to must be GUID.
        public const string Delete = $"{Base}/{{id:guid}}"; // constrained to must be GUID.

        public const string Rate = $"{Base}/{{id:guid}}/ratings";
        public const string DeleteRating = $"{Base}/{{id:guid}}/ratings";    
    }
    
    public static class Ratings
    {
        private const string Base = $"{ApiBase}/ratings";

        public const string GetUserRatings = $"{Base}/me";
    }}