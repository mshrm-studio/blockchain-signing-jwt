using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Options;

public sealed record TokenEndpointOptions
{
    public TokenEndpointOptions() { }

    public string Extension { get; set; } = "blockchain/token";
}