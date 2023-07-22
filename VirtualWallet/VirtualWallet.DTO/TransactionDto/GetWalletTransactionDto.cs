using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.TransactionDto
{
	public class GetWalletTransactionDto
	{
		public string SenderUsername { get; set; }

		public string RecipientUsername { get; set; }

		public string CurrencyCode { get; set; }

		public string Amount { get; set; }

		public DateTime CreatedOn { get; set; }
	}
}
