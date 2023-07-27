using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.DataAccess.Repositories.Contracts
{
    public interface ITransferRepository
    {
        IEnumerable<Transfer> GetTransfers();

        IEnumerable<Transfer> GetWalletTransfers(int walletId);

        Transfer GetTransferById(int transferId);

        void AddTransfer(Transfer transfer);

        void DeleteTransfer(Transfer transfer);
	}
}