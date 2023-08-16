using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Business.Exceptions
{
	public class EntityAlreadyUnblockedException : ApplicationException
	{
		public EntityAlreadyUnblockedException(string message) : base(message) { }
	}
}
