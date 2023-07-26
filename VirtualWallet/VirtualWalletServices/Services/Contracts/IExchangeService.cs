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
	}
}
