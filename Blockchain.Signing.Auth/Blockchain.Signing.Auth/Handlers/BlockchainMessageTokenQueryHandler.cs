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
using Blockchain.Signing.Auth.Models.Enums;
using Org.BouncyCastle.Asn1.Ocsp;
using Solnet.Rpc.Models;
using System.Globalization;
using Blockchain.Signing.Auth.Services;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using NBitcoin.Secp256k1;

namespace Blockchain.Signing.Auth.Handlers
{
    internal class BlockchainMessageTokenQueryHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBlockchainJwtService _blockchainJwtService;
        private readonly HttpContext _httpContext;
        private readonly TokenGenerationOptions _tokenGenerationOptions;

        public BlockchainMessageTokenQueryHandler(
            IServiceProvider serviceProvider,
            IBlockchainJwtService blockchainJwtService,
            HttpContext httpContext,
            IOptions<TokenGenerationOptions> tokenGenerationOptions
        )
        {
            _serviceProvider = serviceProvider;
            _blockchainJwtService = blockchainJwtService;
            _httpContext = httpContext;
            _tokenGenerationOptions = tokenGenerationOptions.Value;
        }

        internal async Task<TokenResponse> HandleAsync(TokenQuery request)
        {
            var publicKey = await GetVerifiedPublicKeyAsync(request.Network.Trim(), request.Signature,
                request.RawMessage.ToString(BlockchainAuthenticationConstants.MessageDateFormat, CultureInfo.InvariantCulture), request.PublicKey);
            if (string.IsNullOrEmpty(publicKey))
            {
                throw new FailedToVerifyException();
            }

            if (Math.Abs((DateTime.UtcNow - request.RawMessage).TotalSeconds) > _tokenGenerationOptions.ExpiresThresholdInSeconds)
            {
                throw new MessageExpiredException("The message has expired.");
            }

            var token = await GenerateTokenAsync(publicKey);

            return new TokenResponse()
            {
                Token = token.Token,
                ExpiresAt = token.ExpiresAt
            };
        }

        internal async Task<Jwt> GenerateTokenAsync(string publicKey)
        {
            var context = new TokenGenerationContext(publicKey, _httpContext);

            await _tokenGenerationOptions.Events.OnGeneration(context);

            return _blockchainJwtService.GenerateJwt(context);
        }

        internal async Task<string?> GetVerifiedPublicKeyAsync(string network, string signature, string message, string? publicKey)
        {
            var signingService = _serviceProvider.GetKeyedService<ISignatureService>(network);
            if (signingService is null)
            {
                throw new NetworkNotRegisteredException($"Network {network} has no implementation registered.");
            }

            string? verifiedPublicKey = null;

            switch (signingService.GetSupportedMethod())
            {
                case SupportedMethod.VerifyAndRecoverPublicKey:
                    verifiedPublicKey = await VerifyAndRecoverPublicKeyFromSignatureAsync(signingService, message, signature);
                    break;
                case SupportedMethod.Verify:
                    verifiedPublicKey = await VerifyAsync(signingService, message, signature, publicKey, network);
                    break;
            }

            return verifiedPublicKey;
        }

        internal async Task<string> VerifyAndRecoverPublicKeyFromSignatureAsync(ISignatureService signatureService, string message, string signature)
        {
            var result = await signatureService.VerifyAndRecoverPublicKeyFromSignatureAsync(message, signature);
            if (!result.Item1 || string.IsNullOrEmpty(result.Item2))
            {
                throw new FailedToParseMessageException("Could not get address from signature provided");
            }

            return result.Item2;
        }

        internal async Task<string> VerifyAsync(ISignatureService signatureService, string message, string signature, string publicKey, string network)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                throw new PublicKeyIsRequiredException($"Network {network} uses a algorithm where the public key is not recoverable from signature.");
            }
            var parsedCorrectly = await signatureService.VerifySignatureAsync(message, signature, publicKey);
            if (!parsedCorrectly)
            {
                throw new FailedToParseMessageException("Could not get address from signature provided");
            }

            return publicKey;
        }
    }
}
