using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.DataAccess.Repositories
{
	public class UserRepo : IUserRepo
	{
		private readonly WalletDbContext database;

		public UserRepo(WalletDbContext DataBase)
		{
			this.database = DataBase;
		}
		//private Wallet CreateWallet(User user)
		//{
		//	var wallet = new Wallet();
		//	wallet.User = user;

		//}
		public bool CreateUser(User user)
		{
			user.RoleId = 2;
			database.Users.Add(user);
			database.SaveChanges();
			//var wallet = DataBase.
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
	}
}
