using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services
{
	public class AdminService : IAdminService
	{
		private readonly IUserService userService;
		public AdminService(IUserService userService)
		{
			this.userService = userService;
		}
		public List<User> GetUsers(UserQueryParameters queryParameters)
		{
			var result = new List<User>();
			if (queryParameters.PhoneNumber is null &&
				queryParameters.Username is null &&
				queryParameters.Email is null)
			{
				result = userService.GetUsers();
			}
			else
			{
				result.Add(userService.SearchBy(queryParameters));
			}

			if (result.Count == 0)
			{
				throw new EntityNotFoundException($"User not found!");
			}
			return result;
		}

		public bool UnBlockUser(int? id, string email)
		{
			throw new NotImplementedException();
		}
		public bool BlockUser(int? id, string email)
		{
			throw new NotImplementedException();
		}
	}
}
