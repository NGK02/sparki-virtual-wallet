using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories;
using VirtualWallet.Dto.UserDto;

namespace VirtualWallet.Web.ApiControllers
{
	[ApiController]
	[Route("api/admin")]
	public class AdminApiController : ControllerBase
	{
		private readonly IAuthManager authManager;
		private readonly IAdminService adminService;
		private readonly IMapper mapper;
		public AdminApiController(IAuthManager authManager, IAdminService adminService, IMapper mapper)
		{
			this.authManager = authManager;
			this.adminService = adminService;
			this.mapper = mapper;
		}

		[HttpPut("block")]
		public IActionResult BlockUser([FromHeader] string credentials, [FromQuery] int? id, string username)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				authManager.IsAdmin(splitCredentials);
				adminService.BlockUser(id, username);
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
			catch (UnauthorizedAccessException e)
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
		public IActionResult UnBlockUser(string credentials, [FromQuery] int? id, string username)
		{
			try
			{
				var splitCredentials = authManager.SplitCredentials(credentials);
				authManager.IsAdmin(splitCredentials);
				adminService.UnBlockUser(id, username);
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
			catch (UnauthorizedAccessException e)
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
