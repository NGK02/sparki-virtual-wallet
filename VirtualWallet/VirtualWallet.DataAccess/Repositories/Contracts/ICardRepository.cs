using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.DataAccess.Repositories.Contracts
{
    public interface ICardRepository
    {
        Card GetCardById(int cardId);

        IEnumerable<Card> GetCards();

        IEnumerable<Card> GetUserCards(int userId);

        void AddCard(Card card);

        void DeleteCard(Card card);

        void UpdateCard(Card card, Card cardToUpdate);
    }
}