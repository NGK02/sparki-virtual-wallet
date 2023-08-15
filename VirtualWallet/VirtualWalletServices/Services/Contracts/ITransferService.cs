using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface ITransferService
    {
        IEnumerable<Transfer> GetTransfers(int userId);

        IEnumerable<Transfer> GetUserTransfers(int userId);

        IEnumerable<Transfer> GetUserTransfers(int userId, QueryParams parameters);

        Transfer GetTransferById(int transferId, int userId);

        void AddTransfer(int userId, Transfer transfer);

		void DeleteTransfer(int transferId, int userId);
    }
}