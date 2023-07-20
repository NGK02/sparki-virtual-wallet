﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DTO.UserDTO;
using System.Net;

namespace VirtualWallet.Web.ApiControllers
{
	[ApiController]
	[Route("api/users")]
	public class UserApiController : ControllerBase
	{
		private readonly IMapper mapper;
		private readonly IUserService userService;
		public UserApiController(IMapper mapper,IUserService userService) 
		{ 
			this.mapper = mapper;
			this.userService = userService;
		}

		[HttpPost("")]
		public IActionResult CreateUser([FromBody] CreateUserDTO userDTO)
		{
			try
			{
				User mappedUser = mapper.Map<User>(userDTO);
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
		[HttpPut("")]
		public IActionResult EditUser([FromHeader] string credentials, [FromBody] UpdateUserDTO userValues)
		{
			string[] usernameAndPassword = credentials.Split(':');
			string userName = usernameAndPassword[0];
			try
			{
				authManager.UserCheck(credentials);
				var mapped = mapper.Map<User>(userValues);
				var updatedUser = userService.UpdateUser(userName, mapped);

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
			catch (EmailAlreadyExistException e)
			{
				return StatusCode(StatusCodes.Status403Forbidden, e.Message);
			}
			catch (UsernameAlreadyExistException e)
			{
				return StatusCode(StatusCodes.Status403Forbidden, e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}
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
