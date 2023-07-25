using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.DataAccess.Models
{
	public class Exchange:Entity
	{
		public int Id { get; set; }

		[Required]
		public int WalletId { get; set; }
		public Wallet Wallet { get; set; }

		[Required]
		public int FromCurrencyId { get; set; }
		public Currency From { get; set; }

		[Required]
		public int ToCurrencyId { get; set; }
		public Currency To { get; set; }

		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage = "The {0} must be greater than 0.")]
		public decimal Amount { get; set; }	

		public decimal ExchangedAmout { get; set; }

		public decimal Rate { get; set; }


	}
}
