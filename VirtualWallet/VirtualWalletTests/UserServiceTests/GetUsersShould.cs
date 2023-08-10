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
		[TestMethod]
		public void GetUsers_Should_Retrun_All_Users()
		{
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

			var userRepomock = new Mock<IUserRepository>();
			var sut = new UserService(userRepomock.Object);

			userRepomock.Setup(repo => repo.GetUsers()).Returns(users);


			var result = sut.GetUsers();

			Assert.AreEqual(users, result);
		}

		[TestMethod]
		public void GetUsers_Should_Throw_Exception_When_NoUsersFound()
		{
			var users = new List<User>();


			var userRepomock = new Mock<IUserRepository>();
			var sut = new UserService(userRepomock.Object);

			userRepomock.Setup(repo => repo.GetUsers()).Returns(users);

			Assert.ThrowsException<EntityNotFoundException>(() => sut.GetUsers());
		}
	}
}

