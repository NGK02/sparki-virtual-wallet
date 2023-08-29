using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.Business.Services
{
    public class CardService : ICardService
    {
        private readonly IAuthManager authManager;
        private readonly ICardRepository cardRepository;
        private readonly IUserService userService;

        public CardService(IAuthManager authManager, ICardRepository cardRepository, IUserService userService)
        {
            this.authManager = authManager;
            this.cardRepository = cardRepository;
            this.userService = userService;
        }

        public Card GetCardById(int cardId, int userId)
        {
            var card = cardRepository.GetCardById(cardId);

            if (card == null)
            {
                throw new EntityNotFoundException("Requested card not found.");
            }

            var user = userService.GetUserById(userId);

            if (!authManager.IsAdmin(user) && card.UserId != userId)
            {
                throw new UnauthorizedOperationException("Access to card denied.");
            }

            return card;
        }

        public IEnumerable<Card> GetCards()
        {
            var cards = cardRepository.GetCards();

            if (!cards.Any() || cards == null)
            {
                throw new EntityNotFoundException("No cards available.");
            }

            return cards;
        }

        public IEnumerable<Card> GetUserCards(int userId)
        {
            var cards = cardRepository.GetUserCards(userId);

            if (!cards.Any() || cards == null)
            {
                throw new EntityNotFoundException("No cards available.");
            }

            return cards;
        }

        public IEnumerable<Card> ListUserCards(int userId)
        {
            return cardRepository.GetUserCards(userId);
        }

        public void CreateCard(Card card, int userId)
        {
            var user = userService.GetUserById(userId);

            if (cardRepository.CardNumberExists(card.CardNumber))
            {
                throw new ArgumentException("A card with the provided number already exists.");
            }

            card.User = user;
            card.UserId = userId;
            user.Cards.Add(card);

            cardRepository.AddCard(card);
        }

        public void DeleteCard(int cardId, int userId)
        {
            var cardToDelete = GetCardById(cardId, userId);

            cardRepository.DeleteCard(cardToDelete);
        }

        public void UpdateCard(Card card, int cardId, int userId)
        {
            var cardToUpdate = GetCardById(cardId, userId);

            if (card.ExpirationDate < DateTime.Now)
            {
                throw new ArgumentException("Invalid expiration date.");
            }

            cardRepository.UpdateCard(card, cardToUpdate);
        }
    }
}