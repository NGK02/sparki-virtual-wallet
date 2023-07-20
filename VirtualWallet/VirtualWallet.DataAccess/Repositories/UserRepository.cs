using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.DataAccess.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly WalletDbContext database;

		public UserRepository(WalletDbContext database)
		{
			this.database = database;
		}

		public bool CreateUser(User user)
		{

			database.Users.Add(user);
			database.SaveChanges();

			var wallet = new Wallet();
			wallet.User = user;

			database.Add(wallet);
			database.SaveChanges();

			user.WalletId = wallet.Id;
			database.SaveChanges();

			return true;
		}

		public bool EmailExist(string email)
		{
			bool result = database.Users.Any(u => u.Email.ToLower() == email.ToLower());
			return result;
		}

		public bool PhoneNumberExist(string phoneNumber)
		{
			bool result = database.Users.Any(u => u.PhoneNumber.ToLower() == phoneNumber.ToLower());
			return result;
		}

		public bool UsernameExist(string userName)
		{
			bool result = database.Users.Any(u => u.Username.ToLower() == userName.ToLower());
			return result;
		}

        public User GetUserById(int Id)
        {
            return database.Users.SingleOrDefault(u => u.Id == Id && u.IsDeleted == false);
        }

		//public User GetUserByUsername(string username)
		//{
		//	return database.Users.SingleOrDefault(u => u.Username == username && u.IsDeleted == false);
		//}
	}
}
