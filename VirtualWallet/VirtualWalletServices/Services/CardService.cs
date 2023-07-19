using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VirtualWallet.Business.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository cardRepository;

        public CardService(ICardRepository cardRepository)
        {
            this.cardRepository = cardRepository;
        }

        public Card GetCardById(int cardId)
        {
            var card = cardRepository.GetCardById(cardId);

            if (card == null)
            {
                throw new ArgumentException($"Card with ID {cardId} not found.");
            }

            return card;
        }

        public IEnumerable<Card> GetAllCards()
        {
            var cards = cardRepository.GetAllCards();

            if (!cards.Any() || cards == null)
            {
                throw new ArgumentException("No cards found.");
            }

            return cards;
        }

        public void AddCard(Card card)
        {
            throw new NotImplementedException();
        }

        public void DeleteCard(Card card)
        {
            throw new NotImplementedException();
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