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
using VirtualWallet.DataAccess;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.Dto.CreateExcahngeDto;

namespace VirtualWallet.Business.Services
{
    public class WalletService : IWalletService
    {
        private readonly IAuthManager authManager;
        private readonly ICurrencyService currencyService;
        private readonly IExchangeService exchangeService;
        private readonly IUserService userService;
        private readonly IWalletRepository walletRepository;

        public WalletService(IAuthManager authManager, ICurrencyService currencyService, IExchangeService exchangeService, IUserService userService, IWalletRepository walletRepository)
        {
            this.authManager = authManager;
            this.currencyService = currencyService;
            this.exchangeService = exchangeService;
            this.userService = userService;
            this.walletRepository = walletRepository;
        }

        public Balance CreateWalletBalance(int currencyId, int walletId)
        {
            return walletRepository.CreateWalletBalance(currencyId, walletId);
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

        public bool ValidateFunds(Wallet wallet, CreateExcahngeDto excahngeValues)
        {
            var fromCurrency = currencyService.GetCurrencyByCode(excahngeValues.From);
            var toCurrency = currencyService.GetCurrencyByCode(excahngeValues.To);
            var fromBalance = wallet.Balances.FirstOrDefault(b => b.CurrencyId == fromCurrency.Id);
            if (fromCurrency == toCurrency)
            {
                throw new ArgumentException("Cannot swap between the same currency!");
            }
            if (fromBalance == null)
            {
                throw new InsufficientFundsException($"Cannot make exchange. No balance with currency '{fromCurrency.Code}'.");
            }
            if (excahngeValues.Amount > fromBalance.Amount)
            {
                throw new InsufficientFundsException($"Insufficient funds. Available balance: {fromBalance.Amount} {fromCurrency.Code}.");
            }
            return true;
        }

        public async Task<Exchange> ExchangeFunds(CreateExcahngeDto excahngeValues, int userId, int walletId)
        {
            var wallet = GetWalletById(walletId);

            var fromCurrency = currencyService.GetCurrencyByCode(excahngeValues.From);
            var toCurrency = currencyService.GetCurrencyByCode(excahngeValues.To);

            var fromBalance = wallet.Balances.SingleOrDefault(b => b.CurrencyId == fromCurrency.Id);

            var toBalance = wallet.Balances.SingleOrDefault(b => b.CurrencyId == toCurrency.Id);

            if (toBalance == null)
            {
                toBalance = CreateWalletBalance(toCurrency.Id, walletId);
            }
            fromBalance.Amount -= excahngeValues.Amount;
            var exchangedAmount = await exchangeService.GetExchangeRateAndExchangedResult(excahngeValues.From, excahngeValues.To, excahngeValues.Amount.ToString());

            toBalance.Amount += exchangedAmount.Item2;

            var exchange = new Exchange
            {
                Amount = excahngeValues.Amount,
                ExchangedAmout = exchangedAmount.Item2,
                FromCurrency = fromCurrency,
                FromCurrencyId = fromCurrency.Id,
                Rate = exchangedAmount.Item1,
                ToCurrency = toCurrency,
                ToCurrencyId = toCurrency.Id,
                Wallet = wallet
            };

            exchangeService.AddExchange(userId, exchange);
            return exchange;
        }

        public Wallet GetWalletById(int walletId)
        {
            var wallet = walletRepository.GetWalletById(walletId);

            if (wallet == null)
            {
                throw new EntityNotFoundException($"Wallet with ID {walletId} not found.");
            }

            //var user = userService.GetUserById(userId);

            //if (!authManager.IsAdmin(user) && user.Id != wallet.UserId)
            //{
            //    throw new UnauthorizedOperationException("Only an admin or the wallet's owner can access wallet details.");
            //}

            return wallet;
        }
    }
}