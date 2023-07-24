﻿using Microsoft.EntityFrameworkCore;
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
        private readonly WalletDbContext walletDbContext;

        public TransferRepository(WalletDbContext walletDbContext)
        {
            this.walletDbContext = walletDbContext;
        }

        public IEnumerable<Transfer> GetTransfers()
        {
            return walletDbContext.Transfers
                .Where(t => !t.IsDeleted)
                .Include(t => t.Card)
                .Include(t => t.Currency)
                .Include(t => t.Wallet)
                .ToList();
        }

        public IEnumerable<Transfer> GetWalletTransfers(int walletId)
        {
            return walletDbContext.Transfers
                .Where(t => !t.IsDeleted && t.WalletId == walletId)
                .Include(t => t.Card)
                .Include(t => t.Currency)
                .Include(t => t.Wallet)
                .ToList();
        }

        public Transfer GetTransferById(int transferId)
        {
            return walletDbContext.Transfers
                .Include(t => t.Card)
                .Include(t => t.Currency)
                .Include(t => t.Wallet)
                .SingleOrDefault(t => !t.IsDeleted && t.Id == transferId);
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