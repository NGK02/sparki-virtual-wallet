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
    public class DeleteTransferShould
    {
        private Mock<IAuthManager> authManagerMock;
        private Mock<ITransferRepository> transferRepositoryMock;
        private Mock<IUserService> userServiceMock;
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
        public void DeleteTransfer_Should_DeleteTransfer_WhenAuthorizedAsAdmin()
        {
            var transferId = 1;
            var userId = 2;
            var transferToDelete = new Transfer { Id = transferId, WalletId = userId };

            var transferRepositoryMock = new Mock<ITransferRepository>();
            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();

            userServiceMock.Setup(service => service.GetUserById(userId)).Returns(new User { Id = userId });
            transferRepositoryMock.Setup(repo => repo.GetTransferById(transferId)).Returns(transferToDelete);
            authManagerMock.Setup(manager => manager.IsAdmin(It.IsAny<User>())).Returns(true);

            var transferService = new TransferService(authManagerMock.Object, null, transferRepositoryMock.Object, userServiceMock.Object, null);

            transferService.DeleteTransfer(transferId, userId);

            transferRepositoryMock.Verify(repo => repo.DeleteTransfer(transferToDelete), Times.Once);
        }

        [TestMethod]
        public void DeleteTransfer_Should_ThrowUnauthorizedOperationException_WhenNotAuthorizedAsAdmin()
        {
            var transferId = 1;
            var userId = 2;

            var transferRepositoryMock = new Mock<ITransferRepository>();
            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();

            userServiceMock.Setup(service => service.GetUserById(userId)).Returns(new User { Id = userId });
            authManagerMock.Setup(manager => manager.IsAdmin(It.IsAny<User>())).Returns(false);

            var transferService = new TransferService(authManagerMock.Object, null, transferRepositoryMock.Object, userServiceMock.Object, null);

            Assert.ThrowsException<UnauthorizedOperationException>(() => transferService.DeleteTransfer(transferId, userId));
        }
    }
}