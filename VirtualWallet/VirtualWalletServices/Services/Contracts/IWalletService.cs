using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

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

        Wallet GetWalletById(int walletId, string username);
    }
}