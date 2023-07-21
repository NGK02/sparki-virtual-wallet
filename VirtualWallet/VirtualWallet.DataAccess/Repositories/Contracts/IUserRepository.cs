﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.DataAccess.Repositories.Contracts
{
	public interface IUserRepository
	{
		bool CreateUser(User user);

		bool EmailExists(string email);
		bool UsernameExists(string userName);
		bool PhoneNumberExists(string phoneNumber);

		List<User> GetUsers();
		User GetUserById(int userId);
		User GetUserByUsername(string username);
		User GetUserByEmail(string email);
		int GetUsersCount();

		User UpdateUser(string userName, User user);
		User UpdateUser(int id, User user);

		User SearchBy(UserQueryParameters queryParams);

		bool DeleteUser(User user);

		Balance CreateUserBalance(int walletId, int currencyId);
	}
}