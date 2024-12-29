using Blockchain.Signing.Auth.Constants;
using Blockchain.Signing.Auth.Models;
using Blockchain.Signing.Auth.Models.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Blockchain.Signing.Auth.Extensions;

namespace Blockchain.Signing.Auth.Services;

public class BlockchainJwtService : IBlockchainJwtService
{
    private readonly TokenGenerationOptions _options;

    public BlockchainJwtService(IOptions<TokenGenerationOptions> options)
    {
        _options = options.Value;
    }

    public Jwt GenerateJwt(TokenGenerationContext context)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_options.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            IssuedAt = DateTime.UtcNow,
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            Subject = new ClaimsIdentity(context.Claims),
            Expires = DateTime.UtcNow.AddMinutes(_options.ExpiresInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var expiresAt = tokenDescriptor.Expires.Value.GetAbsoluteDifferenceInSeconds(DateTime.UtcNow);
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new Jwt(tokenHandler.WriteToken(token), context.RefreshToken, expiresAt);
    }
}
