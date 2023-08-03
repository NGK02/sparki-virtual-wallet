using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.ViewModels.AdminViewModels;
using VirtualWallet.Web.Helper;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.ViewControllers
{
    public class AdminController : Controller
    {
        private readonly IAuthManagerMVC authManagerMVC;
        private readonly IUserService userService;

        public AdminController(IAuthManagerMVC authManagerMVC,IUserService userService)
        {
            this.authManagerMVC = authManagerMVC;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult SearchUser()
        {
            //if (!authManagerMVC.isLogged("LoggedUser"))
            //{
            //    return RedirectToAction("Login", "User");
            //}
            //if (!authManagerMVC.isAdmin("roleId"))
            //{
            //    this.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            //    this.ViewData["ErrorMessage"] = AuthManagerMVC.notAthorized;
            //    return View("Error");
            //}
            var searchUserForm = new SearchUser();
            searchUserForm.users = userService.GetUsers();
            return View(searchUserForm);
        }

        [HttpPost]
        public IActionResult SearchUser(SearchUser filledForm)
        {
            //if (!authManagerMVC.isLogged("LoggedUser"))
            //{
            //    return RedirectToAction("Login", "User");
            //}
            //if (!authManagerMVC.isAdmin("roleId"))
            //{
            //    this.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            //    this.ViewData["ErrorMessage"] = AuthManagerMVC.notAthorized;
            //    return View("Error");
            //}
            try
            {

                if (!this.ModelState.IsValid)
                {
                    return View(filledForm);
                }
                //var parameters = new UserQueryParams();
                //var result = new List<User>();
                //mapper.Map(filledForm, parameters);
                //result = userService.SearchBy(parameters);
                //filledForm.Users = result;
                return View(filledForm);
            }
            catch (EntityNotFoundException e)
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
    }
}
