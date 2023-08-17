using Moq;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.TransferServiceTests
{
    [TestClass]
    public class GetTransferByIdShould
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
        public void GetTransferById_Should_ReturnTransfer_WhenTransferExistsAndAuthorized()
        {
            var transferId = 1;
            var userId = 2;
            var transfer = new Transfer { Id = transferId, WalletId = userId };

            transferRepositoryMock.Setup(repo => repo.GetTransferById(transferId)).Returns(transfer);
            authManagerMock.Setup(manager => manager.IsAdmin(It.IsAny<User>())).Returns(false);

            var result = transferService.GetTransferById(transferId, userId);

            transferRepositoryMock.Verify();
            authManagerMock.Verify();

            Assert.AreEqual(transfer, result);
        }

        [TestMethod]
        public void GetTransferById_Should_ThrowEntityNotFoundException_WhenTransferNotFound()
        {
            var transferId = 1;
            transferRepositoryMock.Setup(repo => repo.GetTransferById(transferId)).Returns((Transfer)null);

            Assert.ThrowsException<EntityNotFoundException>(() => transferService.GetTransferById(transferId, It.IsAny<int>()));
        }

        [TestMethod]
        public void GetTransferById_Should_ThrowUnauthorizedOperationException_WhenNotAuthorized()
        {
            var transferId = 1;
            var userId = 2;
            var transfer = new Transfer { Id = transferId, WalletId = 3 };

            transferRepositoryMock.Setup(repo => repo.GetTransferById(transferId)).Returns(transfer);
            authManagerMock.Setup(manager => manager.IsAdmin(It.IsAny<User>())).Returns(false);

            Assert.ThrowsException<UnauthorizedOperationException>(() => transferService.GetTransferById(transferId, userId));
        }

        [TestMethod]
        public void GetTransferById_Should_NotThrow_WhenAuthorizedAsAdmin()
        {
            var transferId = 1;
            var userId = 2;
            var transfer = new Transfer { Id = transferId, WalletId = 3 };

            transferRepositoryMock.Setup(repo => repo.GetTransferById(transferId)).Returns(transfer);
            authManagerMock.Setup(manager => manager.IsAdmin(It.IsAny<User>())).Returns(true);

            try
            {
                transferService.GetTransferById(transferId, userId);

                // No exception should be thrown
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown: {ex.Message}");
            }
        }
    }
}
