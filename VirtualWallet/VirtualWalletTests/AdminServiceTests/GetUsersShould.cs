using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.AdminServiceTests
{
	[TestClass]
	public class GetUsersShould
	{
		[TestMethod]
		public void GetUsers_Should_Return_All_Users()
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

			var userQueryParams = new UserQueryParameters();

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userServiceMock.Setup(service=>service.GetUsers()).Returns(users);

			var result = sut.GetUsers(userQueryParams);

			Assert.AreEqual(users, result);


		}

		[TestMethod]
		public void GetUsers_Should_Return_Single_User()
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
			var users = new List<User>();
			users.Add(user);

			var userQueryParams = new UserQueryParameters
			{
				Email = "testmail"
			};

			var userServiceMock = new Mock<IUserService>();
			var userRepoMock = new Mock<IUserRepository>();
			var sut = new AdminService(userServiceMock.Object, userRepoMock.Object);

			userServiceMock.Setup(service => service.SearchBy(userQueryParams)).Returns(user);

			var result = sut.GetUsers(userQueryParams);

			CollectionAssert.AreEqual(users, result);


		}

	}
}
