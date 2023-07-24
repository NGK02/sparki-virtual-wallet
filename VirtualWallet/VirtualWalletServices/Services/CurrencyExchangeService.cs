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

		public async Task<decimal> GetExchangeRateAndExchangedResult(string from, string to,string amount)
		{
			using (var client = new HttpClient())
			{
				try
				{
					client.BaseAddress = new Uri("https://www.exchangerate-api.com");
					var response = await client.GetAsync($"https://v6.exchangerate-api.com/v6/0be94dda6c7c1a2c97df4970/pair/{from}/{to}/{amount}");
					var stringResult = await response.Content.ReadAsStringAsync();
					JObject data = JObject.Parse(stringResult);
					decimal conversionRate = (decimal)data["conversion_rate"];
					decimal conversionResul = (decimal)
					return conversionRate;
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
