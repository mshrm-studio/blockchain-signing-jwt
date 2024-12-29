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

public class EvmSignatureService : ISignatureService
{
    private readonly ILogger<EvmSignatureService> _logger; 

    public EvmSignatureService(ILogger<EvmSignatureService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> VerifySignatureAsync(string message, string signature, string address)
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

    public async Task<(bool, string?)> VerifyAndRecoverPublicKeyFromSignatureAsync(string message, string signature)
    {
        try
        {
            var signer = new EthereumMessageSigner();
            var address = signer.EncodeUTF8AndEcRecover(message, signature);

            return (true, address);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Error verifying signature: {ex.Message}");

            return (false, null);
        }
    }

    public SupportedMethod GetSupportedMethod()
    {
        return SupportedMethod.VerifyAndRecoverPublicKey;
    }
}