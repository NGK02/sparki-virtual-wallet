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

        public IEnumerable<Transfer> GetTransfers(string username)
        {
            var user = userService.GetUserByUsername(username);

            if (!authManager.IsAdmin(user))
            {
                throw new UnauthorizedOperationException("Only admins can access all transfers.");
            }

            var transfers = transferRepository.GetTransfers();

            if (!transfers.Any() || transfers == null)
            {
                throw new EntityNotFoundException("No transfers found.");
            }

            return transfers;
        }

        public IEnumerable<Transfer> GetWalletTransfers(string username)
        {
            var user = userService.GetUserByUsername(username);

            var transfers = transferRepository.GetWalletTransfers(user.WalletId);

            if (!transfers.Any() || transfers == null)
            {
                throw new EntityNotFoundException("No transfers found.");
            }

            return transfers;
        }

        public Transfer GetTransferById(int transferId, string username)
        {
            var transfer = transferRepository.GetTransferById(transferId);

            if (transfer == null)
            {
                throw new EntityNotFoundException($"Transfer with ID {transferId} not found.");
            }

            var user = userService.GetUserByUsername(username);

            if (!authManager.IsAdmin(user) && transfer.Wallet.UserId != user.Id)
            {
                throw new UnauthorizedOperationException("Only an admin or the transfer's sender can access transfer details.");
            }

            return transfer;
        }

        public void AddTransfer(string username, Transfer transfer)
        {
            var user = userService.GetUserByUsername(username);

            transferRepository.AddTransfer(transfer);
        }

        public void DeleteTransfer(int transferId, string username)
        {
            var user = userService.GetUserByUsername(username);

            if (!authManager.IsAdmin(user))
            {
                throw new UnauthorizedOperationException("Only an admin can delete transfer details.");
            }

            var transferToDelete = GetTransferById(transferId, username);

            transferRepository.DeleteTransfer(transferToDelete);
        }
    }
}