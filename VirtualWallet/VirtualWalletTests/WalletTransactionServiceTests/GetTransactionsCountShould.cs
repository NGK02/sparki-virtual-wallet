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

namespace VirtualWalletTests.WalletTransactionServiceTests
{
    [TestClass]
    public class GetTransactionsCountShould
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
        public void ReturnCount() 
        {
            //Arrange
            List<WalletTransaction> walletTransactions = new List<WalletTransaction> { new WalletTransaction() };
            mockWalletTransactionRepo.Setup(tr => tr.GetTransactionsCount()).Returns(1);

            //Act
            var result = walletTransactionService.GetTransactionsCount();

            //Assert
            Assert.AreEqual(1, result);
        }
    }
}
