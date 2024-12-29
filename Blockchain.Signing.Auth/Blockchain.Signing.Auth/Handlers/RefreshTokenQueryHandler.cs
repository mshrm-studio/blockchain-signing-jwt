using Blockchain.Signing.Auth.Constants;
using Blockchain.Signing.Auth.Exceptions;
using Blockchain.Signing.Auth.Models;
using Blockchain.Signing.Auth.Models.Options;
using Blockchain.Signing.Auth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Handlers;

internal class RefreshTokenQueryHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IBlockchainJwtService _blockchainJwtService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly TokenGenerationOptions _tokenGenerationOptions;

    public RefreshTokenQueryHandler(
            IServiceProvider serviceProvider,
            IHttpContextAccessor contextAccessor,
            IBlockchainJwtService blockchainJwtService,
            IOptions<TokenGenerationOptions> tokenGenerationOptions
        )
    {
        _serviceProvider = serviceProvider;
        _contextAccessor = contextAccessor;
        _blockchainJwtService = blockchainJwtService;
        _tokenGenerationOptions = tokenGenerationOptions.Value;
    }

    internal async Task<TokenResponse> HandleAsync(RefreshTokenQuery request)
    {
        if (_contextAccessor.HttpContext == null)
        {
            throw new NullReferenceException("Http context is null");
        }

        var parsedToken = ParseToken(request.Token);

        var context = new RefreshTokenGenerationContext(request.Token, request.RefreshToken, _contextAccessor.HttpContext);

        var success = await _tokenGenerationOptions.Events.OnRefreshTokenValidation(context);
        if (!success)
        {
            throw new FailedToVerifyException("Refresh token was not validated.");
        }

        var token = await GenerateTokenAsync(parsedToken.Address, parsedToken.Network, _contextAccessor.HttpContext);

        return new TokenResponse()
        {
            Token = token.Token,
            RefreshToken = token.RefreshToken,
            ExpiresAt = token.ExpiresAt
        };
    }

    internal async Task<Jwt> GenerateTokenAsync(string publicKey, string network, HttpContext httpContext)
    {
        var context = new TokenGenerationContext(publicKey, network, httpContext);

        await _tokenGenerationOptions.Events.PostSignatureValidation(context);

        return _blockchainJwtService.GenerateJwt(context);
    }

    internal (string Network, string Address) ParseToken(string rawToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(rawToken);
        var jwtSecurityToken = jsonToken as JwtSecurityToken;

        var network = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == BlockchainAuthenticationClaimTypes.Network);
        var address = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == BlockchainAuthenticationClaimTypes.Address);

        if (string.IsNullOrEmpty(network?.Value))
        {
            throw new ClaimDoesntExistException($"{nameof(network)} claim named '{BlockchainAuthenticationClaimTypes.Network}' doesn't exist");
        }

        return (network.Value, address.Value);
    }
}
