using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;

namespace VirtualWallet.DataAccess.Models
{
	public class Currency
	{
        // [Required]
        public CurrencyCode Code { get; set; }

        public int Id { get; set; }
	}
}