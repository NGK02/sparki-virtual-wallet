using ForumSystem.DataAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Business.Services
{
	public class AuthManager
	{
		private readonly IUserService userService;

		public AuthManager(IUserService userService)
		{
			this.userService = userService;
		}

		public User IsAuthenticated(string credentials)
		{
			//if (credentials is null) throw new UnauthenticatedOperationException("Please enter credentials!");
			//string[] usernameAndPassword = credentials.Split(':');
			//string userName = usernameAndPassword[0];
			//string password = usernameAndPassword[1];

			//var user = userService.GetUserByUsername(userName);
			//string loginPasswordToBASE64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
			//if (user.Password == loginPasswordToBASE64)
			//{
			//	return user;
			//}

			//throw new UnauthenticatedOperationException("Invalid username or password!");
			throw new NotImplementedException();
		}
	}
}
