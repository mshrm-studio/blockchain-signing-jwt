using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Constants;

public class BlockchainAuthenticationConstants
{
    public const string BlockchainAuthenticateScheme = "BlockchainSignature";
    public const string BlockchainChallengeScheme = "BlockchainSignature";

    public const string BlockchainAuthenticationTypeHeader = "X-Blockchain-Type";

    public class Blockchain
    {
        public const string Ethereum = "Ethereum";
    }
}
