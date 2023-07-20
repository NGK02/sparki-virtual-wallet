using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories.Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

		public bool EmailExists(string email)
		{
			bool result = database.Users.Any(u => u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
			return result;
		}

		public bool PhoneNumberExists(string phoneNumber)
		{
			bool result = database.Users.Any(u => u.PhoneNumber.Equals(phoneNumber, StringComparison.InvariantCultureIgnoreCase));
			return result;
		}

		public bool UsernameExists(string username)
		{
			bool result = database.Users.Any(u => u.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
			return result;
		}

        public User GetUserById(int Id)
        {
            return database.Users.FirstOrDefault(u => u.Id == Id && u.IsDeleted == false);
        }

		//public User GetUserByUsername(string username)
		//{
		//	return database.Users.SingleOrDefault(u => u.Username == username && u.IsDeleted == false);
		//}

		private List<User> GetUsers() 
		{
			var users = database.Users.Where(u => u.IsDeleted == false);
			return users.ToList();
		}

		public User SearchBy(UserQueryParameters queryParams)
		{
			var users = GetUsers();
			User user = null;

			if (queryParams.Username is not null)
			{
				user = users.FirstOrDefault(u => u.Username.Equals(queryParams.Username, StringComparison.InvariantCultureIgnoreCase));
			}

			if (queryParams.Email is not null)
			{
				user = users.FirstOrDefault(u => u.Email.Equals(queryParams.Email, StringComparison.InvariantCultureIgnoreCase));
			}

			if (queryParams.PhoneNumber is not null)
			{
				user = users.FirstOrDefault(u => u.PhoneNumber.Equals(queryParams.PhoneNumber, StringComparison.InvariantCultureIgnoreCase));
			}

			return user;
		}
	}
}
