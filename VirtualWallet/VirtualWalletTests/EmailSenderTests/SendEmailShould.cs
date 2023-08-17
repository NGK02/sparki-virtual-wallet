using VirtualWallet.Business.Services;

namespace VirtualWalletTests.EmailSenderTests
{
    [TestClass]
    public class SendEmailShould
    {
        [TestMethod]
        public void SendEmailShould_SendEmailSuccessfully()
        {
            var emailSender = new EmailSender();
            var subject = "Test Subject";
            var toEmail = "to@example.com";
            var toUser = "To User";
            var message = "Test Message";

            try
            {
                emailSender.SendEmail(subject, toEmail, toUser, message).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown: {ex.Message}");
            }
        }
    }
}
