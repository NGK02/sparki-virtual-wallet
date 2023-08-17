using Moq;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.WalletServiceTests
{
    [TestClass]
    public class DistributeFundsForReferralsShould
    {
        [TestMethod]
        public void DistributeFundsForReferralsShould_DistributeFundsSuccessfully()
        {
            var referrerId = 1;
            var referredUserId = 2;
            var amount = 100.0m;
            var currencyId = 1;

            var referrer = new User { Id = referrerId, Wallet = new Wallet() };
            var referredUser = new User { Id = referredUserId };
            var referrerBalance = new Balance { CurrencyId = currencyId };
            var referredUserBalance = new Balance { CurrencyId = currencyId };

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(mock => mock.GetUserById(referrerId)).Returns(referrer);
            userServiceMock.Setup(mock => mock.GetUserById(referredUserId)).Returns(referredUser);

            var walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock.Setup(mock => mock.CreateWalletBalance(currencyId, referrerId)).Returns(referrerBalance);
            walletRepositoryMock.Setup(mock => mock.CreateWalletBalance(currencyId, referredUserId)).Returns(referredUserBalance);

            var walletService = new WalletService(null, null, userServiceMock.Object, walletRepositoryMock.Object);

            walletService.DistributeFundsForReferrals(referrerId, referredUserId, amount, currencyId);

            Assert.AreEqual(amount, referrerBalance.Amount);
            Assert.AreEqual(amount, referredUserBalance.Amount);
            walletRepositoryMock.Verify(mock => mock.DistributeFundsForReferrals(referrerBalance, referredUserBalance), Times.Once);
        }
    }
}
