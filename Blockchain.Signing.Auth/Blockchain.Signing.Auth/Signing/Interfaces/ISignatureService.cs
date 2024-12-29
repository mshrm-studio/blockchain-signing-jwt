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
        Task<bool> VerifySignatureAsync(string message, string signature, string address);
        Task<(bool, string?)> VerifyAndRecoverPublicKeyFromSignatureAsync(string message, string signature);
        SupportedMethod GetSupportedMethod();
    }
}
