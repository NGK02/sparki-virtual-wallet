using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.DataAccess.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly WalletDbContext walletDbContext;

        public TransferRepository(WalletDbContext walletDbContext)
        {
            this.walletDbContext = walletDbContext;
        }

        private IQueryable<Transfer> GetQueryableTransfers()
        {
            return walletDbContext.Transfers
                .Where(t => !t.IsDeleted)
                .Include(t => t.Card)
                .Include(t => t.Currency)
                .Include(t => t.Wallet);
        }

        private IQueryable<Transfer> SortTransfers(IQueryable<Transfer> queryableTransfers, QueryParams parameters)
        {
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                if (parameters.SortBy.Equals("Amount", StringComparison.InvariantCultureIgnoreCase))
                {
                    queryableTransfers = queryableTransfers.OrderBy(t => t.Amount);
                }

                if (parameters.SortBy.Equals("Card", StringComparison.InvariantCultureIgnoreCase))
                {
                    queryableTransfers = queryableTransfers.OrderBy(t => t.Card.CardNumber);
                }

                if (parameters.SortBy.Equals("Currency", StringComparison.InvariantCultureIgnoreCase))
                {
                    queryableTransfers = queryableTransfers.OrderBy(t => t.Currency.Code);
                }

                if (parameters.SortBy.Equals("Date", StringComparison.InvariantCultureIgnoreCase))
                {
                    queryableTransfers = queryableTransfers.OrderBy(t => t.CreatedOn);
                }
            }

            if (!string.IsNullOrEmpty(parameters.SortOrder))
            {
                if (parameters.SortOrder.Equals("Descending", StringComparison.InvariantCultureIgnoreCase))
                {
                    queryableTransfers = queryableTransfers.Reverse();
                }
            }

            return queryableTransfers;
        }

        public IEnumerable<Transfer> GetTransfers()
        {
            return GetQueryableTransfers().ToList();
        }

        public IEnumerable<Transfer> GetUserTransfers(int userId)
        {
            return GetQueryableTransfers().Where(t => t.WalletId == userId);
        }

        public IEnumerable<Transfer> GetUserTransfers(int userId, QueryParams parameters)
        {
            var transfers = GetQueryableTransfers().Where(t => t.WalletId == userId);

            return SortTransfers(transfers, parameters);
        }

        public Transfer GetTransferById(int transferId)
        {
            return GetQueryableTransfers().SingleOrDefault(t => t.Id == transferId);
        }

		public void AddTransfer(Transfer transfer)
        {
            transfer.CreatedOn = DateTime.Now;
            walletDbContext.Transfers.Add(transfer);

            walletDbContext.SaveChanges();
        }

        public void DeleteTransfer(Transfer transfer)
        {
            transfer.DeletedOn = DateTime.Now;
            transfer.IsDeleted = true;
            walletDbContext.SaveChanges();
        }
    }
}