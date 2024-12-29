using Blockchain.Signing.Auth.Signing;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace Blockchain.Signing.Auth.Tests
{
    [TestClass]
    public class TonSigningServiceTests
    {
        [TestMethod]
        [DataRow("2024-12-28T02:12:47.708Z",
            "7fa5a37b39b2ac1ee37fe9b4124a7fe95c9356b7f3df00cb599ba2b0dc4980bb927b334a572bc5e9bff2d4630d5b936d48e7739c932a6ce94965bb583ca39105",
            "EQA8_2LOwBIyikRkFGsHFbthqiCRYUH2h7pQd603DbwunCp3")
        ]
        public async Task VerifySignatureTest(string message, string signature, string expectedAddress)
        {
            var mockedLogger = new Mock<ILogger<TonSignatureService>>();
            var tonSignatureService = new TonSignatureService(mockedLogger.Object);

            var verified = await tonSignatureService.VerifySignatureAsync(message, signature, expectedAddress);

            Assert.IsTrue(verified);
        }
    }
}