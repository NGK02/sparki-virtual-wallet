using Moq;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.WalletServiceTests
{
    [TestClass]
    public class CreateWalletBalanceShould
    {
        [TestMethod]
        public void CreateWalletBalanceShould_CreateBalanceSuccessfully()
        {
            var currencyId = 1;
            var walletId = 2;

            var walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock.Setup(repo => repo.CreateWalletBalance(currencyId, walletId))
                               .Returns(new Balance { CurrencyId = currencyId, WalletId = walletId })
                               .Verifiable();

            var walletService = new WalletService(null, null, null, walletRepositoryMock.Object);

            var createdBalance = walletService.CreateWalletBalance(currencyId, walletId);

            Assert.IsNotNull(createdBalance);
            Assert.AreEqual(currencyId, createdBalance.CurrencyId);
            Assert.AreEqual(walletId, createdBalance.WalletId);

            walletRepositoryMock.Verify(repo => repo.CreateWalletBalance(currencyId, walletId), Times.Once);
        }
    }
}
