using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface IWalletTransactionService
	{
        /// <summary>
        /// Retrieves a dictionary of incoming transactions for the specified user within the last week,
        /// converted to the specified currency.
        /// </summary>
        /// <param name="userId">The ID of the user whose incoming transactions are being retrieved.</param>
        /// <param name="toCurrency">The currency code to convert the transaction amounts to.</param>
        /// <returns>A dictionary of currency codes and their respective incoming transaction amounts.</returns>
        Dictionary<string, decimal> GetUserIncomingTransactionsForLastWeek(int userId, CurrencyCode toCurrency);

        /// <summary>
        /// Retrieves a dictionary of outgoing transactions for the specified user within the last week,
        /// converted to the specified currency.
        /// </summary>
        /// <param name="userId">The ID of the user whose outgoing transactions are being retrieved.</param>
        /// <param name="toCurrency">The currency code to convert the transaction amounts to.</param>
        /// <returns>A dictionary of currency codes and their respective outgoing transaction amounts.</returns>
        Dictionary<string, decimal> GetUserOutgoingTransactionsForLastWeek(int userId, CurrencyCode toCurrency);

        /// <summary>
        /// Retrieves the count of all transactions in the system.
        /// </summary>
        /// <returns>The count of all transactions in the system.</returns>
        int GetTransactionsCount();

        /// <summary>
        /// Retrieves a list of wallet transactions for the specified user based on query parameters.
        /// </summary>
        /// <param name="queryParameters">Parameters to filter or modify the transaction list.</param>
        /// <param name="userId">The ID of the user whose wallet transactions are being retrieved.</param>
        /// <returns>A list of wallet transactions based on the specified criteria.</returns>
        List<WalletTransaction> GetUserWalletTransactions(WalletTransactionQueryParameters queryParameters, int userId);

        /// <summary>
        /// Retrieves a list of wallet transactions based on query parameters.
        /// </summary>
        /// <param name="queryParameters">Parameters to filter or modify the transaction list.</param>
        /// <returns>A list of wallet transactions based on the specified criteria.</returns>
        List<WalletTransaction> GetWalletTransactions(WalletTransactionQueryParameters queryParameters);

        /// <summary>
        /// Creates a new wallet transaction.
        /// </summary>
        /// <param name="walletTransaction">The wallet transaction object containing transaction details.</param>
        /// <returns>The newly created wallet transaction.</returns>
        WalletTransaction CreateTransaction(WalletTransaction walletTransaction);

        /// <summary>
        /// Creates a new wallet transaction with the sender information.
        /// </summary>
        /// <param name="walletTransaction">The wallet transaction object containing transaction details.</param>
        /// <param name="sender">The user initiating the transaction.</param>
        /// <returns>The newly created wallet transaction.</returns>
        WalletTransaction CreateTransaction(WalletTransaction walletTransaction, User sender);

        /// <summary>
        /// Retrieves a wallet transaction by its ID and the username of the sender.
        /// </summary>
        /// <param name="id">The ID of the wallet transaction to retrieve.</param>
        /// <param name="username">The username of the sender of the transaction.</param>
        /// <returns>The wallet transaction with the specified ID and sender username if found, otherwise null.</returns>
        WalletTransaction GetWalletTransactionById(int id, string username);
    }
}