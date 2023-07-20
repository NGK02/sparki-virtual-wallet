using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DTO.CustomAttributes;

namespace VirtualWallet.DTO.UserDTO
{
	public class UpdateUserDTO
	{

		public string FirstName { get; set; }

		public string LastName { get; set; }


		[EmailAddress(ErrorMessage = "Invalid {0}!")]
		public string Email { get; set; }


		[PasswordRequirements(ErrorMessage = "Invalid password")]
		public string Password { get; set; }


		[RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter a valid phone number.")]
		public string PhoneNumber { get; set; }

		public string ProfilePicPath { get; set; }
	}
}
