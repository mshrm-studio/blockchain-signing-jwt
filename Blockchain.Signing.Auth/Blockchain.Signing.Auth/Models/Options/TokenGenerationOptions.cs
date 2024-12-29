using Blockchain.Signing.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Models.Options;

public sealed record TokenGenerationOptions
{
    public required string Secret { get; init; } = "8a1dd1f0-dc55-4e0e-91a0-81fe44417661";
    public required int ExpiresInMinutes { get; init; } = 60;
    public required long ExpiresThresholdInSeconds { get; init; } = 30;
    public required string Issuer { get; init; } = "https://set-me";
    public required string Audience { get; init; } = "https://set-me-too";

    public required TokenGenerationEvents Events { get; init; } = new TokenGenerationEvents();
}

public sealed class TokenGenerationEvents
{
    public Func<TokenGenerationContext, Task> PostSignatureValidation { get; set; } 
        = context => Task.CompletedTask;
    public Func<RefreshTokenGenerationContext, Task<bool>> OnRefreshTokenValidation { get; set; }
        = context => throw new NotImplementedException(
            $"{nameof(TokenGenerationOptions)}.{nameof(TokenGenerationEvents)}.{nameof(OnRefreshTokenValidation)} has no delegate registered. Please refer to documentation.");
}

