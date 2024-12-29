using Blockchain.Signing.Auth.Exceptions;
using Blockchain.Signing.Auth.Models.Enums;
using Blockchain.Signing.Auth.Signing.Interfaces;
using Microsoft.Extensions.Logging;
using Nethereum.Signer;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math.EC.Rfc8032;
using Org.BouncyCastle.Utilities.Encoders;
using Solnet.Rpc.Models;
using Solnet.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PublicKey = Solnet.Wallet.PublicKey;

namespace Blockchain.Signing.Auth.Signing;

public class SolanaSignatureService : ISignatureService
{
    private readonly ILogger<SolanaSignatureService> _logger;

    public SolanaSignatureService(ILogger<SolanaSignatureService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> VerifySignatureAsync(string message, string signature, string address)
    {
        try
        {
            var publicKey = new PublicKey(address);
            var publicKeyParam = new Ed25519PublicKeyParameters(publicKey.KeyBytes);
            var dataToVerifyBytes = Encoding.UTF8.GetBytes(message);
            var signatureBytes = Convert.FromHexString(signature);
            var verifier = new Ed25519Signer();
    
            verifier.Init(false, publicKeyParam);
            verifier.BlockUpdate(dataToVerifyBytes, 0, dataToVerifyBytes.Length);

            return verifier.VerifySignature(signatureBytes);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Error verifying signature: {ex.Message}");

            return false;
        }
    }

    public async Task<(bool, string?)> VerifyAndRecoverPublicKeyFromSignatureAsync(string message, string signature)
    {
        throw new DoesntSupportRecoverableException();
    }

    public SupportedMethod GetSupportedMethod()
    {
        return SupportedMethod.Verify;
    }
}