using AutoMapper.Execution;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.Dto.CreateExcahngeDto;

namespace VirtualWallet.Business.Services
{
    public class WalletService : IWalletService
    {
        private readonly IAuthManager authManager;
        private readonly IUserService userService;
        private readonly IWalletRepository walletRepository;
        private readonly ICurrencyService currencyService;
        private readonly IExchangeService exchangeService;


		public WalletService(IAuthManager authManager,
            IUserService userService,
            IWalletRepository walletRepository,
            ICurrencyService currencyService,
            IExchangeService exchangeService)
        {
            this.authManager = authManager;
            this.userService = userService;
            this.walletRepository = walletRepository;
            this.currencyService = currencyService;
            this.exchangeService = exchangeService;
        }

        public IEnumerable<Wallet> GetWallets(int userId)
        {
            var user = userService.GetUserById(userId);

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

        public void AddWallet(int userId, Wallet wallet)
        {
            var user = userService.GetUserById(userId);

            if (walletRepository.WalletOwnerExists(user.Id))
            {
                throw new ArgumentException("Wallet for the given user already exists.");
            }

            wallet.User = user;
            wallet.UserId = user.Id;
            walletRepository.AddWallet(wallet);
        }

        public void DeleteWallet(int walletId, int userId)
        {
            var walletToDelete = GetWalletById(walletId, userId);
            //walletRepository.DeleteWallet(walletToDelete);
        }

        // this does not count as a transaction!!
        // a transaction is user to user
        // this is a single user exchanging their funds - don't add transaction to db
        public async Task<Exchange> ExchangeFunds(CreateExcahngeDto excahngeValues, int walletId, int userId)
        {
            var wallet = GetWalletById(walletId, userId);

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
            var exchangedAmount = await exchangeService
				.GetExchangeRateAndExchangedResult(excahngeValues.From,excahngeValues.To,excahngeValues.Amount.ToString());

            toBalance.Amount += exchangedAmount.Item2;

            UpdateWallet(walletId, userId, wallet);

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
            exchangeService.AddExchange(userId, exchange);
            return exchange;
        }

        public void UpdateWallet(int userId, int walletId, Wallet wallet)
        {
            var walletToUpdate = GetWalletById(walletId, userId);
            walletRepository.UpdateWallet(wallet, walletToUpdate);
        }

        public Wallet GetWalletById(int walletId, int userId)
        {
            var wallet = walletRepository.GetWalletById(walletId);

            if (wallet == null)
            {
                throw new EntityNotFoundException($"Wallet with ID {walletId} not found.");
            }

            var user = userService.GetUserById(userId);

            if (!authManager.IsAdmin(user) && user.Id != wallet.UserId)
            {
                throw new UnauthorizedOperationException("Only an admin or the wallet's owner can access wallet details.");
            }

            return wallet;
        }

        // do not call directly from controller!!
        // gets called from ExchangeFunds which is the public 'gateway'
  //      public async Task<decimal> ExchangeCurrencyAsync(User user,CreateExcahngeDto excahngeValues)
  //      {
  //          var fromCurrency = currencyService.GetCurrencyByCode(excahngeValues.From.ToUpper());
		//	var toCurrency = currencyService.GetCurrencyByCode(excahngeValues.To.ToUpper());

  //          decimal rate = await exchangeService
  //              .GetExchangeRate(excahngeValues.From.ToUpper(), excahngeValues.To.ToUpper());

  //          decimal newAmount = excahngeValues.Amount * rate;

  //          return newAmount;

		//}

        public Balance CreateWalletBalance(int walletId, int currencyId)
        {
            return walletRepository.CreateWalletBalance(walletId, currencyId);
        }
	}
}