using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Models.Enums;

public enum SupportedMethod
{
    Verify = 1,
    VerifyAndRecoverPublicKey = 2
}
