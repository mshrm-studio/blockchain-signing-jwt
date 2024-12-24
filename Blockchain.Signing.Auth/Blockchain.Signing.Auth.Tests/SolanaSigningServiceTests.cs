using Blockchain.Signing.Auth.Signing;
using Microsoft.Extensions.Logging;
using Moq;

namespace Blockchain.Signing.Auth.Tests
{
    [TestClass]
    public class SolanaSigningServiceTests
    {
        [TestMethod]
        [DataRow("2024-12-24T09:56:30.952Z", "5d2426fa01f15b72c5333ff89c01432167aa7b4bb7caca95398619bdb8ca374c59db11c5d90ae48f2896793c17db10d3279e5777c77f91a2656fea3da7c5190b", "5ZfTALiEQT1ThYfzirnwJn9EFyAtNEKwP3Q6n8JbSzrb")]
        [DataRow("2024-12-24T09:56:30.952Z", "777FDC8DE330C9FD83DDB32EC7317F86BFDED70E5CFFA018305556EE0DE84031B4066E789CBE3485799F7B452ADAD0411FE513F184BBCBFA46AC13CAB07ACD09", "FKeMHUcgbXLqHCyFQUJArgiw6gZ6JkBJe2YhqXYhwYi8")]
            public void VerifySignatureTest(string message, string signature, string expectedAddress)
        {
            var mockedLogger = new Mock<ILogger<SolanaSignatureService>>();
            var solanaSignatureService = new SolanaSignatureService(mockedLogger.Object);

            var verified = solanaSignatureService.VerifySignature(message, signature, expectedAddress);
        
            Assert.IsTrue(verified);
        }
    }
}