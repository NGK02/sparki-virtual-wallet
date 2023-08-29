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
        private Mock<IUserRepository> userRepoMock;
        private UserService sut;

        [TestInitialize]
        public void TestInitialize()
        {
            this.userRepoMock = new Mock<IUserRepository>();
            this.sut = new UserService(userRepoMock.Object);
        }

        [TestMethod]
		public void Retrun_True_WhenBalanceIsSufficient()
		{
			// Arrange
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

			int lowerAmount = 500;

			// Act & Assert
			Assert.IsTrue(sut.UserHasSufficientBalance(user, lowerAmount, 1));
		}

		[TestMethod]
		public void Retrun_False_WhenUserBalanceIsInsufficient()
		{
			// Arrange
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

			int lowerAmount = 1500;

			//Act & Assert
			Assert.IsFalse(sut.UserHasSufficientBalance(user, lowerAmount, 1));
		}
	}
}
