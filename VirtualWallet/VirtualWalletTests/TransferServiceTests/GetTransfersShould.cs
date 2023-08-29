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
    public class GetTransfersShould
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
        public void GetTransfers_Should_ReturnTransfers_WhenUserIsAdmin()
        {
            var userId = 1;

            var user = new User
            {
                Id = userId,
                FirstName = "Admin",
                LastName = "Adminov",
                Username = "Admin",
                Email = "Admin@gmail.com",
                Password = "MTIz",
                RoleId = 3,
                WalletId = 6,
                IsConfirmed = true,
                PhoneNumber = "0888888886"
            };

            var transfers = new List<Transfer>
            {
                new Transfer { Amount = 100m,
                    CardId = 1,
                    CurrencyId = 3,
                    Id = 1,
                    WalletId = 2},
                new Transfer { Amount = 50m,
                    CardId = 2,
                    CurrencyId = 5,
                    Id = 2,
                    WalletId = 3}
            };

            userServiceMock.Setup(service => service.GetUserById(userId)).Returns(user);
            authManagerMock.Setup(manager => manager.IsAdmin(user)).Returns(true);
            transferRepositoryMock.Setup(repo => repo.GetTransfers()).Returns(transfers);

            var result = transferService.GetTransfers(userId).ToList();

            userServiceMock.Verify();
            authManagerMock.Verify();
            transferRepositoryMock.Verify();

            CollectionAssert.AreEqual(transfers, result);
        }

        [TestMethod]
        public void GetTransfers_Should_ThrowUnauthorizedException_WhenUserIsNotAdmin()
        {
            var userId = 2;

            var user = new User
            {
                Id = userId,
                FirstName = "Kosta",
                LastName = "Kostev",
                Username = "BrainDamage123",
                Email = "Kostev@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 5,
                IsConfirmed = true,
                PhoneNumber = "0888888885"
            };

            userServiceMock.Setup(service => service.GetUserById(userId)).Returns(user);
            authManagerMock.Setup(manager => manager.IsAdmin(user)).Returns(false);

            Assert.ThrowsException<UnauthorizedOperationException>(() => transferService.GetTransfers(userId));
        }

        [TestMethod]
        public void GetTransfers_Should_ThrowEntityNotFoundException_WhenNoTransfersAvailable()
        {
            var userId = 1;

            var user = new User
            {
                Id = userId,
                FirstName = "Admin",
                LastName = "Adminov",
                Username = "Admin",
                Email = "Admin@gmail.com",
                Password = "MTIz",
                RoleId = 3,
                WalletId = 6,
                IsConfirmed = true,
                PhoneNumber = "0888888886"
            };

            var emptyTransfers = new List<Transfer>();

            userServiceMock.Setup(service => service.GetUserById(userId)).Returns(user);
            authManagerMock.Setup(manager => manager.IsAdmin(user)).Returns(true);
            transferRepositoryMock.Setup(repo => repo.GetTransfers()).Returns(emptyTransfers);

            Assert.ThrowsException<EntityNotFoundException>(() => transferService.GetTransfers(userId));
        }
    }
}
