using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DTO.UserDTO;

namespace VirtualWallet.Web.ApiControllers
{
	[ApiController]
	[Route("api/users")]
	public class UserApiController : ControllerBase
	{
		//[HttpPost("")]
		//public IActionResult CreateUser([FromBody] RegisterUserDTO userDTO)
		//{
		//	try
		//	{
		//		User mappedUser = mapper.Map<User>(userDTO);
		//		var createdUser = userService.CreateUser(mappedUser);
		//		GetUserDTO result = mapper.Map<GetUserDTO>(createdUser);
		//		return Ok(result);
		//	}
		//	catch (EmailAlreadyExistException e)
		//	{
		//		return StatusCode(StatusCodes.Status403Forbidden, e.Message);
		//	}
		//	catch (UsernameAlreadyExistException e)
		//	{
		//		return StatusCode(StatusCodes.Status403Forbidden, e.Message);
		//	}
		//	catch (Exception e)
		//	{
		//		return BadRequest(e.Message);
		//	}
		//}
		//[HttpGet("")]
		//public IActionResult GetUserByUsername([FromHeader] string credentials)
		//{
		//	try
		//	{
		//		//authManager.AdminCheck(credentials);
		//		//var usersDTO = userService.SearchBy(queryParams).Select(u => mapper.Map<GetUserDTO>(u));
		//		//return Ok(usersDTO);

		//	}
		//	//catch (EntityNotFoundException e)
		//	//{
		//	//	return NotFound(e.Message);
		//	//}
		//	catch (UnauthorizedAccessException e)
		//	{
		//		return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
		//	}
		//	catch (Exception e)
		//	{
		//		return BadRequest(e.Message);
		//	}
		//}
	}
}
