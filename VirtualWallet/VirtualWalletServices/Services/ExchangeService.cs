using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.Dto.Config;

namespace VirtualWallet.Business.Services
{
    public class ExchangeService : IExchangeService
    {
        private readonly IExchangeRepository exchangeRepository;
        private readonly IMemoryCache cache;
        private readonly IUserService userService;
        private readonly IConfiguration configuration;

        public ExchangeService(IExchangeRepository exchangeRepository,
            IMemoryCache cache,
            IUserService userService,
            IConfiguration configuration)
        {
            this.cache = cache;
            this.exchangeRepository = exchangeRepository;
            this.userService = userService;
            this.configuration = configuration;
        }

        public async Task<Dictionary<string, decimal>> GetAllExchangeRates(CurrencyCode forCurr)
        {
            var forCurrString = forCurr.ToString();

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://www.exchangerate-api.com");
                    //var response = await client.GetAsync($"https://v6.exchangerate-api.com/v6/{keys.Value.ExchangeRateApikey}/latest/{forCurrString}");
                    var response = await client.GetAsync($"https://v6.exchangerate-api.com/v6/{configuration["ApiKeys:ExchangeRateApikey"]}/latest/{forCurrString}");
                    var stringResult = await response.Content.ReadAsStringAsync();

                    var data = JObject.Parse(stringResult);

                    IDictionary<string, JToken> conversionRates = (JObject) data["conversion_rates"];

                    Dictionary<string, decimal> conversionRatesParsed = conversionRates.ToDictionary(pair => pair.Key, pair => (decimal) pair.Value);
                    return conversionRatesParsed;
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.WriteLine(httpRequestException.StackTrace);
                    throw;
                }
            }
        }

        public async Task<decimal> GetExchangeRate(string from, string to)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://www.exchangerate-api.com");
                    //var response = await client.GetAsync($"https://v6.exchangerate-api.com/v6/{keys.Value.ExchangeRateApikey}/pair/{from}/{to}");
                    var response = await client.GetAsync($"https://v6.exchangerate-api.com/v6/{configuration["ApiKeys:ExchangeRateApikey"]}/pair/{from}/{to}");
                    var stringResult = await response.Content.ReadAsStringAsync();

                    JObject jsonObject = JObject.Parse(stringResult);

                    decimal conversionRate = (decimal) jsonObject["conversion_rate"];
                    return conversionRate;
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.WriteLine(httpRequestException.StackTrace);
                    throw;
                }
            }
        }

        public async Task<Tuple<decimal, decimal>> GetExchangeRateAndExchangedResult(CurrencyCode fromCurr, CurrencyCode toCurr, decimal amount)
        {
            var amountString = amount.ToString();
            var fromCurrString = fromCurr.ToString();
            var toCurrString = toCurr.ToString();

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://www.exchangerate-api.com");
                    //var response = await client.GetAsync($"https://v6.exchangerate-api.com/v6/{keys.Value.ExchangeRateApikey}/pair/{fromCurrString}/{toCurrString}/{amountString}");
                    var response = await client.GetAsync($"https://v6.exchangerate-api.com/v6/{configuration["ApiKeys:ExchangeRateApikey"]}/pair/{fromCurrString}/{toCurrString}/{amountString}");
                    var stringResult = await response.Content.ReadAsStringAsync();

                    JObject data = JObject.Parse(stringResult);

                    decimal conversionRate = (decimal) data["conversion_rate"];
                    decimal conversionResult = (decimal) data["conversion_result"];
                    var result = Tuple.Create(conversionRate, conversionResult);

                    return result;
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.WriteLine(httpRequestException.StackTrace);
                    throw;
                }
            }
        }

        public async Task<Tuple<decimal, decimal>> GetExchangeRateAndExchangedResult(string from, string to, string amount)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://www.exchangerate-api.com");
                    //var response = await client.GetAsync($"https://v6.exchangerate-api.com/v6/{keys.Value.ExchangeRateApikey}/pair/{from}/{to}/{amount}");
                    var response = await client.GetAsync($"https://v6.exchangerate-api.com/v6/{configuration["ApiKeys:ExchangeRateApikey"]}/pair/{from}/{to}/{amount}");
                    var stringResult = await response.Content.ReadAsStringAsync();

                    JObject data = JObject.Parse(stringResult);

                    decimal conversionRate = (decimal) data["conversion_rate"];
                    decimal conversionResult = (decimal) data["conversion_result"];
                    var result = Tuple.Create(conversionRate, conversionResult);

                    return result;
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.WriteLine(httpRequestException.StackTrace);
                    throw;
                }
            }
        }

        public bool CreateExchange(int userId, Exchange exchange)
        {
            var user = userService.GetUserById(userId);

            exchange.Wallet = user.Wallet;
            exchangeRepository.AddExchange(exchange);
            return true;
        }

        public Dictionary<string, decimal> GetExchangeRatesFromCache(CurrencyCode forCurr)
        {
            if (cache.TryGetValue(forCurr, out Dictionary<string, decimal> conversionRates))
            {
            }
            else
            {
                conversionRates = GetAllExchangeRates(forCurr).Result;

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(6))
                    .SetPriority(CacheItemPriority.Normal);

                cache.Set(forCurr, conversionRates, cacheOptions);
            }

            return conversionRates;
        }

        public decimal CalculateExchangeResult(Dictionary<string, decimal> conversionRates, CurrencyCode fromCurr, decimal amount)
        {
            var toCurrString = fromCurr.ToString();

            if (!conversionRates.TryGetValue(toCurrString, out decimal conversionRate))
            {
                throw new EntityNotFoundException("Unsupported currency.");
            }

            return amount / conversionRate;
        }

        public IEnumerable<Exchange> GetUserExchanges(int userId, QueryParams parameters)
        {
            var user = userService.GetUserById(userId);

            var exchanges = exchangeRepository.GetUserExchanges(user.WalletId, parameters).ToList();

            if (!exchanges.Any() || exchanges == null)
            {
                throw new EntityNotFoundException("No exchanges available.");
            }

            return exchanges;
        }
    }
}