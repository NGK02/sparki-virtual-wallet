using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.UserServiceTests
{
	[TestClass]
	public class UpdateUserShould
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
		public void UpdateUserByUsername_When_InputIsValid()
		{
			// Arrange
			User userToBeUpdated = new User
			{
				FirstName = "Test"
				,
				LastName = "Test"
				,
				Username = "Test"
				,
				Password = "1234567890"
				,
				Email = "test@mail.com"
				,
				PhoneNumber = "0896342516"

			};
			User userNewValues = new User
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

			userRepoMock.Setup(repo => repo.UpdateUser(new string("test"), userNewValues)).Returns(userNewValues);
			userRepoMock.Setup(repo=>repo.GetUserByUsername(new string("test"))).Returns(userToBeUpdated);

			// Act
			var result = sut.UpdateUser(new string("test"), userNewValues);

			//Assert
			Assert.AreEqual(userNewValues, result);
		}

		[TestMethod]
		public void UpdateUserById_When_InputIsValid()
		{
			// Arrange
			int someId = 1;
			User userToBeUpdated = new User
			{
				FirstName = "Test"
				,
				LastName = "Test"
				,
				Username = "Test"
				,
				Password = "1234567890"
				,
				Email = "test@mail.com"
				,
				PhoneNumber = "0896342516"

			};
			User userNewValues = new User
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

			userRepoMock.Setup(repo => repo.UpdateUser(someId, userNewValues)).Returns(userNewValues);
			userRepoMock.Setup(repo => repo.GetUserById(someId)).Returns(userToBeUpdated);

			// Act
			var result = sut.UpdateUser(someId, userNewValues);

			// Assert
			Assert.AreEqual(userNewValues, result);
		}

		[TestMethod]
		public void Throw_When_UserToUpdateByUsernameNotFound()
		{
			// Arrange
			User userNewValues = new User
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

			userRepoMock.Setup(repo => repo.UpdateUser(new string("test"), userNewValues)).Returns(userNewValues);

			//Act & Assert
			Assert.ThrowsException<EntityNotFoundException>(()=>sut.UpdateUser(new string("test"), userNewValues));
		}

		[TestMethod]
		public void UpdateUser_ById_Should_Throw_When_UserToUpdate_NotFound()
		{
			//Arrange
			int someId = 1;

			User userNewValues = new User
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

			userRepoMock.Setup(repo => repo.UpdateUser(someId, userNewValues)).Returns(userNewValues);

			//Act & Assert
			Assert.ThrowsException<EntityNotFoundException>(() => sut.UpdateUser(someId, userNewValues));
		}
	}
}
