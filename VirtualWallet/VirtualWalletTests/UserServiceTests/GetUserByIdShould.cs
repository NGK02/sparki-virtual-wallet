﻿using AutoMapper;
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
	public class GetUserByIdShould
	{
		[TestMethod]
		public void Return_User_When_Valid_Input()
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

			userRepoMock.Setup(repo => repo.GetUserById(It.IsAny<int>())).Returns(user);

			var result = sut.GetUserById(It.IsAny<int>());

			Assert.AreEqual(user, result);

		}
		[TestMethod]
		public void Throw_When_User_NotFound()
		{

			var userRepoMock = new Mock<IUserRepository>();

			var sut = new UserService(userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserById(It.IsAny<int>()));

			Assert.ThrowsException<EntityNotFoundException>(() => sut.GetUserById(It.IsAny<int>()));

		}
	}
}
