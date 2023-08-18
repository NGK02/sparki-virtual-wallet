using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.UserServiceTests
{
	[TestClass]
	public class GetUsersShould
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
		public void Retrun_All_Users()
		{
			// Arrange
			var users = new List<User> {
			new User
			{
				FirstName = "TestFirstName"
				,
				LastName = "TestLastName"
				,
				Username = "TestUsername"
				,
				Password = "1234567890"
				,
				Email = "test@mail.com"
				,
				PhoneNumber = "0896342516"

			},
			new User
			{
				FirstName = "TestFirstName"
				,
				LastName = "TestLastName"
				,
				Username = "TestUsername"
				,
				Password = "1234567890"
				,
				Email = "test@mail.com"
				,
				PhoneNumber = "0896342516"

			}
			};

			userRepoMock.Setup(repo => repo.GetUsers()).Returns(users);

			// Act
			var result = sut.GetUsers();

			// Assert
			Assert.AreEqual(users, result);
		}

		[TestMethod]
		public void Throw_When_NoUsersFound()
		{
			// Arrange
			var users = new List<User>();
			userRepoMock.Setup(repo => repo.GetUsers()).Returns(users);

			// Act & Assert
			Assert.ThrowsException<EntityNotFoundException>(() => sut.GetUsers());
		}
	}
}

