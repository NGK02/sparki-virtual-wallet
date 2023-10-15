using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using VirtualWallet.Business.Services;
using VirtualWallet.Dto.Config;

namespace VirtualWalletTests.EmailSenderTests
{
    [TestClass]
    public class SendEmailShould
    {
        //[TestMethod]
        //public void SendEmailShould_SendEmailSuccessfully()
        //{
        //    var config = new Mock<IConfiguration>();

        //    var sut = new EmailSender(config.Object);
        //    var subject = "Test Subject";
        //    var toEmail = "to@example.com";
        //    var toUser = "To User";
        //    var message = "Test Message";

        //    try
        //    {
        //        sut.SendEmail(subject, toEmail, toUser, message).GetAwaiter().GetResult();
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail($"Unexpected exception thrown: {ex.Message}");
        //    }
        //}
    }
}
