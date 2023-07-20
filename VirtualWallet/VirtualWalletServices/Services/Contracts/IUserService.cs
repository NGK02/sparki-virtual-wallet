using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services.Contracts
{
	public interface IUserService
	{
		bool CreateUser(User mappedUser);

		int GetUsersCount();
		User GetUserById(int userId);
		public User GetUserByEmail(string email);

		User GetUserByUsername(string userName);

		User SearchBy(UserQueryParameters queryParams);

		List<User> GetUsers();
		User UpdateUser(string username, User userNewValues);
		User UpdateUser(int id, User userNewValues);

		bool DeleteUser(string userName, int? userId);
	}
}
