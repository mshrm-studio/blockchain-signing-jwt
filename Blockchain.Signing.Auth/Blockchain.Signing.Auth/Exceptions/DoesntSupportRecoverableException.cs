using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Exceptions
{
    public sealed class DoesntSupportRecoverableException : BlockchainAuthenticationException
    {
        internal DoesntSupportRecoverableException(string? message = null) : base(message) { }
    }
}
