using Blockchain.Signing.Auth.Models;
using Blockchain.Signing.Auth.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Services;

public interface IBlockchainJwtService
{
    Jwt GenerateJwt(TokenGenerationContext context);
}
