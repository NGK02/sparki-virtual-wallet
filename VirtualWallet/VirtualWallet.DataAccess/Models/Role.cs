using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;

namespace VirtualWallet.DataAccess.Models
{
	public class Role
	{
		public int Id { get; set; }
		public RoleName Name { get; set; }
		public ICollection<User> Users { get; set; } = new List<User>();
	}
}
