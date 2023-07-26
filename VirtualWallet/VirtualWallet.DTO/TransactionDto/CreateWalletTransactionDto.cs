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
	//TODO В ПДФ пише да може да се ипращат пари username,email and phonenumber
		[Required]
		public string RecipientUsername { get; set; }

		[Required]
		public string CurrencyCode { get; set; }

		//Transaction limit? Equated to USD?
		[Required]
		public string Amount { get; set; }
	}
}
