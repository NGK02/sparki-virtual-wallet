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

        public bool WalletOwnerExists(int userId)
        {
            return walletDbContext.Wallets.Any(w => !w.IsDeleted && userId == w.UserId);
        }

        private IQueryable<Wallet> GetWalletsQueryable()
        {
            return walletDbContext.Wallets
                .Where(w => !w.IsDeleted)
                .Include(w => w.Balances)
                .Include(w => w.Transfers)
                .Include(w => w.User);
        }

        public void AddWallet(Wallet wallet)
        {
            wallet.CreatedOn = DateTime.Now;
            walletDbContext.Wallets.Add(wallet);

            walletDbContext.SaveChanges();
        }

        //public void DeleteWallet(Wallet wallet)
        //{
        //    wallet.DeletedOn = DateTime.Now;
        //    wallet.IsDeleted = true;
        //    walletDbContext.SaveChanges();
        //}

        public void UpdateWallet(Wallet wallet, Wallet walletToUpdate)
        {
            walletToUpdate.Balances = wallet.Balances;

            walletDbContext.SaveChanges();
        }

        public Wallet GetWalletById(int walletId)
        {
            return GetWalletsQueryable().FirstOrDefault(w => w.Id == walletId);
        }

		public IEnumerable<Wallet> GetWallets()
		{
			return GetWalletsQueryable().ToList();
		}

		public Balance CreateWalletBalance(int walletId, int currencyId)
		{
			var balance = new Balance { WalletId = walletId, CurrencyId = currencyId };
			walletDbContext.Balances.Add(balance);
			walletDbContext.SaveChanges();
			return balance;
		}
	}
}