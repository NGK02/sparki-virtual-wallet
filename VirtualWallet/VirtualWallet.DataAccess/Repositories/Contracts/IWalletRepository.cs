using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.DataAccess.Repositories.Contracts
{
    public interface IWalletRepository
    {
        Balance CreateWalletBalance(int currencyId, int walletId);

        IEnumerable<Wallet> GetWallets();

        Wallet GetWalletById(int walletId);

		void DistributeFundsForReferrals(Balance refererBalance, Balance referredUserBalance);
	}
}