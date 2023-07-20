﻿
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
			var user = IsAuthenticated(credentials);
			if (!IsAdmin(user))
			{
				throw new UnauthorizedAccessException("You'rе not admin!");
			}
		}

		public bool IsAdmin(User user)
		{
			if (user.RoleId == 3)
			{
				return true;
			}
			return false;
		}

		public bool IsAdmin(int roleId)
		{
			if (roleId == 3)
			{
				return true;
			}
			return false;
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
			var user = userService.GetUserByUserName(userName);
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
			if (user.RoleId == 1)
			{
				return true;
			}
			return false;
		}

		public bool IsBlocked(int roleId)
		{
			if (roleId == 1)
			{
				return true;
			}
			return false;
		}
	}
}
