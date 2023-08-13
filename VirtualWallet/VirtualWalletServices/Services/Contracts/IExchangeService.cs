using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services.Contracts
{
	public interface IExchangeService
	{
		bool AddExchange(int userId, Exchange exchange);

		IEnumerable<Exchange> GetUserExchanges(int userId,QueryParameters parameters);

        /// <summary>
        /// Retruns conversion rate.
        /// </summary>
        Task<decimal> GetExchangeRate(string from, string to);

        /// <summary>
        /// Retruns conversion rate and the exchanged result in a Tuple<conversionRate,conversionResult>.
        /// </summary>
        Task<Tuple<decimal, decimal>> GetExchangeRateAndExchangedResult(string from, string to, string amount);

        /// <summary>
        /// Retruns conversion rate and the exchanged result in a Tuple<conversionRate,conversionResult>.
        /// </summary>
        Task<Tuple<decimal, decimal>> GetExchangeRateAndExchangedResult(CurrencyCode fromCurr, CurrencyCode toCurr, decimal amount);

        Task<Dictionary<string, decimal>> GetAllExchangeRates(CurrencyCode forCurr);

        Dictionary<string, decimal> GetExchangeRatesFromCache(CurrencyCode forCurr);

        decimal CalculateExchangeResult(Dictionary<string, decimal> conversionRates, CurrencyCode fromCurr, decimal amount);
    }
}
