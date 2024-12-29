using Blockchain.Signing.Auth.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Models;

public class TokenGenerationContext
{
    public List<Claim> Claims { get; private set; } = new List<Claim>();    
    public HttpContext HttpContext { get; private set; }
    public string PublicKey { get; private set; }

    public TokenGenerationContext(string publicKey, HttpContext httpContext,
        List<Claim>? claims = null)
    {
        this.PublicKey = publicKey;
        this.HttpContext = httpContext;

        AddPublicKeyClaim(publicKey);
        AddClaims(claims);
    }

    private void AddPublicKeyClaim(string publicKey)
    {
        Claims.Add(new Claim(BlockchainAuthenticationClaimTypes.PublicKey, publicKey));
    }

    private void AddClaims(List<Claim>? claims)
    {
        if (claims?.Any() ?? false)
        {
            Claims.AddRange(claims);
        }

        Claims = Claims.Distinct().ToList();
    }
}
