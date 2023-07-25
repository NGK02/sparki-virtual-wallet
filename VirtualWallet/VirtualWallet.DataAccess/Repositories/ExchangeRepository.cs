using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.DataAccess.Repositories
{
	public class ExchangeRepository:IExchangeRepository
	{
		private readonly WalletDbContext database;
		public ExchangeRepository(WalletDbContext database)
		{
			this.database = database;
		}
		public bool AddExchange(Exchange exchange)
		{
			exchange.CreatedOn = DateTime.Now;
			database.Exchanges.Add(exchange);
			database.SaveChanges();
			return true;
		}
	}
}
