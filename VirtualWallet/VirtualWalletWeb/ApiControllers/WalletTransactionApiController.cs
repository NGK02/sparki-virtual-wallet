using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Dto.TransactionDto;
using VirtualWallet.DataAccess.QueryParameters;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

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
		public IActionResult CreateWalletTransaction([FromBody] CreateWalletTransactionDto transactionDto, [FromHeader] string credentials) 
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
			//catch (ArgumentException)
			//{
			//	return StatusCode(StatusCodes.Status400BadRequest, "Input is not in the correct format!");
			//}
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
			catch (AutoMapperMappingException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.GetBaseException().Message);
			}
			catch (DbUpdateException)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error when completing transaction!");
			}
		}

		[HttpGet("user")]
		public IActionResult GetUserWalletTransactions([FromBody] WalletTransactionQueryParameters queryParameters, [FromHeader] string credentials)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				authManager.IsAuthenticated(splitCredentials);
				string username = splitCredentials[0];

				var walletTransactions = walletTransactionService.GetUserWalletTransactions(queryParameters, username);
				var walletTransactionsMapped = walletTransactions.Select(wt => mapper.Map<GetWalletTransactionDto>(wt)).ToList();
				return StatusCode(StatusCodes.Status200OK, walletTransactionsMapped);
			}
			catch (UnauthenticatedOperationException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (UnauthorizedOperationException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (EntityNotFoundException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (AutoMapperMappingException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.GetBaseException().Message);
			}
		}

		[HttpGet("")]
		public IActionResult GetWalletTransactions([FromBody] WalletTransactionQueryParameters queryParameters, [FromHeader] string credentials)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				authManager.IsAdmin(splitCredentials);
				string username = splitCredentials[0];

				var walletTransactions = walletTransactionService.GetWalletTransactions(queryParameters, username);
				var walletTransactionsMapped = walletTransactions.Select(wt => mapper.Map<GetWalletTransactionDto>(wt)).ToList();
				return StatusCode(StatusCodes.Status200OK, walletTransactionsMapped);
			}
			catch (UnauthenticatedOperationException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (UnauthorizedOperationException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (EntityNotFoundException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (AutoMapperMappingException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.GetBaseException().Message);
			}
		}

		[HttpGet("{id}")]
		public IActionResult GetWalletTransactionById(int id, [FromHeader] string credentials)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				authManager.IsAuthenticated(splitCredentials);
				string username = splitCredentials[0];

				var walletTransaction = walletTransactionService.GetWalletTransactionById(id, username);
				var walletTransactionMapped = mapper.Map<GetWalletTransactionDto>(walletTransaction);
				return StatusCode(StatusCodes.Status200OK, walletTransactionMapped);
			}
			catch (UnauthenticatedOperationException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (UnauthorizedOperationException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (EntityNotFoundException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (AutoMapperMappingException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.GetBaseException().Message);
			}
		}
	}
}
