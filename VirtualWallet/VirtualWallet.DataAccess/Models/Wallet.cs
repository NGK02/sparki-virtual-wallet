using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.DataAccess.Models
{
	public class Wallet
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		//[ForeignKey("UserId")]
		public User User { get; set; }

		public decimal EUR { get; set; }

		public decimal BGN { get; set; }

		public decimal USD { get; set; }

		public decimal GBP { get; set; }	

		public decimal CHF { get; set; }


	}
}
