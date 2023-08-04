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
using VirtualWallet.DataAccess.Enums;

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
		public string Username { get; set; }

		[Required]
		public string Email { get; set; }

		//[Required]
		//[MinLength(8, ErrorMessage = "The {0} must be at least {1} characters long.")]
		//[RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).+$", 
		//	ErrorMessage = "{0} must contain at least one uppercase letter, one symbol, and one digit.")]
		[Required]
		public string Password { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		public string ProfilePicPath { get; set; }

		public int WalletId { get; set; }

		public Wallet Wallet { get; set; }

		public int RoleId { get; set; } = (int)RoleName.User;

		[JsonIgnore]
		public Role Role { get; set; }

		public List<WalletTransaction> Incoming { get; set; } = new List<WalletTransaction>();

		public List<WalletTransaction> Outgoing { get; set; } = new List<WalletTransaction>();

		public List<Card> Cards { get; set; } = new List<Card>();

		public string ConfirmationToken { get; set; }

		public bool IsConfirmed { get; set; }
    }
}
