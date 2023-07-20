using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Business.Exceptions
{
	public class EntityAlreadyBlockedException : ApplicationException
	{
		public EntityAlreadyBlockedException(string message) : base(message) { }
	}
}
