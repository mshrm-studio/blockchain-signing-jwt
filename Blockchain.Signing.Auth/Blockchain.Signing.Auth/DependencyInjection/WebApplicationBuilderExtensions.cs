using Blockchain.Signing.Auth.Constants;
using Blockchain.Signing.Auth.Handlers;
using Blockchain.Signing.Auth.Options;
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

        builder.Services.AddTransient<BlockchainMessageTokenQueryHandler>();

        builder.Services.AddKeyedTransient<ISignatureService, EthereumSignatureService>("Ethereum");
        builder.Services.AddKeyedTransient<ISignatureService, SolanaSignatureService>("Solana");
        
        return builder;
    }
}