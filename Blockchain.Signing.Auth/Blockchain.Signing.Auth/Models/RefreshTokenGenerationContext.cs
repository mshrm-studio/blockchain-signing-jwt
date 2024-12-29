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
        public string Token { get; internal set; }
        public string RefreshToken { get; internal set; }

        public RefreshTokenGenerationContext(string token, string refreshToken, HttpContext httpContext)
        {
            this.Token = token;
            this.RefreshToken = refreshToken;
            this.HttpContext = httpContext;
        }
    }
}
