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
using VirtualWallet.DataAccess.Enums;

namespace VirtualWallet.Business.Services
{
	public class WalletTransactionService : IWalletTransactionService
	{
		private readonly IWalletTransactionRepository walletTransactionRepo;
		private readonly IUserService userService;
		private readonly IWalletService walletService;
		private readonly IAuthManager authManager;
		private readonly IExchangeService exchangeService;

		public WalletTransactionService(IWalletTransactionRepository walletTransactionRepo, IUserService userService, IAuthManager authManager, IWalletService walletService, IExchangeService exchangeService)
        { 
			this.walletTransactionRepo = walletTransactionRepo;
			this.userService = userService;
			this.authManager = authManager;
			this.walletService = walletService;
			this.exchangeService = exchangeService;
		}

		public WalletTransaction CreateTransaction(WalletTransaction walletTransaction, User sender)
		{
			walletTransaction.Sender = sender;
			PrepareTransaction(walletTransaction);
			return walletTransactionRepo.CreateTransaction(walletTransaction);
		}

		public WalletTransaction CreateTransaction(WalletTransaction walletTransaction)
		{
			var sender = userService.GetUserById(walletTransaction.SenderId);
			walletTransaction.Sender = sender;

			walletTransaction.Recipient = userService.SearchBy(new UserQueryParameters { Username = walletTransaction.Recipient.Username, Email = walletTransaction.Recipient.Email, PhoneNumber = walletTransaction.Recipient.PhoneNumber });

			PrepareTransaction(walletTransaction);

			return walletTransactionRepo.CreateTransaction(walletTransaction);
		}

		public Dictionary<string, decimal> GetUserOutgoingTransactionsForLastWeek(int userId, CurrencyCode toCurrency)
        {
			var walletTransactions = walletTransactionRepo.GetUserWalletTransactions(new WalletTransactionQueryParameters { MinDate = DateTime.Today.AddDays(-7), MaxDate = DateTime.Today }, userId);
			var currencyExchanges = exchangeService.GetAllExchangeRates(toCurrency).Result;

            Dictionary<string, decimal> outgoingTransactions = new Dictionary<string, decimal>();
            for (int i = 0; i < 7; i++)
			{
				var date = DateTime.Today.AddDays(-i);
				var dateString = date.ToString("dd'/'MM'/'yy");
                outgoingTransactions.Add(dateString, walletTransactions
					.Where(wt => (wt.CreatedOn.Date == date) & (wt.SenderId == userId))
					.Sum(wt => exchangeService.CalculateExchangeResult(currencyExchanges ,wt.Currency.Code, wt.Amount)));
			}

			return outgoingTransactions;
        }

        public Dictionary<string, decimal> GetUserIncomingTransactionsForLastWeek(int userId, CurrencyCode toCurrency)
        {
            var walletTransactions = walletTransactionRepo.GetUserWalletTransactions(new WalletTransactionQueryParameters { MinDate = DateTime.Today.AddDays(-7), MaxDate = DateTime.Today }, userId);
            var currencyExchanges = exchangeService.GetAllExchangeRates(toCurrency).Result;

            Dictionary<string, decimal> incomingTransactions = new Dictionary<string, decimal>();
            for (int i = 0; i < 7; i++)
            {
                var date = DateTime.Today.AddDays(-i);
                var dateString = date.ToString("dd'/'MM'/'yy");
                incomingTransactions.Add(dateString, walletTransactions
                    .Where(wt => (wt.CreatedOn.Date == date) & (wt.RecipientId == userId))
                    .Sum(wt => exchangeService.CalculateExchangeResult(currencyExchanges, wt.Currency.Code, wt.Amount)));
            }

            return incomingTransactions;
        }

        private void PrepareTransaction(WalletTransaction walletTransaction)
		{
			//Да се овъррайдне Equals?
			if (walletTransaction.Sender.Id == walletTransaction.Recipient.Id)
			{
                throw new InvalidOperationException("You can't send money to yourself!");
            }

			var senderBalance = walletTransaction.Sender.Wallet.Balances.FirstOrDefault(b => b.CurrencyId == walletTransaction.CurrencyId);
			if (senderBalance is null || senderBalance.Amount < walletTransaction.Amount)
			{
				//Custom exception here?
				throw new InvalidOperationException("You don't have sufficient funds!");
			}
			
			//Този може да се рефакторира или е окей по този начин да се променя процеса в зависимост дали е извикан от уеб или апи метода?
			var recipient = walletTransaction.Recipient ?? userService.GetUserById(walletTransaction.RecipientId);
			var recipientBalance = recipient.Wallet.Balances.FirstOrDefault(b => b.CurrencyId == walletTransaction.CurrencyId);
			if (recipientBalance is null)
			{
				recipientBalance = walletService.CreateWalletBalance(walletTransaction.CurrencyId, recipient.WalletId);
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
