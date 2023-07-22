using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.Dto.CardDto;

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
            if (!Enum.TryParse<CurrencyCode>(currencyCode, out var code))
            {
                throw new ArgumentException("Invalid currency code.");
            }

            var currency = currencyRepository.GetCurrencyByCode(code);

            if (currency == null)
            {
                throw new EntityNotFoundException("Currency with the provided code was not found in the database.");
            }

            return currency;
        }
    }
}