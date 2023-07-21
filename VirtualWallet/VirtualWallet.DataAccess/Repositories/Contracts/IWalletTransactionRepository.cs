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
		public bool CreateTransaction(WalletTransaction walletTransaction);
		public bool CompleteTransaction(Balance senderBalance, Balance recipientBalance, decimal walletTransactionAmount);

		public List<WalletTransaction> GetWalletTransactions(WalletTransactionQueryParameters queryParameters);
		public List<WalletTransaction> GetUserWalletTransactions(WalletTransactionQueryParameters queryParameters, int id);
	}
}
