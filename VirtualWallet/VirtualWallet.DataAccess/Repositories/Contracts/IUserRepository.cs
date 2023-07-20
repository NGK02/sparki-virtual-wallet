using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.DataAccess.Repositories.Contracts
{
	public interface IUserRepository
	{
		bool CreateUser(User user);
		bool EmailExists(string email);
		bool UsernameExists(string userName);
		bool PhoneNumberExists(string phoneNumber);

        User GetUserById(int userId);
    }
}
