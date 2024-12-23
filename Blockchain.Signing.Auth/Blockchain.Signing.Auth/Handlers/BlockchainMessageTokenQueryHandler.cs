﻿using Blockchain.Signing.Auth.Constants;
using Blockchain.Signing.Auth.Models;
using Blockchain.Signing.Auth.Exceptions;
using Blockchain.Signing.Auth.Options;
using Blockchain.Signing.Auth.Signing.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Handlers
{
    internal class BlockchainMessageTokenQueryHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public BlockchainMessageTokenQueryHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider; 
        }

        internal async Task<TokenResponse> HandleAsync(TokenQuery request)
        {
            var options = _serviceProvider.GetService<IOptionsMonitor<TokenGenerationOptions>>();
            if (options is null)
            {
                throw new OptionsNotRegisteredException($"Options have not been added.");
            }

            var signingService = _serviceProvider.GetKeyedService<ISignatureService>(request.Network.Trim());
            if (signingService is null)
            {
                throw new NetworkNotRegisteredException($"Network {request.Network} has no implementation registered.");
            }

            var parsedCorrectly = signingService.GetAddressFromSignature(request.RawMessage.ToString(), request.Signature, out var publicKey);
            if (!parsedCorrectly || string.IsNullOrEmpty(publicKey))
            {
                throw new FailedToParseMessageException("Could not get address from signature provided");
            }

            // Check the threshold either way (Math.Abs)
            if (Math.Abs((DateTime.UtcNow - request.RawMessage).TotalSeconds) > options.CurrentValue.ExpiresThresholdInSeconds)
            {
                throw new MessageExpiredException("The message has expired.");
            }

            var token = GenerateToken(options.CurrentValue, publicKey);

            return new TokenResponse()
            {
                Token = token.Token,
                ExpiresAt = token.ExpiresAt
            };
        }

        private (string Token, long ExpiresAt) GenerateToken(TokenGenerationOptions options, string publicKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(options.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.UtcNow,
                Issuer = options.Issuer,
                Audience = options.Audience,
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(BlockchainAuthenticationClaimTypes.PublicKey, publicKey) 
                }),
                Expires = DateTime.UtcNow.AddMinutes(options.ExpiresInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var expiresAt = (tokenDescriptor.Expires.Value - DateTime.UtcNow);
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (tokenHandler.WriteToken(token), (long)expiresAt.TotalSeconds);
        }
    }
}
