using Moq;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.CardServiceTests
{
    [TestClass]
    public class DeleteCardShould
    {
        private Mock<IAuthManager> authManagerMock;
        private Mock<ICardRepository> cardRepositoryMock;
        private Mock<IUserService> userServiceMock;

        [TestInitialize]
        public void Initialize()
        {
            authManagerMock = new Mock<IAuthManager>();
            cardRepositoryMock = new Mock<ICardRepository>();
            userServiceMock = new Mock<IUserService>();
        }

        [TestMethod]
        public void DeleteCard_ValidCardIdAndUserId_CardDeletedSuccessfully()
        {
            int userId = 123;
            int cardId = 456;

            var cardToDelete = new Card {
                CardHolder = "Georgi Georgiev",
                CardNumber = 1234567890123456,
                CheckNumber = 123,
                ExpirationDate = new DateTime(2023, 12, 31),
                Id = cardId,
                UserId = userId
            };

            cardRepositoryMock.Setup(repo => repo.GetCardById(cardId)).Returns(cardToDelete);

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            cardService.DeleteCard(cardId, userId);

            cardRepositoryMock.Verify(repo => repo.DeleteCard(cardToDelete), Times.Once);
        }

        [TestMethod]
        public void DeleteCard_CardNotFound_ThrowsEntityNotFoundException()
        {
            int userId = 123;
            int cardId = 456;

            var cardRepositoryMock = new Mock<ICardRepository>();
            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();

            cardRepositoryMock.Setup(repo => repo.GetCardById(cardId)).Returns((Card)null);

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            Assert.ThrowsException<EntityNotFoundException>(() => cardService.DeleteCard(cardId, userId));
        }
    }
}
