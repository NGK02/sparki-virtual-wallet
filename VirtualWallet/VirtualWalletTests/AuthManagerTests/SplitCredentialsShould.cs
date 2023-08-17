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
    public class SplitCredentialsShould
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
        public void Return_SplitCredentials_When_InputIsValid()
        {
            //Arrange
            var delimiter = ':';
            var usernameCredential = "username";
            var passwordCredential = "password";
            string validCredentials = usernameCredential + delimiter + passwordCredential;

            string[] validSplitCredentials = { usernameCredential, passwordCredential };

            //Act
            var result = authManager.SplitCredentials(validCredentials);

            //Assert
            CollectionAssert.AreEqual(validSplitCredentials, result);
        }

        [TestMethod]
        public void Throw_When_InputIsNullOrEmpty()
        {
            //Arrange
            string credentials = null;

            //Act & Assert
            Assert.ThrowsException<UnauthenticatedOperationException>(() => authManager.SplitCredentials(credentials));
        }

        [TestMethod]
        public void Throw_When_InputDoesntContainDelimiter()
        {
            //Arrange
            var usernameCredential = "username";
            var passwordCredential = "password";
            string credentials = usernameCredential + passwordCredential;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => authManager.SplitCredentials(credentials));
        }
    }
}
