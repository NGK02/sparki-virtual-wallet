using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.UserDTO;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface IWalletService
    {
        IEnumerable<Wallet> GetWallets(string username);

        void AddWallet(string username, Wallet wallet);

        void AddWalletDeposit(string username, Transfer walletDeposit);

        void AddWalletWithdrawal(string username, Transfer walletWithdrawal);

        void DeleteWallet(int walletId, string username);

        void UpdateWallet(int walletId, string username, Wallet wallet);

        Wallet ExchangeFunds(ExcahngeDTO excahngeValues, int walletId, string username);

        Wallet GetWalletById(int walletId, string username);

		Task<decimal> ExchangeCurrencyAsync(User user, ExcahngeDTO excahngeValues);

	}
}