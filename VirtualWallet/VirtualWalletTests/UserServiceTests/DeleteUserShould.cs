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
		[TestMethod]
		public void Deleteuser_Should_Delete_By_Id()
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

			int? id = 1;
			string username = null;
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new UserService(userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserById((int)id)).Returns(user);
			userRepoMock.Setup(repo => repo.DeleteUser(user)).Returns(true);

			Assert.IsTrue(sut.DeleteUser(username, id));

		}

		[TestMethod]
		public void Deleteuser_Should_Throw_When_UserNotFound_By_Id()
		{


			int? id = 1;
			string username = null;
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new UserService(userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserById((int)id));

			Assert.ThrowsException<EntityNotFoundException>(() => sut.DeleteUser(username, id));

		}

		[TestMethod]
		public void Deleteuser_Should_Delete_By_Username()
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

			int? id = null;
			string username = "test";
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new UserService(userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserByUsername(username)).Returns(user);
			userRepoMock.Setup(repo => repo.DeleteUser(user)).Returns(true);

			Assert.IsTrue(sut.DeleteUser(username, id));

		}

		[TestMethod]
		public void Deleteuser_Should_Throw_When_UserNotFound_By_Username()
		{
			int? id = null;
			string username = "test";
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new UserService(userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserByUsername(username));

			Assert.ThrowsException<EntityNotFoundException>(() => sut.DeleteUser(username, id));

		}

		[TestMethod]
		public void Deleteuser_Should_Throw_When_Not_Provided_Input()
		{
			int? id = null;
			string username = null;
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new UserService(userRepoMock.Object);


			Assert.ThrowsException<EntityNotFoundException>(() => sut.DeleteUser(username, id));

		}
	}
}
