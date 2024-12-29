using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Models;
public record Jwt(string Token, string? RefreshToken, long ExpiresAt);