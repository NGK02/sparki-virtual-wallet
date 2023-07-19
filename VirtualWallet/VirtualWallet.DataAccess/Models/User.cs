using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VirtualWallet.DataAccess.Models
{
	public class User : Entity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		[MinLength(2, ErrorMessage = "The {0} must be at least {1} characters long.")]
		[MaxLength(20, ErrorMessage = "The {0} must be no more than {1} characters long.")]
		public string Username { get; set; }

		[EmailAddress]
		public string Email { get; set; }

		public string Password { get; set; }

		[RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter a valid phone number.")]
		public string PhoneNumber { get; set; }

		public int WalletId { get; set; }

		public Wallet Wallet { get; set; }

		public int RoleId { get; set; } = 2;
		public Role Role { get; set; }

		public List<Transaction> Incoming { get; set; }

		public List<Transaction> Outgoing { get; set; }

		public string ProfilePicPath { get; set; }

        public ICollection<Card> Cards { get; set; }
    }
}
