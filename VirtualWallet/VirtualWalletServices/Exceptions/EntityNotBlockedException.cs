using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Business.Exceptions
{
	public class EntityNotBlockedException : ApplicationException
	{
		public EntityNotBlockedException(string message) : base(message) { }

	}
}
