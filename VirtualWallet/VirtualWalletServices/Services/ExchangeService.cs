using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.Business.Services
{
	public class ExchangeService : IExchangeService
	{
		private readonly IExchangeRepository exchangeRepository;
		private readonly IUserService userService;
		public ExchangeService(IExchangeRepository exchangeRepository, IUserService userService)
		{
			this.exchangeRepository = exchangeRepository;
			this.userService = userService;
		}

		public bool AddExchange(int userId, Exchange exchange)
		{
			var user = userService.GetUserById(userId);
			exchange.Wallet = user.Wallet;
			exchangeRepository.AddExchange(exchange);
			return true;
		}

		public IEnumerable<Exchange> GetUserExchanges(int userId)
		{
			var user = userService.GetUserById(userId);

			var exchanges = exchangeRepository.GetUserExchanges(user.WalletId);

			if (!exchanges.Any() || exchanges == null)
			{
				throw new EntityNotFoundException("No exchanges available.");
			}

			return exchanges.ToList();
		}

		/// <summary>
		/// Retruns conversion rate.
		/// </summary>
		public async Task<decimal> GetExchangeRate(string from, string to)
		{
			using (var client = new HttpClient())
			{
				try
				{
					client.BaseAddress = new Uri("https://www.exchangerate-api.com");
					var response = await client.GetAsync($"https://v6.exchangerate-api.com/v6/0be94dda6c7c1a2c97df4970/pair/{from}/{to}");
					var stringResult = await response.Content.ReadAsStringAsync();
					JObject jsonObject = JObject.Parse(stringResult);
					decimal conversionRate = (decimal)jsonObject["conversion_rate"];
					return conversionRate;
				}
				catch (HttpRequestException httpRequestException)
				{
					Console.WriteLine(httpRequestException.StackTrace);
					throw;
				}
			}
		}

		/// <summary>
		/// Retruns conversion rate and the exchanged result in a Tuple<conversionRate,conversionResult>.
		/// </summary>
		public async Task<Tuple<decimal, decimal>> GetExchangeRateAndExchangedResult(string from, string to, string amount)
		{//В документацията пише ,че може да хрърли ексепшън!
			using (var client = new HttpClient())
			{
				try
				{
					client.BaseAddress = new Uri("https://www.exchangerate-api.com");
					var response = await client.GetAsync($"https://v6.exchangerate-api.com/v6/0be94dda6c7c1a2c97df4970/pair/{from}/{to}/{amount}");
					var stringResult = await response.Content.ReadAsStringAsync();
					JObject data = JObject.Parse(stringResult);
					decimal conversionRate = (decimal)data["conversion_rate"];
					decimal conversionResult = (decimal)data["conversion_result"];
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
	}
}
