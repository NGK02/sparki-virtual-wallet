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
		[TestMethod]
		public void UpdateUser_ByUsername_Should_Update()
		{
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

			var userRepomock = new Mock<IUserRepository>();
			var sut = new UserService(userRepomock.Object);

			userRepomock.Setup(repo => repo.UpdateUser(new string("test"), userNewValues)).Returns(userNewValues);
			userRepomock.Setup(repo=>repo.GetUserByUsername(new string("test"))).Returns(userToBeUpdated);

			var result=sut.UpdateUser(new string("test"), userNewValues);

			Assert.AreEqual(userNewValues, result);
		}
		[TestMethod]
		public void UpdateUser_ById_Should_Update()
		{
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

			var userRepomock = new Mock<IUserRepository>();
			var sut = new UserService(userRepomock.Object);

			userRepomock.Setup(repo => repo.UpdateUser(someId, userNewValues)).Returns(userNewValues);
			userRepomock.Setup(repo => repo.GetUserById(someId)).Returns(userToBeUpdated);

			var result = sut.UpdateUser(someId, userNewValues);

			Assert.AreEqual(userNewValues, result);
		}
		[TestMethod]
		public void UpdateUser_ByUsername_Should_Throw_When_UserToUpdate_NotFound()
		{
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

			var userRepomock = new Mock<IUserRepository>();
			var sut = new UserService(userRepomock.Object);

			userRepomock.Setup(repo => repo.UpdateUser(new string("test"), userNewValues)).Returns(userNewValues);

			Assert.ThrowsException<EntityNotFoundException>(()=>sut.UpdateUser(new string("test"), userNewValues));
		}

		[TestMethod]
		public void UpdateUser_ById_Should_Throw_When_UserToUpdate_NotFound()
		{
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

			var userRepomock = new Mock<IUserRepository>();
			var sut = new UserService(userRepomock.Object);

			userRepomock.Setup(repo => repo.UpdateUser(someId, userNewValues)).Returns(userNewValues);

			Assert.ThrowsException<EntityNotFoundException>(() => sut.UpdateUser(someId, userNewValues));
		}
	}
}
