using System.Text;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.Business.Services
{
    public class UserService : IUserService
	{
		private readonly IUserRepository userRepo;

		public UserService(IUserRepository userRepo)
		{
			this.userRepo = userRepo;
		}

        private bool ValidateUserNewValues(User userNewValues)
        {
            if (userNewValues.Email is not null)
            {
                if (userRepo.EmailExists(userNewValues.Email))
                {
                    throw new EmailAlreadyExistException("A user with this email address already exists.");
                }
            }

            if (userNewValues.PhoneNumber is not null)
            {
                if (userRepo.PhoneNumberExists(userNewValues.PhoneNumber))
                {
                    throw new PhoneNumberAlreadyExistException("A user with this phone number already exists.");
                }
            }

            return true;
        }

        public bool CreateUser(User mappedUser)
		{
			if (userRepo.EmailExists(mappedUser.Email))
			{
				throw new EmailAlreadyExistException("A user with this email address already exists.");
			}

			if (userRepo.UsernameExists(mappedUser.Username))
			{
				throw new UsernameAlreadyExistException("This username is already taken.");
			}

			if (userRepo.PhoneNumberExists(mappedUser.PhoneNumber))
			{
				throw new PhoneNumberAlreadyExistException("A user with this phone number already exists.");
			}

			mappedUser.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(mappedUser.Password));
			userRepo.CreateUser(mappedUser);

			return true;
		}

        public bool DeleteUser(string userName, int? userId)
        {
            if (userId is not null)
            {
                var userToDelete = userRepo.GetUserById((int)userId);

                if (userToDelete is null) throw new EntityNotFoundException($"No user with ID {userId} was found.");

                return userRepo.DeleteUser(userToDelete);
            }
            else if (userName is not null)
            {
                var userToDelete = userRepo.GetUserByUsername(userName);

                if (userToDelete is null) throw new EntityNotFoundException($"No user with the username '{userName}' was found.");

                return userRepo.DeleteUser(userToDelete);
            }

            throw new EntityNotFoundException("Please provide the user's ID or Username for deletion.");
        }

        public bool EmailExists(string email)
        {
            return userRepo.EmailExists(email);
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

        public int GetUsersCount()
        {
            return userRepo.GetUsersCount();
        }

        public List<User> GetUsers()
        {
            var users = userRepo.GetUsers();

            if (users.Count == 0)
            {
                throw new EntityNotFoundException("No users available.");
            }

            return users;
        }

        public User GetUserByEmail(string email)
		{
			var originalUser = userRepo.GetUserByEmail(email) ?? throw new EntityNotFoundException($"No user with the email address '{email}' was found.");

			return originalUser;
		}

		public User GetUserById(int userId)
		{
			var originalUser = userRepo.GetUserById(userId) ?? throw new EntityNotFoundException($"No user with ID {userId} was found.");

			return originalUser;
		}

		public User GetUserByUsername(string userName)
		{
			var originalUser = userRepo.GetUserByUsername(userName) ?? throw new EntityNotFoundException($"No user with the username '{userName}' was found.");

			return originalUser;
		}

		public User SearchBy(UserQueryParameters queryParams)
		{
			User user;

			if (queryParams.Username is null &
				   queryParams.PhoneNumber is null &
				   queryParams.Email is null)
			{
				throw new InvalidOperationException("Please provide valid search parameters.");
			}
			else
			{
				user = userRepo.SearchBy(queryParams);
			}

			if (user is null)
			{
				throw new EntityNotFoundException("Requested user not found.");
			}

			return user;
		}

        public User UpdateUser(int id, User userNewValues)
        {
            _ = ValidateUserNewValues(userNewValues);
            var userToUpdate = userRepo.GetUserById(id);

            if (userToUpdate is null)
            {
                throw new EntityNotFoundException($"No user with ID {id} was found.");
            }

            if (userNewValues.Password is not null)
            {
                userNewValues.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(userNewValues.Password));
            }

            var updatedUser = userRepo.UpdateUser(id, userNewValues);

            return updatedUser;
        }

        public User UpdateUser(string username, User userNewValues)
		{
			_ = ValidateUserNewValues(userNewValues);
			var userToUpdate = userRepo.GetUserByUsername(username);

			if (userToUpdate is null)
			{
				throw new EntityNotFoundException($"No user with the username '{username}' was found.");
			}

			if (userNewValues.Password is not null)
			{
				userNewValues.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(userNewValues.Password));
			}

			var updatedUser = userRepo.UpdateUser(username, userNewValues);

			return updatedUser;
		}

        public void ConfirmUser(User user, int userId)
        {
            var userToConfirm = GetUserById(userId);

            userRepo.ConfirmUser(userToConfirm, user);
        }

        public void UpdateUserConfirmationToken(User user, string username)
        {
            var userToUpdate = GetUserByUsername(username);

            userRepo.UpdateUserConfirmationToken(userToUpdate, user);
        }
    }
}