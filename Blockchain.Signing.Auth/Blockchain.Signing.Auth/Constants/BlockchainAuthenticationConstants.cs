using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Constants;

public static class BlockchainAuthenticationConstants
{
    public static string BlockchainAuthenticateScheme = "BlockchainSignature";
    public static string BlockchainChallengeScheme = "BlockchainSignature";
    public static string BlockchainAuthenticationTypeHeader = "X-Blockchain-Type";

    public static class BlockChains
    {
        public static string Ethereum = "Ethereum";
        public static string Solana = "Solana";
    }
}


