
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Business.AuthManager
{
    public class AuthManager:IAuthManager
    {
		private readonly IUserService userService;
		public AuthManager(IUserService userService)
		{
			this.userService = userService;
		}

		public void IsAdmin(string credentials)
		{
			throw new NotImplementedException();
		}

		public bool IsAdmin(User user)
		{
			throw new NotImplementedException();
		}

		public bool IsAdmin(int roleId)
		{
			throw new NotImplementedException();
		}

		public User IsAuthenticated(string credentials)
        {
			if (credentials is null) throw new UnauthenticatedOperationException("Please enter credentials!");
			string[] usernameAndPassword = credentials.Split(':');
			string userName = usernameAndPassword[0];
			string password = usernameAndPassword[1];

			var user = userService.GetUserByUserName(userName);
			string loginPasswordToBASE64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
			if (user.Password == loginPasswordToBASE64)
			{
				return user;
			}

			throw new UnauthenticatedOperationException("Invalid username or password!");
		}

		public User IsAuthenticated(string userName, string password)
		{
			throw new NotImplementedException();
		}

		public void IsBlocked(string credentials)
		{
			throw new NotImplementedException();
		}

		public bool IsBlocked(User user)
		{
			throw new NotImplementedException();
		}

		public bool IsBlocked(int roleId)
		{
			throw new NotImplementedException();
		}
	}
}
