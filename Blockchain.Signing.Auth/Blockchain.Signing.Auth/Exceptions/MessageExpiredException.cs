using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Exceptions
{
    internal class MessageExpiredException : BlockchainAuthenticationException
    {
        internal MessageExpiredException(string? message = null) : base(message) { }
    }
}
