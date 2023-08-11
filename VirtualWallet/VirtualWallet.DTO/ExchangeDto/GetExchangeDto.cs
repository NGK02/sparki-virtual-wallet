using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Dto.ExchangeDto
{
	public class GetExchangeDto
	{


		public string FromCurrency { get; set; }

		public string ToCurrency { get; set; }

		public decimal Amount { get; set; }

		public decimal ExchangedAmout { get; set; }

		public decimal Rate { get; set; }

	}
}
