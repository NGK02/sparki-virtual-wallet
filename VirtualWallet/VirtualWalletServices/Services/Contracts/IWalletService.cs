using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.CreateExcahngeDto;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface IWalletService
    {
        IEnumerable<Wallet> GetWallets(int userId);

        void AddWallet(int userId, Wallet wallet);

        //void AddWalletDeposit(int userId, Transfer walletDeposit);

        //void AddWalletWithdrawal(int userId, Transfer walletWithdrawal);

        void UpdateWallet(int userId, int walletId, Wallet wallet);

        Task<Exchange> ExchangeFunds(CreateExcahngeDto excahngeValues, int walletId, int userId);

        Wallet GetWalletById(int walletId, int userId);

        Balance CreateWalletBalance(int walletId, int currencyId);

		Task<decimal> ExchangeCurrencyAsync(User user, CreateExcahngeDto excahngeValues);

	}
}