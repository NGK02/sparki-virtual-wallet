using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface ICardService
    {
        /// <summary>
        /// Retrieves a card by its ID for the specified user.
        /// </summary>
        /// <param name="cardId">The ID of the card to retrieve.</param>
        /// <param name="userId">The ID of the user requesting the card.</param>
        /// <returns>The card with the specified ID if found, otherwise null.</returns>
        Card GetCardById(int cardId, int userId);

        /// <summary>
        /// Retrieves all cards.
        /// </summary>
        /// <returns>An enumerable collection of all cards.</returns>
        IEnumerable<Card> GetCards();

        /// <summary>
        /// Retrieves all cards belonging to the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user whose cards are being retrieved.</param>
        /// <returns>An enumerable collection of cards owned by the user.</returns>
        IEnumerable<Card> GetUserCards(int userId);

        /// <summary>
        /// Lists all cards belonging to the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user whose cards are being listed.</param>
        /// <returns>An enumerable collection of cards owned by the user.</returns>
        IEnumerable<Card> ListUserCards(int userId);

        /// <summary>
        /// Creates a new card for the specified user.
        /// </summary>
        /// <param name="card">The card object to create.</param>
        /// <param name="userId">The ID of the user who owns the new card.</param>
        void CreateCard(Card card, int userId);

        /// <summary>
        /// Deletes a card by its ID for the specified user.
        /// </summary>
        /// <param name="cardId">The ID of the card to delete.</param>
        /// <param name="userId">The ID of the user who owns the card.</param>
        void DeleteCard(int cardId, int userId);

        /// <summary>
        /// Updates the details of a card by its ID for the specified user.
        /// </summary>
        /// <param name="card">The updated card object.</param>
        /// <param name="cardId">The ID of the card to update.</param>
        /// <param name="userId">The ID of the user who owns the card.</param>
        void UpdateCard(Card card, int cardId, int userId);
    }
}