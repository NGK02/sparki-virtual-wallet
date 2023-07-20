using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.DataAccess.Repositories.Contracts
{
	public interface IUserRepository
	{
		bool CreateUser(User user);

		bool EmailExists(string email);
		bool UsernameExists(string userName);
		bool PhoneNumberExists(string phoneNumber);

        User GetUserById(int userId);
		User GetUserByUsername(string username);
		User SearchBy(UserQueryParameters queryParams);
	}
}
