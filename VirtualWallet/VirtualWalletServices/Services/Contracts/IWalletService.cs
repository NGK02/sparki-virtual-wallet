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
        IEnumerable<Wallet> GetWallets(int userId);

        void AddWallet(int userId, Wallet wallet);

        void AddWalletDeposit(int userId, Transfer walletDeposit);

        void AddWalletWithdrawal(int userId, Transfer walletWithdrawal);

        void UpdateWallet(int walletId, string username, Wallet wallet);

        Task<Wallet> ExchangeFunds(ExcahngeDTO excahngeValues, int walletId, int userId);

        Wallet GetWalletById(int walletId, int userId);

		//Task<decimal> ExchangeCurrencyAsync(User user, ExcahngeDTO excahngeValues);

	}
}