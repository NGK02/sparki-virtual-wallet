using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.Business.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            this.currencyRepository = currencyRepository;
        }

        public Currency GetCurrencyByCode(string currencyCode)
        {
            if (!Enum.TryParse<CurrencyCode>(currencyCode.ToUpper(), out var code))
            {
                throw new ArgumentException("Invalid currency code.");
            }

            var currency = currencyRepository.GetCurrencyByCode(code);

            if (currency == null)
            {
                throw new EntityNotFoundException("Requested currency not found.");
            }

            return currency;
        }

        public Currency GetCurrencyById(int currencyId)
        {
            var currency = currencyRepository.GetCurrencyById(currencyId);

            if (currency == null)
            {
                throw new EntityNotFoundException("Requested currency not found.");
            }

            return currency;
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            return currencyRepository.GetCurrencies();
        }
    }
}