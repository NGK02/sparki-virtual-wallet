using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.DataAccess.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly WalletDbContext walletDbContext;

        public CardRepository(WalletDbContext walletDbContext)
        {
            this.walletDbContext = walletDbContext;
        }

        private IQueryable<Card> GetQueryableCards()
        {
            return walletDbContext.Cards
                .Where(c => !c.IsDeleted)
                .Include(c => c.Currency)
                .Include(c => c.Transfers)
                .Include(c => c.User);
        }

        public bool CardNumberExists(long cardNumber)
        {
            return GetQueryableCards().Any(c => c.CardNumber == cardNumber);
        }

        public Card GetCardById(int cardId)
        {
            return GetQueryableCards().SingleOrDefault(c => c.Id == cardId);
        }

        public IEnumerable<Card> GetCards()
        {
            return GetQueryableCards().ToList();
        }

        public IEnumerable<Card> GetUserCards(int userId)
        {
            return GetQueryableCards().Where(c => c.UserId == userId).ToList();
        }

        public void AddCard(Card card)
        {
            card.CreatedOn = DateTime.Now;
            walletDbContext.Cards.Add(card);
            walletDbContext.SaveChanges();
        }

        public void DeleteCard(Card card)
        {
            card.DeletedOn = DateTime.Now;
            card.IsDeleted = true;
            walletDbContext.SaveChanges();
        }

        public void UpdateCard(Card card, Card cardToUpdate)
        {
            cardToUpdate.CardHolder = card.CardHolder;
            cardToUpdate.CardNumber = card.CardNumber;
            cardToUpdate.CheckNumber = card.CheckNumber;
            cardToUpdate.Currency = card.Currency;
            cardToUpdate.ExpirationDate = card.ExpirationDate;
            walletDbContext.SaveChanges();
        }
    }
}