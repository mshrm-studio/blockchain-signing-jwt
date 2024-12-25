using Blockchain.Signing.Auth.Signing;
using Microsoft.Extensions.Logging;
using Moq;

namespace Blockchain.Signing.Auth.Tests
{
    [TestClass]
    public class EthereumSigningServiceTests
    {
        [TestMethod]
        [DataRow("2024-12-24T09:34:06.951Z", "0xdcd8ea2f84ba028ae746ef61e6c785464bf6c67b1b7599d6ad82b115b8267f1166a954c1b17e7184dfaef24c7e078bb1645ab77957db5191019daba5e96014be1b", "0xa0c2b2319ddd27238dde7a2b525cf9054197e88c")]
        [DataRow("2024-12-24T10:13:42.077Z", "0x8d9843ea6ad748d0df16a9aa94a91b47dd547065eaa1975a7385ec513597a513247049b5d120961bd8c767a7bef437adbd0b13f3c5084f73c15f07c22ada374a1c", "0xbd7dacf31192157f4ce52e1644fef6baac0e77c2")]
        public void GetAddressFromSignatureTest(string message, string signature, string expectedAddress)
        {
            var mockedLogger = new Mock<ILogger<EthereumSignatureService>>();
            var ethereumSignatureService = new EthereumSignatureService(mockedLogger.Object);

            var result = ethereumSignatureService.VerifyAndRecoverPublicKeyFromSignature(message, signature, out var address);
            
            Assert.IsTrue(result);
            Assert.AreEqual(expectedAddress, address, ignoreCase: true);
        }

        [TestMethod]
        [DataRow("2024-12-24T09:34:06.951Z", "0xdcd8ea2f84ba028ae746ef61e6c785464bf6c67b1b7599d6ad82b115b8267f1166a954c1b17e7184dfaef24c7e078bb1645ab77957db5191019daba5e96014be1b", "0xa0c2b2319ddd27238dde7a2b525cf9054197e88c")]
        [DataRow("2024-12-24T10:13:42.077Z", "0x8d9843ea6ad748d0df16a9aa94a91b47dd547065eaa1975a7385ec513597a513247049b5d120961bd8c767a7bef437adbd0b13f3c5084f73c15f07c22ada374a1c", "0xbd7dacf31192157f4ce52e1644fef6baac0e77c2")]
        public void VerifySignatureTest(string message, string signature, string expectedAddress)
        {
            var mockedLogger = new Mock<ILogger<EthereumSignatureService>>();
            var ethereumSignatureService = new EthereumSignatureService(mockedLogger.Object);

            var result = ethereumSignatureService.VerifySignature(message, signature, expectedAddress);

            Assert.IsTrue(result);
        }
    }
}