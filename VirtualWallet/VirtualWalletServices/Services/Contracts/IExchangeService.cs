using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Business.Services.Contracts
{
	public interface IExchangeService
	{
		bool AddExchange(int userId, Exchange exchange);

		IEnumerable<Exchange> GetUserExchanges(int userId);

		Task<decimal> GetExchangeRate(string from, string to);
		Task<Tuple<decimal, decimal>> GetExchangeRateAndExchangedResult(string from, string to, string amount);
	}
}
