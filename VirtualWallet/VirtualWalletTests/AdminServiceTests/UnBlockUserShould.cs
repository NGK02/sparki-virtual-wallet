using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.AdminServiceTests
{
	[TestClass]
	public class UnblockUserShould
	{
		#region "Unblock user by ID tests"
		[TestMethod]
		public void UnblockUser_Should_Unblock_When_Provided_Id()
		{
			var user = new User
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
				,
				RoleId=1

			};
			int? id = 1;
			string username = null;
			string email = null;
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserById((int)id)).Returns(user);
			userRepoMock.Setup(repo => repo.UnblockUser(user)).Returns(true);

			var result = sut.UnblockUser((int)id, username, email, phoneNumber);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void UnblockUser_Should_Throw_When_UserNotFound_By_Id()
		{
			var user = new User
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
				,
				RoleId = 1

			};
			int? id = 1;
			string username = null;
			string email = null;
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			Assert.ThrowsException<EntityNotFoundException>(() => sut.UnblockUser((int)id, username, email, phoneNumber));
		}

		[TestMethod]
		public void UnblockUser_Should_Throw_When_UserAlreadyUnblocked_By_Id()
		{
			var user = new User
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
				,
				RoleId = 2

			};
			int? id = 1;
			string username = null;
			string email = null;
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserById((int)id)).Returns(user);

			Assert.ThrowsException<EntityAlreadyUnblockedException>(() => sut.UnblockUser((int)id, username, email, phoneNumber));
		}
		#endregion
		#region"Unblock user by USERNAME tests"
		[TestMethod]
		public void UnblockUser_Should_Unblock_When_Provided_Username()
		{
			var user = new User
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
				,
				RoleId=1

			};
			int? id = null;
			string username = "test";
			string email = null;
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserByUsername(username)).Returns(user);
			userRepoMock.Setup(repo => repo.UnblockUser(user)).Returns(true);

			var result = sut.UnblockUser(id, username, email, phoneNumber);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void UnblockUser_Should_Throw_When_UserNotFound_By_Username()
		{
			var user = new User
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
				,
				RoleId=1

			};
			int? id = null;
			string username = "test";
			string email = null;
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			Assert.ThrowsException<EntityNotFoundException>(() => sut.UnblockUser(id, username, email, phoneNumber));
		}

		[TestMethod]
		public void UnblockUser_Should_Throw_When_UserAlreadyUnblocked_By_Username()
		{
			var user = new User
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
				,
				RoleId = 2

			};
			int? id = null;
			string username = "test";
			string email = null;
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserByUsername(username)).Returns(user);

			Assert.ThrowsException<EntityAlreadyUnblockedException>(() => sut.UnblockUser(id, username, email, phoneNumber));
		}

		#endregion

		#region"Unblock user by EMAIL tests"
		[TestMethod]
		public void UnblockUser_Should_Unblock_When_Provided_Email()
		{
			var user = new User
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
				,
				RoleId = 1

			};
			int? id = null;
			string username = null;
			string email = "test";
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserByEmail(email)).Returns(user);
			userRepoMock.Setup(repo => repo.UnblockUser(user)).Returns(true);

			var result = sut.UnblockUser(id, username, email, phoneNumber);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void UnblockUser_Should_Throw_When_UserNotFound_By_Email()
		{
			var user = new User
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
				,
				RoleId = 1

			};
			int? id = null;
			string username = null;
			string email = "test";
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			Assert.ThrowsException<EntityNotFoundException>(() => sut.UnblockUser(id, username, email, phoneNumber));
		}

		[TestMethod]
		public void UnblockUser_Should_Throw_When_UserAlreadyUnblocked_By_Email()
		{
			var user = new User
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
				,
				RoleId = 2

			};
			int? id = null;
			string username = null;
			string email = "test";
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserByEmail(email)).Returns(user);

			Assert.ThrowsException<EntityAlreadyUnblockedException>(() => sut.UnblockUser(id, username, email, phoneNumber));
		}

		#endregion

		#region"Unblock user by PHONENUMBER tests"
		[TestMethod]
		public void UnblockUser_Should_Unblock_When_Provided_Phonenumber()
		{
			var user = new User
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
				,
				RoleId = 1

			};
			int? id = null;
			string username = null;
			string email = null;
			string phoneNumber = "0192837456";

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserByPhoneNumber(phoneNumber)).Returns(user);
			userRepoMock.Setup(repo => repo.UnblockUser(user)).Returns(true);

			var result = sut.UnblockUser(id, username, email, phoneNumber);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void UnblockUser_Should_Throw_When_UserNotFound_By_Phonenumber()
		{
			var user = new User
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
				,
				RoleId = 1

			};
			int? id = null;
			string username = null;
			string email = null;
			string phoneNumber = "0192837456";

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			Assert.ThrowsException<EntityNotFoundException>(() => sut.UnblockUser(id, username, email, phoneNumber));
		}

		[TestMethod]
		public void UnblockUser_Should_Throw_When_UserAlreadyUnblocked_By_Phonenumber()
		{
			var user = new User
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
				,
				RoleId = 2

			};
			int? id = null;
			string username = null;
			string email = null;
			string phoneNumber = "0192837456";

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserByPhoneNumber(phoneNumber)).Returns(user);

			Assert.ThrowsException<EntityAlreadyUnblockedException>(() => sut.UnblockUser(id, username, email, phoneNumber));
		}

		#endregion

		[TestMethod]
		public void UnblockUser_Should_Throw_When_NoInput_Provided()
		{
			var user = new User
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
				,
				RoleId = 1

			};
			int? id = null;
			string username = null;
			string email = null;
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			//userRepoMock.Setup(repo => repo.GetUserByPhoneNumber(phoneNumber)).Returns(user);

			Assert.ThrowsException<ArgumentNullException>(() => sut.UnblockUser(id, username, email, phoneNumber));
		}
	}
}
