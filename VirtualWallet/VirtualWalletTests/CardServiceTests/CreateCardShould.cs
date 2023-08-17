using Moq;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.CardServiceTests
{
    [TestClass]
    public class CreateCardShould
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
        public void CreateCard_ValidCard_CardAddedSuccessfully()
        {
            // Arrange
            int userId = 123;

            var user = new User {
                Id = userId,
                FirstName = "Georgi",
                LastName = "Georgiev",
                Username = "goshoXx123",
                Email = "gosho@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 1,
                IsConfirmed = true,
                PhoneNumber = "0888888881"
            };

            var card = new Card {
                CardHolder = "Georgi Georgiev",
                CardNumber = 1234567890123456,
                CheckNumber = 123,
                ExpirationDate = new DateTime(2023, 12, 31),
                Id = 1,
                UserId = userId
            };

            userServiceMock.Setup(repo => repo.GetUserById(userId)).Returns(user);
            cardRepositoryMock.Setup(repo => repo.CardNumberExists(card.CardNumber)).Returns(true);

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            // Act and Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => cardService.CreateCard(card, userId));
            Assert.AreEqual("A card with the provided number already exists.", ex.Message);
        }

        [TestMethod]
        public void CreateCard_CardNumberExists_ThrowsArgumentException()
        {
            // Arrange
            int userId = 123;

            var user = new User
            {
                Id = userId,
                FirstName = "Georgi",
                LastName = "Georgiev",
                Username = "goshoXx123",
                Email = "gosho@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 1,
                IsConfirmed = true,
                PhoneNumber = "0888888881"
            };

            var card = new Card
            {
                CardHolder = "Georgi Georgiev",
                CardNumber = 1234567890123456,
                CheckNumber = 123,
                ExpirationDate = new DateTime(2023, 12, 31),
                Id = 1,
                UserId = userId
            };

            userServiceMock.Setup(repo => repo.GetUserById(userId)).Returns(user);
            cardRepositoryMock.Setup(repo => repo.CardNumberExists(card.CardNumber)).Returns(true);

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            // Act and Assert
            Assert.ThrowsException<ArgumentException>(() => cardService.CreateCard(card, userId));
        }

        [TestMethod]
        public void CreateCard_CardNumberDoesNotExist_CardCreatedSuccessfully()
        {
            // Arrange
            int userId = 123;
            var cardNumber = 1234567890123456;

            var user = new User
            {
                Id = userId,
                FirstName = "Georgi",
                LastName = "Georgiev",
                Username = "goshoXx123",
                Email = "gosho@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 1,
                IsConfirmed = true,
                PhoneNumber = "0888888881"
            };

            userServiceMock.Setup(service => service.GetUserById(userId)).Returns(user);

            cardRepositoryMock.Setup(repo => repo.CardNumberExists(cardNumber)).Returns(false);

            var cardService = new CardService(authManagerMock.Object, cardRepositoryMock.Object, userServiceMock.Object);

            // Act and Assert
            var card = new Card
            {
                CardHolder = "Georgi Georgiev",
                CardNumber = cardNumber,
                CheckNumber = 123,
                ExpirationDate = new DateTime(2023, 12, 31),
                Id = 1,
                UserId = userId
            };

            try
            {
                cardService.CreateCard(card, userId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"CreateCard threw an unexpected exception: {ex}");
            }

            // Additional Asserts
            cardRepositoryMock.Verify(repo => repo.AddCard(card), Times.Once);
            Assert.AreEqual(user, card.User);
            Assert.AreEqual(userId, card.UserId);
            Assert.IsTrue(user.Cards.Contains(card));
        }

    }
}
