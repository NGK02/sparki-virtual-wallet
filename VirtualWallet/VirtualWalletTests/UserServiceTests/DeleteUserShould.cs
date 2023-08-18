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
	public class DeleteUserShould
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
		public void DeleteById_When_InputIsValid()
		{
			// Arrange
			User user = new User
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

			};

			int? id = 1;
			string username = null;

			userRepoMock.Setup(repo => repo.GetUserById((int)id)).Returns(user);
			userRepoMock.Setup(repo => repo.DeleteUser(user)).Returns(true);

			// Act & Assert
			Assert.IsTrue(sut.DeleteUser(username, id));
		}

		[TestMethod]
		public void Throw_When_UserNotFoundById()
		{
			// Arrange
			int? id = 1;
			string username = null;

			userRepoMock.Setup(repo => repo.GetUserById((int)id));

			// Act & Assert
			Assert.ThrowsException<EntityNotFoundException>(() => sut.DeleteUser(username, id));
		}

		[TestMethod]
		public void Delete_When_ValidInput()
		{
			// Arrange
			User user = new User
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

			};

			int? id = null;
			string username = "test";

			userRepoMock.Setup(repo => repo.GetUserByUsername(username)).Returns(user);
			userRepoMock.Setup(repo => repo.DeleteUser(user)).Returns(true);

			// Act & Assert
			Assert.IsTrue(sut.DeleteUser(username, id));
		}

		[TestMethod]
		public void Throw_When_UserNotFound()
		{
			// Arrange
			int? id = null;
			string username = "test";

			userRepoMock.Setup(repo => repo.GetUserByUsername(username));

			// Act & Assert
			Assert.ThrowsException<EntityNotFoundException>(() => sut.DeleteUser(username, id));
		}

		[TestMethod]
		public void Throw_When_NotProvidedInput()
		{
			//Arrange
			int? id = null;
			string username = null;

			//Act & Assert
			Assert.ThrowsException<EntityNotFoundException>(() => sut.DeleteUser(username, id));
		}
	}
}
