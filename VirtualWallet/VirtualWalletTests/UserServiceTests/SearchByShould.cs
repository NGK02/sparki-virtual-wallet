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
	public class SearchByShould
	{
		[TestMethod]
		public void SearchBy_Should_Return_Specific_User()
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

			var queryParams = new UserQueryParameters
			{
				Username = "Random"
			};

			var userRepomock = new Mock<IUserRepository>();
			var sut = new UserService(userRepomock.Object);

			userRepomock.Setup(repo=>repo.SearchBy(queryParams)).Returns(user);

			var result=sut.SearchBy(queryParams);

			Assert.AreEqual(user,result);
		}
		[TestMethod]
		public void SearchBy_Should_Throw_When_Invalid_Input()
		{

			var queryParams = new UserQueryParameters();

			var userRepomock = new Mock<IUserRepository>();
			var sut = new UserService(userRepomock.Object);

			Assert.ThrowsException<InvalidOperationException>(() => sut.SearchBy(queryParams));
		}

		[TestMethod]
		public void SearchBy_Should_Throw_When_User_Is_Null()
		{

			var queryParams = new UserQueryParameters
			{
				PhoneNumber = "1234567890"
			};	

			var userRepomock = new Mock<IUserRepository>();
			var sut = new UserService(userRepomock.Object);

			userRepomock.Setup(repo => repo.SearchBy(queryParams)).Returns((User)null);

			Assert.ThrowsException<EntityNotFoundException>(() => sut.SearchBy(queryParams));
		}


	}
}
