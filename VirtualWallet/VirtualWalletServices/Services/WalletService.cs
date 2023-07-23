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
    public class WalletService : IWalletService
    {
        private readonly IAuthManager authManager;
        private readonly IUserService userService;
        private readonly IWalletRepository walletRepository;

        public WalletService(IAuthManager authManager, IUserService userService, IWalletRepository walletRepository)
        {
            this.authManager = authManager;
            this.userService = userService;
            this.walletRepository = walletRepository;
        }

        public Wallet GetWalletById(int walletId, string username)
        {
            var wallet = walletRepository.GetWalletById(walletId);

            if (wallet == null)
            {
                throw new EntityNotFoundException($"Wallet with ID {walletId} not found.");
            }

            var user = userService.GetUserByUsername(username);

            if (!authManager.IsAdmin(user) && user.Id != wallet.UserId)
            {
                throw new UnauthorizedOperationException("Only an admin or the wallet's owner can access wallet details.");
            }

            return wallet;
        }
    }
}