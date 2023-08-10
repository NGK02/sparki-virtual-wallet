using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.DataAccess.Models
{
	public class Referral : Entity
	{
		public int Id { get; set; }

		public int ReferrerId { get; set; }

		public User Referrer { get; set; }

		public bool IsConfirmed { get; set; }

		public DateTime ConfirmationTokenExpiry { get; set; }

		public string ConfirmationToken { get; set; }

		public string ReferredEmail { get; set; }
	}
}