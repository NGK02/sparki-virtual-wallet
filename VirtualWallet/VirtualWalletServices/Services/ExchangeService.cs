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
	}
}
