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
			bool result = database.Users.Any(u => u.Email.ToLower() == email.ToLower());
			return result;
		}

		public bool PhoneNumberExists(string phoneNumber)
		{
			bool result = database.Users.Any(u => u.PhoneNumber == phoneNumber);
			return result;
		}

		public bool UsernameExists(string username)
		{
			bool result = database.Users.Any(u => u.Username.ToLower() == username.ToLower());
			return result;
		}

        public User GetUserById(int Id)
        {
            return GetUsers().FirstOrDefault(u => u.Id == Id && u.IsDeleted == false);
        }

		public User GetUserByUsername(string username)
		{
			return GetUsers().FirstOrDefault(u => u.Username == username && u.IsDeleted == false);
		}

		private IQueryable<User> GetUsers()
		{
			var users = database.Users.Where(u => u.IsDeleted == false);
			return users;
		}

		public User SearchBy(UserQueryParameters queryParams)
		{
			var users = GetUsers();
			User user = null;

			if (queryParams.Username is not null)
			{
				user = users.FirstOrDefault(u => u.Username.ToLower() == queryParams.Username.ToLower());
			}

			if (queryParams.Email is not null)
			{
				user = users.FirstOrDefault(u => u.Email.ToLower() == queryParams.Email.ToLower());
			}

			if (queryParams.PhoneNumber is not null)
			{
				user = users.FirstOrDefault(u => u.PhoneNumber == queryParams.PhoneNumber);
			}

			return user;
		}

	}
}
