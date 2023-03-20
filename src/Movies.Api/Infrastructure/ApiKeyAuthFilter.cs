using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Movies.Api.Infrastructure.Constants;

namespace Movies.Api.Infrastructure;

public class ApiKeyAuthFilter : IAuthorizationFilter // See also IAsyncAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyAuthFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName,
                out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key missing");
            return;
        }

        var apiKey = _configuration["ApiKey"]!;
        if (apiKey != extractedApiKey)
        {
            context.Result = new UnauthorizedObjectResult("Invalid API Key");
        }
    }

    // See also IAsyncAuthorizationFilter
    // public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    // {
    //     throw new NotImplementedException();
    // }
}