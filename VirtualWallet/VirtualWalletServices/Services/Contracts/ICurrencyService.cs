using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface ICurrencyService
    {
        /// <summary>
        /// Retrieves a currency by its currency code.
        /// </summary>
        /// <param name="currencyCode">The code of the currency to retrieve.</param>
        /// <returns>The currency with the specified code if found, otherwise null.</returns>
        Currency GetCurrencyByCode(string currencyCode);

        /// <summary>
        /// Retrieves a currency by its currency ID.
        /// </summary>
        /// <param name="currencyId">The ID of the currency to retrieve.</param>
        /// <returns>The currency with the specified ID if found, otherwise null.</returns>
        Currency GetCurrencyById(int currencyId);

        /// <summary>
        /// Retrieves all available currencies.
        /// </summary>
        /// <returns>An enumerable collection of all available currencies.</returns>
        IEnumerable<Currency> GetCurrencies();
    }
}