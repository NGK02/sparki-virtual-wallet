using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.DataAccess.Exceptions
{
	public class PhoneNumberAlreadyExistException : ApplicationException
	{
		public PhoneNumberAlreadyExistException(string message) : base(message) { }
	}
}
