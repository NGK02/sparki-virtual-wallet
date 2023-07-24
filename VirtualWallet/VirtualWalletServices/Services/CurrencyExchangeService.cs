using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VirtualWallet.Business.Services
{
	public class CurrencyExchangeService
	{
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
		public async Task<Tuple<decimal, decimal>> GetExchangeRateAndExchangedResult(string from, string to,string amount)
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
