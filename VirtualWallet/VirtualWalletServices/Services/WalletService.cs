using System.Transactions;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.Dto.ExchangeDto;

namespace VirtualWallet.Business.Services
{
    public class WalletService : IWalletService
    {
        private readonly ICurrencyService currencyService;
        private readonly IExchangeService exchangeService;
        private readonly IUserService userService;
        private readonly IWalletRepository walletRepository;

        public WalletService(ICurrencyService currencyService, IExchangeService exchangeService, IUserService userService, IWalletRepository walletRepository)
        {
            this.currencyService = currencyService;
            this.exchangeService = exchangeService;
            this.userService = userService;
            this.walletRepository = walletRepository;
        }

        public async Task<Exchange> ExchangeFunds(CreateExchangeDto excahngeValues, int userId, int walletId)
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

            var exchangedAmount = await exchangeService.GetExchangeRateAndExchangedResult(excahngeValues.From, excahngeValues.To, excahngeValues.Amount.ToString());

            using (TransactionScope transactionScope = new TransactionScope())
            {
                fromBalance.Amount -= excahngeValues.Amount;
                toBalance.Amount += exchangedAmount.Item2;
                transactionScope.Complete();
            }

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

            exchangeService.CreateExchange(userId, exchange);
            return exchange;
        }

        public Balance CreateWalletBalance(int currencyId, int walletId)
        {
            return walletRepository.CreateWalletBalance(currencyId, walletId);
        }

        public bool ValidateFunds(Wallet wallet, CreateExchangeDto excahngeValues)
        {
            var fromCurrency = currencyService.GetCurrencyByCode(excahngeValues.From);
            var toCurrency = currencyService.GetCurrencyByCode(excahngeValues.To);

            var fromBalance = wallet.Balances.FirstOrDefault(b => b.CurrencyId == fromCurrency.Id);

            if (fromCurrency == toCurrency)
            {
                throw new ArgumentException("Currency exchange cannot be performed between the same currencies.");
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

        public List<Balance> GetWalletBalances(int walletId) 
        {
            return walletRepository.GetWalletBalances(walletId);
        }

		public void DistributeFundsForReferrals(int referrerId, int referredUserId, decimal amount, int currencyId)
		{
            var referrer = userService.GetUserById(referrerId);
			var referrerBalance = referrer.Wallet.Balances.SingleOrDefault(b => b.CurrencyId == currencyId);

			if (referrerBalance == null)
			{
                referrerBalance = CreateWalletBalance(currencyId, referrerId);
			}

			referrerBalance.Amount += amount;
			var referredUser = userService.GetUserById(referredUserId);
			var referredUserBalance = CreateWalletBalance(currencyId, referredUserId);

            referredUserBalance.Amount += amount;
            walletRepository.DistributeFundsForReferrals(referrerBalance, referredUserBalance);
		}

        public Wallet GetWalletById(int walletId)
        {
            var wallet = walletRepository.GetWalletById(walletId);

            if (wallet == null)
            {
                throw new EntityNotFoundException("Requested wallet not found.");
            }

            return wallet;
        }
    }
}