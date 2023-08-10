using AutoMapper;
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
		[TestMethod]
		public void CreateUser_When_All_Input_Is_Valid()
		{
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
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new UserService(userRepoMock.Object);

			userRepoMock.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(true);
			userRepoMock.Setup(repo => repo.EmailExists(It.IsAny<string>())).Returns(false);
			userRepoMock.Setup(repo => repo.UsernameExists(It.IsAny<string>())).Returns(false);
			userRepoMock.Setup(repo => repo.PhoneNumberExists(It.IsAny<string>())).Returns(false);


			var result = sut.CreateUser(user);

			Assert.IsTrue(result);

		}
		[TestMethod]
		public void Throw_When_Email_Exist()
		{
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
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new UserService(userRepoMock.Object);

			userRepoMock.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(true);
			userRepoMock.Setup(repo => repo.EmailExists(It.IsAny<string>())).Returns(true);
			userRepoMock.Setup(repo => repo.UsernameExists(It.IsAny<string>())).Returns(false);
			userRepoMock.Setup(repo => repo.PhoneNumberExists(It.IsAny<string>())).Returns(false);


			Assert.ThrowsException<EmailAlreadyExistException>(() => sut.CreateUser(user));
		}

		[TestMethod]
		public void Throw_When_Username_Exist()
		{
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
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new UserService(userRepoMock.Object);

			userRepoMock.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(true);
			userRepoMock.Setup(repo => repo.EmailExists(It.IsAny<string>())).Returns(false);
			userRepoMock.Setup(repo => repo.UsernameExists(It.IsAny<string>())).Returns(true);
			userRepoMock.Setup(repo => repo.PhoneNumberExists(It.IsAny<string>())).Returns(false);


			Assert.ThrowsException<UsernameAlreadyExistException>(() => sut.CreateUser(user));
		}

		[TestMethod]
		public void Throw_When_PhoneNumber_Exist()
		{
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
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new UserService(userRepoMock.Object);

			userRepoMock.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(true);
			userRepoMock.Setup(repo => repo.EmailExists(It.IsAny<string>())).Returns(false);
			userRepoMock.Setup(repo => repo.UsernameExists(It.IsAny<string>())).Returns(false);
			userRepoMock.Setup(repo => repo.PhoneNumberExists(It.IsAny<string>())).Returns(true);


			Assert.ThrowsException<PhoneNumberAlreadyExistException>(() => sut.CreateUser(user));
		}

	}
}
