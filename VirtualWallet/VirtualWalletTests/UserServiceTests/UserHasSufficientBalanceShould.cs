using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.UserServiceTests
{
	[TestClass]
	public class UserHasSufficientBalanceShould
	{
		[TestMethod]
		public void UserHasSufficientBalance_Should_Retrun_True()
		{
			User user = new User
			{
				FirstName = "TestFirstName",
				LastName = "TestLastName",
				Username = "TestUsername",
				Password = "1234567890",
				Email = "test@mail.com",
				PhoneNumber = "0896342516",
				Wallet = new Wallet
				{
					Balances = new List<Balance>
					{
						new Balance
						{
							CurrencyId = 1,
							Amount = 1000
						}
					}
				}
			};


			var userRepomock = new Mock<IUserRepository>();
			var sut = new UserService(userRepomock.Object);

			int lowerAmount = 500;

			Assert.IsTrue(sut.UserHasSufficientBalance(user, lowerAmount, 1));

		}
		[TestMethod]
		public void UserHasSufficientBalance_Should_Retrun_False()
		{
			User user = new User
			{
				FirstName = "TestFirstName",
				LastName = "TestLastName",
				Username = "TestUsername",
				Password = "1234567890",
				Email = "test@mail.com",
				PhoneNumber = "0896342516",
				Wallet = new Wallet
				{
					Balances = new List<Balance>
					{
						new Balance
						{
							CurrencyId = 1,
							Amount = 1000
						}
					}
				}
			};


			var userRepomock = new Mock<IUserRepository>();
			var sut = new UserService(userRepomock.Object);

			int lowerAmount = 1500;

			Assert.IsFalse(sut.UserHasSufficientBalance(user, lowerAmount, 1));

		}
	}
}
