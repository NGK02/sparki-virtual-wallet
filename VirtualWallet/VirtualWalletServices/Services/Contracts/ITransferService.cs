﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface ITransferService
    {
        IEnumerable<Transfer> GetTransfers(int userId);

        IEnumerable<Transfer> GetWalletTransfers(int userId);

        Transfer GetTransferById(int transferId, int userId);

        void AddTransfer(Transfer transfer);

        void DeleteTransfer(int transferId, int userId);
    }
}