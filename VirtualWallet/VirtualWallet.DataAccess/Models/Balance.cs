using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.DataAccess.Models
{
	public class Balance
	{
		public int WalletId { get; set; }
		public Wallet Wallet { get; set; }

		public int CurrencyId { get; set; }
		public Currency Currency { get; set; }

		public decimal Amount { get; set; }
	}
}
