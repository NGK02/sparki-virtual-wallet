using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;

namespace VirtualWallet.DataAccess.Models
{
	public class Currency
	{
		public int Id { get; set; }
		public CurrencyCode Code { get; set; }
	}
}
