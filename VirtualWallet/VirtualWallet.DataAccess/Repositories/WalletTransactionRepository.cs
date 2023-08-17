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

        public WalletTransaction CreateTransaction(WalletTransaction walletTransaction)
        {
            database.WalletTransactions.Add(walletTransaction);
            database.SaveChanges();
            return walletTransaction;
        }

        public bool CompleteTransaction(Balance senderBalance, Balance recipientBalance, decimal walletTransactionAmount)
        {
            //Тук може би е по-подходящо да се използва SqlTransaction?
            using (TransactionScope transactionScope = new TransactionScope())
            {
                senderBalance.Amount -= walletTransactionAmount;
                recipientBalance.Amount += walletTransactionAmount;
                database.SaveChanges();
                transactionScope.Complete();
                //catch (OverflowException)
                //{
                //	throw new InvalidOperationException("Too much money?");
                //}
            }
            return true;
        }

        public WalletTransaction GetWalletTransactionById(int id)
        {
            var walletTransaction = GetWalletTransactionsQueryable().FirstOrDefault(wt => wt.Id == id);
            return walletTransaction;
        }

        public List<WalletTransaction> GetWalletTransactions(WalletTransactionQueryParameters queryParameters)
        {
            var walletTransactions = GetWalletTransactionsQueryable();
            walletTransactions = FilterBy(queryParameters, walletTransactions);
            walletTransactions = SortBy(queryParameters, walletTransactions);
            return walletTransactions.ToList();
        }

        public List<WalletTransaction> GetUserWalletTransactions(WalletTransactionQueryParameters queryParameters, int id)
        {
            var userTransactions = GetWalletTransactionsQueryable().Where(t => t.SenderId == id || t.RecipientId == id);
            userTransactions = FilterBy(queryParameters, userTransactions);
            userTransactions = SortBy(queryParameters, userTransactions);
            return userTransactions.ToList();
        }

        private IQueryable<WalletTransaction> FilterBy(WalletTransactionQueryParameters queryParameters, IQueryable<WalletTransaction> walletTransactions)
        {
            if (!string.IsNullOrEmpty(queryParameters.SenderUsername))
            {
                walletTransactions = walletTransactions.Where(t => t.Sender.Username.ToLower() == queryParameters.SenderUsername.ToLower());
            }

            if (!string.IsNullOrEmpty(queryParameters.RecipientUsername))
            {
                walletTransactions = walletTransactions.Where(t => t.Recipient.Username.ToLower() == queryParameters.RecipientUsername.ToLower());
            }

            if (queryParameters.MaxDate.HasValue)
            {
                DateTime maxDate = queryParameters.MaxDate.Value.Date;
                walletTransactions = walletTransactions.Where(t => t.CreatedOn.Date <= maxDate);
            }

            if (queryParameters.MinDate.HasValue)
            {
                DateTime minDate = queryParameters.MinDate.Value.Date;
                walletTransactions = walletTransactions.Where(t => t.CreatedOn.Date >= minDate);
            }

            return walletTransactions;
        }

        private IQueryable<WalletTransaction> SortBy(WalletTransactionQueryParameters queryParameters, IQueryable<WalletTransaction> walletTransactions)
        {
            if (queryParameters.SortBy is not null)
            {

                if (queryParameters.SortBy.Equals("Date", StringComparison.InvariantCultureIgnoreCase))
                {
                    walletTransactions = walletTransactions.OrderBy(t => t.CreatedOn);
                }
                if (queryParameters.SortBy.Equals("Amount", StringComparison.InvariantCultureIgnoreCase))
                {
                    walletTransactions = walletTransactions.OrderBy(t => t.Amount);
                }
                if (queryParameters.SortBy.Equals("Currency", StringComparison.InvariantCultureIgnoreCase))
                {
                    walletTransactions = walletTransactions.OrderBy(t => t.Currency.Code);
                }

                if (queryParameters.SortBy is not null && queryParameters.SortOrder is not null)
                {
                    if (queryParameters.SortOrder.Equals("Descending", StringComparison.InvariantCultureIgnoreCase))
                    {
                        walletTransactions = walletTransactions.Reverse();
                    }
                }
            }

            return walletTransactions;
        }

        private IQueryable<WalletTransaction> GetWalletTransactionsQueryable()
        {
            return database.WalletTransactions.Where(wt => wt.IsDeleted == false)
                .Include(wt => wt.Sender)
                .Include(wt => wt.Recipient)
                .Include(wt => wt.Currency);
        }

        public int GetTransactionsCount()
        {
            return database.WalletTransactions.Where(t => t.IsDeleted == false).Count();
        }
    }
}
