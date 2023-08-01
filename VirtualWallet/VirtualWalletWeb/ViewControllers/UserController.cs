using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.UserDto;
using VirtualWallet.Dto.ViewModels.UserViewModels;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.ViewControllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IAuthManager authManager;
        private readonly IImageManager imageManager;

        public UserController(IUserService userService,
                                IAuthManager authManager,
                                IMapper mapper,
                                IImageManager imageManager)
        {
            this.userService = userService;
            this.authManager = authManager;
            this.mapper = mapper;
            this.imageManager = imageManager;
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
        [HttpGet]
        public IActionResult Logout()
        {
            this.HttpContext.Session.Remove("LoggedUser");
            this.HttpContext.Session.Remove("userId");
            this.HttpContext.Session.Remove("roleId");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var registerUser = new RegisterUser();
            return View(registerUser);
        }
        [HttpPost]
        public IActionResult Register(RegisterUser filledForm)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return View(filledForm);
                }

                var user = mapper.Map<User>(filledForm);

                if (filledForm.ProfilePic is null)
                {
                    filledForm.ProfilePic = imageManager.GeneratePlaceholderAvatar(filledForm.FirstName, filledForm.LastName);
                    user.ProfilePicPath = imageManager.UploadGeneratedProfilePicInRoot(filledForm.ProfilePic);
                }
                else
                {
                    user.ProfilePicPath = imageManager.UploadOriginalProfilePicInRoot(filledForm.ProfilePic);
                }
                userService.CreateUser(user);
                return RedirectToAction("Index", "Home");

            }
            catch (EmailAlreadyExistException e)
            {
                this.Response.StatusCode = StatusCodes.Status409Conflict;
                this.ViewData["ErrorMessage"] = e.Message;
                return View(filledForm);
            }
            catch (UsernameAlreadyExistException e)
            {
                this.Response.StatusCode = StatusCodes.Status409Conflict;
                this.ViewData["ErrorMessage"] = e.Message;
                return View(filledForm);
            }
            catch (PhoneNumberAlreadyExistException e)
            {
                this.Response.StatusCode = StatusCodes.Status409Conflict;
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
