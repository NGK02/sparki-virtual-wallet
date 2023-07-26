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
        Card GetCardById(int cardId, int userId);

        IEnumerable<Card> GetCards(int userId);

        IEnumerable<Card> GetUserCards(int userId);

        void AddCard(Card card, int userId);

        void DeleteCard(int cardId, int userId);

        void UpdateCard(Card card, int cardId, int userId);
    }
}