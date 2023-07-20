﻿using Microsoft.EntityFrameworkCore;
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

        public Card GetCardById(int cardId)
        {
            return walletDbContext.Cards.Include(c => c.User).FirstOrDefault(c => !c.IsDeleted && c.Id == cardId);
        }

        public IEnumerable<Card> GetCards()
        {
            return walletDbContext.Cards
                .Where(c => !c.IsDeleted)
                .Include(c => c.User)
                .ToList();
        }

        public void AddCard(Card card)
        {
            walletDbContext.Cards.Add(card);
            walletDbContext.SaveChanges();
        }

        public void DeleteCard(Card card)
        {
            card.IsDeleted = true;
            walletDbContext.SaveChanges();
        }

        public void UpdateCard(Card card, Card cardToUpdate)
        {
            cardToUpdate.CardNumber = card.CardNumber;
            cardToUpdate.CheckNumber = card.CheckNumber;
            cardToUpdate.ExpirationDate = card.ExpirationDate;
            walletDbContext.SaveChanges();
        }
    }
}