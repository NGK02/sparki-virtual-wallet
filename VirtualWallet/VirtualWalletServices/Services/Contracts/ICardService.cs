using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface ICardService
    {
        Card GetCardById(int cardId, string username);

        IEnumerable<Card> GetCards(string username);

        IEnumerable<Card> GetUserCards(string username);

        void AddCard(Card card, string username);

        void DeleteCard(int cardId, string username);

        void UpdateCard(Card card, int cardId, string username);
    }
}