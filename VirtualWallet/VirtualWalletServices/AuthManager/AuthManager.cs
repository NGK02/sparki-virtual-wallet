
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Enums;

namespace VirtualWallet.Business.AuthManager
{
    public class AuthManager : IAuthManager
    {
		private readonly IUserService userService;
		public AuthManager(IUserService userService)
		{
			this.userService = userService;
		}

		public bool AreCredentialsNullOrEmpty(string credentials)
		{
			if (string.IsNullOrEmpty(credentials))
			{
				throw new UnauthenticatedOperationException("Please provide credentials!");
			}
			return true;
		}

		public void IsAdmin(string credentials)
		{
			var user = IsAuthenticated(credentials);
			if (!IsAdmin(user))
			{
				throw new UnauthorizedAccessException("You'rе not admin!");
			}
		}

		public bool IsAdmin(User user)
		{
			if (user.RoleId == (int)RoleName.Admin)
			{
				return true;
			}
			return false;
		}

		public bool IsAdmin(int roleId)
		{
			if (roleId == (int)RoleName.Admin)
			{
				return true;
			}
			return false;
		}

		public User IsAuthenticated(string credentials)
        {
			AreCredentialsNullOrEmpty(credentials);
			string[] usernameAndPassword = credentials.Split(':');
			string userName = usernameAndPassword[0];
			string password = usernameAndPassword[1];

			var user = userService.GetUserByUsername(userName);
			string loginPasswordToBASE64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
			if (user.Password == loginPasswordToBASE64)
			{
				return user;
			}

			throw new UnauthenticatedOperationException("Invalid username or password!");
		}

		public User IsAuthenticated(string userName, string password)
		{
			var user = userService.GetUserByUsername(userName);
			string loginPasswordToBASE64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
			if (user.Password == loginPasswordToBASE64)
			{
				return user;
			}

			throw new UnauthenticatedOperationException("Invalid username or password!");
		}

		public void IsBlocked(string credentials)
		{
			var user = IsAuthenticated(credentials);
			if (!IsBlocked(user))
			{
				throw new UnauthorizedAccessException("You'rе blocked, can't perform this action");
			}
		}

		public bool IsBlocked(User user)
		{
			if (user.RoleId == (int)RoleName.Blocked)
			{
				return true;
			}
			return false;
		}

		public bool IsBlocked(int roleId)
		{
			if (roleId == (int)RoleName.Blocked)
			{
				return true;
			}
			return false;
		}
	}
}
