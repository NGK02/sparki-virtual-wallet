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
		bool EmailExist(string email);
		bool UsernameExist(string userName);
		bool PhoneNumberExist(string phoneNumber);

        User GetUserById(int userId);
    }
}
