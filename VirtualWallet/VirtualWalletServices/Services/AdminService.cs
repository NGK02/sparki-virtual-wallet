using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.Business.Services
{
	public class AdminService : IAdminService
	{
		private readonly IUserService userService;
		private readonly IUserRepository userRepository;
		public AdminService(IUserService userService, IUserRepository userRepository)
		{
			this.userService = userService;
			this.userRepository = userRepository;
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

		public bool UnBlockUser(int? id, string username)
		{
			if (id is not null)
			{
				var user = userRepository.GetUserById((int)id);
				if (user is null) throw new EntityNotFoundException($"User with Id:{id} was not found!");
				if (user.RoleId == 2) throw new EntityAlreadyUnBlockedException($"User with Id:{id} is already UNBLOCKED!");
				return userRepository.UnBlockUser(user);
			}
			else if (!string.IsNullOrEmpty(username))
			{
				var user = userRepository.GetUserByUsername(username);
				if (user is null) throw new EntityNotFoundException($"User with Username:{username} was not found!");
				if (user.RoleId == 2) throw new EntityAlreadyUnBlockedException($"User with Username:{username} is already UNBLOCKED!");
				return userRepository.UnBlockUser(user);
			}
			else
			{
				throw new ArgumentNullException("Please privide Id or email of the user!");
			}
		}
		public bool BlockUser(int? id, string username)
		{
			if (id is not null)
			{
				var user = userRepository.GetUserById((int)id);
				if (user is null) throw new EntityNotFoundException($"User with Id:{id} was not found!");
				if (user.RoleId == (int)RoleName.Blocked) throw new EntityAlreadyBlockedException($"User with Id:{id} is already BLOCKED!");
				return userRepository.BlockUser(user);
			}
			else if (!string.IsNullOrEmpty(username))
			{
				var user = userRepository.GetUserByUsername(username);
				if (user is null) throw new EntityNotFoundException($"User with Username:{username} was not found!");
				if (user.RoleId == (int)RoleName.Blocked) throw new EntityAlreadyBlockedException($"User with Username:{username} is already BLOCKED!");
				return userRepository.BlockUser(user);
			}
			else
			{
				throw new ArgumentNullException("Please privide Id or email of the user!");
			}
		}
	}
}
