using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Signing.Auth.Extensions
{
    public static class DateTimeExtensions
    {
        public static long GetAbsoluteDifferenceInSeconds(this DateTime dateTime1, DateTime dateTime2)
        {
            return (long)Math.Abs((dateTime1 - dateTime2).TotalSeconds);
        }
    }
}
