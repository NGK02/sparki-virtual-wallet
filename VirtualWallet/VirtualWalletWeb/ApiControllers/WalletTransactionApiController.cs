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
using System.Reflection;

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
				var sender = authManager.IsAuthenticated(splitCredentials);
				authManager.IsAdminOrBlocked(sender);

				var walletTransactionEntryData = mapper.Map<WalletTransaction>(transactionDto);
				var walletTransactionDto = mapper.Map<GetWalletTransactionDto>(walletTransactionService.CreateTransaction(walletTransactionEntryData, sender));
				return StatusCode(StatusCodes.Status200OK, walletTransactionDto);
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
			catch (ArgumentException ex)
			{
				return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		[HttpGet("users/{userId}")]
		public IActionResult GetUserWalletTransactions([FromQuery] WalletTransactionQueryParameters queryParameters, [FromHeader] string credentials, int userId)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				var user = authManager.IsAuthenticated(splitCredentials);
				authManager.IsContentCreatorOrAdmin(user, userId);

				var walletTransactions = walletTransactionService.GetUserWalletTransactions(queryParameters, userId);
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
			catch (ArgumentException ex)
			{
				return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		

		[HttpGet("{transactionId}")]
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
			catch (ArgumentException ex)
			{
				return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

	}
}
