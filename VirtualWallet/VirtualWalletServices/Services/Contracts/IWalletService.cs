using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.ExchangeDto;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface IWalletService
    {
        Balance CreateWalletBalance(int currencyId, int walletId);

        //IEnumerable<Wallet> GetWallets(int userId);

        // Task<decimal> ExchangeCurrencyAsync(User user, CreateExcahngeDto excahngeValues);

        bool ValidateFunds(Wallet wallet, CreateExchangeDto excahngeValues);

        Task<Exchange> ExchangeFunds(CreateExchangeDto excahngeValues, int userId, int walletId);

        Wallet GetWalletById(int walletId);

        List<Balance> GetWalletBalances(int walletId);

        void DistributeFundsForReferrals(int referrerId, int referredUserId, decimal amount, int currencyId);
	}
}