using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface IWalletTransactionService
	{
        Dictionary<string, decimal> GetUserIncomingTransactionsForLastWeek(int userId, CurrencyCode toCurrency);

        Dictionary<string, decimal> GetUserOutgoingTransactionsForLastWeek(int userId, CurrencyCode toCurrency);

        int GetTransactionsCount();

        List<WalletTransaction> GetUserWalletTransactions(WalletTransactionQueryParameters queryParameters, int userId);

        List<WalletTransaction> GetWalletTransactions(WalletTransactionQueryParameters queryParameters);

        WalletTransaction CreateTransaction(WalletTransaction walletTransaction);

        WalletTransaction CreateTransaction(WalletTransaction walletTransaction, User sender);

		WalletTransaction GetWalletTransactionById(int id, string username);
    }
}