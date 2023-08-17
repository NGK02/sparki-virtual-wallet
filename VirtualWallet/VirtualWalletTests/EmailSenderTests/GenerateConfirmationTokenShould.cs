using VirtualWallet.Business.Services;

namespace VirtualWalletTests.EmailSenderTests
{
    [TestClass]
    public class GenerateConfirmationTokenShould
    {
        [TestMethod]
        public void GenerateConfirmationTokenShould_GenerateTokenWithCorrectLength()
        {
            var expectedLength = 32;
            var token = EmailSender.GenerateConfirmationToken(expectedLength);

            Assert.AreEqual(expectedLength, token.Length);
        }

        [TestMethod]
        public void GenerateConfirmationTokenShould_GenerateTokenWithValidChars()
        {
            var validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var token = EmailSender.GenerateConfirmationToken(validChars.Length);

            foreach (char c in token)
            {
                Assert.IsTrue(validChars.Contains(c));
            }
        }

        [TestMethod]
        public void GenerateConfirmationTokenShould_GenerateUniqueTokens()
        {
            var numTokens = 1000;
            var tokenLength = 32;
            var tokens = new HashSet<string>();

            for (int i = 0; i < numTokens; i++)
            {
                var token = EmailSender.GenerateConfirmationToken(tokenLength);
                tokens.Add(token);
            }

            Assert.AreEqual(numTokens, tokens.Count);
        }
    }
}
