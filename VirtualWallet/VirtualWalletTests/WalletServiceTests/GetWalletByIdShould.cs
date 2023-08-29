using Moq;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.WalletServiceTests
{
    [TestClass]
    public class GetWalletByIdShould
    {
        [TestMethod]
        public void GetWalletByIdShould_ReturnWalletSuccessfully()
        {
            var walletId = 1;
            var wallet = new Wallet { Id = walletId };

            var walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock.Setup(repo => repo.GetWalletById(walletId))
                               .Returns(wallet);

            var walletService = new WalletService(null, null, null, walletRepositoryMock.Object);

            var retrievedWallet = walletService.GetWalletById(walletId);

            Assert.IsNotNull(retrievedWallet);
            Assert.AreEqual(walletId, retrievedWallet.Id);
        }

        [TestMethod]
        public void GetWalletByIdShould_ThrowEntityNotFoundException()
        {
            var walletId = 1;

            var walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock.Setup(repo => repo.GetWalletById(walletId))
                               .Returns((Wallet)null);

            var walletService = new WalletService(null, null, null, walletRepositoryMock.Object);

            // Act and Assert
            Assert.ThrowsException<EntityNotFoundException>(() => walletService.GetWalletById(walletId));
        }
    }
}
