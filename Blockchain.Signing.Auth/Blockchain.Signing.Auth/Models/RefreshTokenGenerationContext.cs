using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Models
{
    public sealed class RefreshTokenGenerationContext
    {
        public HttpContext HttpContext { get; internal set; }
        public string RawToken { get; internal set; }
        public string Address { get; internal set; }
        public string Network { get; internal set; }

        public RefreshTokenGenerationContext(string rawToken, string address, string network, HttpContext httpContext)
        {
            this.Address = address;
            this.Network = network;
            this.RawToken = rawToken;
            this.HttpContext = httpContext;
        }
    }
}
