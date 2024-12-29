using Blockchain.Signing.Auth.Constants;
using Blockchain.Signing.Auth.Models;
using Blockchain.Signing.Auth.Handlers;
using Blockchain.Signing.Auth.Signing;
using Blockchain.Signing.Auth.Signing.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Blockchain.Signing.Auth.Models.Options;

namespace Blockchain.Signing.Auth.DependencyInjection;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddBlockchainTokenIssuanceEndpoint(this IApplicationBuilder builder, Action<TokenEndpointOptions> configureOptions)
    {
        var tokenEndpointOptions = Configure(configureOptions);

        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPost(tokenEndpointOptions.Extension, async ([FromBody]TokenQuery request, HttpContext httpContext) =>
            {
                var handler = httpContext.RequestServices.GetRequiredService<BlockchainMessageTokenQueryHandler>();

                return await handler.HandleAsync(request);
            });
        });

        return builder;
    }

    private static TokenEndpointOptions Configure(Action<TokenEndpointOptions> configureOptions)
    {
        var tokenEndpointOptions = new TokenEndpointOptions();

        configureOptions(tokenEndpointOptions);

        return tokenEndpointOptions;
    }
}
