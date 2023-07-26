using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.Business.Services
{
    public class TransferService : ITransferService
    {
        private readonly IAuthManager authManager;
        private readonly ITransferRepository transferRepository;
        private readonly IUserService userService;

        public TransferService(IAuthManager authManager, ITransferRepository transferRepository, IUserService userService)
        {
            this.authManager = authManager;
            this.transferRepository = transferRepository;
            this.userService = userService;
        }

        public IEnumerable<Transfer> GetTransfers(int userId)
        {
            var user = userService.GetUserById(userId);

            if (!authManager.IsAdmin(user))
            {
                throw new UnauthorizedOperationException("Access to all transfers is restricted.");
            }

            var transfers = transferRepository.GetTransfers();

            if (!transfers.Any() || transfers == null)
            {
                throw new EntityNotFoundException("No transfers available.");
            }

            return transfers;
        }

        public IEnumerable<Transfer> GetUserTransfers(int userId)
        {
            var user = userService.GetUserById(userId);

            var transfers = transferRepository.GetUserTransfers(user.WalletId);

            if (!transfers.Any() || transfers == null)
            {
                throw new EntityNotFoundException("No transfers available.");
            }

            return transfers;
        }

        public Transfer GetTransferById(int transferId, int userId)
        {
            var transfer = transferRepository.GetTransferById(transferId);

            if (transfer == null)
            {
                throw new EntityNotFoundException("Requested transfer not found.");
            }

            var user = userService.GetUserById(userId);

            if (!authManager.IsAdmin(user) && transfer.Wallet.UserId != userId)
            {
                throw new UnauthorizedOperationException("Access to transfer denied.");
            }

            return transfer;
        }

        public void AddTransfer(int userId, Transfer transfer)
        {
            var user = userService.GetUserById(userId);

            transferRepository.AddTransfer(transfer);
        }

        public void DeleteTransfer(int transferId, int userId)
        {
            var user = userService.GetUserById(userId);

            if (!authManager.IsAdmin(user))
            {
                throw new UnauthorizedOperationException("Insufficient privileges to delete transfer details.");
            }

            var transferToDelete = GetTransferById(transferId, userId);

            transferRepository.DeleteTransfer(transferToDelete);
        }
    }
}