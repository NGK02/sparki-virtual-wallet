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
    public class GetCardsShould
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
        public void GetCards_ValidCards_ReturnsListOfCards()
        {
            var cards = new List<Card>
            {
                new Card { CardHolder = "Georgi Georgiev",
                    CardNumber = 1234567890123456,
                    CheckNumber = 123,
                    ExpirationDate = new DateTime(2023, 12, 31),
                    Id = 1,
                    UserId = 1 },
                new Card { CardHolder = "Nikolai Barekov",
                    CardNumber = 9876543210987654,
                    CheckNumber = 456,
                    ExpirationDate = new DateTime(2024, 12, 31),
                    Id = 2,
                    UserId = 2 },
                new Card { CardHolder = "Shtilian Uzunov",
                    CardNumber = 1111222233334444,
                    CheckNumber = 032,
                    ExpirationDate = new DateTime(2024, 10, 31),
                    Id = 3,
                    UserId = 3 }
            };

            cardRepositoryMock.Setup(repo => repo.GetCards()).Returns(cards);

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            // Act
            var resultCards = cardService.GetCards().ToList();

            // Assert
            CollectionAssert.AreEqual(cards, resultCards);
        }

        [TestMethod]
        public void GetCards_NoCardsAvailable_ThrowsEntityNotFoundException()
        {
            cardRepositoryMock.Setup(repo => repo.GetCards()).Returns(new List<Card>());

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            // Act and Assert
            Assert.ThrowsException<EntityNotFoundException>(() => cardService.GetCards());
        }
    }
}
