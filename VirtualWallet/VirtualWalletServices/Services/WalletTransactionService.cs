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
using VirtualWallet.Business.Exceptions;

namespace VirtualWallet.Business.Services
{
	public class WalletTransactionService : IWalletTransactionService
	{
		private readonly IWalletTransactionRepository walletTransactionRepo;
		private readonly IUserService userService;
		private readonly IWalletService walletService;
		private readonly IAuthManager authManager;

		public WalletTransactionService(IWalletTransactionRepository walletTransactionRepo, IUserService userService, IAuthManager authManager, IWalletService walletService)
		{ 
			this.walletTransactionRepo = walletTransactionRepo;
			this.userService = userService;
			this.authManager = authManager;
			this.walletService = walletService;
		}

		public bool CreateTransaction(WalletTransaction walletTransaction, string senderUsername)
		{
			var sender = userService.GetUserByUsername(senderUsername);
			if (authManager.IsBlocked(sender) || authManager.IsAdmin(sender))
			{
				throw new UnauthorizedAccessException("You can't make transactions!");
			}
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
				recipientBalance = walletService.CreateWalletBalance(walletTransaction.Recipient.WalletId, walletTransaction.CurrencyId);
			}
			return walletTransactionRepo.CompleteTransaction(senderBalance, recipientBalance, walletTransaction.Amount);
		}

		public WalletTransaction GetWalletTransactionById(int id, string username) 
		{
			var user = userService.GetUserByUsername(username);
			var walletTransaction = walletTransactionRepo.GetWalletTransactionById(id);
			if (walletTransaction is null)
			{
				throw new EntityNotFoundException("Transaction doesn't exist!");
			}
			if (walletTransaction.SenderId != user.Id & walletTransaction.RecipientId != user.Id & !authManager.IsAdmin(user))
			{
				throw new UnauthorizedOperationException("You aren't authorized to view this transaction!");
			}

			return walletTransaction;
		}

		public List<WalletTransaction> GetUserWalletTransactions(WalletTransactionQueryParameters queryParameters, string username)
		{
			var user = userService.GetUserByUsername(username);
			return walletTransactionRepo.GetUserWalletTransactions(queryParameters, user.Id);
		}

		public List<WalletTransaction> GetWalletTransactions(WalletTransactionQueryParameters queryParameters, string username)
		{
			var user = userService.GetUserByUsername(username);
			authManager.IsAdmin(user);
			return walletTransactionRepo.GetWalletTransactions(queryParameters);
		}
	}
}
