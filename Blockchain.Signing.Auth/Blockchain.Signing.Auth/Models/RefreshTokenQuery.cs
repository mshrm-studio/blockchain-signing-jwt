using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Models;

public record RefreshTokenQuery
{
    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; init; }
}