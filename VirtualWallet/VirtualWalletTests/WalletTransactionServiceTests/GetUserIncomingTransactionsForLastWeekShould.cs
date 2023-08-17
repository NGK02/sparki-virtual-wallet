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
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWalletTests.WalletTransactionServiceTests
{
    [TestClass]
    public class GetUserIncomingTransactionsForLastWeekShould
    {
        private Mock<IAuthManager> mockAuthManager;
        private Mock<IExchangeService> mockExchangeService;
        private Mock<IUserService> mockUserService;
        private Mock<IWalletService> mockWalletService;
        private Mock<IWalletTransactionRepository> mockWalletTransactionRepo;
        private WalletTransactionService walletTransactionService;

        private CurrencyCode toCurrency = CurrencyCode.USD;

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
        public void Return_TransactionsInUsd_When_HasTransactions()
        {
            // Arrange
            var exchangeRate = 2M;
            var transactionAmount = 1M;
            var transactionDate = DateTime.Today;
            var fromCurrency = new Currency { Id = (int)CurrencyCode.BGN, Code = CurrencyCode.BGN };
            var userId = 1;

            var transactions = new List<WalletTransaction>
            {
                new WalletTransaction
                {
                    Currency = fromCurrency,
                    Amount = transactionAmount,
                    CreatedOn = transactionDate,
                    RecipientId = userId,
                },
            };
            var expectedData = new Dictionary<string, decimal> 
            {
                { transactionDate.ToString("dd'/'MM'/'yy"), transactionAmount*exchangeRate }
            };
            var exchangeRates = new Dictionary<string, decimal>
            {
                { fromCurrency.Code.ToString(), exchangeRate}
            };

            // Act
            mockWalletTransactionRepo.Setup(tr => tr.GetUserWalletTransactions(It.IsAny<WalletTransactionQueryParameters>(), It.IsAny<int>())).Returns(transactions);
            mockExchangeService.Setup(es => es.GetExchangeRatesFromCache(It.IsAny<CurrencyCode>())).Returns(exchangeRates);
            mockExchangeService.Setup(es => es.CalculateExchangeResult(
                It.IsAny<Dictionary<string, decimal>>(), 
                It.IsAny<CurrencyCode>(), 
                It.IsAny<decimal>())).Returns(transactionAmount*exchangeRate);

            var actualTransactions = walletTransactionService.GetUserIncomingTransactionsForLastWeek(userId, toCurrency);

            // Assert
            Assert.AreEqual(expectedData.First(), actualTransactions.First());
        }
    }
}
