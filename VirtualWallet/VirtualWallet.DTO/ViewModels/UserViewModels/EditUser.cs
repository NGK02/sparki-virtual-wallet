using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Dto.CustomAttributes;

namespace VirtualWallet.Dto.ViewModels.UserViewModels
{
    public class EditUser
    {
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }


        [EmailAddress(ErrorMessage = "Invalid {0}!")]
        public string Email { get; set; }


        [PasswordRequirements(ErrorMessage = "Invalid password")]
        public string Password { get; set; }

        [DisplayName("Phone number")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }

        public IFormFile ProfilePic { get; set; }
    }
}
