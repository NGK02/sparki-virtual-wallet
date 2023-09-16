using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface IEmailSender
    {
         Task SendEmail(string subject, string toEmail, string toUser, string message);

    }
}
