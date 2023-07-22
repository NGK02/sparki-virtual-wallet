using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services.Contracts
{
	public interface IWalletTransactionService
	{
		public bool CreateTransaction(WalletTransaction walletTransaction, string senderUsername);

		List<WalletTransaction> GetUserWalletTransactions(WalletTransactionQueryParameters queryParameters, string username);
	}
}
