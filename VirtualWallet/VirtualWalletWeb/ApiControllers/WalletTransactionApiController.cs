using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Dto.TransactionDto;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Web.ApiControllers
{
	[ApiController]
	[Route("api/transactions")]
	public class WalletTransactionApiController : ControllerBase
	{
		private IMapper mapper;
		private IWalletTransactionService walletTransactionService;
		private IAuthManager authManager;

		public WalletTransactionApiController(IMapper mapper, IWalletTransactionService walletTransactionService, IAuthManager authManager)
		{ 
			this.mapper = mapper;
			this.walletTransactionService = walletTransactionService;
			this.authManager = authManager;
		}

		[HttpPost("")]
		public IActionResult CreateWalletTransaction([FromBody] CreateTransactionDto transactionDto, [FromHeader] string credentials) 
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				authManager.IsAuthenticated(splitCredentials);
				string senderUsername = splitCredentials[0];

				var walletTransaction = mapper.Map<WalletTransaction>(transactionDto);
				walletTransactionService.CreateTransaction(walletTransaction, senderUsername);
				return StatusCode(StatusCodes.Status200OK, true);
			}
			catch (ArgumentException)
			{
				return StatusCode(StatusCodes.Status400BadRequest, "Input is not in the correct format!");
			}
			catch (UnauthenticatedOperationException e)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
			}
			catch (UnauthorizedOperationException e)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
			}
			catch (EntityNotFoundException e)
			{
				return StatusCode(StatusCodes.Status404NotFound, e.Message);
			}
			catch (InvalidOperationException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
		}

		//[HttpGet("")]
		//public IActionResult GetUserWalletTransactions([FromBody] WalletTransactionQueryParameters queryParameters, [FromHeader] string credentials)
		//{
		//	try
		//	{
		//		authManager.AreCredentialsNullOrEmpty(credentials);
		//		authManager.IsAuthenticated(credentials);
		//		string requesterUsername = credentials.Split(':')[0];
		//		walletTransactionService.GetUserWalletTransactions(queryParameters, requesterUsername);
		//		return StatusCode(StatusCodes.Status200OK, true);
		//	}
		//	catch (UnauthenticatedOperationException e)
		//	{
		//		return StatusCode(StatusCodes.Status400BadRequest, e.Message);
		//	}
		//	catch (UnauthorizedOperationException e)
		//	{
		//		return StatusCode(StatusCodes.Status400BadRequest, e.Message);
		//	}
		//	catch (EntityNotFoundException e)
		//	{
		//		return StatusCode(StatusCodes.Status400BadRequest, e.Message);
		//	}
		//	throw new NotImplementedException();
		//}
	}
}
