using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;

namespace VirtualWallet.DataAccess.Models
{
	public class Currency
	{
        
        public CurrencyCode Code { get; set; }

        public int Id { get; set; }

		[JsonIgnore]
		public List<Exchange> Exchanges { get; set; } = new List<Exchange>();
	}
}