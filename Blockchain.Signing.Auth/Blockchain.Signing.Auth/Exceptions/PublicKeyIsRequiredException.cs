using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Exceptions;
public class PublicKeyIsRequiredException : BlockchainAuthenticationException
{
    public PublicKeyIsRequiredException(string? message = null) : base(message) { }
}
