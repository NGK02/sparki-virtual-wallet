using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Dto.Config;

namespace VirtualWallet.Business.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<ApiKeys> keys;
        private readonly IConfiguration configuration;

        public EmailSender(IOptions<ApiKeys> keys, IConfiguration configuration)
        {
            this.keys = keys;
            this.configuration = configuration;
        }

        public async Task SendEmail(string subject, string toEmail, string toUser, string message)
        {
            //var client = new SendGridClient(keys.Value.SendGridApiKey);
            var client = new SendGridClient(configuration["ApiKeys:SendGridApiKey"]);
            var from = new EmailAddress("sparki-wallet@outlook.com", "Sparki Wallet");
            var to = new EmailAddress(toEmail, toUser);
            var plainTextContent = message;
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public static string GenerateConfirmationToken()
        {
            int length = 32;
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(validChars[random.Next(validChars.Length)]);
            }

            return result.ToString();
        }
    }
}