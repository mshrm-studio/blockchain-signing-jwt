using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Models.Options;

public sealed record TokenEndpointOptions
{
    public TokenEndpointOptions() { }

    public string TokenExtension { get; set; } = "blockchain/token";
    public string RefreshTokenExtension { get; set; } = "blockchain/refresh-token";
}