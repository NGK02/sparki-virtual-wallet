using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Dto.TransactionDto
{
	public class CreateWalletTransactionDto
	{
		[Required]
		public int RecipientId { get; set; }

		[Required]
		public int CurrencyId { get; set; }

		[Required]
		public decimal Amount { get; set; }
	}
}
