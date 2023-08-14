using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.ViewModels.UserViewModels
{
    public class GetUserView
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public int CardsCount { get; set; }

        public int transactionsCount { get; set; }

        public string Role { get; set; }

        public string ProfilePicPath { get; set; }
    }
}
