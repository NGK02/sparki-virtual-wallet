using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;

namespace VirtualWalletTests.AuthManagerTests
{
    [TestClass]
    public class IsAuthenticatedShould
    {
        private Mock<IUserService> mockUserService;
        private AuthManager authManager;

        [TestInitialize]
        public void TestInitialize()
        {
            mockUserService = new Mock<IUserService>();
            authManager = new AuthManager(mockUserService.Object);
        }

        [TestMethod]
        public void Return_User_When_InputIsValid()
        {
            //Arrange
            var username = "username";
            var password = "password";

            var encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            var validUser = new User { Username = username, Password = encodedPassword };

            string[] credentials = new string[] { username, password };

            mockUserService.Setup(us => us.GetUserByUsername(username)).Returns(validUser);

            //Act
            var result = authManager.IsAuthenticated(credentials);

            //Assert
            Assert.AreEqual(result, validUser);
        }

        [TestMethod]
        public void Throw_User_When_PasswordDoesntMatch()
        {
            //Arrange
            var username = "username";
            var password = "password";

            var encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            var validUser = new User { Username = username, Password = string.Empty };

            string[] credentials = new string[] { username, password };

            mockUserService.Setup(us => us.GetUserByUsername(username)).Returns(validUser);

            //Act & Assert
            Assert.ThrowsException<UnauthenticatedOperationException>(() => authManager.IsAuthenticated(credentials));
        }

        [TestMethod]
        public void Throw_User_When_UserNotFound()
        {
            //Arrange
            var username = "username";
            var password = "password";

            var encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            var validUser = new User { Username = string.Empty, Password = encodedPassword };

            string[] credentials = new string[] { username, password };

            //Act
            mockUserService.Setup(us => us.GetUserByUsername(username)).Throws(new EntityNotFoundException("Test exception"));

            //Assert
            Assert.ThrowsException<UnauthenticatedOperationException>(() => authManager.IsAuthenticated(credentials));
        }
    }
}
