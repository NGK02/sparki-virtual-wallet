using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface ITransferService
    {
        IEnumerable<Transfer> GetTransfers(string username);

        IEnumerable<Transfer> GetWalletTransfers(string username);

        Transfer GetTransferById(int transferId, string username);

        void AddTransfer(string username, Transfer transfer);

        void DeleteTransfer(int transferId, string username);
    }
}