using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.TransferDto
{
	public class GetTransferDto
	{
		public int WalletId { get; set; }

		public bool HasCardSender { get; set; }

		public int CardId { get; set; }

		public string CurrencyCode { get; set; }

		public decimal Amount { get; set; }
	}
}
