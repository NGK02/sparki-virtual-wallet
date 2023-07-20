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

		public List<User> GetUsers()
		{
			return GetUsersQuerable().ToList();
		}

        public User GetUserById(int Id)
        {
            return GetUsersQuerable().FirstOrDefault(u => u.Id == Id && u.IsDeleted == false);
        }

		public User GetUserByUsername(string username)
		{
			return GetUsersQuerable().FirstOrDefault(u => u.Username == username && u.IsDeleted == false);
		}
		public User GetUserByEmail(string email)
		{
			return GetUsersQuerable().FirstOrDefault(u => u.Email == email && u.IsDeleted == false);
		}

		public int GetUsersCount()
		{
			throw new NotImplementedException();
		}

		public User UpdateUser(string userName, User userNewValues)
		{
			var userToUpdate = database.Users.FirstOrDefault(u => u.Username == userName);
			userToUpdate.FirstName = userNewValues.FirstName ?? userToUpdate.FirstName;
			userToUpdate.LastName = userNewValues.LastName ?? userToUpdate.LastName;
			userToUpdate.Email = userNewValues.Email ?? userToUpdate.Email;
			userToUpdate.Password = userNewValues.Password ?? userToUpdate.Password;
			userToUpdate.PhoneNumber = userNewValues.PhoneNumber ?? userToUpdate.PhoneNumber;
			userToUpdate.ProfilePicPath = userNewValues.ProfilePicPath ?? userToUpdate.ProfilePicPath;
			if (userNewValues.ProfilePicPath == "Delete") { userToUpdate.ProfilePicPath = null; }
			database.SaveChanges();
			return userToUpdate;
		}
		public User UpdateUser(int id, User userNewValues)
		{
			var userToUpdate = database.Users.FirstOrDefault(u => u.Id == id);
			userToUpdate.FirstName = userNewValues.FirstName ?? userToUpdate.FirstName;
			userToUpdate.LastName = userNewValues.LastName ?? userToUpdate.LastName;
			userToUpdate.Email = userNewValues.Email ?? userToUpdate.Email;
			userToUpdate.Password = userNewValues.Password ?? userToUpdate.Password;
			userToUpdate.PhoneNumber = userNewValues.PhoneNumber ?? userToUpdate.PhoneNumber;
			userToUpdate.ProfilePicPath = userNewValues.ProfilePicPath ?? userToUpdate.ProfilePicPath;
			if (userNewValues.ProfilePicPath == "Delete") { userToUpdate.ProfilePicPath = null; }
			database.SaveChanges();
			return userToUpdate;
		}
		private void Update(User userToUpdate, User userNewValues)
		{
			userToUpdate.FirstName = userNewValues.FirstName ?? userToUpdate.FirstName;
			userToUpdate.LastName = userNewValues.LastName ?? userToUpdate.LastName;
			userToUpdate.Email = userNewValues.Email ?? userToUpdate.Email;
			userToUpdate.Password = userNewValues.Password ?? userToUpdate.Password;
			userToUpdate.PhoneNumber = userNewValues.PhoneNumber ?? userToUpdate.PhoneNumber;
			userToUpdate.ProfilePicPath = userNewValues.ProfilePicPath ?? userToUpdate.ProfilePicPath;
			database.SaveChanges();
		}


		public User SearchBy(UserQueryParameters queryParams)
		{
			var users = GetUsersQuerable();
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


		public bool DeleteUser(User user)
		{
			//TODO
			//ще трябва да доуточним какво да трием на юзъра
			throw new NotImplementedException();
		}
		private IQueryable<User> GetUsersQuerable()
		{
			var users = database.Users.Where(u => u.IsDeleted == false);
			return users;
		}
	}
}
