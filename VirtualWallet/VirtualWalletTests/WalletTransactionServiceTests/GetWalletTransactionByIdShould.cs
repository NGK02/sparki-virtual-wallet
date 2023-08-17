using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.DataAccess.Enums;

namespace VirtualWalletTests.WalletTransactionServiceTests
{
    [TestClass]
    public class GetWalletTransactionByIdShould
    {
        private Mock<IAuthManager> mockAuthManager;
        private Mock<IExchangeService> mockExchangeService;
        private Mock<IUserService> mockUserService;
        private Mock<IWalletService> mockWalletService;
        private Mock<IWalletTransactionRepository> mockWalletTransactionRepo;
        private WalletTransactionService walletTransactionService;

        [TestInitialize]
        public void TestInitialize()
        {
            mockAuthManager = new Mock<IAuthManager>();
            mockExchangeService = new Mock<IExchangeService>();
            mockUserService = new Mock<IUserService>();
            mockWalletService = new Mock<IWalletService>();
            mockWalletTransactionRepo = new Mock<IWalletTransactionRepository>();

            walletTransactionService = new WalletTransactionService(mockAuthManager.Object,
                                                                    mockExchangeService.Object,
                                                                    mockUserService.Object,
                                                                    mockWalletService.Object,
                                                                    mockWalletTransactionRepo.Object);
        }

        [TestMethod]
        public void ReturnWalletTransaction_When_InputIsValid()
        {
            // Arrange
            int id = 1;
            string username = "Alice";

            var walletTransaction = new WalletTransaction
            {
                Id = id,
                SenderId = 1,
                RecipientId = 2,
                Amount = 100,
                CurrencyId = 1,
                CreatedOn = DateTime.Now
            };

            var user = new User
            {
                Id = 1,
                Username = username
            };

            mockWalletTransactionRepo.Setup(repo => repo.GetWalletTransactionById(id)).Returns(walletTransaction);
            mockUserService.Setup(us => us.GetUserByUsername(username)).Returns(user);

            // Act
            var result = walletTransactionService.GetWalletTransactionById(id, username);

            // Assert
            Assert.AreEqual(walletTransaction, result);
        }

        [TestMethod]
        public void Throw_When_NoMatchingWalletTransaction()
        {
            // Arrange
            int id = 1;
            string username = "Alice";

            var user = new User
            {
                Id = 1,
                Username = username
            };

            mockWalletTransactionRepo.Setup(repo => repo.GetWalletTransactionById(id)).Returns((WalletTransaction)null);
            mockUserService.Setup(us => us.GetUserByUsername(username)).Returns(user);

            // Act & Assert
            Assert.ThrowsException<EntityNotFoundException>(() => walletTransactionService.GetWalletTransactionById(id, username));
        }

        [TestMethod]
        public void Throw_When_UserNotAuthorized()
        {
            // Arrange
            int userId = 1;
            int walletTransactionId = 1;
            string username = "Alice";

            var user = new User
            {
                Id = userId,
                Username = username,
                RoleId = (int)RoleName.User
            };

            var walletTransaction = new WalletTransaction
            {
                Id = walletTransactionId,
                RecipientId = 2,
                SenderId = 3
            };

            mockWalletTransactionRepo.Setup(repo => repo.GetWalletTransactionById(walletTransactionId)).Returns(walletTransaction);
            mockUserService.Setup(us => us.GetUserByUsername(username)).Returns(user);

            // Act & Assert
            Assert.ThrowsException<UnauthorizedOperationException>(() => walletTransactionService.GetWalletTransactionById(walletTransactionId, username));
        }
    }
}
