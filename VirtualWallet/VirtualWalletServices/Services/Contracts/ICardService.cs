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
        Card GetCardById(int cardId);

        IEnumerable<Card> GetAllCards();

        void AddCard(Card card, int userId);

        void DeleteCard(Card card, int cardId);

        void UpdateCard(Card card, int cardId);
    }
}