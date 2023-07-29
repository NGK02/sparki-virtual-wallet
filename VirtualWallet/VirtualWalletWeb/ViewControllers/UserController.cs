using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Dto.ViewModels.UserViewModels;

namespace VirtualWallet.Web.ViewControllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IAuthManager authManager;
        public UserController(IUserService userService,
                                IAuthManager authManager)
        {
            this.userService = userService;
            this.authManager = authManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            var login = new Login();
            return View(login);
        }

        [HttpPost]
        public IActionResult Login(Login filledLoginForm)
        {
            //Влизаме тук ако имаме Null Username or Password!
            if (!this.ModelState.IsValid)
            {
                return View(filledLoginForm);
            }

            try
            {
                var user = authManager.IsAuthenticated(new string[] { filledLoginForm.Username, filledLoginForm.Password });

                this.HttpContext.Session.SetString("LoggedUser", filledLoginForm.Username);
                this.HttpContext.Session.SetInt32("userId", user.Id);
                this.HttpContext.Session.SetInt32("roleId", user.RoleId);

                return RedirectToAction("Index", "Home");

            }
			catch (EntityNotFoundException)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = "Invalid username or password!";
				return View(filledLoginForm);
			}
			catch (UnauthenticatedOperationException e)
			{
				this.Response.StatusCode = StatusCodes.Status403Forbidden;
				this.ViewData["ErrorMessage"] = e.Message;
				return View(filledLoginForm);
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
