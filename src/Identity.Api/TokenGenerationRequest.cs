namespace Identity.Api;

public class TokenGenerationRequest
{
    public Guid UserId { get; set; }
    
    public string Email { get; set; } = String.Empty; // compiler warning.

    public Dictionary<string, object> CustomClaims { get; set; } = new();
}
