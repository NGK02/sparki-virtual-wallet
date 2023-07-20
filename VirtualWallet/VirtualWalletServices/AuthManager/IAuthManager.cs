using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Business.AuthManager
{
	public interface IAuthManager
	{
		bool AreCredentialNull(string credentials);
		User IsAuthenticated(string credentials);

		User IsAuthenticated(string userName, string password);

		void IsAdmin(string credentials);

		bool IsAdmin(User user);

		bool IsAdmin(int roleId);

		void IsBlocked(string credentials);

		bool IsBlocked(User user);

		bool IsBlocked(int roleId);
	}
}
