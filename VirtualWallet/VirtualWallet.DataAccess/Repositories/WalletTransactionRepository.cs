using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.DataAccess.QueryParameters;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace VirtualWallet.DataAccess.Repositories
{
	public class WalletTransactionRepository : IWalletTransactionRepository
	{
		private readonly WalletDbContext database;

		public WalletTransactionRepository(WalletDbContext database)
		{
			this.database = database;
		}

		public bool CreateTransaction(WalletTransaction walletTransaction)
		{
			database.WalletTransactions.Add(walletTransaction);
			database.SaveChanges();
			return true;
		}

		public bool CompleteTransaction(Balance senderBalance, Balance recipientBalance, decimal walletTransactionAmount)
		{
			//Тук може би е по-подходящо да се използва SqlTransaction?
			using (TransactionScope transactionScope = new TransactionScope())
			{
				try
				{
					senderBalance.Amount -= walletTransactionAmount;
					recipientBalance.Amount += walletTransactionAmount;
					database.SaveChanges();
					transactionScope.Complete();
				}
				catch (DbUpdateException)
				{
					//TODO: Custom exception here.
					throw new InvalidOperationException("Transaction failed due to database error!");
				}
				//catch (OverflowException)
				//{
				//	throw new InvalidOperationException("Too much money?");
				//}
			}
			return true;
		}

		public List<WalletTransaction> GetWalletTransactions(WalletTransactionQueryParameters queryParameters)
		{
			var transactions = GetWalletTransactionsQueryable();
			transactions = FilterBy(queryParameters, transactions);
			transactions = SortBy(queryParameters, transactions);
			return transactions.ToList();
		}

		public List<WalletTransaction> GetUserWalletTransactions(WalletTransactionQueryParameters queryParameters, int id)
		{
			var userTransactions = GetWalletTransactionsQueryable().Where(t => t.SenderId == id || t.RecipientId == id);
			userTransactions = FilterBy(queryParameters, userTransactions);
			userTransactions = SortBy(queryParameters, userTransactions);
			return userTransactions.ToList();
		}

		private IQueryable<WalletTransaction> FilterBy(WalletTransactionQueryParameters queryParameters, IQueryable<WalletTransaction> transactions)
		{
			if (!string.IsNullOrEmpty(queryParameters.SenderUsername))
			{
				transactions = transactions.Where(t => t.Sender.Username.ToLower() == queryParameters.SenderUsername.ToLower());
			}

			if (!string.IsNullOrEmpty(queryParameters.RecipientUsername))
			{
				transactions = transactions.Where(t => t.Recipient.Username.ToLower() == queryParameters.RecipientUsername.ToLower());
			}

			if (queryParameters.MaxDate.HasValue)
			{
				DateTime maxDate = queryParameters.MaxDate.Value.Date;
				transactions = transactions.Where(t => t.CreatedOn.Date <= maxDate);
			}

			if (queryParameters.MinDate.HasValue)
			{
				DateTime minDate = queryParameters.MinDate.Value.Date;
				transactions = transactions.Where(t => t.CreatedOn.Date >= minDate);
			}

			return transactions;
		}

		private IQueryable<WalletTransaction> SortBy(WalletTransactionQueryParameters queryParameters, IQueryable<WalletTransaction> transactions)
		{
			if (!string.IsNullOrEmpty(queryParameters.SortBy))
			{

				if (queryParameters.SortBy.Equals("date", StringComparison.InvariantCultureIgnoreCase))
				{
					transactions = transactions.OrderBy(t => t.CreatedOn);
				}
				if (queryParameters.SortBy.Equals("amount", StringComparison.InvariantCultureIgnoreCase))
				{
					transactions = transactions.OrderBy(t => t.Amount);
				}

				if (!string.IsNullOrEmpty(queryParameters.SortOrder) && queryParameters.SortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
				{
					transactions.Reverse();
				}
			}

			return transactions;
		}

		private IQueryable<WalletTransaction> GetWalletTransactionsQueryable()
		{
			return database.WalletTransactions.Where(t => t.IsDeleted == false);
		}
	}
}
