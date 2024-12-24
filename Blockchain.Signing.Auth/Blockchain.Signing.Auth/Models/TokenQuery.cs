using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Models
{
    public record TokenQuery
    {
        [JsonPropertyName("network")]
        public required string Network { get; init; }

        [JsonPropertyName("signature")]
        public required string Signature { get; init; }

        [JsonPropertyName("raw_message")]
        public required DateTime RawMessage { get; init; }

        [JsonPropertyName("public_key")]
        public string? PublicKey { get; init; }
    }
}
