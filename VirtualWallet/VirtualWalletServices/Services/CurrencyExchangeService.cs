using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VirtualWallet.Business.Services
{
	public class CurrencyExchangeService
	{
		public async Task<string> GetExchangeRate(string from, string to)
		{
			using (var client = new HttpClient())
			{
				try
				{
					client.BaseAddress = new Uri("https://www.exchangerate-api.com");
					var response = await client.GetAsync($"https://v6.exchangerate-api.com/v6/0be94dda6c7c1a2c97df4970/pair/{from}/{to}");
					var stringResult = await response.Content.ReadAsStringAsync();
					var dictResult = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(stringResult);
					return dictResult[$"{from}_{to}"]["val"];
				}
				catch (HttpRequestException httpRequestException)
				{
					Console.WriteLine(httpRequestException.StackTrace);
					return "Error calling API. Please do manual lookup.";
				}
			}
		}
	}
}
