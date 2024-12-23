using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Options
{
    public sealed record TokenGenerationOptions
    {
        public required string Secret { get; init; }
        public required int ExpiresInMinutes { get; init; }
        public required int ExpiresThresholdInSeconds { get; init; }
    }
}
