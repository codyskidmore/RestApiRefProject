﻿namespace Movies.Api.Infrastructure;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Movies
    {
        private const string Base = $"{ApiBase}/movies";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}"; // constrained to must be GUID.
        public const string GetBySlug = $"{Base}/{{slug}}"; // constrained to must be string.
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}"; // constrained to must be GUID.
        public const string Delete = $"{Base}/{{id:guid}}"; // constrained to must be GUID.
    }
}