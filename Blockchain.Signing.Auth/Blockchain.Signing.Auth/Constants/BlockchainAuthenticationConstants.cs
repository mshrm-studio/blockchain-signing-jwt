using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Constants;

public static class BlockchainAuthenticationConstants
{
    public static string MessageDateFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

    public static class BlockChains
    {
        public static string Evm = "Evm";
        public static string Solana = "Solana";
    }
}


