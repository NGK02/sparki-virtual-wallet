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
    public class GetCardByIdShould
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
        public void GetCardById_ValidCardAndUser_ReturnsCard()
        {
            // Arrange
            int cardId = 1;
            int userId = 123;

            var expectedCard = new Card
            {
                CardHolder = "Georgi Georgiev",
                CardNumber = 1234567890123456,
                CheckNumber = 123,
                ExpirationDate = new DateTime(2023, 12, 31),
                Id = cardId,
                UserId = userId
            };

            cardRepositoryMock.Setup(repo => repo.GetCardById(cardId)).Returns(expectedCard);
            userServiceMock.Setup(service => service.GetUserById(userId)).Returns(new User());
            authManagerMock.Setup(manager => manager.IsAdmin(It.IsAny<User>())).Returns(false);

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            // Act
            var resultCard = cardService.GetCardById(cardId, userId);

            // Assert
            Assert.AreEqual(expectedCard, resultCard);
        }

        [TestMethod]
        public void GetCardById_CardNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            int cardId = 1;
            int userId = 123;

            cardRepositoryMock.Setup(repo => repo.GetCardById(cardId)).Returns((Card)null);

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            // Act and Assert
            Assert.ThrowsException<EntityNotFoundException>(() => cardService.GetCardById(cardId, userId));
        }

        [TestMethod]
        public void GetCardById_UnauthorizedAccess_ThrowsUnauthorizedOperationException()
        {
            // Arrange
            int cardId = 1;
            int userId = 123;

            var card = new Card
            {
                CardHolder = "Georgi Georgiev",
                CardNumber = 1234567890123456,
                CheckNumber = 123,
                ExpirationDate = new DateTime(2023, 12, 31),
                Id = cardId,
                UserId = 1 // different from 'userId'
            };

            cardRepositoryMock.Setup(repo => repo.GetCardById(cardId)).Returns(card);
            userServiceMock.Setup(service => service.GetUserById(userId)).Returns(new User());
            authManagerMock.Setup(manager => manager.IsAdmin(It.IsAny<User>())).Returns(false);

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            // Act and Assert
            Assert.ThrowsException<UnauthorizedOperationException>(() => cardService.GetCardById(cardId, userId));
        }
    }
}
