using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.DataAccess.Models
{
	public abstract class Entity
	{
        public DateTime CreatedOn { get; set; } = DateTime.Now;
		public DateTime? DeletedOn { get; set; }
		public bool IsDeleted { get; set; } = false;
	}
}
