using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.DataAccess.Repositories
{
    public class ExchangeRepository : IExchangeRepository
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

        public IEnumerable<Exchange> GetUserExchanges(int walletId, QueryParameters.QueryParameters parameters)
        {
            var exchangesQuerable = GetQueryableExchanges().Where(t => t.WalletId == walletId);
            return SortExchangesBy(exchangesQuerable, parameters).ToList();

        }

        private IQueryable<Exchange> SortExchangesBy(IQueryable<Exchange> exchangesQuerable, QueryParameters.QueryParameters parameters)
        {
            if (parameters.SortBy is not null)
            {

                if (parameters.SortBy.Equals("Date", StringComparison.InvariantCultureIgnoreCase))
                {
                    exchangesQuerable = exchangesQuerable.OrderBy(e => e.CreatedOn);
                }
                if (parameters.SortBy.Equals("FromCurrency", StringComparison.InvariantCultureIgnoreCase))
                {
                    exchangesQuerable = exchangesQuerable.OrderBy(e => e.FromCurrency.Code);
                }
                if (parameters.SortBy.Equals("ToCurrency", StringComparison.InvariantCultureIgnoreCase))
                {
                    exchangesQuerable = exchangesQuerable.OrderBy(t => t.ToCurrency.Code);
                }
                if (parameters.SortBy.Equals("Amount", StringComparison.InvariantCultureIgnoreCase))
                {
                    exchangesQuerable = exchangesQuerable.OrderBy(e => e.Amount);
                }
                if (parameters.SortBy.Equals("ExchangedAmount", StringComparison.InvariantCultureIgnoreCase))
                {
                    exchangesQuerable = exchangesQuerable.OrderBy(e => e.ExchangedAmout);
                }

            }

            if (parameters.SortOrder is not null)
            {
                if (parameters.SortOrder.Equals("Descending", StringComparison.InvariantCultureIgnoreCase))
                {
                    exchangesQuerable = exchangesQuerable.Reverse();
                }
            }
            return exchangesQuerable;
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
