﻿using System;
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
		int GetTransactionsCount();
		WalletTransaction CreateTransaction(WalletTransaction walletTransaction, User sender);
		WalletTransaction CreateTransaction(WalletTransaction walletTransaction, int senderId);

		WalletTransaction GetWalletTransactionById(int id, string username);
		List<WalletTransaction> GetUserWalletTransactions(WalletTransactionQueryParameters queryParameters, int userId);
		List<WalletTransaction> GetWalletTransactions(WalletTransactionQueryParameters queryParameters);
	}
}
