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
	public class GetUserByEmailShould
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
		public void ReturnUser_When_InputIsValid()
		{
			//Arrange
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

			userRepoMock.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).Returns(user);

			//Act
			var result = sut.GetUserByEmail(It.IsAny<string>());

			//Assert
			Assert.AreEqual(user, result);
		}

		[TestMethod]
		public void Throw_When_User_NotFound()
		{
			// Arrange
			userRepoMock.Setup(repo => repo.GetUserByEmail(It.IsAny<string>()));

			//Act & Assert
			Assert.ThrowsException<EntityNotFoundException>(() => sut.GetUserByEmail(It.IsAny<string>()));
		}
	}
}
