using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movies.Api.Infrastructure.Constants;
using Movies.Api.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movies.Api.Infrastructure;

public static class MovieApiConfiguration
{
    public static IServiceCollection AddMovieApiServices(this IServiceCollection services)
    {
        var moviesApiAssembly = Assembly.GetExecutingAssembly();
        services.AddAutoMapper(moviesApiAssembly);
        return services;
    }
    public static IServiceCollection AddMovieApiAuthentication(this IServiceCollection services, IConfigurationRoot config)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = config["Jwt:Issuer"],
                ValidAudience = config["Jwt:Audience"],
                ValidateIssuer = true,
                ValidateAudience = true
            };
        });

        services.AddAuthorization(x =>
        {
            // x.AddPolicy(AuthConstants.AdminUserPolicyName, 
            //     p => p.RequireClaim(AuthConstants.AdminUserClaimName, "true"));

            x.AddPolicy(AuthConstants.AdminUserPolicyName,
                // This adds multiple authorization types (API Key, credentials, etc.
                p => p.AddRequirements(new AdminAuthRequirement(config["ApiKey"]!)));

            x.AddPolicy(AuthConstants.TrustedMemberPolicyName,
                p => p.RequireAssertion(c => 
                    c.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: "true" }) || 
                    c.User.HasClaim(m => m is { Type: AuthConstants.TrustedMemberClaimName, Value: "true" })));
        });

        return services;
    }
    public static IServiceCollection AddMovieApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(x =>
        {
            x.DefaultApiVersion = new ApiVersion(1.0);
            x.AssumeDefaultVersionWhenUnspecified = true;
            x.ReportApiVersions = true;
            x.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
        }).AddApiExplorer();

        services.AddEndpointsApiExplorer();
        
        return services;
    }
    
    public static IServiceCollection AddMovieApiSwaggerOptions(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(x => x.OperationFilter<SwaggerDefaultValues>());
        return services;
    }
    
    public static WebApplication UseMovieApiSwaggerUI(this WebApplication app)
    {
        app.UseSwaggerUI(x =>
        {
            foreach (var description in app.DescribeApiVersions())
            {
                x.SwaggerEndpoint( $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName);
            }
        });
        return app;
    }
    
    public static IServiceCollection AddMovieApiCache(this IServiceCollection services, CacheSettings cacheSettings)
    {
        services.AddOutputCache(x =>
        {
            x.AddBasePolicy(c => c.Cache());
            x.AddPolicy(cacheSettings.PolicyName, c => 
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(cacheSettings.Expiration))
                    .SetVaryByQuery(cacheSettings.QueryKeys)
                    .Tag(cacheSettings.TagName));
        });
        return services;
    }
    
}