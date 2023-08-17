using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.CardServiceTests
{
    [TestClass]
    public class UpdateCardShould
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
        public void UpdateCard_ValidExpirationDate_CardUpdatedSuccessfully()
        {
            // Arrange
            int userId = 123;
            int cardId = 456;
            var expirationDate = DateTime.Now.AddDays(30);

            var cardToUpdate = new Card {
                CardHolder = "Georgi Georgiev",
                CardNumber = 1234567890123456,
                CheckNumber = 123,
                ExpirationDate = expirationDate,
                Id = cardId,
                UserId = userId
            };

            cardRepositoryMock.Setup(repo => repo.GetCardById(cardId)).Returns(cardToUpdate);

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            // Act
            var updatedCard = new Card { CardHolder = "Georgi Georgiev",
                CardNumber = 1234567890123456,
                CheckNumber = 123,
                ExpirationDate = DateTime.Now.AddMonths(2),
                Id = cardId,
                UserId = userId
            };

            try
            {
                cardService.UpdateCard(updatedCard, cardId, userId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"UpdateCard threw an unexpected exception: {ex}");
            }

            // Assert
            cardRepositoryMock.Verify(repo => repo.UpdateCard(updatedCard, cardToUpdate), Times.Once);
        }

        [TestMethod]
        public void UpdateCard_InvalidExpirationDate_ThrowsArgumentException()
        {
            // Arrange
            int userId = 123;
            int cardId = 456;
            var expirationDate = DateTime.Now.AddDays(-1); // An expired card

            var cardRepositoryMock = new Mock<ICardRepository>();
            var userServiceMock = new Mock<IUserService>();
            var authManagerMock = new Mock<IAuthManager>();

            var cardToUpdate = new Card {
                CardHolder = "Georgi Georgiev",
                CardNumber = 1234567890123456,
                CheckNumber = 123,
                ExpirationDate = expirationDate,
                Id = cardId,
                UserId = userId
            };
            cardRepositoryMock.Setup(repo => repo.GetCardById(cardId)).Returns(cardToUpdate);

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            // Act and Assert
            var updatedCard = new Card { ExpirationDate = DateTime.Now.AddDays(-2),
                CardHolder = "Georgi Georgiev",
                CardNumber = 1234567890123456,
                CheckNumber = 123,
                Id = cardId,
                UserId = userId
            };

            Assert.ThrowsException<ArgumentException>(() => cardService.UpdateCard(updatedCard, cardId, userId));
        }
    }
}
