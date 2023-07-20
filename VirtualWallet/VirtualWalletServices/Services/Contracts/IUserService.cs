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

		IEnumerable<User> GetAllUsers();

		int GetUsersCount();

		User GetUserById(int userId);
		public User GetUserByEmail(string email);

		User GetUserByUserName(string userName);

		List<User> GetUsersByUsernameContains(string input);

		List<User> SearchBy(UserQueryParameters queryParams);

		User UpdateUser(string username, User user);

		User UpdateUser(int id, User userNewValues);

		bool DeleteUser(string userName, int? userId);
	}
}
