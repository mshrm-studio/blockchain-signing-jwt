using Blockchain.Signing.Auth.Models.Enums;
using Blockchain.Signing.Auth.Signing.Interfaces;
using Microsoft.Extensions.Logging;
using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Signing;

public class EthereumSignatureService : ISignatureService
{
    private readonly ILogger<EthereumSignatureService> _logger; 

    public EthereumSignatureService(ILogger<EthereumSignatureService> logger)
    {
        _logger = logger;
    }

    public bool VerifySignature(string message, string signature, string address)
    {
        try
        {
            var signer = new EthereumMessageSigner();
            var addressRecovered = signer.EncodeUTF8AndEcRecover(message, signature);

            return string.Equals(addressRecovered, address, StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Error verifying signature: {ex.Message}");

            return false;
        }
    }

    public bool VerifyAndRecoverPublicKeyFromSignature(string message, string signature, out string? address)
    {
        address = null;

        try
        {
            var signer = new EthereumMessageSigner();
            address = signer.EncodeUTF8AndEcRecover(message, signature);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Error verifying signature: {ex.Message}");

            return false;
        }
    }

    public SupportedMethod GetSupportedMethod()
    {
        return SupportedMethod.VerifyAndRecoverPublicKey;
    }
}