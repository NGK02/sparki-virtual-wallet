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
		public string RecipientUsername { get; set; }

		[Required]
		public string CurrencyCode { get; set; }

		//Transaction limit? Equated to USD?
		[Required]
		public string Amount { get; set; }
	}
}
