using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.DataAccess.Repositories;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services
{
	public class WalletTransactionService : IWalletTransactionService
	{
		private readonly IWalletTransactionRepository walletTransactionRepo;
		private readonly IUserService userService;
		private readonly IUserRepository userRepository;
		private readonly IAuthManager authManager;

		public WalletTransactionService(IWalletTransactionRepository walletTransactionRepo, IUserService userService, IAuthManager authManager, IUserRepository userRepository)
		{ 
			this.walletTransactionRepo = walletTransactionRepo;
			this.userService = userService;
			this.authManager = authManager;
			this.userRepository = userRepository;
		}

		public bool CreateTransaction(WalletTransaction walletTransaction, string senderUsername)
		{
			var sender = userService.GetUserByUsername(senderUsername);
			authManager.IsBlocked(sender);
			walletTransaction.Sender = sender;
			PrepareTransaction(walletTransaction);
			return walletTransactionRepo.CreateTransaction(walletTransaction);
		}

		public bool PrepareTransaction(WalletTransaction walletTransaction)
		{
			var senderBalance = walletTransaction.Sender.Wallet.Balances.FirstOrDefault(b => b.CurrencyId == walletTransaction.CurrencyId);
			if (senderBalance is null || senderBalance.Amount < walletTransaction.Amount)
			{
				//Custom exception here?
				throw new InvalidOperationException("You don't have sufficient funds!");
			}
			var recipientBalance = walletTransaction.Recipient.Wallet.Balances.FirstOrDefault(b => b.CurrencyId == walletTransaction.CurrencyId);
			if (recipientBalance is null)
			{
				recipientBalance = userRepository.CreateUserBalance(walletTransaction.Recipient.WalletId, walletTransaction.CurrencyId);
			}
			return walletTransactionRepo.CompleteTransaction(senderBalance, recipientBalance, walletTransaction.Amount);
		}

		//public List<WalletTransaction> GetUserWalletTransactions(WalletTransactionQueryParameters queryParameters, string requesterUsername)
		//{ 

		//}
	}
}
