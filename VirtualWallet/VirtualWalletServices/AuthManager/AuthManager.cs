
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

		public string[] SplitCredentials(string credentials)
		{
			if (string.IsNullOrEmpty(credentials))
			{
				throw new UnauthenticatedOperationException("Please provide credentials!");
			}
			if (!credentials.Contains(':'))
			{
				throw new ArgumentException("Please split credentials with ':' !");
			}
			string[] splitCredentials = credentials.Split(':');
			return splitCredentials;
		}

		/// <summary>
		/// Check if logged user ID matches the content creator ID or logged user is Admin.
		/// </summary>
		public bool IsContentCreatorOrAdmin(User user, int contentCreatorId)
		{
			if (user.Id != contentCreatorId && user.RoleId != (int)RoleName.Admin)
			{
				throw new UnauthorizedOperationException("Not Authorized to do this!");
			}
			return true;
		}

		public bool IsAdminOrBlocked(User user)
		{
			if (IsBlocked(user) || IsAdmin(user))
			{
				throw new UnauthorizedAccessException("You can't make transactions!");
			}
			return false;
		}

		public User IsAdmin(string[] splitCredentials)
		{
			var user = IsAuthenticated(splitCredentials);
			if (!IsAdmin(user))
			{
				throw new UnauthorizedOperationException("You'rе not admin!");
			}
			return user;
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

		public User IsAuthenticated(string[] splitCredentials)
		{
			string userName = splitCredentials[0];
			string password = splitCredentials[1];
			try
			{
				var user = userService.GetUserByUsername(userName);

				string loginPasswordToBASE64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
				if (user.Password == loginPasswordToBASE64)
				{
					return user;
				}

				throw new UnauthenticatedOperationException("Invalid username or password!");
			}
			catch (EntityNotFoundException)
			{
				throw new UnauthenticatedOperationException("Invalid username or password!");
			}
		}

		//public User IsAuthenticated(string userName, string password)
		//{
		//	var user = userService.GetUserByUsername(userName);
		//	string loginPasswordToBASE64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
		//	if (user.Password == loginPasswordToBASE64)
		//	{
		//		return user;
		//	}

		//	throw new UnauthenticatedOperationException("Invalid username or password!");
		//}

		public void IsBlocked(string[] splitCredentials)
		{
			var user = IsAuthenticated(splitCredentials);
			if (IsBlocked(user))
			{
				throw new UnauthorizedAccessException("You'rе blocked, can't perform this action");
			}
		}

		public bool IsBlocked(User user)
		{
			//if (user.RoleId == (int)RoleName.Blocked)
			//{
			//	return true;
			//}
			//return false;

			return user.RoleId == (int)RoleName.Blocked;
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
