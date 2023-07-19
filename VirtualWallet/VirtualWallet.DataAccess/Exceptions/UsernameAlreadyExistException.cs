using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.DataAccess.Exceptions
{
	public class UsernameAlreadyExistException : ApplicationException
	{
		public UsernameAlreadyExistException(string message) : base(message) { }
	}
}
