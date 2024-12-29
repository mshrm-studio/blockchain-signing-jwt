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
        [DataRow("2024-12-25T06:43:48.977Z", "43355bbbe9f8a0261daeacfd87f6114248631c14da604f86efc5e81076680333a79031c07dbcf0db227f64ba93b35282c1bd2af8fb05324bc9da09bbb0b17101", "5ZfTALiEQT1ThYfzirnwJn9EFyAtNEKwP3Q6n8JbSzrb")]
        [DataRow("2024-12-25T15:19:01.413Z", "deab962b50257018b2aa8d4a2d63e3517601b2a2571f3764ca6eb07022b89b79f135f89b479d93c4338da4e7c3982249d6036c8e1d59e4fe51033120002c0106", "5ZfTALiEQT1ThYfzirnwJn9EFyAtNEKwP3Q6n8JbSzrb")]
        public async Task VerifySignatureTest(string message, string signature, string expectedAddress)
        {
            var mockedLogger = new Mock<ILogger<SolanaSignatureService>>();
            var solanaSignatureService = new SolanaSignatureService(mockedLogger.Object);

            var verified = await solanaSignatureService.VerifySignatureAsync(message, signature, expectedAddress);
        
            Assert.IsTrue(verified);
        }
    }
}