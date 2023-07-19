using ForumSystem.DataAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.Business.Services
{
	public class UserService:IUserService
	{
		private readonly IUserRepo userRepo;
		public UserService(IUserRepo userRepo) 
		{
			this.userRepo = userRepo;
		}

		public bool CreateUser(User mappedUser)
		{
			if (userRepo.EmailExist(mappedUser.Email))
			{
				throw new EmailAlreadyExistException("Email already exists!");
			}
			if (userRepo.UsernameExist(mappedUser.Username))
			{
				throw new UsernameAlreadyExistException("Username already exists!");
			}
			if (userRepo.UsernameExist(mappedUser.Username))
			{
				throw new PhoneNumberAlreadyExistException("Phonenumber already exists!");
			}

			mappedUser.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(mappedUser.Password));

			userRepo.CreateUser(mappedUser);

			return true;
		}

	}
}
