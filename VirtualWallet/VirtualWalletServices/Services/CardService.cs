using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories;
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

        public Card GetCardById(int cardId, string username)
        {
            var card = cardRepository.GetCardById(cardId);

            if (card == null)
            {
                throw new EntityNotFoundException($"Card with ID {cardId} not found.");
            }

            var user = userService.GetUserByUsername(username);

            if (!authManager.IsAdmin(user) && card.UserId != user.Id)
            {
                throw new UnauthorizedOperationException("Only an admin or the card's owner can access card details.");
            }

            return card;
        }

        public IEnumerable<Card> GetCards(string username)
        {
            var user = userService.GetUserByUsername(username);

            if (!authManager.IsAdmin(user))
            {
                throw new UnauthorizedOperationException("Only admins can access all cards.");
            }

            var cards = cardRepository.GetCards();

            if (!cards.Any() || cards == null)
            {
                throw new EntityNotFoundException("No cards found.");
            }

            return cards;
        }

        public IEnumerable<Card> GetUserCards(string username)
        {
            var user = userService.GetUserByUsername(username);

            var cards = cardRepository.GetUserCards(user.Id);

            if (!cards.Any() || cards == null)
            {
                throw new EntityNotFoundException("No cards found.");
            }

            return cards;
        }

        public void AddCard(Card card, string username)
        {
            var user = userService.GetUserByUsername(username);

            if (cardRepository.CardNumberExists(card.CardNumber))
            {
                throw new ArgumentException("Card with the given number already exists.");
            }

            card.User = user;
            card.UserId = user.Id;
            user.Cards.Add(card);

            cardRepository.AddCard(card);
        }

        public void DeleteCard(int cardId, string username)
        {
            var cardToDelete = GetCardById(cardId, username);

            cardRepository.DeleteCard(cardToDelete);
        }

        public void UpdateCard(Card card, int cardId, string username)
        {
            var cardToUpdate = GetCardById(cardId, username);

            if (card.ExpirationDate < DateTime.Now)
            {
                throw new ArgumentException("Expiration date must be in the future.");
            }

            cardRepository.UpdateCard(card, cardToUpdate);
        }
    }
}