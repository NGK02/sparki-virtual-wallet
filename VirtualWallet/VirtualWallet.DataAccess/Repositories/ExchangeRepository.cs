using Microsoft.EntityFrameworkCore;
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

		public IEnumerable<Exchange> GetUserExchanges(int walletId)
		{
			return GetQueryableExchanges().Where(t => t.WalletId == walletId).ToList();
		}

		private IQueryable<Exchange> GetQueryableExchanges()
		{
			return database.Exchanges
				.Where(e => !e.IsDeleted)
				.Include(e => e.FromCurrency)
				.Include(e => e.Wallet);
		}
	}
}
