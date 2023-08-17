using Moq;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.TransferServiceTests
{
    [TestClass]
    public class GetUserTransfersShould
    {
        private Mock<IUserService> userServiceMock;
        private Mock<IAuthManager> authManagerMock;
        private Mock<ITransferRepository> transferRepositoryMock;
        private TransferService transferService;

        [TestInitialize]
        public void Initialize()
        {
            userServiceMock = new Mock<IUserService>();
            authManagerMock = new Mock<IAuthManager>();
            transferRepositoryMock = new Mock<ITransferRepository>();

            transferService = new TransferService(
                authManager: authManagerMock.Object,
                currencyService: null,
                transferRepository: transferRepositoryMock.Object,
                userService: userServiceMock.Object,
                walletService: null
            );
        }

        [TestMethod]
        public void GetUserTransfers_Should_ReturnTransfers_WhenTransfersExist()
        {
            var userId = 1;
            var transfers = new List<Transfer>
            {
                new Transfer { Id = 1 },
                new Transfer { Id = 2 }
            };

            transferRepositoryMock.Setup(repo => repo.GetUserTransfers(userId)).Returns(transfers);

            var result = transferService.GetUserTransfers(userId).ToList();

            transferRepositoryMock.Verify();

            CollectionAssert.AreEqual(transfers, result);
        }

        [TestMethod]
        public void GetUserTransfers_Should_ThrowEntityNotFoundException_WhenNoTransfersAvailable()
        {
            var userId = 1;
            var emptyTransfers = new List<Transfer>();

            transferRepositoryMock.Setup(repo => repo.GetUserTransfers(userId)).Returns(emptyTransfers);

            Assert.ThrowsException<EntityNotFoundException>(() => transferService.GetUserTransfers(userId));
        }

        [TestMethod]
        public void GetUserTransfersWithParams_Should_ReturnTransfers_WhenTransfersExist()
        {
            var userId = 1;
            var parameters = new QueryParams();
            var transfers = new List<Transfer>
            {
                new Transfer { Id = 1 },
                new Transfer { Id = 2 }
            };

            transferRepositoryMock.Setup(repo => repo.GetUserTransfers(userId, parameters)).Returns(transfers);

            var result = transferService.GetUserTransfers(userId, parameters).ToList();

            transferRepositoryMock.Verify();

            CollectionAssert.AreEqual(transfers, result);
        }

        [TestMethod]
        public void GetUserTransfersWithParams_Should_ThrowEntityNotFoundException_WhenNoTransfersAvailable()
        {
            var userId = 1;
            var parameters = new QueryParams();
            var emptyTransfers = new List<Transfer>();

            transferRepositoryMock.Setup(repo => repo.GetUserTransfers(userId, parameters)).Returns(emptyTransfers);

            Assert.ThrowsException<EntityNotFoundException>(() => transferService.GetUserTransfers(userId, parameters));
        }
    }
}
