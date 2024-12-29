using Blockchain.Signing.Auth.Signing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blockchain.Signing.Auth.Extensions;

namespace Blockchain.Signing.Auth.Tests
{
    [TestClass]
    public class DateTimeExtensionTests
    {
        [TestMethod]
        [DataRow("2024-12-25T15:09:22.000Z", "2024-12-25T15:10:22.000Z", 60)]
        [DataRow("2024-12-25T15:10:22.000Z", "2024-12-25T15:09:22.000Z", 60)]
        [DataRow("2024-12-25T16:09:22.000Z", "2024-12-25T15:09:22.000Z", 3600)]
        public void GetAbsoluteDifferenceInSecondsTest(string dateTime1Str, string dateTime2Str, int expected)
        {
            var dateTime1 = DateTime.Parse(dateTime1Str);
            var dateTime2 = DateTime.Parse(dateTime2Str);

            var difference = dateTime1.GetAbsoluteDifferenceInSeconds(dateTime2);

            Assert.AreEqual(expected, difference);
        }
    }
}
