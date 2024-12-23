using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Exceptions
{
    public abstract class BlockchainAuthenticationException : Exception
    {
        public BlockchainAuthenticationException(string? message = null): base(message)
        {
        }
    }
}
