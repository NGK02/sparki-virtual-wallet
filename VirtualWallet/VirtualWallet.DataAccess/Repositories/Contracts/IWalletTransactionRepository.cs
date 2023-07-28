using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.DataAccess.Repositories.Contracts
{
	public interface IWalletTransactionRepository
	{
		int GetTransactionsCount();
		WalletTransaction CreateTransaction(WalletTransaction walletTransaction);
		bool CompleteTransaction(Balance senderBalance, Balance recipientBalance, decimal walletTransactionAmount);

		WalletTransaction GetWalletTransactionById(int id);
		List<WalletTransaction> GetWalletTransactions(WalletTransactionQueryParameters queryParameters);
		List<WalletTransaction> GetUserWalletTransactions(WalletTransactionQueryParameters queryParameters, int id);
	}
}
