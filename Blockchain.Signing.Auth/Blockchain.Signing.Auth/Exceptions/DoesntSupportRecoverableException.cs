﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Exceptions
{
    internal class DoesntSupportRecoverableException : BlockchainAuthenticationException
    {
        internal DoesntSupportRecoverableException(string? message = null) : base(message) { }
    }
}
