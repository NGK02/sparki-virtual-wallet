using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services.Contracts
{
	public interface IWalletTransactionService
	{
		int GetTransactionsCount();
		WalletTransaction CreateTransaction(WalletTransaction walletTransaction, User sender);
		WalletTransaction CreateTransaction(WalletTransaction walletTransaction);

		WalletTransaction GetWalletTransactionById(int id, string username);
		List<WalletTransaction> GetUserWalletTransactions(WalletTransactionQueryParameters queryParameters, int userId);
		List<WalletTransaction> GetWalletTransactions(WalletTransactionQueryParameters queryParameters);
		Dictionary<string, decimal> GetUserOutgoingTransactionsForLastWeek(int userId, CurrencyCode toCurrency);
        Dictionary<string, decimal> GetUserIncomingTransactionsForLastWeek(int userId, CurrencyCode toCurrency);
    }
}
