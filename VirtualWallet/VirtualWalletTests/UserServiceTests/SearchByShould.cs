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
        private Mock<IUserRepository> userRepoMock;
        private UserService sut;

        [TestInitialize]
        public void TestInitialize()
        {
            this.userRepoMock = new Mock<IUserRepository>();
            this.sut = new UserService(userRepoMock.Object);
        }

        [TestMethod]
		public void ReturnUser_When_InputIsValid()
		{
			//Arrange
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

			userRepoMock.Setup(repo=>repo.SearchBy(queryParams)).Returns(user);

			// Act
			var result = sut.SearchBy(queryParams);

			//Assert
			Assert.AreEqual(user,result);
		}
		[TestMethod]
		public void Throw_When_InputIsInvalid()
		{
			//Arrange
			var queryParams = new UserQueryParameters();

			//Act & Assert
			Assert.ThrowsException<InvalidOperationException>(() => sut.SearchBy(queryParams));
		}

		[TestMethod]
		public void SearchBy_Should_Throw_When_User_Is_Null()
		{
			//Arrange
			var queryParams = new UserQueryParameters
			{
				PhoneNumber = "1234567890"
			};	

			userRepoMock.Setup(repo => repo.SearchBy(queryParams)).Returns((User)null);

			//Act & Assert
			Assert.ThrowsException<EntityNotFoundException>(() => sut.SearchBy(queryParams));
		}
	}
}
