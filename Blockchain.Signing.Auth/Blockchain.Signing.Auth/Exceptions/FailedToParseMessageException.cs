using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Exceptions
{
    internal class FailedToParseMessageException : BlockchainAuthenticationException
    {
        internal FailedToParseMessageException(string? message = null) : base(message) { }
    }
}
