using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.Business.Services
{
    public class AdminService : IAdminService
	{
        private readonly IUserRepository userRepository;
        private readonly IUserService userService;

		public AdminService(IUserRepository userRepository, IUserService userService)
		{
            this.userRepository = userRepository;
            this.userService = userService;
		}

		public bool BlockUser(int? id, string username, string email, string phoneNumber)
		{
			if (id is not null)
			{
				var user = userRepository.GetUserById((int) id);

				if (user is null) throw new EntityNotFoundException($"No user with ID {id} was found.");

				if (user.RoleId == (int) RoleName.Blocked) throw new EntityAlreadyBlockedException("User is already blocked.");

				return userRepository.BlockUser(user);
			}
			else if (!string.IsNullOrEmpty(username))
			{
				var user = userRepository.GetUserByUsername(username);

				if (user is null) throw new EntityNotFoundException($"No user with the username '{username}' was found.");

				if (user.RoleId == (int) RoleName.Blocked) throw new EntityAlreadyBlockedException("User is already blocked.");

				return userRepository.BlockUser(user);
			}
			else if (!string.IsNullOrEmpty(email))
			{
				var user = userRepository.GetUserByEmail(email);

				if (user is null) throw new EntityNotFoundException($"No user with the email address '{email}' was found.");

				if (user.RoleId == (int) RoleName.Blocked) throw new EntityAlreadyBlockedException("User is already blocked.");

				return userRepository.BlockUser(user);
			}
			else if (!string.IsNullOrEmpty(phoneNumber))
			{
				var user = userRepository.GetUserByPhoneNumber(phoneNumber);

				if (user is null) throw new EntityNotFoundException($"No user with the phone number '{phoneNumber}' was found.");

				if (user.RoleId == (int) RoleName.Blocked) throw new EntityAlreadyBlockedException("User is already blocked.");

				return userRepository.BlockUser(user);
			}
			else
			{
				throw new ArgumentNullException("Please provide the user's ID, Username, Email, or Phone Number.");
			}
		}

        public bool UnblockUser(int? id, string username, string email, string phoneNumber)
        {
            if (id is not null)
            {
                var user = userRepository.GetUserById((int) id);

                if (user is null) throw new EntityNotFoundException($"No user with ID {id} was found.");

                if (user.RoleId == (int) RoleName.User) throw new EntityAlreadyUnblockedException("User is already unblocked.");

                return userRepository.UnblockUser(user);
            }
            else if (!string.IsNullOrEmpty(username))
            {
                var user = userRepository.GetUserByUsername(username);

                if (user is null) throw new EntityNotFoundException($"No user with the username '{username}' was found.");

                if (user.RoleId == (int) RoleName.User) throw new EntityAlreadyUnblockedException("User is already unblocked.");

                return userRepository.UnblockUser(user);
            }
            else if (!string.IsNullOrEmpty(email))
            {
                var user = userRepository.GetUserByEmail(email);

                if (user is null) throw new EntityNotFoundException($"No user with the email address '{email}' was found.");

                if (user.RoleId == (int) RoleName.User) throw new EntityAlreadyUnblockedException("User is already unblocked.");

                return userRepository.UnblockUser(user);
            }
            else if (!string.IsNullOrEmpty(phoneNumber))
            {
                var user = userRepository.GetUserByPhoneNumber(phoneNumber);

                if (user is null) throw new EntityNotFoundException($"No user with the phone number '{phoneNumber}' was found.");

                if (user.RoleId == (int) RoleName.User) throw new EntityAlreadyUnblockedException("User is already unblocked.");

                return userRepository.UnblockUser(user);
            }
            else
            {
                throw new ArgumentNullException("Please provide the user's ID, Username, Email, or Phone Number.");
            }
        }

        public List<User> GetUsers(UserQueryParameters queryParameters)
        {
            var result = new List<User>();

            if (queryParameters.PhoneNumber is null &&
                queryParameters.Username is null &&
                queryParameters.Email is null)
            {
                result = userService.GetUsers();
            }
            else
            {
                result.Add(userService.SearchBy(queryParameters));
            }

            return result;
        }
    }
}