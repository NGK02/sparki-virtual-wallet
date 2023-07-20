
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services
{
	public class UserService:IUserService
	{
		private readonly IUserRepository userRepo;
		public UserService(IUserRepository userRepo) 
		{
			this.userRepo = userRepo;
		}

		public bool CreateUser(User mappedUser)
		{
			if (userRepo.EmailExists(mappedUser.Email))
			{
				throw new EmailAlreadyExistException("Email already exists!");
			}
			if (userRepo.UsernameExists(mappedUser.Username))
			{
				throw new UsernameAlreadyExistException("Username already exists!");
			}
			if (userRepo.UsernameExists(mappedUser.Username))
			{
				throw new PhoneNumberAlreadyExistException("Phonenumber already exists!");
			}

			mappedUser.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(mappedUser.Password));

			userRepo.CreateUser(mappedUser);

			return true;
		}

		public bool DeleteUser(string userName, int? userId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<User> GetAllUsers()
		{
			throw new NotImplementedException();
		}

		public User GetUserByEmail(string email)
		{
			throw new NotImplementedException();
		}

		public User GetUserById(int userId)
        {
            var originalUser = userRepo.GetUserById(userId) ?? throw new EntityNotFoundException($"User with Id={userId} was not found!");
            return originalUser;
        }

		public User GetUserByUsername(string userName)
		{
			var originalUser=userRepo.GetUserByUsername(userName) ?? throw new EntityNotFoundException($"User with Username={userName} was not found!");
			return originalUser; ;
		}

		public List<User> GetUsersByUsernameContains(string input)
		{
			throw new NotImplementedException();
		}

		public int GetUsersCount()
		{
			throw new NotImplementedException();
		}

		public List<User> SearchBy(UserQueryParameters queryParams)
		{
			throw new NotImplementedException();
		}

		public User UpdateUser(string username, User user)
		{
			throw new NotImplementedException();
		}

		public User UpdateUser(int id, User userNewValues)
		{
			throw new NotImplementedException();
		}
	}
}
