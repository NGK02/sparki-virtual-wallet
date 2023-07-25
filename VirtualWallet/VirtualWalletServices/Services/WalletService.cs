using AutoMapper.Execution;
using Newtonsoft.Json.Linq;
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
using VirtualWallet.Dto.ExchangeDto;

namespace VirtualWallet.Business.Services
{
    public class WalletService : IWalletService
    {
        private readonly IAuthManager authManager;
        private readonly ITransferService transferService;
        private readonly IUserService userService;
        private readonly IWalletRepository walletRepository;
        private readonly ICurrencyService currencyService;
        private readonly CurrencyExchangeService currencyExchangeService;
        private readonly IWalletTransactionService walletTransactionService;
        private readonly IExchangeService exchangeService;


		public WalletService(IAuthManager authManager,
            ITransferService transferService,
            IUserService userService,
            IWalletRepository walletRepository,
            ICurrencyService currencyService,
            CurrencyExchangeService currencyExchangeService,
            IWalletTransactionService walletTransactionService,
            IExchangeService exchangeService)
        {
            this.authManager = authManager;
            this.transferService = transferService;
            this.userService = userService;
            this.walletRepository = walletRepository;
            this.currencyService = currencyService;
            this.currencyExchangeService = currencyExchangeService;
            this.walletTransactionService = walletTransactionService;
            this.exchangeService = exchangeService;
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

        // this does not count as a transaction!!
        // a transaction is user to user
        // this is a single user exchanging their funds - don't add transaction to db
        public Wallet ExchangeFunds(ExcahngeDTO excahngeValues, int walletId, string username)
        {
            var wallet = GetWalletById(walletId, username);

            var fromCurrency = currencyService.GetCurrencyByCode(excahngeValues.From.ToUpper());
            var toCurrency = currencyService.GetCurrencyByCode(excahngeValues.To.ToUpper());

            var fromBalance = wallet.Balances.SingleOrDefault(b => b.CurrencyId == fromCurrency.Id);

            // tova e taka samo dokato go izmislq lol
            if (fromBalance == null)
            {
                // this is a temporary exception - need a proper one
                throw new ArgumentException($"You cannot transfer {excahngeValues.Amount} {fromCurrency.Code} because you do not have such balance");
                // maybe prompt user to create a new balance and transfer money to it from another balance that has sufficient funds
            }

            var toBalance = wallet.Balances.SingleOrDefault(b => b.CurrencyId == toCurrency.Id);

            if (toBalance == null)
            {
                toBalance = new Balance { CurrencyId = toCurrency.Id };

                wallet.Balances.Add(toBalance);
            }

            // proceed to make the exchange only if there's sufficient funding
            if (fromBalance.Amount < excahngeValues.Amount)
            {
                throw new InsufficientFundsException($"Insufficient funds. Available balance: {fromBalance.Amount} {fromBalance.Currency.Code}");
            }

            fromBalance.Amount -= excahngeValues.Amount;
            var exchangedAmount = currencyExchangeService.GetExchangeRateAndExchangedResult(excahngeValues.From,excahngeValues.To,excahngeValues.Amount.ToString()).Result;

            toBalance.Amount += exchangedAmount.Item2;

            UpdateWallet(walletId, username, wallet);

            var exchange = new Exchange 
            { 
                FromCurrencyId= fromCurrency.Id,
                FromCurrency = fromCurrency,
                ToCurrencyId= toCurrency.Id,
                ToCurrency = toCurrency,
                Amount = excahngeValues.Amount,
                ExchangedAmout=exchangedAmount.Item2,
                Wallet=wallet,
                Rate= exchangedAmount.Item1
			};
            exchangeService.AddExchange(username, exchange);
            return wallet;
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

        // do not call directly from controller!!
        // gets called from ExchangeFunds which is the public 'gateway'
        public async Task<decimal> ExchangeCurrencyAsync(User user,ExcahngeDTO excahngeValues)
        {
            var fromCurrency = currencyService.GetCurrencyByCode(excahngeValues.From.ToUpper());
			var toCurrency = currencyService.GetCurrencyByCode(excahngeValues.To.ToUpper());

            decimal rate = await currencyExchangeService.GetExchangeRate(excahngeValues.From.ToUpper(), excahngeValues.To.ToUpper());

            decimal newAmount = excahngeValues.Amount * rate;

            return newAmount;

		}
	}
}