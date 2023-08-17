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
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.WalletTransactionServiceTests
{
    [TestClass]
    public class CreateTransactionWithOneParamShould
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
        public void CreateTransaction_When_ValidInput()
        {
            // Arrange
            var senderBalance = new Balance { WalletId = 1, CurrencyId = 1 };
            var senderWallet = new Wallet { UserId = 1, Balances = new List<Balance> { senderBalance } };
            var sender = new User { Id = 1, Username = "sender", Email = "sender@example.com", PhoneNumber = "0987654321", Wallet = senderWallet };

            var recipientWallet = new Wallet { UserId = 2 };
            var recipient = new User { Id = 2, Username = "recipient", Email = "recipient@example.com", PhoneNumber = "1234567890", Wallet = recipientWallet };

            var walletTransaction = new WalletTransaction
            {
                CurrencyId = 1,
                SenderId = sender.Id,
                Recipient = recipient
            };

            var mockWalletTransactionService = new Mock<WalletTransactionService>();

            mockUserService.Setup(us => us.GetUserById(walletTransaction.SenderId)).Returns(sender);
            mockUserService.Setup(us => us.SearchBy(It.IsAny<UserQueryParameters>())).Returns(recipient);
            mockWalletTransactionRepo.Setup(tr => tr.CreateTransaction(walletTransaction)).Returns(walletTransaction);

            // Act
            var result = walletTransactionService.CreateTransaction(walletTransaction);

            // Assert
            Assert.AreEqual(walletTransaction, result);
            Assert.AreEqual(sender, result.Sender);
            Assert.AreEqual(recipient, result.Recipient);
        }

        [TestMethod]
        public void Throw_When_SenderIsRecipient()
        {
            // Arrange
            var sender = new User { Id = 1, Username = "sender", Email = "sender@example.com", PhoneNumber = "0987654321"};

            var recipient = new User { Id = sender.Id, Username = sender.Username, Email = sender.Username, PhoneNumber = sender.PhoneNumber};

            var walletTransaction = new WalletTransaction
            {
                Sender = sender,
                Recipient = recipient
            };

            var mockWalletTransactionService = new Mock<WalletTransactionService>();

            mockUserService.Setup(us => us.GetUserById(walletTransaction.SenderId)).Returns(sender);
            mockUserService.Setup(us => us.SearchBy(It.IsAny<UserQueryParameters>())).Returns(recipient);
            mockWalletTransactionRepo.Setup(tr => tr.CreateTransaction(walletTransaction)).Returns(walletTransaction);

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => walletTransactionService.CreateTransaction(walletTransaction));
        }

        [TestMethod]
        public void Throw_When_Sender_HasNoBalance()
        {
            // Arrange
            var senderWallet = new Wallet { UserId = 1, Balances = new List<Balance>() };
            var sender = new User { Id = 1, Username = "sender", Email = "sender@example.com", PhoneNumber = "0987654321", Wallet = senderWallet };

            var recipientWallet = new Wallet { UserId = 2 };
            var recipient = new User { Id = 2, Username = "recipient", Email = "recipient@example.com", PhoneNumber = "1234567890", Wallet = recipientWallet };

            var walletTransaction = new WalletTransaction
            {
                CurrencyId = 1,
                SenderId = sender.Id,
                Recipient = recipient
            };

            var mockWalletTransactionService = new Mock<WalletTransactionService>();

            mockUserService.Setup(us => us.GetUserById(walletTransaction.SenderId)).Returns(sender);
            mockUserService.Setup(us => us.SearchBy(It.IsAny<UserQueryParameters>())).Returns(recipient);
            mockWalletTransactionRepo.Setup(tr => tr.CreateTransaction(walletTransaction)).Returns(walletTransaction);

            // Act & Assert
            Assert.ThrowsException<InsufficientFundsException>(() => walletTransactionService.CreateTransaction(walletTransaction));
        }

        [TestMethod]
        public void Throw_When_Sender_HasInsufficientBalance()
        {
            // Arrange
            var senderBalance = new Balance
            {
                WalletId = 1,
                CurrencyId = 1,
                Amount = 0,
                Currency = new Currency 
                { Code = CurrencyCode.USD, Id = 1 } 
            };
            var senderWallet = new Wallet { UserId = 1, Balances = new List<Balance> { senderBalance } };
            var sender = new User { Id = 1, Username = "sender", Email = "sender@example.com", PhoneNumber = "0987654321", Wallet = senderWallet };

            var recipientWallet = new Wallet { UserId = 2 };
            var recipient = new User { Id = 2, Username = "recipient", Email = "recipient@example.com", PhoneNumber = "1234567890", Wallet = recipientWallet };

            var walletTransaction = new WalletTransaction
            {
                CurrencyId = 1,
                SenderId = sender.Id,
                Recipient = recipient,
                Amount = 1
            };

            var mockWalletTransactionService = new Mock<WalletTransactionService>();

            mockUserService.Setup(us => us.GetUserById(walletTransaction.SenderId)).Returns(sender);
            mockUserService.Setup(us => us.SearchBy(It.IsAny<UserQueryParameters>())).Returns(recipient);
            mockWalletTransactionRepo.Setup(tr => tr.CreateTransaction(walletTransaction)).Returns(walletTransaction);

            // Act & Assert
            Assert.ThrowsException<InsufficientFundsException>(() => walletTransactionService.CreateTransaction(walletTransaction));
        }
    }
}
