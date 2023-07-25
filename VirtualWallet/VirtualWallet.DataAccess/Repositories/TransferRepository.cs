using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.DataAccess.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly IQueryable<Transfer> transfers;
        private readonly WalletDbContext walletDbContext;

        public TransferRepository(WalletDbContext walletDbContext)
        {
            this.walletDbContext = walletDbContext;
            transfers = GetQueryableTransfers();
        }

        private IQueryable<Transfer> GetQueryableTransfers()
        {
            return walletDbContext.Transfers
                .Where(t => !t.IsDeleted)
                .Include(t => t.Card)
                .Include(t => t.Currency)
                .Include(t => t.Wallet);
        }

        public IEnumerable<Transfer> GetTransfers()
        {
            return transfers.ToList();
        }

        public IEnumerable<Transfer> GetWalletTransfers(int walletId)
        {
            return transfers.Where(t => t.WalletId == walletId).ToList();
        }

        public Transfer GetTransferById(int transferId)
        {
            return transfers.SingleOrDefault(t => t.Id == transferId);
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