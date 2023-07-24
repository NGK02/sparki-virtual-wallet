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
        private readonly ITransferService transferService;
        private readonly IUserService userService;
        private readonly IWalletRepository walletRepository;

        public WalletService(IAuthManager authManager, ITransferService transferService, IUserService userService, IWalletRepository walletRepository)
        {
            this.authManager = authManager;
            this.transferService = transferService;
            this.userService = userService;
            this.walletRepository = walletRepository;
        }

        public IEnumerable<Wallet> GetWallets(string username)
        {
            var user = userService.GetUserByUsername(username);

            if (!authManager.IsAdmin(user))
            {
                throw new UnauthorizedOperationException("Only admins can access all wallets.");
            }

            var wallets = walletRepository.GetWallets();

            if (!wallets.Any() || wallets == null)
            {
                throw new EntityNotFoundException("No wallets found.");
            }

            return wallets;
        }

        public void AddWallet(string username, Wallet wallet)
        {
            var user = userService.GetUserByUsername(username);

            if (walletRepository.WalletOwnerExists(user.Id))
            {
                throw new ArgumentException("Wallet for the given user already exists.");
            }

            wallet.User = user;
            wallet.UserId = user.Id;
            walletRepository.AddWallet(wallet);
        }

        public void AddWalletDeposit(string username, Transfer walletDeposit)
        {
            var wallet = GetWalletById(walletDeposit.WalletId, username);

            var depositBalance = wallet.Balances.SingleOrDefault(b => b.CurrencyId == walletDeposit.CurrencyId);

            if (depositBalance == null)
            {
                depositBalance = new Balance { CurrencyId = walletDeposit.CurrencyId };
                wallet.Balances.Add(depositBalance);
            }

            depositBalance.Amount += walletDeposit.Amount;
            UpdateWallet(walletDeposit.WalletId, username, wallet);
            walletDeposit.IsCardSender = true;

            transferService.AddTransfer(username, walletDeposit);
        }

        public void AddWalletWithdrawal(string username, Transfer walletWithdrawal)
        {
            var wallet = GetWalletById(walletWithdrawal.WalletId, username);
            var withdrawalBalance = wallet.Balances.SingleOrDefault(b => b.CurrencyId == walletWithdrawal.CurrencyId);

            if (withdrawalBalance == null)
            {
                withdrawalBalance = new Balance { CurrencyId = walletWithdrawal.CurrencyId };

                wallet.Balances.Add(withdrawalBalance);
            }

            decimal amountToWithdraw = walletWithdrawal.Amount;
            decimal availableBalance = withdrawalBalance.Amount;

            if (amountToWithdraw > availableBalance)
            {
                throw new InsufficientFundsException($"Insufficient funds. Available balance: {availableBalance} {withdrawalBalance.Currency.Code}");
            }

            withdrawalBalance.Amount -= walletWithdrawal.Amount;

            UpdateWallet(walletWithdrawal.WalletId, username, wallet);
            walletWithdrawal.IsCardSender = false;

            transferService.AddTransfer(username, walletWithdrawal);
        }

        public void DeleteWallet(int walletId, string username)
        {
            var walletToDelete = GetWalletById(walletId, username);
            walletRepository.DeleteWallet(walletToDelete);
        }

        public void UpdateWallet(int walletId, string username, Wallet wallet)
        {
            var walletToUpdate = GetWalletById(walletId, username);
            walletRepository.UpdateWallet(wallet, walletToUpdate);
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