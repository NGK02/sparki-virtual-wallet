using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.Business.Exceptions;

namespace VirtualWalletTests.AdminServiceTests
{
	[TestClass]
	public class BlockUserShould
	{
	#region "Block user by ID tests"
		[TestMethod]
		public void BlockUser_Should_Block_When_Provided_Id()
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

			};
			int? id = 1;
			string username = null;
			string email = null;
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserById((int)id)).Returns(user);
			userRepoMock.Setup(repo => repo.BlockUser(user)).Returns(true);

			var result = sut.BlockUser((int)id, username, email, phoneNumber);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void BlockUser_Should_Throw_When_UserNotFound_By_Id()
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

			};
			int? id = 1;
			string username = null;
			string email = null;
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			Assert.ThrowsException<EntityNotFoundException>(()=>sut.BlockUser((int)id, username, email, phoneNumber));
		}

		[TestMethod]
		public void BlockUser_Should_Throw_When_UserAlreadyBlocked_By_Id()
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

			userRepoMock.Setup(repo => repo.GetUserById((int)id)).Returns(user);

			Assert.ThrowsException<EntityAlreadyBlockedException>(() => sut.BlockUser((int)id, username, email, phoneNumber));
		}
		#endregion
		#region"Block user by USERNAME tests"
		[TestMethod]
		public void BlockUser_Should_Block_When_Provided_Username()
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

			};
			int? id = null;
			string username = "test";
			string email = null;
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserByUsername(username)).Returns(user);
			userRepoMock.Setup(repo => repo.BlockUser(user)).Returns(true);

			var result = sut.BlockUser(id, username, email, phoneNumber);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void BlockUser_Should_Throw_When_UserNotFound_By_Username()
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

			};
			int? id = null;
			string username = "test";
			string email = null;
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			Assert.ThrowsException<EntityNotFoundException>(() => sut.BlockUser(id, username, email, phoneNumber));
		}

		[TestMethod]
		public void BlockUser_Should_Throw_When_UserAlreadyBlocked_By_Username()
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
			string username = "test";
			string email =null;
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserByUsername(username)).Returns(user);

			Assert.ThrowsException<EntityAlreadyBlockedException>(() => sut.BlockUser(id, username, email, phoneNumber));
		}

		#endregion

		#region"Block user by EMAIL tests"
		[TestMethod]
		public void BlockUser_Should_Block_When_Provided_Email()
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

			};
			int? id = null;
			string username = null;
			string email = "test";
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserByEmail(email)).Returns(user);
			userRepoMock.Setup(repo => repo.BlockUser(user)).Returns(true);

			var result = sut.BlockUser(id, username, email, phoneNumber);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void BlockUser_Should_Throw_When_UserNotFound_By_Email()
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

			};
			int? id = null;
			string username = null;
			string email = "test";
			string phoneNumber = null;

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			Assert.ThrowsException<EntityNotFoundException>(() => sut.BlockUser(id, username, email, phoneNumber));
		}

		[TestMethod]
		public void BlockUser_Should_Throw_When_UserAlreadyBlocked_By_Email()
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

			Assert.ThrowsException<EntityAlreadyBlockedException>(() => sut.BlockUser(id, username, email, phoneNumber));
		}

		#endregion

		#region"Block user by PHONENUMBER tests"
		[TestMethod]
		public void BlockUser_Should_Block_When_Provided_Phonenumber()
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

			};
			int? id = null;
			string username = null;
			string email = null;
			string phoneNumber = "0192837456";

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userRepoMock.Setup(repo => repo.GetUserByPhoneNumber(phoneNumber)).Returns(user);
			userRepoMock.Setup(repo => repo.BlockUser(user)).Returns(true);

			var result = sut.BlockUser(id, username, email, phoneNumber);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void BlockUser_Should_Throw_When_UserNotFound_By_Phonenumber()
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

			};
			int? id = null;
			string username = null;
			string email = null;
			string phoneNumber = "0192837456";

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			Assert.ThrowsException<EntityNotFoundException>(() => sut.BlockUser(id, username, email, phoneNumber));
		}

		[TestMethod]
		public void BlockUser_Should_Throw_When_UserAlreadyBlocked_By_Phonenumber()
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

			Assert.ThrowsException<EntityAlreadyBlockedException>(() => sut.BlockUser(id, username, email, phoneNumber));
		}

		#endregion

		[TestMethod]
		public void BlockUser_Should_Throw_When_NoInput_Provided()
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

			Assert.ThrowsException<ArgumentNullException>(() => sut.BlockUser(id, username, email, phoneNumber));
		}
	}
}
