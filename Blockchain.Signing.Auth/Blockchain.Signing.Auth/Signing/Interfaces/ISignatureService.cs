using Blockchain.Signing.Auth.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Signing.Interfaces
{
    public interface ISignatureService
    {
        bool VerifySignature(string message, string signature, string address);
        bool VerifyAndRecoverPublicKeyFromSignature(string message, string signature, out string? address);
        SupportedMethod GetSupportedMethod();
    }
}
