using Movies.Api.Infrastructure.Constants;
using Http = Microsoft.AspNetCore.Http;

namespace Movies.Api.Infrastructure;

public static class IdentityExtensions
{
    // public static T? GetJwtClaim<T>(this HttpContext context, string claimType) //where T : IConvertible
    // {
    //     try
    //     {
    //         return (T?)Convert.ChangeType(context.User.Claims.SingleOrDefault(x => x.Type == claimType), typeof(T));
    //     }
    //     catch (InvalidCastException  e)
    //     {
    //         Console.WriteLine(e);
    //     }
    //
    //     return default;
    // }
    
    public static Guid? GetUserId(this Http.HttpContext context)
    {
        var userId = context.User.Claims.SingleOrDefault(x => x.Type == JwtConstants.UserId);

        if (Guid.TryParse(userId?.Value, out var parsedId))
        {
            return parsedId;
        }

        return null;
    }
}
