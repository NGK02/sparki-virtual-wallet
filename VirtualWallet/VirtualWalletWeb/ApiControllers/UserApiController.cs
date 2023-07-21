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

namespace VirtualWallet.Web.ApiControllers
{
	[ApiController]
	[Route("api/users")]
	public class UserApiController : ControllerBase
	{
		private readonly IMapper mapper;
		private readonly IUserService userService;
		private readonly IAuthManager authManager;
		public UserApiController(IMapper mapper, IUserService userService, IAuthManager authManager)
		{
			this.mapper = mapper;
			this.userService = userService;
			this.authManager = authManager;
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
	}
}
