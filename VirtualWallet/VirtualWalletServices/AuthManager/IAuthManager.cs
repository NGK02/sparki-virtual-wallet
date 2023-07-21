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
		string[] SplitCredentials(string credentials);

		User IsAuthenticated(string[] splitCredentials);

		//User IsAuthenticated(string[] userName, string password);

		void IsAdmin(string[] splitCredentials);

		bool IsAdmin(User user);

		bool IsAdmin(int roleId);

		void IsBlocked(string[] splitCredentials);

		bool IsBlocked(User user);

		bool IsBlocked(int roleId);
	}
}
