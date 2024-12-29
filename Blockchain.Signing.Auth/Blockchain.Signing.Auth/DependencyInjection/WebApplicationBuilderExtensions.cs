using Blockchain.Signing.Auth.Constants;
using Blockchain.Signing.Auth.Handlers;
using Blockchain.Signing.Auth.Models.Options;
using Blockchain.Signing.Auth.Services;
using Blockchain.Signing.Auth.Signing;
using Blockchain.Signing.Auth.Signing.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.DependencyInjection;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddBlockchainSignatureVerification(this WebApplicationBuilder builder, Action<TokenGenerationOptions> options)
    {
        builder.Services.Configure(options);

        builder.Services.AddTransient<IBlockchainJwtService, BlockchainJwtService>();

        builder.Services.AddTransient<BlockchainMessageTokenQueryHandler>();

        builder.Services.AddKeyedTransient<ISignatureService, EvmSignatureService>(BlockchainAuthenticationConstants.BlockChains.Evm);
        builder.Services.AddKeyedTransient<ISignatureService, SolanaSignatureService>(BlockchainAuthenticationConstants.BlockChains.Solana);
        //TODO: Fix
        //builder.Services.AddKeyedTransient<ISignatureService, TonSignatureService>("Ton");

        builder.Services.AddHttpContextAccessor();

        return builder;
    }

    /// <summary>
    /// Add a custom implementation of ISignatureService
    /// </summary>
    /// <typeparam name="TImplementation">A class implementing ISignatureService</typeparam>
    /// <param name="builder">The builder</param>
    /// <param name="networkName">The network Name (keyed service)</param>
    /// <returns>The builder</returns>
    public static WebApplicationBuilder AddCustomSignatureService<TImplementation>(this WebApplicationBuilder builder, string networkName)
        where TImplementation : class, ISignatureService
    {
        builder.Services.AddKeyedTransient<ISignatureService, TImplementation>(networkName);

        return builder;
    }
}