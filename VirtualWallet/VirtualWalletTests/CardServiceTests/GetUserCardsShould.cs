using Moq;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.Business.Exceptions;

namespace VirtualWalletTests.CardServiceTests
{
    [TestClass]
    public class GetUserCardsShould
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
        public void GetUserCards_ValidUserAndCards_ReturnsListOfCards()
        {
            int userId = 123;

            var cards = new List<Card>
            {
                new Card { CardHolder = "Georgi Georgiev",
                    CardNumber = 1234567890123456,
                    CheckNumber = 123,
                    ExpirationDate = new DateTime(2023, 12, 31),
                    Id = 1,
                    UserId = userId },
                new Card { CardHolder = "Nikolai Barekov",
                    CardNumber = 9876543210987654,
                    CheckNumber = 456,
                    ExpirationDate = new DateTime(2024, 12, 31),
                    Id = 2,
                    UserId = userId },
                new Card { CardHolder = "Shtilian Uzunov",
                    CardNumber = 1111222233334444,
                    CheckNumber = 032,
                    ExpirationDate = new DateTime(2024, 10, 31),
                    Id = 3,
                    UserId = userId }
            };

            cardRepositoryMock.Setup(repo => repo.GetUserCards(userId)).Returns(cards);

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            // Act
            var resultCards = cardService.GetUserCards(userId).ToList();

            // Assert
            CollectionAssert.AreEqual(cards, resultCards);
        }

        [TestMethod]
        public void GetUserCards_NoCardsAvailable_ThrowsEntityNotFoundException()
        {
            // Arrange
            int userId = 123;

            cardRepositoryMock.Setup(repo => repo.GetUserCards(userId)).Returns(new List<Card>());

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            // Act and Assert
            Assert.ThrowsException<EntityNotFoundException>(() => cardService.GetUserCards(userId));
        }

        [TestMethod]
        public void ListUserCards_ReturnsListOfCards()
        {
            // Arrange
            int userId = 123;

            var cards = new List<Card>
            {
                new Card { CardHolder = "Georgi Georgiev",
                    CardNumber = 1234567890123456,
                    CheckNumber = 123,
                    ExpirationDate = new DateTime(2023, 12, 31),
                    Id = 1,
                    UserId = userId },
                new Card { CardHolder = "Nikolai Barekov",
                    CardNumber = 9876543210987654,
                    CheckNumber = 456,
                    ExpirationDate = new DateTime(2024, 12, 31),
                    Id = 2,
                    UserId = userId },
                new Card { CardHolder = "Shtilian Uzunov",
                    CardNumber = 1111222233334444,
                    CheckNumber = 032,
                    ExpirationDate = new DateTime(2024, 10, 31),
                    Id = 3,
                    UserId = userId }
            };

            cardRepositoryMock.Setup(repo => repo.GetUserCards(userId)).Returns(cards);

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            // Act
            var resultCards = cardService.ListUserCards(userId).ToList();

            // Assert
            CollectionAssert.AreEqual(cards, resultCards);
        }
    }
}
