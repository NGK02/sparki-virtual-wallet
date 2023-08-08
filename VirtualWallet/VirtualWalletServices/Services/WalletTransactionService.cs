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
using System.Transactions;

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

		public WalletTransaction CreateTransaction(WalletTransaction walletTransaction, User sender)
		{
			walletTransaction.Sender = sender;
			PrepareTransaction(walletTransaction);
			return walletTransactionRepo.CreateTransaction(walletTransaction);
		}

		public WalletTransaction CreateTransaction(WalletTransaction walletTransaction, int senderId)
		{
			var sender = userService.GetUserById(senderId);
			walletTransaction.Sender = sender;

			walletTransaction.Recipient = userService.SearchBy(new UserQueryParameters { Username = walletTransaction.Recipient.Username, Email = walletTransaction.Recipient.Email, PhoneNumber = walletTransaction.Recipient.PhoneNumber });

			PrepareTransaction(walletTransaction);

			return walletTransactionRepo.CreateTransaction(walletTransaction);
		}

		private void PrepareTransaction(WalletTransaction walletTransaction)
		{
			var senderBalance = walletTransaction.Sender.Wallet.Balances.FirstOrDefault(b => b.CurrencyId == walletTransaction.CurrencyId);
			if (senderBalance is null || senderBalance.Amount < walletTransaction.Amount)
			{
				//Custom exception here?
				throw new InvalidOperationException("You don't have sufficient funds!");
			}
			//Този метод трябва да се рефакторира, защото е неефективен за уеб частта! (Взима се един и същ юзър два пъти)
			var recipient = walletTransaction.Recipient ?? userService.GetUserById(walletTransaction.RecipientId);
			var recipientBalance = recipient.Wallet.Balances.FirstOrDefault(b => b.CurrencyId == walletTransaction.CurrencyId);
			if (recipientBalance is null)
			{
				recipientBalance = walletService.CreateWalletBalance(recipient.WalletId, walletTransaction.CurrencyId);
			}
			walletTransactionRepo.CompleteTransaction(senderBalance, recipientBalance, walletTransaction.Amount);
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

		public List<WalletTransaction> GetUserWalletTransactions(WalletTransactionQueryParameters queryParameters, int userId)
		{
			return walletTransactionRepo.GetUserWalletTransactions(queryParameters, userId);
		}

		public List<WalletTransaction> GetWalletTransactions(WalletTransactionQueryParameters queryParameters)
		{
			return walletTransactionRepo.GetWalletTransactions(queryParameters);
		}

        public int GetTransactionsCount()
        {
            return walletTransactionRepo.GetTransactionsCount();
        }
    }
}
