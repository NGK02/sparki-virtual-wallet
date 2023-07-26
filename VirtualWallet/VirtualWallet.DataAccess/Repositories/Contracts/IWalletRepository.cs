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
        bool WalletOwnerExists(int userId);

        IEnumerable<Wallet> GetWallets();

        void AddWallet(Wallet wallet);

        void UpdateWallet(Wallet wallet, Wallet walletToUpdate);

        Wallet GetWalletById(int walletId);
    }
}