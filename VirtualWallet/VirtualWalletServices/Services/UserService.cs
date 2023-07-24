
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
	public class UserService : IUserService
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
			if (userRepo.PhoneNumberExists(mappedUser.PhoneNumber))
			{
				throw new PhoneNumberAlreadyExistException("Phonenumber already exists!");
			}

			mappedUser.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(mappedUser.Password));

			userRepo.CreateUser(mappedUser);

			return true;
		}

		public User GetUserByEmail(string email)
		{
			var originalUser = userRepo.GetUserByEmail(email) ?? throw new EntityNotFoundException($"User with Email {email} was not found!");
			return originalUser;
		}

		public User GetUserById(int userId)
		{
			var originalUser = userRepo.GetUserById(userId) ?? throw new EntityNotFoundException($"User with Id {userId} was not found!");
			return originalUser;
		}

		public User GetUserByUsername(string userName)
		{
			var originalUser = userRepo.GetUserByUsername(userName) ?? throw new EntityNotFoundException($"User with Username {userName} was not found!");
			return originalUser; ;
		}

		public int GetUsersCount()
		{
			throw new NotImplementedException();
		}

		public List<User> GetUsers()
		{
			var users = userRepo.GetUsers();
			if (users.Count == 0)
			{
				throw new EntityNotFoundException($"Users not found!");
			}
			return users;
		}

		public User SearchBy(UserQueryParameters queryParams)
		{
			var user = new User();
			if (queryParams.Username is null &
				   queryParams.PhoneNumber is null &
				   queryParams.Email is null)
			{
				throw new InvalidOperationException("Please provide search parameters!");
			}
			else
			{
				user = userRepo.SearchBy(queryParams);
			}

			if (user is null)
			{
				throw new EntityNotFoundException($"User not found!");
			}
			return user;
		}

		public User UpdateUser(string username, User userNewValues)
		{
			_ = UserNewValuesValidator(userNewValues);
			var userToUpdate = userRepo.GetUserByUsername(username);
			if (userToUpdate is null)
			{
				throw new EntityNotFoundException($"User with username:{username} was not found!");
			}
			if (userNewValues.Password is not null)
			{
				userNewValues.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(userNewValues.Password));
			}
			var updatedUser = userRepo.UpdateUser(username, userNewValues);
			return updatedUser;
		}

		public User UpdateUser(int id, User userNewValues)
		{
			_ = UserNewValuesValidator(userNewValues);
			var userToUpdate = userRepo.GetUserById(id);
			if (userToUpdate is null)
			{
				throw new EntityNotFoundException($"User with Id:{id} was not found!");
			}
			if (userNewValues.Password is not null)
			{
				userNewValues.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(userNewValues.Password));
			}
			var updatedUser = userRepo.UpdateUser(id, userNewValues);
			return updatedUser;
		}

		public bool DeleteUser(string userName, int? userId)
		{
			if (userId is not null)
			{
				var userToDelete = userRepo.GetUserById((int)userId);
				if (userToDelete is null) throw new EntityNotFoundException($"User with Id={userId} was not found!");
				return userRepo.DeleteUser(userToDelete);

			}
			else if (userName is not null)
			{
				var userToDelete = userRepo.GetUserByUsername(userName);
				if (userToDelete is null) throw new EntityNotFoundException($"User with username={userName} was not found!");
				return userRepo.DeleteUser(userToDelete);
			}
			throw new EntityNotFoundException("Please provide Id or Username for the user to be deleted!");
		}

		private bool UserNewValuesValidator(User userNewValues)
		{
			if (userNewValues.Email is not null)
			{
				if (userRepo.EmailExists(userNewValues.Email))
				{
					throw new EmailAlreadyExistException("Email already exist!");
				}
			}
			if (userNewValues.PhoneNumber is not null)
			{
				if (userRepo.PhoneNumberExists(userNewValues.PhoneNumber))
				{
					throw new UsernameAlreadyExistException("Phonenumber already exist!");
				}
			}
			return true;
		}

		public bool UserHasSufficientBalance(User user, int amount, int currencyId)
		{
			var balance = user.Wallet.Balances.FirstOrDefault(b => b.CurrencyId == currencyId);
			if (balance is not null)
			{
				if (balance.Amount >= amount)
				{
					return true;
				}
			}
			return false;
		}
	}
}
