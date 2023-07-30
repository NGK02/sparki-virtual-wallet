using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.CustomAttributes;

namespace VirtualWallet.Dto.UserDto
{
	public class CreateUserDto
	{

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		[MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
		[MaxLength(20, ErrorMessage = "The {0} must be no more than {1} characters long.")]
		public string Username { get; set; }

		[Required]
		[EmailAddress(ErrorMessage = "Invalid {0}!")]
		public string Email { get; set; }

		[Required]
		[PasswordRequirements(ErrorMessage = "Invalid password")]
		public string Password { get; set; }

		[Required]
		[RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter a valid phone number.")]
		public string PhoneNumber { get; set; }

		public string ProfilePicPath { get; set; }


	}
}
