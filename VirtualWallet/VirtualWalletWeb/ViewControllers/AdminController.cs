using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.Dto.ViewModels.AdminViewModels;
using VirtualWallet.Web.Helper;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.ViewControllers
{
	public class AdminController : Controller
	{
		private readonly IAuthManagerMVC authManagerMVC;
		private readonly IUserService userService;
		private readonly IAdminService adminService;
		private readonly IMapper mapper;

		public AdminController(IAuthManagerMVC authManagerMVC, IUserService userService, IAdminService adminService, IMapper mapper)
		{
			this.authManagerMVC = authManagerMVC;
			this.userService = userService;
			this.adminService = adminService;
			this.mapper = mapper;
		}

		[HttpGet]
		public IActionResult SearchUser()
		{
			if (!authManagerMVC.isLogged("LoggedUser"))
			{
				return RedirectToAction("Login", "User");
			}
			if (!authManagerMVC.isAdmin("roleId"))
			{
				this.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
				this.ViewData["ErrorMessage"] = AuthManagerMVC.notAthorized;
				return View("Error");
			}
			var searchUserForm = new SearchUser();
			searchUserForm.users = userService.GetUsers();
			return View(searchUserForm);
		}

		[HttpPost]
		public IActionResult SearchUser(SearchUser filledForm)
		{
			if (!authManagerMVC.isLogged("LoggedUser"))
			{
				return RedirectToAction("Login", "User");
			}
			if (!authManagerMVC.isAdmin("roleId"))
			{
				this.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
				this.ViewData["ErrorMessage"] = AuthManagerMVC.notAthorized;
				return View("Error");
			}
			try
			{

				if (!this.ModelState.IsValid)
				{
					filledForm.users = userService.GetUsers();
					return View(filledForm);
				}
				var parameters = new UserQueryParameters();
				
				mapper.Map(filledForm, parameters);
				var result = userService.SearchBy(parameters);
				filledForm.users.Add(result);
				return View(filledForm);
			}
			catch (EntityNotFoundException e)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = e.Message;
				return View(filledForm);
			}
			catch (InvalidOperationException e)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = e.Message;
				return View(filledForm);
			}
			catch (Exception e)
			{
				this.Response.StatusCode = StatusCodes.Status500InternalServerError;
				this.ViewData["ErrorMessage"] = e.Message;
				return View("Error");
			}
		}

		[HttpGet]
		public IActionResult BlockUser([FromRoute] int id)
		{
			try
			{
				if (!authManagerMVC.isLogged("LoggedUser"))
				{
					return RedirectToAction("Login", "User");
				}
				if (!authManagerMVC.isAdmin("roleId"))
				{
					this.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
					this.ViewData["ErrorMessage"] = AuthManagerMVC.notAthorized;
					return View("Error");
				}
				var user = userService.GetUserById(id);
				this.ViewBag.userIdToBlock = id;
				this.ViewBag.userUsernameToBlock = user.Username;
				return View();
			}
			catch (EntityNotFoundException e)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = e.Message;
				return View();
			}
			catch (Exception e)
			{
				this.Response.StatusCode = StatusCodes.Status500InternalServerError;
				this.ViewData["ErrorMessage"] = e.Message;
				return View("Error");
			}
		}
		[HttpPost]
		public IActionResult Block([FromRoute] int id)
		{
			try
			{
				if (!authManagerMVC.isLogged("LoggedUser"))
				{
					return RedirectToAction("Login", "User");
				}
				if (!authManagerMVC.isAdmin("roleId"))
				{
					this.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
					this.ViewData["ErrorMessage"] = AuthManagerMVC.notAthorized;
					return View("Error");
				}

				_ = adminService.BlockUser(id, null, null, null);
				this.ViewBag.SuccessMessage = "User Blocked Successfully!";
				return View("Successful");
			}
			catch (EntityNotFoundException e)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = e.Message;
				return View();
			}
			catch (EntityAlreadyBlockedException e)
			{
				this.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
				this.ViewData["ErrorMessage"] = e.Message;
				return View("BlockUser");
			}
			catch (Exception e)
			{
				this.Response.StatusCode = StatusCodes.Status500InternalServerError;
				this.ViewData["ErrorMessage"] = e.Message;
				return View("Error");
			}
		}
	}
}
