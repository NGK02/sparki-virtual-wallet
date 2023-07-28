using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.ViewModels.UserViewModels
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter {0}!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter {0}!")]
        public string Password { get; set; }
    }
}
