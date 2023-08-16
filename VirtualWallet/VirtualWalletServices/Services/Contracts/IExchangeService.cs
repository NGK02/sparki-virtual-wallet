using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface IExchangeService
	{
        /// <summary>
        /// Creates an exchange record for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user initiating the exchange.</param>
        /// <param name="exchange">The exchange object containing details of the exchange.</param>
        /// <returns>True if the exchange was successfully created, otherwise false.</returns>
        bool CreateExchange(int userId, Exchange exchange);

        /// <summary>
        /// Retrieves cached exchange rates for the specified target currency.
        /// </summary>
        /// <param name="forCurr">The currency code to retrieve exchange rates for.</param>
        /// <returns>A dictionary of currency codes and their respective conversion rates.</returns>
        Dictionary<string, decimal> GetExchangeRatesFromCache(CurrencyCode forCurr);

        /// <summary>
        /// Calculates the result of an exchange using provided conversion rates.
        /// </summary>
        /// <param name="conversionRates">Dictionary of currency codes and their conversion rates.</param>
        /// <param name="fromCurr">The source currency code.</param>
        /// <param name="amount">The amount to exchange.</param>
        /// <returns>The converted amount after the exchange.</returns>
        decimal CalculateExchangeResult(Dictionary<string, decimal> conversionRates, CurrencyCode fromCurr, decimal amount);

        /// <summary>
        /// Retrieves exchange records for the specified user using optional query parameters.
        /// </summary>
        /// <param name="userId">The ID of the user whose exchanges are being retrieved.</param>
        /// <param name="parameters">Optional query parameters to filter or modify the results.</param>
        /// <returns>An enumerable collection of exchange records based on the specified criteria.</returns>
        IEnumerable<Exchange> GetUserExchanges(int userId, QueryParams parameters);

        /// <summary>
        /// Retrieves all exchange rates for the specified target currency asynchronously.
        /// </summary>
        /// <param name="forCurr">The currency code to retrieve exchange rates for.</param>
        /// <returns>A task representing the asynchronous operation with a dictionary of currency codes and their respective conversion rates.</returns>
        Task<Dictionary<string, decimal>> GetAllExchangeRates(CurrencyCode forCurr);

        /// <summary>
        /// Retrieves the exchange rate between two currencies asynchronously.
        /// </summary>
        /// <param name="from">The source currency code.</param>
        /// <param name="to">The target currency code.</param>
        /// <returns>A task representing the asynchronous operation with the exchange rate.</returns>
        Task<decimal> GetExchangeRate(string from, string to);

        /// <summary>
        /// Retrieves the exchange rate and the result of an exchange between two currencies asynchronously.
        /// </summary>
        /// <param name="fromCurr">The source currency code.</param>
        /// <param name="toCurr">The target currency code.</param>
        /// <param name="amount">The amount to exchange.</param>
        /// <returns>A task representing the asynchronous operation with a tuple containing the exchange rate and the exchanged result.</returns>
        Task<Tuple<decimal, decimal>> GetExchangeRateAndExchangedResult(CurrencyCode fromCurr, CurrencyCode toCurr, decimal amount);

        /// <summary>
        /// Retrieves the exchange rate and the result of an exchange between two currencies asynchronously.
        /// </summary>
        /// <param name="from">The source currency code.</param>
        /// <param name="to">The target currency code.</param>
        /// <param name="amount">The amount to exchange.</param>
        /// <returns>A task representing the asynchronous operation with a tuple containing the exchange rate and the exchanged result.</returns>
        Task<Tuple<decimal, decimal>> GetExchangeRateAndExchangedResult(string from, string to, string amount);
    }
}