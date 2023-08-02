using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.DataAccess.Repositories.Contracts
{
    public interface ICurrencyRepository
    {
        Currency GetCurrencyByCode(CurrencyCode currencyCode);

        Currency GetCurrencyById(int currencyId);

        IEnumerable<Currency> GetCurrencies();
    }
}