using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.Business.Services
{
	public class ExchangeService : IExchangeService
	{
		private readonly IExchangeRepository exchangeRepository;
		private readonly IUserRepository userRepository;
		public ExchangeService(IExchangeRepository exchangeRepository, IUserRepository userRepository)
		{
			this.exchangeRepository = exchangeRepository;
			this.userRepository = userRepository;
		}

		public bool AddExchange(string username, Exchange exchange)
		{
			var user = userRepository.GetUserByUsername(username);
			exchange.Wallet = user.Wallet;
			exchangeRepository.AddExchange(exchange);
			return true;
		}
	}
}
