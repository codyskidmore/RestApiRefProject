﻿namespace Identity.Api;

public class TokenGenerationRequest
{
    public Guid UserId { get; set; }

    public string Email { get; set; } = string.Empty; // Compiler warning

    public Dictionary<string, object> CustomClaims { get; set; } = new();
}
