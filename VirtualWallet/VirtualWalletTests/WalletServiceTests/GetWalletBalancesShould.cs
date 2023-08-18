using Moq;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.WalletServiceTests
{
    [TestClass]
    public class GetWalletBalancesShould
    {
        [TestMethod]
        public void GetWalletBalancesShould_ReturnBalancesSuccessfully()
        {
            var walletId = 1;

            var balances = new List<Balance>
            {
                new Balance { CurrencyId = 1, WalletId = walletId },
                new Balance { CurrencyId = 2, WalletId = walletId }
            };

            var walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock.Setup(repo => repo.GetWalletBalances(walletId))
                               .Returns(balances);

            var walletService = new WalletService(null, null, null, walletRepositoryMock.Object);

            var retrievedBalances = walletService.GetWalletBalances(walletId);

            Assert.IsNotNull(retrievedBalances);
            Assert.AreEqual(balances.Count, retrievedBalances.Count);
            CollectionAssert.AreEqual(balances, retrievedBalances);
        }
    }
}
