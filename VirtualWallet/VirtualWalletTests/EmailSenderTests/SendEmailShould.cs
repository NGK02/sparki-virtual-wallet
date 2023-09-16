using Microsoft.Extensions.Options;
using Moq;
using VirtualWallet.Business.Services;
using VirtualWallet.Dto.Config;

namespace VirtualWalletTests.EmailSenderTests
{
    [TestClass]
    public class SendEmailShould
    {
        [TestMethod]
        public void SendEmailShould_SendEmailSuccessfully()
        {
            var apiKeys = new ApiKeys { SendGridApiKey = "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" };

            var apiKeysOptions = Options.Create(apiKeys);
            var sut = new EmailSender(apiKeysOptions);
            var subject = "Test Subject";
            var toEmail = "to@example.com";
            var toUser = "To User";
            var message = "Test Message";

            try
            {
                sut.SendEmail(subject, toEmail, toUser, message).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown: {ex.Message}");
            }
        }
    }
}
