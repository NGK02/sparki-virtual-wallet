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
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletDbContext walletDbContext;

        public WalletRepository(WalletDbContext walletDbContext)
        {
            this.walletDbContext = walletDbContext;
        }

        private IQueryable<Wallet> GetQueryableWallets()
        {
            return walletDbContext.Wallets
                .Where(w => !w.IsDeleted)
                .Include(w => w.Balances)
                .Include(w => w.Transfers)
                .Include(w => w.User);
        }

        public Balance CreateWalletBalance(int currencyId, int walletId)
        {
            var balance = new Balance { CurrencyId = currencyId, WalletId = walletId };
            walletDbContext.Balances.Add(balance);

            return balance;
        }

        public IEnumerable<Wallet> GetWallets()
        {
            return GetQueryableWallets().ToList();
        }

        public Wallet GetWalletById(int walletId)
        {
            return GetQueryableWallets().FirstOrDefault(w => w.Id == walletId);
        }
	}
}