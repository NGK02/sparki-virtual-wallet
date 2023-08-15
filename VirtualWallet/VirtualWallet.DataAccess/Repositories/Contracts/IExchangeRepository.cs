using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.DataAccess.Repositories.Contracts
{
	public interface IExchangeRepository
	{
		 bool AddExchange(Exchange exchange);

        public IEnumerable<Exchange> GetUserExchanges(int walletId, VirtualWallet.DataAccess.QueryParameters.QueryParams parameters);
	}
}
