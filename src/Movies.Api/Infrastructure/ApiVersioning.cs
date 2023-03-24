using System.Diagnostics.CodeAnalysis;
using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;

namespace Movies.Api.Infrastructure;

public static class ApiVersioning
{
// Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
// Not sure valid in this case.
#pragma warning disable CS8618
    public static ApiVersionSet VersionSet { get; private set; }

	public static IEndpointRouteBuilder CreateApiVersionSet(this IEndpointRouteBuilder app)
    {
        VersionSet = app.NewApiVersionSet()
            .HasApiVersion(1.0)
            .HasApiVersion(2.0)
            .ReportApiVersions()
            .Build();

        return app;
    }
#pragma warning restore CS8618 
}
