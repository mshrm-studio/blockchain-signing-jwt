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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.DependencyInjection;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddBlockchainTokenIssuanceEndpoint(this IApplicationBuilder builder, Action<TokenOptions> options)
    {
        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("blockchain/token", async ([FromBody]TokenQuery request, HttpContext httpContext) =>
            {
                var handler = httpContext.RequestServices.GetRequiredService<BlockchainMessageTokenQueryHandler>();

                return await handler.HandleAsync(request);
            });
        });

        return builder;
    }
}
