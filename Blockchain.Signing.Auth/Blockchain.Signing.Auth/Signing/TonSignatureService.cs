using Blockchain.Signing.Auth.Exceptions;
using Blockchain.Signing.Auth.Models.Enums;
using Blockchain.Signing.Auth.Signing.Interfaces;
using Microsoft.Extensions.Logging;
using Nethereum.Signer;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math.EC.Rfc8032;
using Org.BouncyCastle.Utilities.Encoders;
using Solnet.Rpc.Models;
using Solnet.Wallet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PublicKey = Solnet.Wallet.PublicKey;

namespace Blockchain.Signing.Auth.Signing;

public class TonSignatureService : ISignatureService
{
    private readonly ILogger<TonSignatureService> _logger;

    public TonSignatureService(ILogger<TonSignatureService> logger)
    {
        _logger = logger;
    }


    public async Task<bool> VerifySignatureAsync(string message, string signature, string address)
    {
        try
        {
            var publicKeyHash = DecodeTonAddress(address);

            var signatureBytes = Convert.FromHexString(signature);

            var messageBytes = Encoding.UTF8.GetBytes(message);
            var messageHex = Convert.ToHexString(messageBytes);
            var messageHexBytes = Encoding.UTF8.GetBytes(messageHex);

            var publicKeyParams = new Ed25519PublicKeyParameters(publicKeyHash, 0);
            var verifier = new Ed25519Signer();

            verifier.Init(false, publicKeyParams);
            verifier.BlockUpdate(messageHexBytes, 0, messageHexBytes.Length);

            return verifier.VerifySignature(signatureBytes);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Verification error: {ex.Message}");
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

    private static byte[] DecodeTonAddress(string tonAddress)
    {
        if (!Regex.IsMatch(tonAddress, @"^[A-Za-z0-9\-_]+$"))
        {
            throw new FormatException("Invalid Base64URL characters in TON address.");
        }

        string base64Address = tonAddress.Replace('-', '+').Replace('_', '/');
        switch (base64Address.Length % 4)
        {
            case 2: base64Address += "=="; break;
            case 3: base64Address += "="; break;
        }

        byte[] decodedBytes = Convert.FromBase64String(base64Address);
        if (decodedBytes.Length != 36)
        {
            throw new Exception($"Invalid TON address length: {decodedBytes.Length} bytes.");
        }

        byte[] address = new byte[32];
        Array.Copy(decodedBytes, 2, address, 0, 32);

        return address;
    }
}