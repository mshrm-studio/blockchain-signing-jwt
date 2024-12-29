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
    public List<Claim> Claims { get; internal set; } = new List<Claim>();    
    public HttpContext HttpContext { get; internal set; }
    public string Address { get; internal set; }
    public string Network { get; internal set; }

    public TokenGenerationContext(string publicKey, string network, HttpContext httpContext,
        List<Claim>? claims = null)
    {
        this.Address = publicKey;
        this.Network = network;
        this.HttpContext = httpContext;

        AddDefaultClaims();
        AddClaims(claims);
    }

    private void AddDefaultClaims()
    {
        Claims.Add(new Claim(BlockchainAuthenticationClaimTypes.Address, Address));
        Claims.Add(new Claim(BlockchainAuthenticationClaimTypes.Network, Network));
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
