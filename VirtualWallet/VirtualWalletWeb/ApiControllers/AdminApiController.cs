﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories;
using VirtualWallet.Dto.TransactionDto;
using VirtualWallet.Dto.UserDto;

namespace VirtualWallet.Web.ApiControllers
{
	[ApiController]
	[Route("api/admin")]
	public class AdminApiController : ControllerBase
	{
		private readonly IAuthManager authManager;
		private readonly IAdminService adminService;
		private readonly ICardService cardService;
		private readonly ITransferService transferService;
		private readonly IWalletTransactionService walletTransactionService;
		private readonly IMapper mapper;
		public AdminApiController(IAuthManager authManager,
								IAdminService adminService,
								ICardService cardService,
								ITransferService transferService,
								IWalletTransactionService walletTransactionService,
								IMapper mapper)
		{
			this.authManager = authManager;
			this.adminService = adminService;
			this.cardService = cardService;
			this.mapper = mapper;
			this.transferService = transferService;
			this.walletTransactionService= walletTransactionService;
		}

		[HttpPut("block")]
		public IActionResult BlockUser([FromHeader] string credentials, [FromQuery] int? id, string username,string email,string phoneNumber)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				authManager.IsAdmin(splitCredentials);
				adminService.BlockUser(id, username,email,phoneNumber);
				return Ok("User blocked successfully!");
			}
			catch (EntityNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch (UnauthenticatedOperationException e)
			{
				return Unauthorized(e.Message);
			}
			catch (UnauthorizedOperationException e)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
			}
			catch (ArgumentNullException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (EntityAlreadyBlockedException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}

		}

		[HttpPut("unblock")]
		public IActionResult UnBlockUser(string credentials, [FromQuery] int? id, string username, string email, string phoneNumber)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				authManager.IsAdmin(splitCredentials);
				adminService.UnBlockUser(id, username, email, phoneNumber);
				return Ok("User unblocked successfully!");
			}
			catch (EntityNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch (UnauthenticatedOperationException e)
			{
				return Unauthorized(e.Message);
			}
			catch (UnauthorizedOperationException e)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
			}
			catch (ArgumentNullException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (EntityAlreadyUnBlockedException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}

		}

		[HttpGet("cards")]
		public IActionResult GetAllCards([FromHeader] string credentials)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				var user = authManager.IsAdmin(splitCredentials);
				var cards = cardService.GetCards(user.Id);

				return Ok(cards);
			}
			catch (EntityNotFoundException ex)
			{
				return StatusCode(StatusCodes.Status404NotFound, ex.Message);
			}
			catch (UnauthenticatedOperationException ex)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
			}
			catch (UnauthorizedOperationException ex)
			{
				return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
			}
			catch (ArgumentException ex)
			{
				return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("transfers")]
		public IActionResult GetAllTransfers([FromHeader] string credentials)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				var user = authManager.IsAdmin(splitCredentials);
				var transfers = transferService.GetTransfers(user.Id);

				return Ok(transfers);
			}
			catch (EntityNotFoundException ex)
			{
				return StatusCode(StatusCodes.Status404NotFound, ex.Message);
			}
			catch (UnauthenticatedOperationException ex)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
			}
			catch (UnauthorizedOperationException ex)
			{
				return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
			}
			catch (ArgumentException ex)
			{
				return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		[HttpGet("transactions")]
		public IActionResult GetAllWalletTransactions([FromHeader] string credentials,[FromQuery] WalletTransactionQueryParameters queryParameters)
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
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		[HttpGet("users")]
		public IActionResult GetAllUsers(string credentials, [FromQuery] UserQueryParameters userParameters)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				authManager.IsAdmin(splitCredentials);
				var result = adminService.GetUsers(userParameters).Select(u => mapper.Map<GetUserDto>(u));
				return Ok(result);
			}
			catch (EntityNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch (UnauthenticatedOperationException e)
			{
				return Unauthorized(e.Message);
			}
			catch (UnauthorizedOperationException e)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}
	}
}
