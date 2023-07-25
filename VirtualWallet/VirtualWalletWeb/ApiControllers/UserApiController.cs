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
using VirtualWallet.Dto.UserDTO;

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
		public UserApiController(IMapper mapper, IUserService userService, IAuthManager authManager,IWalletService walletService)
		{
			this.mapper = mapper;
			this.userService = userService;
			this.authManager = authManager;
			this.walletService = walletService;
		}


		[HttpPost("")]
		//TODO: Authentication
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

		[HttpGet("")]
		public IActionResult GetUserByUsername([FromHeader] string credentials)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				authManager.IsAuthenticated(splitCredentials);
				string username = splitCredentials[0];

				var user = userService.GetUserByUsername(username);
				var mappedUser = mapper.Map<GetUserDto>(user);
				return Ok(mappedUser);

			}
			catch (EntityNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch (UnauthenticatedOperationException e)
			{
				return Unauthorized(e.Message);
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

		[HttpPut("")]
		public IActionResult EditUser([FromHeader] string credentials, [FromBody] UpdateUserDto userValues)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				authManager.IsAuthenticated(splitCredentials);
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
				return Unauthorized(e.Message);
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
		

		[HttpDelete("")]
		public IActionResult DeleteUser([FromHeader] string credentials)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				authManager.IsAuthenticated(splitCredentials);
				string username = splitCredentials[0];

				userService.DeleteUser(username, null);
				return Ok("User Deleted!");

			}
			catch (UnauthenticatedOperationException e)
			{
				return Unauthorized(e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

        //[HttpPut("exchange")]
        //public async Task<IActionResult> ExchangeCurrency([FromHeader] string credentials, [FromBody] ExcahngeDTO excahngeValues)
        //{
        //	try
        //	{
        //		var splitCredentials = authManager.SplitCredentials(credentials);
        //		var user = authManager.IsAuthenticated(splitCredentials);

        //		string username = splitCredentials[0];
        //		var result =  await walletService.ExchangeCurrencyAsync(user, excahngeValues);

        //		return Ok(result);
        //	}
        //	catch (Exception e )
        //	{

        //		return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        //	}
        //}

        [HttpPut("exchange")]
        public async Task<IActionResult> ExchangeCurrency([FromHeader] string credentials, [FromBody] ExcahngeDTO excahngeValues)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);
                var user = authManager.IsAuthenticated(splitCredentials);

                string username = splitCredentials[0];
				//var result = await walletService.ExchangeCurrencyAsync(user, excahngeValues);

				var wallet = walletService.ExchangeFunds(excahngeValues, user.WalletId, username);

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
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
