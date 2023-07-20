using ForumSystem.DataAccess.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.Business.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository cardRepository;
        private readonly IUserService userService;

        public CardService(ICardRepository cardRepository, IUserService userService)
        {
            this.cardRepository = cardRepository;
            this.userService = userService;
        }

        public Card GetCardById(int cardId)
        {
            var card = cardRepository.GetCardById(cardId);

            if (card == null)
            {
                throw new EntityNotFoundException($"Card with ID {cardId} not found.");
            }

            return card;
        }

        public IEnumerable<Card> GetAllCards()
        {
            var cards = cardRepository.GetAllCards();

            if (!cards.Any() || cards == null)
            {
                throw new EntityNotFoundException("No cards found.");
            }

            return cards;
        }

        public void AddCard(Card card, int userId)
        {
            var user = userService.GetUserById(userId);

            card.User = user;
            user.Cards.Add(card);

            cardRepository.AddCard(card);
        }

        public void DeleteCard(int cardId)
        {
            var cardToDelete = GetCardById(cardId);

            cardRepository.DeleteCard(cardToDelete);
        }

        public void UpdateCard(Card card, int cardId)
        {
            var cardToUpdate = GetCardById(cardId);

            if (card.ExpirationDate < DateTime.Now)
            {
                throw new ArgumentException("Expiration date must be in the future.");
            }

            cardRepository.UpdateCard(card, cardToUpdate);
        }
    }
}