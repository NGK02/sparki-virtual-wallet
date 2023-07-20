using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Dto.TransactionDto;

namespace VirtualWallet.Web.ApiControllers
{
	[ApiController]
	[Route("api/transactions")]
	public class TransactionApiController : ControllerBase
	{
		private IMapper mapper;
		private ITransactionService transactionService;
		private IAuthManager authManager;

		public TransactionApiController(IMapper mapper, ITransactionService transactionService, IAuthManager authManager)
		{ 
			this.mapper = mapper;
			this.transactionService = transactionService;
			this.authManager = authManager;
		}

		[HttpPost("")]
		public IActionResult CreateTransaction([FromBody] CreateTransactionDto transactionDto) 
		{
			throw new NotImplementedException();
		}
	}
}
