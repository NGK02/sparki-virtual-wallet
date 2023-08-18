using AutoMapper;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
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
	public class CreateUserShould
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
		public void CreateUser_When_InputIsValid()
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

			userRepoMock.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(true);
			userRepoMock.Setup(repo => repo.EmailExists(It.IsAny<string>())).Returns(false);
			userRepoMock.Setup(repo => repo.UsernameExists(It.IsAny<string>())).Returns(false);
			userRepoMock.Setup(repo => repo.PhoneNumberExists(It.IsAny<string>())).Returns(false);

			// Act
			var result = sut.CreateUser(user);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Throw_When_EmailExist()
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

			userRepoMock.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(true);
			userRepoMock.Setup(repo => repo.EmailExists(It.IsAny<string>())).Returns(true);
			userRepoMock.Setup(repo => repo.UsernameExists(It.IsAny<string>())).Returns(false);
			userRepoMock.Setup(repo => repo.PhoneNumberExists(It.IsAny<string>())).Returns(false);

			//Act & Assert
			Assert.ThrowsException<EmailAlreadyExistException>(() => sut.CreateUser(user));
		}

		[TestMethod]
		public void Throw_When_UsernameExist()
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

			userRepoMock.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(true);
			userRepoMock.Setup(repo => repo.EmailExists(It.IsAny<string>())).Returns(false);
			userRepoMock.Setup(repo => repo.UsernameExists(It.IsAny<string>())).Returns(true);
			userRepoMock.Setup(repo => repo.PhoneNumberExists(It.IsAny<string>())).Returns(false);

			// Act & Assert
			Assert.ThrowsException<UsernameAlreadyExistException>(() => sut.CreateUser(user));
		}

		[TestMethod]
		public void Throw_When_PhoneNumberExist()
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

			userRepoMock.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(true);
			userRepoMock.Setup(repo => repo.EmailExists(It.IsAny<string>())).Returns(false);
			userRepoMock.Setup(repo => repo.UsernameExists(It.IsAny<string>())).Returns(false);
			userRepoMock.Setup(repo => repo.PhoneNumberExists(It.IsAny<string>())).Returns(true);

			//Act & Assert
			Assert.ThrowsException<PhoneNumberAlreadyExistException>(() => sut.CreateUser(user));
		}

	}
}
