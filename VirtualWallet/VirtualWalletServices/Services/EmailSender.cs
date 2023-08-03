using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Business.Services
{
    public class EmailSender
    {
        public async Task SendEmail(string subject, string toEmail, string toUser, string message)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("sparki-wallet@outlook.com", "Sparki Wallet");
            var to = new EmailAddress(toEmail, toUser);
            var plainTextContent = message;
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
