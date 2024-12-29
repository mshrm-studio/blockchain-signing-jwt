using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Exceptions
{
    public sealed class OptionsNotRegisteredException : BlockchainAuthenticationException
    {
        public OptionsNotRegisteredException(string? message = null) : base(message)
        { 
        }
    }
}
