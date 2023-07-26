using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.UserDto;
using System.Net;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Dto.ExchangeDto;

namespace VirtualWallet.Web.ApiControllers
{
	[ApiController]
	[Route("api/users")]
	public class UserApiController : ControllerBase
	{
		private readonly IMapper mapper;
		private readonly IUserService userService;
		private readonly IAuthManager authManager;
		private readonly IWalletService walletService;
		public UserApiController(IMapper mapper, IUserService userService, IAuthManager authManager, IWalletService walletService)
		{
			this.mapper = mapper;
			this.userService = userService;
			this.authManager = authManager;
			this.walletService = walletService;
		}


		[HttpPost("")]
		public IActionResult CreateUser([FromBody] CreateUserDto userDto)
		{
			try
			{
				User mappedUser = mapper.Map<User>(userDto);
				_ = userService.CreateUser(mappedUser);

				return Ok("Registered Successfully!");
			}
			catch (EmailAlreadyExistException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (UsernameAlreadyExistException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (PhoneNumberAlreadyExistException e)
			{
				return StatusCode(StatusCodes.Status400BadRequest, e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		[HttpGet("{id}")]
		public IActionResult GetUserById([FromHeader] string credentials, [FromRoute] int id)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				_ = authManager.IsAuthenticated(splitCredentials);
				string username = splitCredentials[0];
				var user = userService.GetUserById(id);
				var mappedUser = mapper.Map<GetUserDto>(user);
				return Ok(mappedUser);

			}
			catch (EntityNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch (UnauthenticatedOperationException e)
			{
				return StatusCode(StatusCodes.Status403Forbidden, e.Message);
			}
			catch (UnauthorizedAccessException e)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		[HttpPut("{id}")]
		public IActionResult EditUser([FromHeader] string credentials,[FromRoute] int id, [FromBody] UpdateUserDto userValues)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				var loggedUser = authManager.IsAuthenticated(splitCredentials);
				authManager.IsContentCreatorOrAdmin(loggedUser, id);
				string username = splitCredentials[0];
				var mapped = mapper.Map<User>(userValues);
				var updatedUser = userService.UpdateUser(username, mapped);

				return Ok("Updated Successfully!");

			}
			catch (EntityNotFoundException e)
			{
				return StatusCode(StatusCodes.Status404NotFound, e.Message);
			}
			catch (UnauthenticatedOperationException e)
			{
				return StatusCode(StatusCodes.Status403Forbidden, e.Message);
			}
			catch (UnauthorizedAccessException e)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
			}
			catch (EmailAlreadyExistException e)
			{
				return StatusCode(StatusCodes.Status403Forbidden, e.Message);
			}
			catch (UsernameAlreadyExistException e)
			{
				return StatusCode(StatusCodes.Status403Forbidden, e.Message);
			}
			catch (PhoneNumberAlreadyExistException e)
			{
				return StatusCode(StatusCodes.Status403Forbidden, e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}


		[HttpDelete("{id}")]
		public IActionResult DeleteUser([FromHeader] string credentials, [FromRoute] int id)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				var loggedUser=authManager.IsAuthenticated(splitCredentials);
				authManager.IsContentCreatorOrAdmin(loggedUser, id);
				string username = splitCredentials[0];

				userService.DeleteUser(username, null);
				return Ok("User Deleted!");

			}
			catch (EntityNotFoundException e)
			{
				return StatusCode(StatusCodes.Status404NotFound, e.Message);
			}
			catch (UnauthenticatedOperationException e)
			{
				return StatusCode(StatusCodes.Status403Forbidden, e.Message);
			}
			catch (UnauthorizedAccessException e)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		[HttpPut("{id}/exchange")]
		public async Task<IActionResult> ExchangeCurrency([FromHeader] string credentials, [FromRoute] int id, [FromBody] ExcahngeDTO excahngeValues)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				var loggedUser = authManager.IsAuthenticated(splitCredentials);
				authManager.IsContentCreatorOrAdmin(loggedUser, id);
				string username = splitCredentials[0];

				var wallet = await walletService.ExchangeFunds(excahngeValues, loggedUser.WalletId, id);

				return Ok(wallet);
			}
			catch (InsufficientFundsException ex)
			{
				return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
			}
			catch (ArgumentException ex)
			{
				return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
			}
			catch (UnauthenticatedOperationException e)
			{
				return StatusCode(StatusCodes.Status403Forbidden, e.Message);
			}
			catch (UnauthorizedAccessException e)
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
