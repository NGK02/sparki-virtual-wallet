using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.DataAccess.Models
{
	public class WalletTransaction : Entity
	{
		public int Id { get; set; }

		public User Recipient { get; set; }
		public int RecipientId { get; set; }

		public User Sender { get; set; }
		public int SenderId { get; set; }

		public int CurrencyId { get; set; }
		public Currency Currency { get; set; }

		public decimal Amount { get; set; }
	}
}
